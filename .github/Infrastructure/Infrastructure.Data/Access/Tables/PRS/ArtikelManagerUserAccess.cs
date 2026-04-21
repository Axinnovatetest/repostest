using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class ArtikelManagerUserAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ArtikelManagerUser] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ArtikelManagerUser]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [__PRS_ArtikelManagerUser] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__PRS_ArtikelManagerUser] ([ArtikelNr],[Artikelnummer],[UserFullName],[UserId],[UserName])  VALUES (@ArtikelNr,@Artikelnummer,@UserFullName,@UserId,@UserName); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("UserFullName", item.UserFullName == null ? (object)DBNull.Value : item.UserFullName);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> items)
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
						query += " INSERT INTO [__PRS_ArtikelManagerUser] ([ArtikelNr],[Artikelnummer],[UserFullName],[UserId],[UserName]) VALUES ( "

							+ "@ArtikelNr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@UserFullName" + i + ","
							+ "@UserId" + i + ","
							+ "@UserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("UserFullName" + i, item.UserFullName == null ? (object)DBNull.Value : item.UserFullName);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_ArtikelManagerUser] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [UserFullName]=@UserFullName, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("UserFullName", item.UserFullName == null ? (object)DBNull.Value : item.UserFullName);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> items)
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
						query += " UPDATE [__PRS_ArtikelManagerUser] SET "

							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[UserFullName]=@UserFullName" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("UserFullName" + i, item.UserFullName == null ? (object)DBNull.Value : item.UserFullName);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
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
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__PRS_ArtikelManagerUser] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
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
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					string query = "DELETE FROM [__PRS_ArtikelManagerUser] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int UpdateByArticleNr(Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_ArtikelManagerUser] SET [UserFullName]=@UserFullName, [UserId]=@UserId, [UserName]=@UserName WHERE [ArtikelNr]=@ArtikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("UserFullName", item.UserFullName == null ? (object)DBNull.Value : item.UserFullName);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity GetByArtikelNr(int nr)
		{
			string tmp = string.Empty;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ArtikelManagerUser] WHERE [ArtikelNr]=@nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity GetByArtikelNr(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__PRS_ArtikelManagerUser] WHERE [ArtikelNr]=@nr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("nr", nr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity GetSingleByArtikelNr(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ArtikelManagerUser] WHERE [ArtikelNr]=@nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity GetSingleByArtikelNr(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
				string query = "SELECT * FROM [__PRS_ArtikelManagerUser] WHERE [ArtikelNr]=@nr";
			using(var sqlCommand = new SqlCommand(query, connection,transaction))
			{
				sqlCommand.Parameters.AddWithValue("nr", nr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> GetHasArtikels(int userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ArtikelManagerUser] WHERE [UserId]=@id AND [ArtikelNr] is not NULL AND [ArtikelNr] > 0";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", userId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> GetByUserId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ArtikelManagerUser] WHERE [UserId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelManagerUserEntity> GetLikeName(string searchText)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = " SELECT [UserId], [UserName], [UserFullName], -1 [Id],-1 [ArtikelNr],'' [Artikelnummer] FROM [__PRS_ArtikelManagerUser] WHERE [UserFullName] LIKE @searchText OR [UserName] LIKE @searchText GROUP BY [UserId], [UserName], [UserFullName]";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.PRS.ArtikelManagerUserEntity>();
			}
		}
		public static List<Entities.Tables.PRS.ArtikelManagerUserEntity> GetUniqueUsers()
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = " SELECT -1 [Id], -1 [ArtikelNr], [UserId], [UserName], [UserFullName],'' [Artikelnummer] FROM [__PRS_ArtikelManagerUser] GROUP BY [UserId], [UserName], [UserFullName]";
			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.PRS.ArtikelManagerUserEntity>();
			}
		}

		public static int DeleteByUser(int userId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__PRS_ArtikelManagerUser] WHERE [UserId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", userId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
