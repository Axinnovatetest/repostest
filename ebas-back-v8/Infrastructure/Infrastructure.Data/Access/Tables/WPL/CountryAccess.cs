using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class CountryAccess
	{
		public static Boolean GetArchived { get; private set; } = false;
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.CountryEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Countries WHERE Id=@Id AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.CountryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Countries WHERE Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

			sqlCommand.CommandText = "SELECT * FROM Countries WHERE Id IN (" + queryIds + ") AND Is_Archived=@IsArchived";

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
			}
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.CountryEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Countries] ([Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Designation],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[MtdArticleSequence],[Name]) OUTPUT INSERTED.[Id] VALUES (@Creation_Date,@Creation_User_Id,@Delete_Date,@Delete_User_Id,@Designation,@Is_Archived,@Last_Edit_Date,@Last_Edit_User_Id,@MtdArticleSequence,@Name); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Creation_Date", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Delete_Date", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
					sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("MtdArticleSequence", item.MtdArticleSequence == null ? (object)DBNull.Value : item.MtdArticleSequence);
					sqlCommand.Parameters.AddWithValue("Name", item.Name);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Countries] ([Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Designation],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[MtdArticleSequence],[Name]) VALUES ( "

							+ "@Creation_Date" + i + ","
							+ "@Creation_User_Id" + i + ","
							+ "@Delete_Date" + i + ","
							+ "@Delete_User_Id" + i + ","
							+ "@Designation" + i + ","
							+ "@Is_Archived" + i + ","
							+ "@Last_Edit_Date" + i + ","
							+ "@Last_Edit_User_Id" + i + ","
							+ "@MtdArticleSequence" + i + ","
							+ "@Name" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("MtdArticleSequence" + i, item.MtdArticleSequence == null ? (object)DBNull.Value : item.MtdArticleSequence);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.WPL.CountryEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Countries] SET [Creation_Date]=@Creation_Date, [Creation_User_Id]=@Creation_User_Id, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [Designation]=@Designation, [Is_Archived]=@Is_Archived, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [MtdArticleSequence]=@MtdArticleSequence, [Name]=@Name WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Creation_Date", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Delete_Date", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
				sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
				sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("MtdArticleSequence", item.MtdArticleSequence == null ? (object)DBNull.Value : item.MtdArticleSequence);
				sqlCommand.Parameters.AddWithValue("Name", item.Name);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Countries] SET "

							+ "[Creation_Date]=@Creation_Date" + i + ","
							+ "[Creation_User_Id]=@Creation_User_Id" + i + ","
							+ "[Delete_Date]=@Delete_Date" + i + ","
							+ "[Delete_User_Id]=@Delete_User_Id" + i + ","
							+ "[Designation]=@Designation" + i + ","
							+ "[Is_Archived]=@Is_Archived" + i + ","
							+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
							+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
							+ "[MtdArticleSequence]=@MtdArticleSequence" + i + ","
							+ "[Name]=@Name" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("MtdArticleSequence" + i, item.MtdArticleSequence == null ? (object)DBNull.Value : item.MtdArticleSequence);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM Countries WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConection.Close();

			return response;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

				if(ids.Count <= maxParamsNumber)
				{
					return delete(ids);
				}
				int result = 0;
				int batchNumber = ids.Count / maxParamsNumber;
				for(int i = 0; i < batchNumber; i++)
				{
					result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
				}
				result += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				return result;
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

				string query = "DELETE FROM Countries WHERE Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				int response = DbExecution.ExecuteNonQuery(sqlCommand);

				sqlConnection.Close();

				return response;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.WPL.CountryEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Countries] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.CountryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Countries]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.CountryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [Countries] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.CountryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.WPL.CountryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Countries] ([Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Designation],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[MtdArticleSequence],[Name]) OUTPUT INSERTED.[Id] VALUES (@Creation_Date,@Creation_User_Id,@Delete_Date,@Delete_User_Id,@Designation,@Is_Archived,@Last_Edit_Date,@Last_Edit_User_Id,@MtdArticleSequence,@Name); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Creation_Date", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Delete_Date", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
			sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("MtdArticleSequence", item.MtdArticleSequence == null ? (object)DBNull.Value : item.MtdArticleSequence);
			sqlCommand.Parameters.AddWithValue("Name", item.Name);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Countries] ([Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Designation],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[MtdArticleSequence],[Name]) VALUES ( "

						+ "@Creation_Date" + i + ","
						+ "@Creation_User_Id" + i + ","
						+ "@Delete_Date" + i + ","
						+ "@Delete_User_Id" + i + ","
						+ "@Designation" + i + ","
						+ "@Is_Archived" + i + ","
						+ "@Last_Edit_Date" + i + ","
						+ "@Last_Edit_User_Id" + i + ","
						+ "@MtdArticleSequence" + i + ","
						+ "@Name" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation);
					sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("MtdArticleSequence" + i, item.MtdArticleSequence == null ? (object)DBNull.Value : item.MtdArticleSequence);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.WPL.CountryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Countries] SET [Creation_Date]=@Creation_Date, [Creation_User_Id]=@Creation_User_Id, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [Designation]=@Designation, [Is_Archived]=@Is_Archived, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [MtdArticleSequence]=@MtdArticleSequence, [Name]=@Name WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Creation_Date", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Delete_Date", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("Designation", item.Designation);
			sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("MtdArticleSequence", item.MtdArticleSequence == null ? (object)DBNull.Value : item.MtdArticleSequence);
			sqlCommand.Parameters.AddWithValue("Name", item.Name);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Countries] SET "

					+ "[Creation_Date]=@Creation_Date" + i + ","
					+ "[Creation_User_Id]=@Creation_User_Id" + i + ","
					+ "[Delete_Date]=@Delete_Date" + i + ","
					+ "[Delete_User_Id]=@Delete_User_Id" + i + ","
					+ "[Designation]=@Designation" + i + ","
					+ "[Is_Archived]=@Is_Archived" + i + ","
					+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
					+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
					+ "[MtdArticleSequence]=@MtdArticleSequence" + i + ","
					+ "[Name]=@Name" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation);
					sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("MtdArticleSequence" + i, item.MtdArticleSequence == null ? (object)DBNull.Value : item.MtdArticleSequence);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "DELETE FROM [Countries] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [Countries] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.WPL.CountryEntity GetByName(string name)
		{
			if(string.IsNullOrEmpty(name))
			{
				return null;
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Countries WHERE Name=@Name AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Name", name);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.CountryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.WPL.CountryEntity GetByCode(string code)
		{
			code = code ?? "";
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Countries WHERE Designation=@code AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("code", code);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.CountryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.CountryEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
