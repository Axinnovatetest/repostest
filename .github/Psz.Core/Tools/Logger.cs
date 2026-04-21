using NLog.Common;
using System;

namespace Psz.Core.Tools
{
	public class Logger
	{
		public Logger()
		{
			_nlogger = NLog.LogManager.GetCurrentClassLogger();

			InternalLogger.LogFile = "nlog-start.log";
			InternalLogger.LogLevel = NLog.LogLevel.Trace;
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

		public void Log(Levels level,
			string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			_nlogger.Log(getNLogLevel(level),
				message
				+ " >--> " + memberName
				+ " >--> " + sourceFilePath
				+ " >--> " + sourceLineNumber);
		}
		public void Log(string message,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			this.Log(Levels.Trace,
				message,
				memberName,
				sourceFilePath,
				sourceLineNumber);
		}
		public void Log(Exception exception,
			[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			this.Log(Logger.Levels.Error,
				"Exception: " + exception?.Message,
				memberName,
				sourceFilePath,
				sourceLineNumber);
		}

		#region > Nlog
		private NLog.Logger _nlogger = null;

		private NLog.LogLevel getNLogLevel(Levels level)
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
		#endregion
	}
}
