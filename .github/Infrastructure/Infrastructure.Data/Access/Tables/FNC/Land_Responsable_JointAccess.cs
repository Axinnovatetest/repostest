using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Land_Responsable_JointAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Land_Responsable_Joint] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Land_Responsable_Joint]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = "SELECT * FROM [Land_Responsable_Joint] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Land_Responsable_Joint] ([ID_Land],[ID_user])  VALUES (@ID_Land,@ID_user)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ID_Land", item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
					sqlCommand.Parameters.AddWithValue("ID_user", item.ID_user == null ? (object)DBNull.Value : item.ID_user);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [ID] FROM [Land_Responsable_Joint] WHERE [ID] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 3; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Land_Responsable_Joint] ([ID_Land],[ID_user]) VALUES ( "

							+ "@ID_Land" + i + ","
							+ "@ID_user" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ID_Land" + i, item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
						sqlCommand.Parameters.AddWithValue("ID_user" + i, item.ID_user == null ? (object)DBNull.Value : item.ID_user);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Land_Responsable_Joint] SET [ID_Land]=@ID_Land, [ID_user]=@ID_user WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("ID_Land", item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
				sqlCommand.Parameters.AddWithValue("ID_user", item.ID_user == null ? (object)DBNull.Value : item.ID_user);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 3; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Land_Responsable_Joint] SET "

							+ "[ID_Land]=@ID_Land" + i + ","
							+ "[ID_user]=@ID_user" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("ID_Land" + i, item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
						sqlCommand.Parameters.AddWithValue("ID_user" + i, item.ID_user == null ? (object)DBNull.Value : item.ID_user);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Land_Responsable_Joint] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [Land_Responsable_Joint] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity GetSiteResponsable(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				//string query = "SELECT * FROM [Land_Responsable_Joint] WHERE [ID_Land]=@Id";
				string query = "SELECT resp.ID,resp.ID_Land,resp.ID_user,user0.Name,user0.Username FROM [Land_Responsable_Joint] as resp Left join [dbo].[User] as user0 on user0.[Id]= resp.[ID_user] WHERE[ID_Land]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Land_Responsable_JointEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
