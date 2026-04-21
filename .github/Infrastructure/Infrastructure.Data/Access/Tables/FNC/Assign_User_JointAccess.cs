using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Assign_User_JointAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_User_Joint] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_User_Joint]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Assign_User_Joint] WHERE [ID] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Assign_User_Joint] ([ID_AssignUser],[ID_user])  VALUES (@ID_AssignUser,@ID_user)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ID_AssignUser", item.ID_AssignUser == null ? (object)DBNull.Value : item.ID_AssignUser);
					sqlCommand.Parameters.AddWithValue("ID_user", item.ID_user == null ? (object)DBNull.Value : item.ID_user);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [ID] FROM [Assign_User_Joint] WHERE [ID] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> items)
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
						query += " INSERT INTO [Assign_User_Joint] ([ID_AssignUser],[ID_user]) VALUES ( "

							+ "@ID_AssignUser" + i + ","
							+ "@ID_user" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ID_AssignUser" + i, item.ID_AssignUser == null ? (object)DBNull.Value : item.ID_AssignUser);
						sqlCommand.Parameters.AddWithValue("ID_user" + i, item.ID_user == null ? (object)DBNull.Value : item.ID_user);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Assign_User_Joint] SET [ID_AssignUser]=@ID_AssignUser, [ID_user]=@ID_user WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("ID_AssignUser", item.ID_AssignUser == null ? (object)DBNull.Value : item.ID_AssignUser);
				sqlCommand.Parameters.AddWithValue("ID_user", item.ID_user == null ? (object)DBNull.Value : item.ID_user);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> items)
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
						query += " UPDATE [Assign_User_Joint] SET "

							+ "[ID_AssignUser]=@ID_AssignUser" + i + ","
							+ "[ID_user]=@ID_user" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("ID_AssignUser" + i, item.ID_AssignUser == null ? (object)DBNull.Value : item.ID_AssignUser);
						sqlCommand.Parameters.AddWithValue("ID_user" + i, item.ID_user == null ? (object)DBNull.Value : item.ID_user);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Assign_User_Joint] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [Assign_User_Joint] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods


		public static List<Data.Entities.Tables.FNC.Assign_User_JointEntity> GetByUserId(int userId)
		{
			SqlDataAdapter SelectAdapter = new SqlDataAdapter();
			DataTable dt = new DataTable();
			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_User_Joint] Where ID_user=@UserId";
				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("UserId", userId);

				SelectAdapter = new SqlDataAdapter(sqlCommand);
				SelectAdapter.Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Data.Entities.Tables.FNC.Assign_User_JointEntity>();
			}
		}



		public static List<Data.Entities.Tables.FNC.Assign_User_JointEntity> GetByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				int maxQuerySize = Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.FNC.Assign_User_JointEntity>();
				if(usersIds.Count <= maxQuerySize)
				{
					result = getByUsersIds(usersIds);
				}
				else
				{
					int batchSize = usersIds.Count / maxQuerySize;
					result = new List<Data.Entities.Tables.FNC.Assign_User_JointEntity>();
					for(int i = 0; i < batchSize; i++)
					{
						result.AddRange(getByUsersIds(usersIds.GetRange(i * maxQuerySize, maxQuerySize)));
					}
					result.AddRange(getByUsersIds(usersIds.GetRange(batchSize * maxQuerySize, usersIds.Count - batchSize * maxQuerySize)));
				}
				return result;
			}
			return new List<Data.Entities.Tables.FNC.Assign_User_JointEntity>();
		}
		private static List<Data.Entities.Tables.FNC.Assign_User_JointEntity> getByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < usersIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, usersIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Assign_User_Joint] WHERE ID_user IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>();
		}

		public static List<Data.Entities.Tables.FNC.Assign_User_JointEntity> GetByUserAssignId(int UserAssignId)
		{
			SqlDataAdapter SelectAdapter = new SqlDataAdapter();
			DataTable dt = new DataTable();
			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_User_Joint] WHERE ID_AssignUser=@UserAssignId";
				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("UserAssignId", UserAssignId);

				SelectAdapter = new SqlDataAdapter(sqlCommand);
				SelectAdapter.Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>();
			}
		}

		public static int DeleteByUserId(int userId)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [Assign_User_Joint] WHERE ID_user=@UserId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("UserId", userId);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
