using Microsoft.AspNetCore.Http;
using NLog.Common;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services.Logging
{
	public class Logger
	{
		private static object _lock = new object();
		private static NLog.Logger _shadowNLogger;
		private static NLog.Logger _nLogger
		{
			get
			{
				lock(_lock)
				{
					if(_shadowNLogger == null)
					{
						_shadowNLogger = NLog.LogManager.GetCurrentClassLogger();

						InternalLogger.LogFile = "nlog-start.log";
						InternalLogger.LogLevel = NLog.LogLevel.Trace;
					}

					return _shadowNLogger;
				}
			}
		}

		public enum Levels: int
		{
			Trace = 0,
			Debug = 1,
			Info = 2,
			Warn = 3,
			Error = 4,
			Fatal = 5
		}

		public static void Log(Levels level,
			string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0,
			int userId = -1,
			bool apiCall = false
			)
		{
			var correlationId = GetCorrelationId();
			message = $"{message} | CorrelationId: {correlationId}";
			_nLogger.Log(getNLogLevel(level),
				message
				+ " >--> " + memberName
				+ " >--> " + sourceFilePath
				+ " >--> " + sourceLineNumber);

			var memberName_ = memberName;
			var sourceFilePath_ = sourceFilePath;
			var userId_ = userId;
			var sourceLineNumber_ = sourceLineNumber;
			if(apiCall)
			{
				var parts = message.Split("|");
				var parts2 = parts[3].Split(">-->");
				memberName_ = Path.GetFileName(parts[1]);
				sourceFilePath_ = parts[1];
				userId_ = int.Parse(parts[0]);
				sourceLineNumber_ = int.Parse(parts2[3]);
			}
			if(Module.SaveLogsToSqlite)
				Task.Run(async () =>
				{
					await Infrastructure.Data.Access.Tables.NLogs.ERP_Nlog_ExceptionsAccess.Insert(new Infrastructure.Data.Entities.Tables.NLogs.ERP_Nlog_ExceptionsEntity
					{
						Date = DateTime.Now,
						EventId = userId_,
						Level = level.ToString(),
						MemberName = memberName_,
						SourceFilePath = sourceFilePath_,
						SourceLineNumber = sourceLineNumber_,
						Message = message,
						IsCall = apiCall
					});
				}).GetAwaiter().GetResult();
		}

		public static void Log(Exception exception,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(Levels.Error,
				"Exception: " + exception?.Message,
				memberName,
				sourceFilePath,
				sourceLineNumber);
			Log(Levels.Trace,
				"Exception: " + exception?.StackTrace,
				memberName,
				sourceFilePath,
				sourceLineNumber);
		}

		public static void LogDebug(string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(Levels.Debug,
				message
				+ " >--> " + memberName
				+ " >--> " + sourceFilePath
				+ " >--> " + sourceLineNumber);
		}
		public static void LogTrace(string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(Levels.Trace,
				message
				+ " >--> " + memberName
				+ " >--> " + sourceFilePath
				+ " >--> " + sourceLineNumber);
		}
		public static void Log(string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(Levels.Trace,
				message,
				memberName,
				sourceFilePath,
				sourceLineNumber);
		}
		public static void LogInfo(string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0, bool apiCall = false)
		{
			Log(Levels.Info,
				message
				+ " >--> " + memberName
				+ " >--> " + sourceFilePath
				+ " >--> " + sourceLineNumber, apiCall: apiCall);
		}
		public static void LogWarning(string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(Levels.Warn,
				message
				+ " >--> " + memberName
				+ " >--> " + sourceFilePath
				+ " >--> " + sourceLineNumber);
		}
		public static void LogError(string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(Levels.Error,
				message
				+ " >--> " + memberName
				+ " >--> " + sourceFilePath
				+ " >--> " + sourceLineNumber);
		}
		public static void LogFatal(string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(Levels.Fatal,
				message
				+ " >--> " + memberName
				+ " >--> " + sourceFilePath
				+ " >--> " + sourceLineNumber);
		}

		#region > Helpers
		private static NLog.LogLevel getNLogLevel(Levels level)
		{
			switch(level)
			{
				default:
				case Levels.Trace:
					return NLog.LogLevel.Trace;

				case Levels.Debug:
					return NLog.LogLevel.Debug;

				case Levels.Info:
					return NLog.LogLevel.Info;

				case Levels.Warn:
					return NLog.LogLevel.Warn;

				case Levels.Error:
					return NLog.LogLevel.Error;

				case Levels.Fatal:
					return NLog.LogLevel.Fatal;
			}
		}
		public static string GetCorrelationId()
		{
			var context = new HttpContextAccessor().HttpContext;

			if(context != null && context.Items.TryGetValue("CorrelationId", out var id))
				return id?.ToString() ?? "-";

			return "-";
		}
		#endregion
	}
	public class DbLogger: IDbLogger
	{
		public void Info(string message)
			=> Logger.LogInfo(message);

		public void Warning(string message)
			=> Logger.LogWarning(message);

		public void Error(string message, Exception ex = null)
			=> Logger.LogError($"{message}: ExMessage:{ex}");
	}
}
