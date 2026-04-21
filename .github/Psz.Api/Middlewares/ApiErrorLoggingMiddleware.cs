using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Psz.Api.Middlewares
{
	public class ApiErrorLoggingMiddleware
	{
		public readonly RequestDelegate _next;

		public ApiErrorLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			context.Request.EnableBuffering();
			var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
			context.Request.Body.Position = 0;

			var originalBodyStream = context.Response.Body;
			using var responseBody = new MemoryStream();
			context.Response.Body = responseBody;
			string exceptionMessage = null;

			try
			{
				await _next(context);
			} catch(Exception ex)
			{
				exceptionMessage = ex.ToString();
				throw;
			} finally
			{
				context.Response.Body.Seek(0, SeekOrigin.Begin);
				var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
				context.Response.Body.Seek(0, SeekOrigin.Begin);

				var (shouldLog, errors) = ShouldLogResponse(responseText);

				if(shouldLog || context.Response.StatusCode != StatusCodes.Status200OK)
					await LogToDatabase(context, requestBody, responseText, exceptionMessage, errors);

				await responseBody.CopyToAsync(originalBodyStream);
			}
		}
		private (bool shouldLog, List<string> errors) ShouldLogResponse(string responseText)
		{
			if(string.IsNullOrWhiteSpace(responseText))
				return (false, null);
			try
			{
				// Parse JSON and check "Success" flag
				using var doc = JsonDocument.Parse(responseText);
				if(doc.RootElement.TryGetProperty("Success", out var successProp))
				{
					if(successProp.ValueKind == JsonValueKind.False)
					{
						// Extract Errors property if it exists
						List<string> errors = null;
						if(doc.RootElement.TryGetProperty("Errors", out var errorsProp)
							&& errorsProp.ValueKind == JsonValueKind.Array)
						{
							errors = new List<string>();
							foreach(var error in errorsProp.EnumerateArray())
							{
								if(error.ValueKind == JsonValueKind.String)
								{
									errors.Add(error.GetString());
								}
								else if(error.ValueKind == JsonValueKind.Object)
								{
									if(error.TryGetProperty("Value", out var valueProp)
										&& valueProp.ValueKind == JsonValueKind.String)
									{
										errors.Add(valueProp.GetString());
									}
								}
							}
						}
						return (true, errors);
					}
				}
			} catch(Exception e)
			{
				//Infrastructure.Services.Logging.Logger.Log(e);
			}

			return (false, null);

		}
		public async Task LogToDatabase(HttpContext context, string requestBody, string responseText,
		string exceptionMessage, List<string> errors)
		{
			try
			{
				var routeData = context.GetRouteData();
				string controller = routeData?.Values["controller"]?.ToString() ?? "";
				string action = routeData?.Values["action"]?.ToString() ?? "";
				var save = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveLogsToSqlite"]);
				if(!action.IsNullOrEmptyOrWhiteSpaces() && !controller.IsNullOrEmptyOrWhiteSpaces() && !action.IsNullOrEmptyOrWhiteSpaces() && !requestBody.IsNullOrEmptyOrWhiteSpaces())
				{
					if(save)
					{
						var logEntity = new Infrastructure.Data.Entities.Tables.NLogs.ERP_Nlog_ExceptionsEntity
						{
							Date = System.DateTime.Now,
							EventId = -1,
							Level = "Error",
							MemberName = action,
							Message = errors != null && errors.Count > 0 ? string.Join("|", errors) : null,
							SourceFilePath = $"{controller}/{action}",
							SourceLineNumber = null,
							Body = string.Equals(action, "login", StringComparison.OrdinalIgnoreCase)
				? ControllerExtension.removePasswordFromRequestBody(requestBody)
				: requestBody
						};
						await Infrastructure.Data.Access.Tables.NLogs.ERP_Nlog_ExceptionsAccess.Insert(logEntity);
					}
				}

			} catch(System.Exception e)
			{
				//Infrastructure.Services.Logging.Logger.Log(e);
			}
		}
	}
}
