
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Psz.Api.Middlewares
{
	public class RequestTelemetryMiddleware
	{
		private const string CorrelationHeader = "X-Correlation-Id";
		private readonly RequestDelegate _next;
		private readonly int _maxMilliseconds;

		public RequestTelemetryMiddleware(RequestDelegate next, int maxMillseconds)
		{
			_next = next;
			_maxMilliseconds = maxMillseconds;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var correlationId = GetOrCreateCorrelationId(context);
			var stopwatch = Stopwatch.StartNew();

			context.Items["CorrelationId"] = correlationId;

			try
			{
				await _next(context);
			} catch(Exception ex)
			{
				stopwatch.Stop();

				Infrastructure.Services.Logging.Logger.LogError(
					$"HTTP {context.Request.Method} {context.Request.Path}{context.Request.QueryString} " +
					$"FAILED in {stopwatch.ElapsedMilliseconds} ms " +
					$"Status: 500 | CorrelationId: {correlationId}",
					ex.Message
				);

				throw;
			}

			stopwatch.Stop();

			// - log execution time only for SLow requests
			if(stopwatch.ElapsedMilliseconds > _maxMilliseconds)
			{
				Infrastructure.Services.Logging.Logger.LogWarning(
					$"SLOW REQUEST: HTTP {context.Request.Method} {context.Request.Path} " +
					$" | {((decimal)stopwatch.ElapsedMilliseconds)/1000} seconds | CorrelationId: {correlationId}"
				);
			}
			else
			{
				Infrastructure.Services.Logging.Logger.LogInfo(
					$"HTTP {context.Request.Method} {context.Request.Path}{context.Request.QueryString} " +
					$"Responded {context.Response.StatusCode} | {((decimal)stopwatch.ElapsedMilliseconds) / 1000} seconds " +
					$"| CorrelationId: {correlationId}"
				);
			}
		}

		private string GetOrCreateCorrelationId(HttpContext context)
		{
			if(context.Request.Headers.TryGetValue(CorrelationHeader, out var existing))
			{
				context.Response.Headers[CorrelationHeader] = existing.ToString();
				return existing.ToString();
			}

			var correlationId = Guid.NewGuid().ToString();
			context.Response.Headers[CorrelationHeader] = correlationId;
			return correlationId;
		}
	}
}
