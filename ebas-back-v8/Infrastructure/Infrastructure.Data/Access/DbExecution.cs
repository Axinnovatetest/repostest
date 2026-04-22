using Psz.Core.SharedKernel.Interfaces;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access
{
	public static class DbExecution
	{
		public static IDbLogger Logger { get; set; }

		// Master switch
		public static bool Enabled { get; set; } = true;

		public static int SlowThresholdMs { get; set; } = 15000;

		public static void Fill(
			SqlCommand command,
			DataTable table,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string filePath = "")
		{
			Profile(() =>
			{
				new SqlDataAdapter(command).Fill(table);
			}, command, memberName, filePath);
		}

		public static int ExecuteNonQuery(
			SqlCommand command,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string filePath = "")
		{
			return Profile(() => command.ExecuteNonQuery(), command, memberName, filePath);
		}

		public static object ExecuteScalar(
			SqlCommand command,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string filePath = "")
		{
			return Profile(() => command.ExecuteScalar(), command, memberName, filePath);
		}

		public static SqlDataReader ExecuteReader(
			SqlCommand command,
			CommandBehavior behavior = CommandBehavior.Default,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string filePath = "")
		{
			return Profile(() => command.ExecuteReader(behavior), command, memberName, filePath);
		}

		public static Task<SqlDataReader> ExecuteReaderAsync(
			SqlCommand command,
			CommandBehavior behavior = CommandBehavior.Default,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string filePath = "")
		{
			return ProfileAsync(() => command.ExecuteReaderAsync(behavior), command, memberName, filePath);
		}

		private static T Profile<T>(
			Func<T> action,
			SqlCommand command,
			string memberName,
			string filePath)
		{
			if(!Enabled || Logger == null)
				return action();

			var sw = Stopwatch.StartNew();

			try
			{
				var result = action();
				sw.Stop();

				Log(command, sw.ElapsedMilliseconds, memberName, filePath);

				return result;
			} catch(Exception ex)
			{
				sw.Stop();
				LogError(sw.ElapsedMilliseconds, memberName, filePath, ex);
				throw;
			}
		}

		private static async Task<T> ProfileAsync<T>(
			Func<Task<T>> action,
			SqlCommand command,
			string memberName,
			string filePath)
		{
			if(!Enabled || Logger == null)
				return await action();

			var sw = Stopwatch.StartNew();

			try
			{
				var result = await action();
				sw.Stop();

				Log(command, sw.ElapsedMilliseconds, memberName, filePath);

				return result;
			} catch(Exception ex)
			{
				sw.Stop();
				LogError(sw.ElapsedMilliseconds, memberName, filePath, ex);
				throw;
			}
		}

		private static void Profile(
			Action action,
			SqlCommand command,
			string memberName,
			string filePath)
		{
			if(!Enabled || Logger == null)
			{
				action();
				return;
			}

			var sw = Stopwatch.StartNew();

			try
			{
				action();
				sw.Stop();

				Log(command, sw.ElapsedMilliseconds, memberName, filePath);
			} catch(Exception ex)
			{
				sw.Stop();
				LogError(sw.ElapsedMilliseconds, memberName, filePath, ex);
				throw;
			}
		}

		private static void Log(SqlCommand command, long elapsedMs, string memberName, string filePath)
		{
			var operation = BuildOperationName(memberName, filePath);

			var message =
				$"DB {operation} | {elapsedMs / 1000.0:F3}s | Params:{command.Parameters.Count}";

			if(elapsedMs > SlowThresholdMs)
				Logger.Warning("SLOW QUERY: " + message);
			//else
			//	Logger.Info(message);
		}

		private static void LogError(long elapsedMs, string memberName, string filePath, Exception ex)
		{
			Logger?.Error(
				$"DB ERROR {BuildOperationName(memberName, filePath)} | {elapsedMs / 1000.0:F3}s",
				ex);
		}

		private static string BuildOperationName(string memberName, string filePath)
		{
			var className = Path.GetFileNameWithoutExtension(filePath);
			return $"{className}.{memberName}";
		}
	}
}
