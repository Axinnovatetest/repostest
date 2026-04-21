using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Api
{
	public static class RequestResponseLoggingMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
		}
	}
	public class RequestResponseLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
		public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			_next = next;
			_logger = loggerFactory
					  .CreateLogger<RequestResponseLoggingMiddleware>();
			_recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
		}
		public async Task Invoke(HttpContext context)
		{
			await LogRequest(context);
			await LogResponse(context);
		}
		private async Task LogRequest(HttpContext context)
		{
			context.Request.EnableBuffering();

			using(var requestStream = _recyclableMemoryStreamManager.GetStream())
			{
				await context.Request.Body.CopyToAsync(requestStream);
				Infrastructure.Services.Logging.Logger.LogInfo(
					$"HTTP REQUEST:: " +
									   //$">>->\tSchema: {context.Request.Scheme} " +
									   $">>->\tHost: {context.Request.Host} " +
									   $">>->\tPath: {context.Request.Path} " +
									   $">>->\tHeaders: [{string.Join(" | ", context.Request.Headers?.Select(x => $"{x.Key}: {x.Value}")?.ToList())}] " +
									   $">>->\tQueryString: {context.Request.QueryString} " /*+
                                       $">>->\tRequest Body: {ReadStreamInChunks(requestStream)}"*/);
			}
			context.Request.Body.Position = 0;
		}
		private async Task LogResponse(HttpContext context)
		{
			var originalBodyStream = context.Response.Body;

			using(var responseBody = _recyclableMemoryStreamManager.GetStream())
			{
				context.Response.Body = responseBody;

				await _next(context);

				context.Response.Body.Seek(0, SeekOrigin.Begin);
				var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
				context.Response.Body.Seek(0, SeekOrigin.Begin);

				Infrastructure.Services.Logging.Logger.LogInfo(
					$"HTTP RESPONSE:: " +
									   //$">>->\tSchema: {context.Request.Scheme} " +
									   $">>->\tHost: {context.Request.Host} " +
									   $">>->\tPath: {context.Request.Path} " +
									   $">>->\tQueryString: {context.Request.QueryString} " +
									   $">>->\tResponse Body: {text}");

				await responseBody.CopyToAsync(originalBodyStream);
			}
		}
		private static string ReadStreamInChunks(Stream stream)
		{
			const int readChunkBufferLength = 4096;
			stream.Seek(0, SeekOrigin.Begin);
			using(var textWriter = new StringWriter())
			using(var reader = new StreamReader(stream))
			{
				var readChunk = new char[readChunkBufferLength];
				int readChunkLength;

				do
				{
					readChunkLength = reader.ReadBlock(readChunk,
													   0,
													   readChunkBufferLength);
					textWriter.Write(readChunk, 0, readChunkLength);
				} while(readChunkLength > 0);

				return textWriter.ToString();
			}
		}
	}
}