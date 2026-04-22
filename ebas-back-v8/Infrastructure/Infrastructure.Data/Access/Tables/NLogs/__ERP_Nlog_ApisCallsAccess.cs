using Infrastructure.Data.Entities.Tables.Logistics;
using Infrastructure.Data.Entities.Tables.NLogs;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.NLogs
{
	public class __ERP_Nlog_ApisCallsAccess
	{
		private static readonly SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1, 1);

		public static async Task<int> Delete()
		{
			await _writeSemaphore.WaitAsync();
			try
			{
				using(var connection = new SQLiteConnection(Settings.ConnectionStringNlog))
				{
					await connection.OpenAsync();
					using(var command = connection.CreateCommand())
					{
						command.CommandText = "delete from __ERP_Nlog_ApisCalls";
						return await command.ExecuteNonQueryAsync();
					}
				}
			} finally
			{
				_writeSemaphore.Release();
			}
		}
		public static List<__ERP_Nlog_ApisCallsEntity> GetApiCalls(string searchValue, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = $@"select * from [__ERP_Nlog_ApisCalls]";

				if(!String.IsNullOrEmpty(searchValue))
				{
					query += $" WHERE Api LIKE '%{searchValue}%'";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Api desc ";
				}
				if(paging != null)
				{
					query += $" LIMIT {paging.RequestRows} OFFSET {paging.FirstRowNumber} ";
				}

				var sqlCommand = new SQLiteCommand(query, sqlConnection);
				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new __ERP_Nlog_ApisCallsEntity(x)).ToList();
			}
			else
			{
				return new List<__ERP_Nlog_ApisCallsEntity>();
			}
		}
		public static int GetApiCalls_Count(string searchValue)
		{
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = $@"select count(*) from [__ERP_Nlog_ApisCalls]";
				if(!String.IsNullOrEmpty(searchValue))
				{
					query += $" WHERE Api LIKE '%{searchValue}%'";
				}
				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
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
						command.CommandText = @"insert into __ERP_Nlog_ApisCalls (Api,Url,calls_all,calls_last_3_months,calls_last_6_months,calls_last_12_months)
						select MemberName AS Api,SourceFilePath as Url,count(*) as CallCount,
						SUM(Date >= datetime('now', '-3 months'))  AS calls_last_3_months,
						SUM(Date >= datetime('now', '-6 months'))  AS calls_last_6_months,
						SUM(Date >= datetime('now', '-12 months')) AS calls_last_12_months
						from __ERP_Nlog_Exceptions
						where IsCall=1
						group by MemberName,SourceFilePath
						ORDER BY MemberName desc";
						return await command.ExecuteNonQueryAsync();
					}
				}
			} finally
			{
				_writeSemaphore.Release();
			}
		}
		public static int GetTotalApisCalled()
		{
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = @$"select count(distinct Api) as totalApisCalled from __ERP_Nlog_ApisCalls";

				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}
		public static KeyValuePair<string, int> LeastOrLeastCalledApi(bool most = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = @$"SELECT Url,calls_all
								FROM __ERP_Nlog_ApisCalls
								ORDER BY calls_all {(most ? "DESC" : "")}
								LIMIT 1;";
				var sqlCommand = new SQLiteCommand(query, sqlConnection);
				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return new KeyValuePair<string, int>(dataTable.Rows[0]["Url"].ToString(), int.TryParse(dataTable.Rows[0]["calls_all"].ToString(), out var val) ? val : 0);
			}
			else
			{
				return new KeyValuePair<string, int>();
			}
		}
	}
}