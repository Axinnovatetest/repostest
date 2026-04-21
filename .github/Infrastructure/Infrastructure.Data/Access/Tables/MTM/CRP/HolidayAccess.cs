using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class HolidayAccess
	{
		#region Default Methods
		public static Entities.Tables.MTM.HolidayEntity Get(int id,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_Holiday] WHERE [Id]=@Id "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.HolidayEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.MTM.HolidayEntity> Get(bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_Holiday] "
					+ (isArchived.HasValue ? $" WHERE [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.HolidayEntity>();
			}
		}
		public static List<Entities.Tables.MTM.HolidayEntity> Get(List<int> ids,
			bool? isArchived = false)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.MTM.HolidayEntity> results = new List<Entities.Tables.MTM.HolidayEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids, isArchived);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.MTM.HolidayEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), isArchived));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), isArchived));
				}
				return results;
			}
			return new List<Entities.Tables.MTM.HolidayEntity>();
		}
		private static List<Entities.Tables.MTM.HolidayEntity> get(List<int> ids,
			bool? isArchived)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [__MTM_CRP_Holiday] WHERE [Id] IN (" + queryIds + ") "
						+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.MTM.HolidayEntity>();
				}
			}
			return new List<Entities.Tables.MTM.HolidayEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.HolidayEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_Holiday] ([ArchiveTime],[ArchiveUserId],[CountryId],[CountryName],[CreationTime],[CreationUserId],[Day],[HallId],[HallName],[IsArchived],[IsOverwritten],[Name],[WeekNumber])  VALUES (@ArchiveTime,@ArchiveUserId,@CountryId,@CountryName,@CreationTime,@CreationUserId,@Day,@HallId,@HallName,@IsArchived,@IsOverwritten,@Name,@WeekNumber); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
					sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Day", item.Day);
					sqlCommand.Parameters.AddWithValue("HallId", item.HallId);
					sqlCommand.Parameters.AddWithValue("HallName", item.HallName);
					sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
					sqlCommand.Parameters.AddWithValue("IsOverwritten", item.IsOverwritten == null ? (object)DBNull.Value : item.IsOverwritten);
					sqlCommand.Parameters.AddWithValue("Name", item.Name);
					sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber == null ? (object)DBNull.Value : item.WeekNumber);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.HolidayEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.HolidayEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__MTM_CRP_Holiday] ([ArchiveTime],[ArchiveUserId],[CountryId],[CountryName],[CreationTime],[CreationUserId],[Day],[HallId],[HallName],[IsArchived],[IsOverwritten],[Name],[WeekNumber]) VALUES ( "

							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@CountryId" + i + ","
							+ "@CountryName" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@Day" + i + ","
							+ "@HallId" + i + ","
							+ "@HallName" + i + ","
							+ "@IsArchived" + i + ","
							+ "@IsOverwritten" + i + ","
							+ "@Name" + i + ","
							+ "@WeekNumber" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Day" + i, item.Day);
						sqlCommand.Parameters.AddWithValue("HallId" + i, item.HallId);
						sqlCommand.Parameters.AddWithValue("HallName" + i, item.HallName);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("IsOverwritten" + i, item.IsOverwritten == null ? (object)DBNull.Value : item.IsOverwritten);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
						sqlCommand.Parameters.AddWithValue("WeekNumber" + i, item.WeekNumber == null ? (object)DBNull.Value : item.WeekNumber);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.HolidayEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_Holiday] SET [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [CountryId]=@CountryId, [CountryName]=@CountryName, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [Day]=@Day, [HallId]=@HallId, [HallName]=@HallName, [IsArchived]=@IsArchived, [IsOverwritten]=@IsOverwritten, [Name]=@Name, [WeekNumber]=@WeekNumber WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Day", item.Day);
				sqlCommand.Parameters.AddWithValue("HallId", item.HallId);
				sqlCommand.Parameters.AddWithValue("HallName", item.HallName);
				sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
				sqlCommand.Parameters.AddWithValue("IsOverwritten", item.IsOverwritten == null ? (object)DBNull.Value : item.IsOverwritten);
				sqlCommand.Parameters.AddWithValue("Name", item.Name);
				sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber == null ? (object)DBNull.Value : item.WeekNumber);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.HolidayEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.HolidayEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__MTM_CRP_Holiday] SET "

							+ "[ArchiveTime]=@ArchiveTime" + i + ","
							+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
							+ "[CountryId]=@CountryId" + i + ","
							+ "[CountryName]=@CountryName" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[Day]=@Day" + i + ","
							+ "[HallId]=@HallId" + i + ","
							+ "[HallName]=@HallName" + i + ","
							+ "[IsArchived]=@IsArchived" + i + ","
							+ "[IsOverwritten]=@IsOverwritten" + i + ","
							+ "[Name]=@Name" + i + ","
							+ "[WeekNumber]=@WeekNumber" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Day" + i, item.Day);
						sqlCommand.Parameters.AddWithValue("HallId" + i, item.HallId);
						sqlCommand.Parameters.AddWithValue("HallName" + i, item.HallName);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("IsOverwritten" + i, item.IsOverwritten == null ? (object)DBNull.Value : item.IsOverwritten);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
						sqlCommand.Parameters.AddWithValue("WeekNumber" + i, item.WeekNumber == null ? (object)DBNull.Value : item.WeekNumber);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_Holiday] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [__MTM_CRP_Holiday] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Entities.Tables.MTM.HolidayEntity> GetBy_CountryId_HallId_DayRange(int? countryId,
			int? hallId,
			DateTime? dayFrom,
			DateTime? dayUntil,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_Holiday] ";

				using(var sqlCommand = new SqlCommand())
				{
					bool isFirstCondition = true;

					if(countryId.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [CountryId]=@countryId ";
						sqlCommand.Parameters.AddWithValue("countryId", countryId.Value);
						isFirstCondition = false;
					}

					if(hallId.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [HallId]=@hallId ";
						sqlCommand.Parameters.AddWithValue("hallId", hallId.Value);
						isFirstCondition = false;
					}

					if(isArchived.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [IsArchived]=@isArchived ";
						sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
						isFirstCondition = false;
					}

					if(dayFrom.HasValue && dayUntil.HasValue)
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [Day] >= @dayFrom AND [Day] <= @dayUntil ";
						sqlCommand.Parameters.AddWithValue("dayFrom", dayFrom.Value);
						sqlCommand.Parameters.AddWithValue("dayUntil", dayUntil.Value);
						isFirstCondition = false;
					}

					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY CreationTime DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Entities.Tables.MTM.HolidayEntity>();
			}

			return toList(dataTable);
		}

		public static int SetOverwritten(List<int> ids, bool overwritten = true)
		{
			if(ids == null && ids.Count <= 0)
				return -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = $"UPDATE [__MTM_CRP_Holiday] SET [IsOverwritten]=@IsOverwritten WHERE [Id] IN ({string.Join(",", ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("IsOverwritten", overwritten);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static List<Entities.Tables.MTM.HolidayEntity> GetByCountryHall(int countryId,
			int? hallId,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_Holiday] WHERE CountryId=@countryId"
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : string.Empty)
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.HolidayEntity>();
			}
		}
		public static List<Entities.Tables.MTM.HolidayEntity> GetSimilar(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT A.* FROM [__MTM_CRP_Holiday] A Join [__MTM_CRP_Holiday] B ON B.Id<>A.Id AND B.CountryId=A.CountryId AND B.HallId<>A.HallId AND B.Day=A.Day AND B.Name=A.Name WHERE B.Id=@id AND A.[IsArchived]=0";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.HolidayEntity>();
			}
		}
		public static int UpdateWSimilar(Infrastructure.Data.Entities.Tables.MTM.HolidayEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = $"UPDATE [__MTM_CRP_Holiday] SET [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [CountryId]=@CountryId, [CountryName]=@CountryName, [Day]=@Day, [IsArchived]=@IsArchived, [IsOverwritten]=@IsOverwritten, [Name]=@Name, [WeekNumber]=@WeekNumber " +
					$"WHERE  [Id]=@id OR [Id] IN (SELECT A.Id FROM [__MTM_CRP_Holiday] A Join [__MTM_CRP_Holiday] B ON /*B.Id<>A.Id AND */B.CountryId=A.CountryId AND B.Day=A.Day AND B.HallId<>A.HallId AND B.Name=A.Name WHERE B.Id=@id AND A.[IsArchived]=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Day", item.Day);
				sqlCommand.Parameters.AddWithValue("HallId", item.HallId);
				sqlCommand.Parameters.AddWithValue("HallName", item.HallName);
				sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
				sqlCommand.Parameters.AddWithValue("IsOverwritten", item.IsOverwritten == null ? (object)DBNull.Value : item.IsOverwritten);
				sqlCommand.Parameters.AddWithValue("Name", item.Name);
				sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber == null ? (object)DBNull.Value : item.WeekNumber);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static List<Entities.Tables.MTM.HolidayEntity> Get2PreviousYear(bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = $"SELECT DISTINCT [Day], [Name], [CountryId], [CountryName], [WeekNumber] " +
					$",MAX([Id]) [Id] " +
					$",MAX([HallId]) [HallId] " +
					$",MAX([HallName]) [HallName] " +
					$",MAX([CreationUserId]) [CreationUserId] " +
					$",MAX([CreationTime]) [CreationTime] " +
					$",MAX(CAST([IsArchived] AS INT)) [IsArchived] " +
					$",MAX([ArchiveUserId]) [ArchiveUserId] " +
					$",MAX([ArchiveTime]) [ArchiveTime] " +
					$",MAX(CAST([IsOverwritten] AS INT)) [IsOverwritten] " +
					$"FROM[__MTM_CRP_Holiday] " +
					$"WHERE Year([Day]) >= Year(GetDate()) - 1 " +
					(isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty) +
					$"GROUP BY [Day], [Name], [CountryId], [CountryName], [WeekNumber]";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.HolidayEntity>();
			}
		}
		public static List<Entities.Tables.MTM.HolidayEntity> GetPrevious(int year, bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_Holiday] WHERE Year([Day]) >= @year "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.HolidayEntity>();
			}
		}
		#endregion

		#region Helpers
		private static List<Entities.Tables.MTM.HolidayEntity> toList(DataTable dataTable)
		{
			var list = new List<Entities.Tables.MTM.HolidayEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Entities.Tables.MTM.HolidayEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
