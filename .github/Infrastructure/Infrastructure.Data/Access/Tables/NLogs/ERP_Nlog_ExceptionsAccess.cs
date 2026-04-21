using Infrastructure.Data.Entities.Joins.NLog;
using Infrastructure.Data.Entities.Tables.Logistics;
using Infrastructure.Data.Entities.Tables.NLogs;
using Psz.Core.Support.Models;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.NLogs
{
	public static class ERP_Nlog_ExceptionsAccess
	{
		private static readonly SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1, 1);

		#region Default Methods
		public static ERP_Nlog_ExceptionsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__ERP_Nlog_Exceptions] WHERE [Id]=@Id";
				var sqlCommand = new SQLiteCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Id", id);

				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new ERP_Nlog_ExceptionsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<ERP_Nlog_ExceptionsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__ERP_Nlog_Exceptions]";
				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_Nlog_ExceptionsEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_Nlog_ExceptionsEntity>();
			}
		}
		public static List<ERP_Nlog_ExceptionsEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
				{
					sqlConnection.Open();
					var sqlCommand = new SQLiteCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [__ERP_Nlog_Exceptions] WHERE [Id] IN ({queryIds})";
					new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_Nlog_ExceptionsEntity(x)).ToList();
				}
				else
				{
					return new List<ERP_Nlog_ExceptionsEntity>();
				}
			}
			return new List<ERP_Nlog_ExceptionsEntity>();
		}
		public static List<ERP_Nlog_ExceptionsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<ERP_Nlog_ExceptionsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<ERP_Nlog_ExceptionsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<ERP_Nlog_ExceptionsEntity>();
		}
		public static async Task<int> Insert(ERP_Nlog_ExceptionsEntity item)
		{
			await _writeSemaphore.WaitAsync();
			try
			{
				using(var connection = new SQLiteConnection(Settings.ConnectionStringNlog))
				{
					await connection.OpenAsync();
					using(var command = connection.CreateCommand())
					{
						command.CommandText = "INSERT INTO __ERP_Nlog_Exceptions (EventId, Date, Level, MemberName, SourceFilePath, SourceLineNumber,Message,Body,IsCall) VALUES (@EventId, @Date, @Level, @MemberName, @SourceFilePath, @SourceLineNumber,@Message,@Body,@IsCall); SELECT last_insert_rowid();";
						command.Parameters.AddWithValue(parameterName: "EventId", item.EventId == null ? (object)DBNull.Value : item.EventId);
						command.Parameters.AddWithValue("Date", item.Date == null ? (object)DBNull.Value : item.Date);
						command.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
						command.Parameters.AddWithValue("MemberName", item.MemberName == null ? (object)DBNull.Value : item.MemberName);
						command.Parameters.AddWithValue("SourceFilePath", item.SourceFilePath == null ? (object)DBNull.Value : item.SourceFilePath);
						command.Parameters.AddWithValue("SourceLineNumber", item.SourceLineNumber == null ? (object)DBNull.Value : item.SourceLineNumber);
						command.Parameters.AddWithValue("Message", item.Message == null ? (object)DBNull.Value : item.Message);
						command.Parameters.AddWithValue("Body", item.Body == null ? (object)DBNull.Value : item.Body);
						command.Parameters.AddWithValue("IsCall", item.IsCall == null ? (object)DBNull.Value : item.IsCall);
						return await command.ExecuteNonQueryAsync();
					}
				}
			} finally
			{
				_writeSemaphore.Release();
			}
		}
		public static async Task<int> InsertAsync(ERP_Nlog_ExceptionsEntity item)
		{
			var result = -1;
			using(var connection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				connection.Open();
				string query = "INSERT INTO __ERP_Nlog_Exceptions (EventId, Date, Level, MemberName, SourceFilePath, SourceLineNumber,Message,Body,IsCall) VALUES (@EventId, @Date, @Level, @MemberName, @SourceFilePath, @SourceLineNumber,@Message,@Body,@IsCall); SELECT last_insert_rowid();";
				using(var command = new SQLiteCommand(query, connection))
				{
					command.Parameters.AddWithValue("EventId", item.EventId == null ? (object)DBNull.Value : item.EventId);
					command.Parameters.AddWithValue("Date", item.Date == null ? (object)DBNull.Value : item.Date);
					command.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
					command.Parameters.AddWithValue("MemberName", item.MemberName == null ? (object)DBNull.Value : item.MemberName);
					command.Parameters.AddWithValue("SourceFilePath", item.SourceFilePath == null ? (object)DBNull.Value : item.SourceFilePath);
					command.Parameters.AddWithValue("SourceLineNumber", item.SourceLineNumber == null ? (object)DBNull.Value : item.SourceLineNumber);
					command.Parameters.AddWithValue("Message", item.Message == null ? (object)DBNull.Value : item.Message);
					command.Parameters.AddWithValue("Body", item.Body == null ? (object)DBNull.Value : item.Body);
					command.Parameters.AddWithValue("IsCall", item.IsCall == null ? (object)DBNull.Value : item.IsCall);

					result = await command.ExecuteNonQueryAsync();
				}
			}
			return result;
		}
		public static int insert(List<ERP_Nlog_ExceptionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SQLiteCommand(query, sqlConnection);
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__ERP_Nlog_Exceptions] ([EventId],[Date],[Level],[MemberName],[SourceFilePath],[SourceLineNumber]) VALUES ("
							+ "@EventId" + i +
							 ","
							+ "@Date" + i +
							 ","
							+ "@Level" + i +
							 ","
							+ "@MemberName" + i +
							 ","
							+ "@SourceFilePath" + i +
							 ","
							+ "@SourceLineNumber" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("EventId" + i, item.EventId == null ? (object)DBNull.Value : item.EventId);
						sqlCommand.Parameters.AddWithValue("Date" + i, item.Date == null ? (object)DBNull.Value : item.Date);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("MemberName" + i, item.MemberName == null ? (object)DBNull.Value : item.MemberName);
						sqlCommand.Parameters.AddWithValue("SourceFilePath" + i, item.SourceFilePath == null ? (object)DBNull.Value : item.SourceFilePath);
						sqlCommand.Parameters.AddWithValue("SourceLineNumber" + i, item.SourceLineNumber == null ? (object)DBNull.Value : item.SourceLineNumber);
					}
					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}
				return results;
			}
			return -1;
		}
		public static int Insert(List<ERP_Nlog_ExceptionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7;
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}
			return -1;
		}
		public static int Update(ERP_Nlog_ExceptionsEntity item)
		{
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "UPDATE [__ERP_Nlog_Exceptions] SET [EventId] = @EventId, [Date] = @Date, [Level] = @Level, [MemberName] = @MemberName, [SourceFilePath] = @SourceFilePath, [SourceLineNumber] = @SourceLineNumber WHERE [Id] = @Id";

				using(var sqlCommand = new SQLiteCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", item.EventId);
					sqlCommand.Parameters.AddWithValue("EventId", item.EventId == null ? (object)DBNull.Value : item.EventId);
					sqlCommand.Parameters.AddWithValue("Date", item.Date == null ? (object)DBNull.Value : item.Date);
					sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
					sqlCommand.Parameters.AddWithValue("MemberName", item.MemberName == null ? (object)DBNull.Value : item.MemberName);
					sqlCommand.Parameters.AddWithValue("SourceFilePath", item.SourceFilePath == null ? (object)DBNull.Value : item.SourceFilePath);
					sqlCommand.Parameters.AddWithValue("SourceLineNumber", item.SourceLineNumber == null ? (object)DBNull.Value : item.SourceLineNumber);

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int update(List<ERP_Nlog_ExceptionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SQLiteCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__ERP_Nlog_Exceptions] SET "
						  + "[EventId]=@EventId" + i +
						   ","
						  + "[Date]=@Date" + i +
						   ","
						  + "[Level]=@Level" + i +
						   ","
						  + "[MemberName]=@MemberName" + i +
						   ","
						  + "[SourceFilePath]=@SourceFilePath" + i +
						   ","
						  + "[SourceLineNumber]=@SourceLineNumber" + i +
						 " WHERE [Id]=@Id" + i
							+ "; ";
						sqlCommand.Parameters.AddWithValue("EventId" + i, item.EventId == null ? (object)DBNull.Value : item.EventId);
						sqlCommand.Parameters.AddWithValue("Date" + i, item.Date == null ? (object)DBNull.Value : item.Date);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("MemberName" + i, item.MemberName == null ? (object)DBNull.Value : item.MemberName);
						sqlCommand.Parameters.AddWithValue("SourceFilePath" + i, item.SourceFilePath == null ? (object)DBNull.Value : item.SourceFilePath);
						sqlCommand.Parameters.AddWithValue("SourceLineNumber" + i, item.SourceLineNumber == null ? (object)DBNull.Value : item.SourceLineNumber);
						sqlCommand.Parameters.AddWithValue("Id" + i, item.EventId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}
		public static int Update(List<ERP_Nlog_ExceptionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7;
				int results = 0;

				if(items.Count <= maxParamsNumber)
				{
					results = update(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}

			return -1;
		}
		public static int Delete(int id)
		{
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "DELETE FROM [__ERP_Nlog_Exceptions] WHERE [Id] = @Id";

				using(var sqlCommand = new SQLiteCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", id);

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int delete(List<int> ids)
		{
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string queryIds = string.Join(",", Enumerable.Range(0, ids.Count).Select(i => "@Id" + i));
				string query = "DELETE FROM [__ERP_Nlog_Exceptions] WHERE [Id] IN (" + queryIds + ")";

				using(var sqlCommand = new SQLiteCommand(query, sqlConnection, sqlTransaction))
				{
					for(int i = 0; i < ids.Count; i++)
					{
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;

				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}
			else
			{
				return -1;
			}
		}
		#endregion Default Methods

		#region Custom Methods
		public static List<ERP_Nlog_ExceptionsEntity> GetLogsExceptions(string level, string searchValue, DateTime? searchDate, string sortFieldName, bool sortDesc, int currentPage = 0, int? pageSize = 0)
		{
			if(string.IsNullOrWhiteSpace(sortFieldName))
				sortFieldName = "Date";
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__ERP_Nlog_Exceptions] WHERE IsCall<>1";
				var clauses = new List<string>();

				if(searchDate.HasValue)
				{
					clauses.Add($" date([Date]) = date('{searchDate.Value.ToString("yyyy-MM-dd")}')");
				}

				if(!string.IsNullOrWhiteSpace(searchValue))
				{
					clauses.Add($" [Message] LIKE '%{searchValue}%' OR [MemberName] LIKE '%{searchValue}%' ");
				}
				if(!string.IsNullOrWhiteSpace(level))
				{
					clauses.Add($" LOWER([Level])='{level}' ");
				}

				if(clauses.Count > 0)
				{
					query += $" {string.Join(" AND ", clauses)}";
				}

				if(!string.IsNullOrWhiteSpace(sortFieldName))
				{
					query += $" ORDER BY {sortFieldName} {(sortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [Date]";
				}

				if(pageSize != 0)
				{
					query += $"LIMIT {pageSize} OFFSET {currentPage * 10}";
				}


				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_Nlog_ExceptionsEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_Nlog_ExceptionsEntity>();
			}
		}
		public static int GetLogsExceptions_Count(string level, string searchValue, DateTime? searchDate)
		{
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(*) As Nb FROM  [__ERP_Nlog_Exceptions] WHERE IsCall<>1";

				var clauses = new List<string>();

				if(searchDate.HasValue)
				{
					clauses.Add($" date([Date]) = date('{searchDate.Value.ToString("yyyy-MM-dd")}')");
				}

				if(!string.IsNullOrWhiteSpace(searchValue))
				{
					clauses.Add($" [Message] LIKE '%{searchValue}%' OR [MemberName] LIKE '%{searchValue}%' ");
				}
				if(!string.IsNullOrWhiteSpace(level))
				{
					clauses.Add($"  LOWER([Level])='{level}' ");
				}

				if(clauses.Count > 0)
				{
					query += $" {string.Join(" AND ", clauses)}";
				}

				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}


		}
		public static List<NLogsCountEntity> GetApiCalls6Months(string apiUrl, string sortFieldName, bool sortDesc, int currentPage = 0, int? pageSize = 0)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = @"WITH LogData AS (
								SELECT 
									CASE 
										WHEN Message LIKE '%|%|%' THEN
											substr(
												Message,
												instr(Message, '|') + 1,
												instr(substr(Message, instr(Message, '|') + 1), '|') - 1
											)
										ELSE NULL
									END AS Url,
									strftime('%Y-%m', [Date]) AS YearMonth,
									CASE strftime('%m', [Date])
										WHEN '01' THEN 'January'
										WHEN '02' THEN 'February'
										WHEN '03' THEN 'March'
										WHEN '04' THEN 'April'
										WHEN '05' THEN 'May'
										WHEN '06' THEN 'June'
										WHEN '07' THEN 'July'
										WHEN '08' THEN 'August'
										WHEN '09' THEN 'September'
										WHEN '10' THEN 'October'
										WHEN '11' THEN 'November'
										WHEN '12' THEN 'December'
									END AS MonthName,
									Level
								FROM __ERP_Nlog_Exceptions
								WHERE 
									Message LIKE '%|%|%'
									AND [Date] IS NOT NULL
							),

							RecentMonths AS (
								SELECT DISTINCT 
									YearMonth,
									MonthName
								FROM LogData
								WHERE Url IS NOT NULL AND Url != ''
								ORDER BY YearMonth DESC
								LIMIT 6
							),

							MonthlyCounts AS (
								SELECT 
									Url,
									YearMonth,
									MonthName,
									COUNT(*) as Count
								FROM LogData
								WHERE 
									Url IS NOT NULL 
									AND Url != ''
									AND YearMonth IN (SELECT YearMonth FROM RecentMonths)
								GROUP BY Url, YearMonth, MonthName
							)

							SELECT 
								mc.Url,
								SUM(CASE WHEN rm.MonthRank = 1 THEN mc.Count ELSE 0 END) AS 'Recent_Month_Count' ,
								SUM(CASE WHEN rm.MonthRank = 2 THEN mc.Count ELSE 0 END) AS 'Month_2_Count',
								SUM(CASE WHEN rm.MonthRank = 3 THEN mc.Count ELSE 0 END) AS 'Month_3_Count', 
								SUM(CASE WHEN rm.MonthRank = 4 THEN mc.Count ELSE 0 END) AS 'Month_4_Count',
								SUM(CASE WHEN rm.MonthRank = 5 THEN mc.Count ELSE 0 END) AS 'Month_5_Count',
								SUM(CASE WHEN rm.MonthRank = 6 THEN mc.Count ELSE 0 END) AS 'Month_6_Count',
								SUM(mc.Count) AS Total_Count
							FROM MonthlyCounts mc
							JOIN (
								SELECT 
									YearMonth, 
									MonthName,
									ROW_NUMBER() OVER (ORDER BY YearMonth DESC) as MonthRank
								FROM RecentMonths
							) rm ON mc.YearMonth = rm.YearMonth ";

				var clauses = new List<string>();

				if(!String.IsNullOrEmpty(apiUrl))
				{
					clauses.Add($" mc.Url LIKE '%{apiUrl}%'");
				}

				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}
				if(!string.IsNullOrWhiteSpace(sortFieldName))
				{
					query += $" ORDER BY {sortFieldName} {(sortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += @" GROUP by  mc.Url";
				}

				if(pageSize != 0)
				{
					query += $" LIMIT {pageSize} OFFSET {currentPage * 10}";
				}
				var sqlCommand = new SQLiteCommand(query, sqlConnection);
				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new NLogsCountEntity(x)).ToList();
			}
			else
			{
				return new List<NLogsCountEntity>();
			}
		}
		public static int GetApiCalls6Months_Count(string ApiUrl)
		{
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = $@"WITH LogData AS (
								SELECT 
									CASE 
										WHEN Message LIKE '%|%|%' THEN
											substr(
												Message,
												instr(Message, '|') + 1,
												instr(substr(Message, instr(Message, '|') + 1), '|') - 1
											)
										ELSE NULL
									END AS Url,
									strftime('%Y-%m', [Date]) AS YearMonth,
									CASE strftime('%m', [Date])
										WHEN '01' THEN 'January'
										WHEN '02' THEN 'February'
										WHEN '03' THEN 'March'
										WHEN '04' THEN 'April'
										WHEN '05' THEN 'May'
										WHEN '06' THEN 'June'
										WHEN '07' THEN 'July'
										WHEN '08' THEN 'August'
										WHEN '09' THEN 'September'
										WHEN '10' THEN 'October'
										WHEN '11' THEN 'November'
										WHEN '12' THEN 'December'
									END AS MonthName,
									Level
								FROM __ERP_Nlog_Exceptions
								WHERE 
									Message LIKE '%|%|%'
									AND [Date] IS NOT NULL
							),

							RecentMonths AS (
								SELECT DISTINCT 
									YearMonth,
									MonthName
								FROM LogData
								WHERE Url IS NOT NULL AND Url != ''
								ORDER BY YearMonth DESC
								LIMIT 6
							),

							MonthlyCounts AS (
								SELECT 
									Url,
									YearMonth,
									MonthName,
									COUNT(*) as Count
								FROM LogData
								WHERE 
									Url IS NOT NULL 
									AND Url != ''
									AND YearMonth IN (SELECT YearMonth FROM RecentMonths)
								GROUP BY Url, YearMonth, MonthName
							),
							FinalResult As (
							SELECT 
								mc.Url,
								SUM(CASE WHEN rm.MonthRank = 1 THEN mc.Count ELSE 0 END) AS 'Recent_Month_Count' ,
								SUM(CASE WHEN rm.MonthRank = 2 THEN mc.Count ELSE 0 END) AS 'Month_2_Count',
								SUM(CASE WHEN rm.MonthRank = 3 THEN mc.Count ELSE 0 END) AS 'Month_3_Count', 
								SUM(CASE WHEN rm.MonthRank = 4 THEN mc.Count ELSE 0 END) AS 'Month_4_Count',
								SUM(CASE WHEN rm.MonthRank = 5 THEN mc.Count ELSE 0 END) AS 'Month_5_Count',
								SUM(CASE WHEN rm.MonthRank = 6 THEN mc.Count ELSE 0 END) AS 'Month_6_Count',
								SUM(mc.Count) AS Total_Count
							FROM MonthlyCounts mc
							JOIN (
								SELECT 
									YearMonth, 
									MonthName,
									ROW_NUMBER() OVER (ORDER BY YearMonth DESC) as MonthRank
								FROM RecentMonths
							) rm ON mc.YearMonth = rm.YearMonth
							{(!String.IsNullOrEmpty(ApiUrl) ? $"mc.Url LIKE '%{ApiUrl}%'" : "")}
							GROUP BY mc.Url
							)
							select Count(*) as NB from FinalResult";

				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}
		public static List<NLogsLastCallEntity> GetApiLastCall(string apiUrl, string sortFieldName, bool sortDesc, int currentPage = 0, int? pageSize = 0)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = @$"select substr(
												Message,
								instr(Message,'|')+1,
								instr(substr(Message,instr(Message,'|')+1),'|')-1
								)  AS Url,MAX(Date) AS LastCall
								FROM __ERP_Nlog_Exceptions";

				var clauses = new List<string>();

				clauses.Add("Message like '%|%'");
				clauses.Add("Url<>''");
				clauses.Add("Level='Info'");

				if(!String.IsNullOrEmpty(apiUrl))
				{
					clauses.Add($" [Url] LIKE '%{apiUrl}%'");
				}

				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}
				if(!string.IsNullOrWhiteSpace(sortFieldName))
				{
					query += $" ORDER BY {sortFieldName} {(sortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += "GROUP BY Url ORDER BY [LastCall]";
				}

				if(pageSize != 0)
				{
					query += $"LIMIT {pageSize} OFFSET {currentPage * 10}";
				}
				var sqlCommand = new SQLiteCommand(query, sqlConnection);
				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new NLogsLastCallEntity(x)).ToList();
			}
			else
			{
				return new List<NLogsLastCallEntity>();
			}
		}
		public static int GetApiLastCall_Count(string ApiUrl)
		{
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = @$"select Count(*) as Nb, substr(
								Message,
								instr(Message,'|')+1,
								instr(substr(Message,instr(Message,'|')+1),'|')-1
								)  AS Url
								FROM __ERP_Nlog_Exceptions";

				var clauses = new List<string>();

				clauses.Add("Message like '%|%'");
				clauses.Add(@"Url <>''");
				clauses.Add("Level='Info'");

				if(!String.IsNullOrEmpty(ApiUrl))
				{
					clauses.Add(@$" Url  LIKE '%{ApiUrl}%'");
				}

				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}

				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}
		public static NLogsSummary GetNLogsSummary()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = @"SELECT
								-- 1) Total API calls
								(SELECT COUNT(*)
								 FROM __ERP_Nlog_Exceptions 
								 WHERE Message LIKE '%|%' AND Level = 'Info') AS TotalApiCalls,

								-- 2) Most called API
								(SELECT Url FROM (
									SELECT 
										SUBSTR(
											Message,
											INSTR(Message, '|') + 1,
											INSTR(SUBSTR(Message, INSTR(Message, '|') + 1), '|') - 1
										) AS Url,
										COUNT(*) AS CallCount
									FROM __ERP_Nlog_Exceptions
									WHERE Message LIKE '%|%' AND Level = 'Info'
									GROUP BY Url
									ORDER BY CallCount DESC
									LIMIT 1
								)) AS MostCalledApi,

								-- 3) Last update date
								(SELECT MAX(Date) 
								 FROM __ERP_Nlog_Exceptions 
								 WHERE Message LIKE '%|%' AND Level = 'Info') AS LastUpdateDate;";
				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new NLogsSummary(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<ApisCallEntity> GetApiCalls()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = $@"select MemberName AS Api,SourceFilePath as Url,count(*) as CallCount,
								  SUM(Date >= datetime('now', '-3 months'))  AS calls_last_3_months,
								  SUM(Date >= datetime('now', '-6 months'))  AS calls_last_6_months,
								  SUM(Date >= datetime('now', '-12 months')) AS calls_last_12_months
                                from __ERP_Nlog_Exceptions
                                where IsCall=1
                                group by MemberName,SourceFilePath
                                ORDER BY MemberName desc ";

				var sqlCommand = new SQLiteCommand(query, sqlConnection);
				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ApisCallEntity(x)).ToList();
			}
			else
			{
				return new List<ApisCallEntity>();
			}
		}
		public static List<ApiCallsSixMonthsPriorEntity> GetApiCallsSixMonthPrior(string from, string to, string api)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = $@"select MemberName,SourceFilePath as Url,strftime('%m/%Y', date) AS MonthYear,count(*) as CallCount 
								from __ERP_Nlog_Exceptions
								where IsCall=1
								AND date(date) >= @from
								AND date(date) <  @to
								and MemberName=@api
								group by MonthYear
								order by date desc";

				var sqlCommand = new SQLiteCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue(parameterName: "from", from);
				sqlCommand.Parameters.AddWithValue(parameterName: "to", to);
				sqlCommand.Parameters.AddWithValue(parameterName: "api", api);
				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ApiCallsSixMonthsPriorEntity(x)).ToList();
			}
			else
			{
				return new List<ApiCallsSixMonthsPriorEntity>();
			}
		}
		public static (DateTime? firstDate, DateTime? lastDate) GetFristAndLastCall(string api)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = $@"select max(date) as lastCall,min(date) as firstCall from __ERP_Nlog_Exceptions where MemberName=@api and IsCall=1";
				var sqlCommand = new SQLiteCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue(parameterName: "api", api);
				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return (Convert.ToDateTime(dataTable.Rows[0][0].ToString()), Convert.ToDateTime(dataTable.Rows[0][0].ToString()));
			}
			else
			{
				return (null, null);
			}
		}
		public static MostCalledApiEntity GetMostCalledApi()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SQLiteConnection(Settings.ConnectionStringNlog))
			{
				sqlConnection.Open();
				string query = @"SELECT 
								t.MemberName,
								t._count AS MostCalledCount,
								total.TotalCount
							FROM (
								SELECT 
									MemberName,
									COUNT(*) AS _count
								FROM __ERP_Nlog_Exceptions
								WHERE IsCall = 1
								GROUP BY MemberName
								ORDER BY _count DESC
								LIMIT 1
							) t
							CROSS JOIN (
								SELECT COUNT(*) AS TotalCount
								FROM __ERP_Nlog_Exceptions
								WHERE IsCall = 1
							) total;";

				var sqlCommand = new SQLiteCommand(query, sqlConnection);

				new SQLiteDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return new MostCalledApiEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
			#endregion Custom Methods
		}
}