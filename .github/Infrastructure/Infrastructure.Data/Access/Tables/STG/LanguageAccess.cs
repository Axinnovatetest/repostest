using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.STG
{

	public class LanguageAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.STG.LanguageEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Language] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.LanguageEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Language]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [__STG_Language] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.STG.LanguageEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__STG_Language] ([Code],[CreationDate],[CreationUserId],[DeleteDate],[DeleteUserID],[Description],[IsArchived],[LastEditDate],[LastUserId],[Name])  VALUES (@Code,@CreationDate,@CreationUserId,@DeleteDate,@DeleteUserID,@Description,@IsArchived,@LastEditDate,@LastUserId,@Name);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Code", item.Code);
					sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("DeleteDate", item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
					sqlCommand.Parameters.AddWithValue("DeleteUserID", item.DeleteUserID == null ? (object)DBNull.Value : item.DeleteUserID);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
					sqlCommand.Parameters.AddWithValue("LastEditDate", item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
					sqlCommand.Parameters.AddWithValue("LastUserId", item.LastUserId == null ? (object)DBNull.Value : item.LastUserId);
					sqlCommand.Parameters.AddWithValue("Name", item.Name);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__STG_Language] ([Code],[CreationDate],[CreationUserId],[DeleteDate],[DeleteUserID],[Description],[IsArchived],[LastEditDate],[LastUserId],[Name]) VALUES ( "

							+ "@Code" + i + ","
							+ "@CreationDate" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@DeleteDate" + i + ","
							+ "@DeleteUserID" + i + ","
							+ "@Description" + i + ","
							+ "@IsArchived" + i + ","
							+ "@LastEditDate" + i + ","
							+ "@LastUserId" + i + ","
							+ "@Name" + i
								+ "); ";


						sqlCommand.Parameters.AddWithValue("Code" + i, item.Code);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("DeleteDate" + i, item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
						sqlCommand.Parameters.AddWithValue("DeleteUserID" + i, item.DeleteUserID == null ? (object)DBNull.Value : item.DeleteUserID);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastEditDate" + i, item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
						sqlCommand.Parameters.AddWithValue("LastUserId" + i, item.LastUserId == null ? (object)DBNull.Value : item.LastUserId);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.STG.LanguageEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__STG_Language] SET [Code]=@Code, [CreationDate]=@CreationDate, [CreationUserId]=@CreationUserId, [DeleteDate]=@DeleteDate, [DeleteUserID]=@DeleteUserID, [Description]=@Description, [IsArchived]=@IsArchived, [LastEditDate]=@LastEditDate, [LastUserId]=@LastUserId, [Name]=@Name WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Code", item.Code);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("DeleteDate", item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
				sqlCommand.Parameters.AddWithValue("DeleteUserID", item.DeleteUserID == null ? (object)DBNull.Value : item.DeleteUserID);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
				sqlCommand.Parameters.AddWithValue("LastEditDate", item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
				sqlCommand.Parameters.AddWithValue("LastUserId", item.LastUserId == null ? (object)DBNull.Value : item.LastUserId);
				sqlCommand.Parameters.AddWithValue("Name", item.Name);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__STG_Language] SET "

								+ "[Code]=@Code" + i + ","
								+ "[CreationDate]=@CreationDate" + i + ","
								+ "[CreationUserId]=@CreationUserId" + i + ","
								+ "[DeleteDate]=@DeleteDate" + i + ","
								+ "[DeleteUserID]=@DeleteUserID" + i + ","
								+ "[Description]=@Description" + i + ","
								+ "[IsArchived]=@IsArchived" + i + ","
								+ "[LastEditDate]=@LastEditDate" + i + ","
								+ "[LastUserId]=@LastUserId" + i + ","
								+ "[Name]=@Name" + i + " WHERE [Id]=@Id" + i
								+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Code" + i, item.Code);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("DeleteDate" + i, item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
						sqlCommand.Parameters.AddWithValue("DeleteUserID" + i, item.DeleteUserID == null ? (object)DBNull.Value : item.DeleteUserID);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastEditDate" + i, item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
						sqlCommand.Parameters.AddWithValue("LastUserId" + i, item.LastUserId == null ? (object)DBNull.Value : item.LastUserId);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
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
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__STG_Language] WHERE [Id]=@Id";
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
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [__STG_Language] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods


		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.STG.LanguageEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
