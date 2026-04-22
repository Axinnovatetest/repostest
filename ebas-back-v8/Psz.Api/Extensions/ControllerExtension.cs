using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json;
using System.Threading.Tasks;

namespace Psz.Api
{
	public static class ControllerExtension
	{
		public static Core.Identity.Models.UserModel GetCurrentUser(this ControllerBase controller, bool includeAccess = true)
		{
			var user = new Psz.Core.Identity.Handlers.User.GetHandler(controller.Request.Headers, includeAccess).Handle();
			Infrastructure.Services.Logging.Logger.LogInfo($"{user?.Id}|{parseRequest(controller)}", apiCall: true);
			return user;
		}
		static string parseRequest(ControllerBase controller)
		{
			return $"{controller?.HttpContext?.Request?.Path}|{(controller?.HttpContext?.Request?.Method)}|{controller?.HttpContext?.Request?.QueryString}";
		}

		public static IActionResult HandleException(this ControllerBase controller,
			Exception exception,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			var user = new Psz.Core.Identity.Handlers.User.GetHandler(controller.Request.Headers, true).Handle();
			if(exception is Core.Exceptions.UnauthorizedException)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
					"Controller response: Not authorized",
					memberName,
					sourceFilePath,
					sourceLineNumber);
				return controller.Ok(Core.Common.Models.ResponseModel<object>.AccessDeniedResponse()); //controller.Unauthorized();
			}
			else if(exception is Core.Exceptions.NotFoundException)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
					"Controller response: Not found",
					memberName,
					sourceFilePath,
					sourceLineNumber);
				return controller.Ok(Core.Common.Models.ResponseModel<object>.NotFoundResponse()); //controller.NotFound();
			}
			else
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace); // - save stack trace
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Error,
					"Controller response: Error: " + exception.Message,
					memberName,
					sourceFilePath,
					sourceLineNumber,
					user.Id);
				return controller.Ok(Core.Common.Models.ResponseModel<object>.UnexpectedErrorResponse()); //controller.StatusCode(500);
			}
		}
		public static IActionResult HandleException(this ControllerBase controller,
			Exception exception,
			object input,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			var user = new Psz.Core.Identity.Handlers.User.GetHandler(controller.Request.Headers, true).Handle();
			Infrastructure.Services.Logging.Logger.Log($"[Input] {System.Text.Json.JsonSerializer.Serialize(input)}");
			var save = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveLogsToSqlite"]);
			if(save)
				Task.Run(async () =>
				{
					await Infrastructure.Data.Access.Tables.NLogs.ERP_Nlog_ExceptionsAccess.Insert(new Infrastructure.Data.Entities.Tables.NLogs.ERP_Nlog_ExceptionsEntity
					{
						Date = DateTime.Now,
						EventId = user.Id,
						Level = "Input",
						MemberName = memberName,
						SourceFilePath = sourceFilePath,
						SourceLineNumber = sourceLineNumber,
						Message = exception.Message,
						Body = string.Equals(memberName, "login", StringComparison.OrdinalIgnoreCase)
										? ControllerExtension.removePasswordFromRequestBody(System.Text.Json.JsonSerializer.Serialize(input))
										: System.Text.Json.JsonSerializer.Serialize(input)
					});
				}).GetAwaiter().GetResult();
			if(exception is Core.Exceptions.UnauthorizedException)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
					"Controller response: Not authorized",
					memberName,
					sourceFilePath,
					sourceLineNumber);
				return controller.Ok(Core.Common.Models.ResponseModel<object>.AccessDeniedResponse()); //controller.Unauthorized();
			}
			else if(exception is Core.Exceptions.NotFoundException)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn,
					"Controller response: Not found",
					memberName,
					sourceFilePath,
					sourceLineNumber);
				return controller.Ok(Core.Common.Models.ResponseModel<object>.NotFoundResponse()); //controller.NotFound();
			}
			else
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace); // - save stack trace
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Error,
					"Controller response: Error: " + exception.Message,
					memberName,
					sourceFilePath,
					sourceLineNumber);
				return controller.Ok(Core.Common.Models.ResponseModel<object>.UnexpectedErrorResponse()); //controller.StatusCode(500);
			}
		}

		public static string removePasswordFromRequestBody(string requestBody)
		{
			try
			{
				using var doc = JsonDocument.Parse(requestBody);
				var root = doc.RootElement;
				if(root.ValueKind == JsonValueKind.Object && root.TryGetProperty("password", out var passwordProp))
				{
					var modifiedObject = new Dictionary<string, object>();
					foreach(var property in root.EnumerateObject())
					{
						if(property.NameEquals("password"))
						{
							modifiedObject[property.Name] = "****";
						}
						else
						{
							modifiedObject[property.Name] = property.Value.Clone();
						}
					}
					return JsonSerializer.Serialize(modifiedObject);
				}
			} catch(Exception e)
			{
				//Infrastructure.Services.Logging.Logger.Log(e);
			}
			return requestBody;
		}
	}
}
