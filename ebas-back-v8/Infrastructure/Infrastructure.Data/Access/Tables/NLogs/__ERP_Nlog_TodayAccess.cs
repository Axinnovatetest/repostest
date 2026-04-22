using Infrastructure.Data.Entities.Tables.Logistics;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.NLogs
{
	public class __ERP_Nlog_TodayAccess
	{
		private static readonly SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1, 1);
		public static async Task<int> DeleteAsync()
		{
			await _writeSemaphore.WaitAsync();
			try
			{
				using(var connection = new SQLiteConnection(Settings.ConnectionStringNlog))
				{
					await connection.OpenAsync();
					using(var command = connection.CreateCommand())
					{
						command.CommandText = "DELETE FROM __ERP_Nlog_Today";
						return await command.ExecuteNonQueryAsync();
					}
				}
			} finally
			{
				_writeSemaphore.Release();
			}
		}
		public static async Task<int> InsertAsync()
		{
			await _writeSemaphore.WaitAsync();
			try
			{
				using(var connection = new SQLiteConnection(Settings.ConnectionStringNlog))
				{
					await connection.OpenAsync();
					using(var command = connection.CreateCommand())
					{
						command.CommandText = @"insert into __ERP_Nlog_Today (EventId,[Date],[Level],MemberName,SourceFilePath,SourceLineNumber,Message,Body)
						select EventId,[Date],[Level],MemberName,SourceFilePath,SourceLineNumber,Message,Body
						from __ERP_Nlog_Exceptions where IsCall<>1 and date(Date) = date('now', '-1 day');";
						return await command.ExecuteNonQueryAsync();
					}
				}
			} finally
			{
				_writeSemaphore.Release();
			}
		}
	}
}