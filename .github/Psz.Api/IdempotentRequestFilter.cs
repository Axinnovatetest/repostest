using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Psz.Api
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
	public sealed class IdempotentRequestAttribute: Attribute, IFilterFactory
	{
		/// <param name="windowSeconds">Time window in seconds</param>
		/// <param name="maxCalls">Max calls allowed for same user+same input within the window</param>
		public bool IsReusable => false;

		private readonly int _windowSeconds;
		private readonly int _maxCalls;
		public IdempotentRequestAttribute(int windowSeconds = 60, int maxCalls = 1)
		{
			_windowSeconds = windowSeconds;
			_maxCalls = maxCalls;
		}
		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			var cache = serviceProvider.GetRequiredService<IMemoryCache>();
			return new IdempotentRequestFilter(cache, _windowSeconds, _maxCalls);
		}
	}
	public sealed class IdempotentRequestFilter: IAsyncActionFilter
	{
		private readonly IMemoryCache _cache;
		private readonly TimeSpan _window;
		private readonly int _maxCalls;

		public IdempotentRequestFilter(IMemoryCache cache, int windowSeconds, int maxCalls)
		{
			_cache = cache;
			_window = TimeSpan.FromSeconds(windowSeconds);
			_maxCalls = maxCalls;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var http = context.HttpContext;
			var userName =
				http.User.FindFirstValue(ClaimTypes.NameIdentifier)
				?? http.User.Identity?.Name
				?? "anonymous";

			var path = http.Request.Path.Value ?? "unknown";

			// Read body without breaking model binding
			http.Request.EnableBuffering();
			byte[] bodyBytes;
			using(var ms = new MemoryStream())
			{
				await http.Request.Body.CopyToAsync(ms);
				bodyBytes = ms.ToArray();
				http.Request.Body.Position = 0;
			}

			// Canonical JSON fingerprint (if body is JSON; if not, treat as raw)
			string bodyHash;
			try
			{
				bodyHash = JsonFingerprint.ComputeFromUtf8Body(bodyBytes);
			} catch(JsonException)
			{
				bodyHash = Convert.ToHexString(SHA256.HashData(bodyBytes));
			}

			var query = CanonicalizeQuery(http.Request.Query);
			var countKey = $"dedup:{userName}:{path}:{query}:{bodyHash}:count";

			// Fast path: if already at limit, short-circuit BEFORE running the action
			if(_cache.TryGetValue(countKey, out Counter? existing) && existing is not null && existing.Current >= _maxCalls)
			{
				context.Result = new ObjectResult(new
				{
					Success = false,
					Errors = new List<string>
					{
						$"Duplicate request limit exceeded [Max call = {_maxCalls}] for same input in window [{(int)_window.TotalSeconds} sec]."
					}
				})
				{ StatusCode = StatusCodes.Status429TooManyRequests };

				return;
			}

			// Execute action
			var executed = await next();

			// Don't count unhandled exceptions
			if(executed.Exception != null && !executed.ExceptionHandled)
				return;

			// Determine Success from ResponseModel<T>
			bool success = false;
			object? value = executed.Result switch
			{
				ObjectResult o => o.Value,
				JsonResult j => j.Value,
				_ => null
			};

			if(value is not null)
			{
				var t = value.GetType();
				if(t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ResponseModel<>))
				{
					var prop = t.GetProperty(nameof(ResponseModel<int>.Success));
					if(prop?.PropertyType == typeof(bool))
						success = (bool)(prop.GetValue(value) ?? false);
				}
			}

			// Only count when Success == true
			if(!success)
				return;

			// Increment success counter
			var counter = _cache.GetOrCreate(countKey, entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = _window;
				return new Counter();
			})!;

			var newValue = counter.Increment();

			if(newValue > _maxCalls)
			{
				// Recommended: undo the increment (requires Counter.Decrement())
				counter.Decrement();

				// IMPORTANT: override the RESULT, not context.Result
				executed.Result = new ObjectResult(new
				{
					Success = false,
					Errors = new List<string>
					{
						$"Duplicate request limit exceeded [Max call = {_maxCalls}] for same input in window [{(int)_window.TotalSeconds} sec]."
					}
				})
				{ StatusCode = StatusCodes.Status429TooManyRequests };
			}
		}


		private sealed class Counter
		{
			private int _value;
			public int Current => Volatile.Read(ref _value);
			public int Increment() => Interlocked.Increment(ref _value);
			public int Decrement() => Interlocked.Decrement(ref _value);

		}
		private static string CanonicalizeQuery(IQueryCollection query) =>
		string.Join("&", query
			.OrderBy(k => k.Key, StringComparer.Ordinal)
			.SelectMany(k =>
				k.Value.OrderBy(v => v, StringComparer.Ordinal)
					   .Select(v => $"{Uri.EscapeDataString(k.Key)}={Uri.EscapeDataString(v)}")));
	}
	public static class RequestDedupKey
	{
		private static readonly JsonSerializerOptions JsonOpts = new()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = false
		};

		public static string BuildKey(string userName, string endpointPath, IDictionary<string, object?> actionArguments)
		{
			// Normalize arguments into a deterministic JSON string
			var normalized = Normalize(actionArguments);

			var payload = $"{userName}|{endpointPath}|{normalized}";
			var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload)));

			return $"dedup:{userName}:{endpointPath}:{hash}";
		}

		private static string Normalize(IDictionary<string, object?> args)
		{
			// Sort keys for determinism
			var sorted = args
				.OrderBy(k => k.Key, StringComparer.Ordinal)
				.ToDictionary(
					k => k.Key,
					v => NormalizeValue(v.Value),
					StringComparer.Ordinal);

			return JsonSerializer.Serialize(sorted, JsonOpts);
		}

		private static object? NormalizeValue(object? value)
		{
			if(value is null)
				return null;

			// If controller args include CancellationToken etc., ignore them
			if(value is System.Threading.CancellationToken)
				return null;

			// If it's already a simple type, keep it
			var t = value.GetType();
			if(t.IsPrimitive || value is string or decimal or DateTime or Guid)
				return value;

			// For complex DTOs: serialize, parse to JsonElement, then normalize JSON properties order
			var json = JsonSerializer.Serialize(value, JsonOpts);
			using var doc = JsonDocument.Parse(json);
			return NormalizeJson(doc.RootElement);
		}

		private static object NormalizeJson(JsonElement el)
		{
			return el.ValueKind switch
			{
				JsonValueKind.Object => el.EnumerateObject()
					.OrderBy(p => p.Name, StringComparer.Ordinal)
					.ToDictionary(p => p.Name, p => NormalizeJson(p.Value), StringComparer.Ordinal),

				JsonValueKind.Array => el.EnumerateArray()
					// If order matters for your API, remove OrderBy below.
					// If order does NOT matter (typical for "items" lists), sort by the element's JSON string.
					.Select(NormalizeJson)
					.OrderBy(x => JsonSerializer.Serialize(x, JsonOpts), StringComparer.Ordinal)
					.ToList(),

				JsonValueKind.String => el.GetString()!,
				JsonValueKind.Number => el.TryGetInt64(out var l) ? l : el.GetDecimal(),
				JsonValueKind.True => true,
				JsonValueKind.False => false,
				_ => null
			};
		}
	}
	public static class JsonFingerprint
	{
		public static string ComputeFromUtf8Body(byte[] bodyBytes)
		{
			if(bodyBytes is null || bodyBytes.Length == 0)
				return "empty";

			using var doc = JsonDocument.Parse(bodyBytes);
			var normalized = Normalize(doc.RootElement);

			// Deterministic JSON string
			var canonical = JsonSerializer.Serialize(normalized, new JsonSerializerOptions
			{
				WriteIndented = false
			});

			var hash = SHA256.HashData(Encoding.UTF8.GetBytes(canonical));
			return Convert.ToHexString(hash);
		}

		private static object? Normalize(JsonElement el) =>
			el.ValueKind switch
			{
				JsonValueKind.Object => el.EnumerateObject()
					.OrderBy(p => p.Name, StringComparer.Ordinal)
					.ToDictionary(p => p.Name, p => Normalize(p.Value), StringComparer.Ordinal),

				JsonValueKind.Array => el.EnumerateArray()
					// with “no assumptions”, keep array order (safest)
					.Select(Normalize)
					.ToList(),

				JsonValueKind.String => el.GetString(),
				JsonValueKind.Number => el.TryGetInt64(out var l) ? l : el.GetDecimal(),
				JsonValueKind.True => true,
				JsonValueKind.False => false,
				_ => null
			};
	}
}
