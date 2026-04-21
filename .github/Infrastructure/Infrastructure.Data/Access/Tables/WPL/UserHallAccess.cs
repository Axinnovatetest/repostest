using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class UserHallAccess
	{
		#region Default Methods
		public static Data.Entities.Tables.WPL.UserHallEntity Get(int id)
		{
			SqlDataAdapter SelectAdapter = new SqlDataAdapter();
			DataTable dt = new DataTable();
			using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM User_Hall WHERE Id=@Id";
				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				SelectAdapter = new SqlDataAdapter(sqlCommand);
				SelectAdapter.Fill(dt);

			}

			if(dt.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.UserHallEntity(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Data.Entities.Tables.WPL.UserHallEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM User_Hall";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
			}
		}
		public static List<Data.Entities.Tables.WPL.UserHallEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQuerySize = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.WPL.UserHallEntity>();
				if(ids.Count <= maxQuerySize)
				{
					result = get(ids);
				}
				else
				{
					int batchSize = ids.Count / maxQuerySize;
					result = new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
					for(int i = 0; i < batchSize; i++)
					{
						result.AddRange(get(ids.GetRange(i * maxQuerySize, maxQuerySize)));
					}
					result.AddRange(get(ids.GetRange(batchSize * maxQuerySize, ids.Count - batchSize * maxQuerySize)));
				}
				return result;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
		}
		private static List<Data.Entities.Tables.WPL.UserHallEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM User_Hall WHERE Id IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
		}

		public static int Insert(Data.Entities.Tables.WPL.UserHallEntity element)
		{
			int response = -1;
			using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "INSERT INTO User_Hall(Hall_Id,User_Id)  VALUES (@Hall_Id,@User_Id);";
				query += "SELECT SCOPE_IDENTITY();";

				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Hall_Id", element.HallId);
				sqlCommand.Parameters.AddWithValue("User_Id", element.UserId);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int Insert(List<Data.Entities.Tables.WPL.UserHallEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = insert(elements);
				}
				else
				{
					int batchSize = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchSize; i++)
					{
						result += insert(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += insert(elements.GetRange(batchSize * maxParamsNumber, elements.Count - batchSize * maxParamsNumber));
				}
				return result;
			}

			return -1;
		}
		private static int insert(List<Data.Entities.Tables.WPL.UserHallEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int r = -1;
				using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(Infrastructure.Data.Entities.Tables.WPL.UserHallEntity t in elements)
					{
						i++;
						query += " INSERT INTO User_Hall(Hall_Id,User_Id) VALUES( "

							+ "@Hall_Id" + i + ","
							+ "@User_Id" + i
							 + "); ";


						sqlCommand.Parameters.AddWithValue("Hall_Id" + i, t.HallId);
						sqlCommand.Parameters.AddWithValue("User_Id" + i, t.UserId);
					}

					sqlCommand.CommandText = query;

					r = sqlCommand.ExecuteNonQuery();
				}

				return r;
			}

			return -1;
		}

		public static int Update(Data.Entities.Tables.WPL.UserHallEntity elements)
		{
			int r = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE User_Hall SET Hall_Id=@Hall_Id,User_Id=@User_Id WHERE Id=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", elements.Id);
				sqlCommand.Parameters.AddWithValue("Hall_Id", elements.HallId);
				sqlCommand.Parameters.AddWithValue("User_Id", elements.UserId);

				r = sqlCommand.ExecuteNonQuery();
			}

			return r;
		}
		public static int Update(List<Data.Entities.Tables.WPL.UserHallEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = update(elements);
				}
				else
				{
					int batchSize = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchSize; i++)
					{
						result += update(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += update(elements.GetRange(batchSize * maxParamsNumber, elements.Count - batchSize * maxParamsNumber));
				}
				return result;
			}

			return -1;
		}
		private static int update(List<Data.Entities.Tables.WPL.UserHallEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int r = -1;
				using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(Infrastructure.Data.Entities.Tables.WPL.UserHallEntity t in elements)
					{
						i++;
						query += " UPDATE User_Hall SET "

							+ "Hall_Id=@Hall_Id" + i + ","
							+ "User_Id=@User_Id" + i + " WHERE Id=@Id" + i
							 + "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, t.Id);
						sqlCommand.Parameters.AddWithValue("Hall_Id" + i, t.HallId);
						sqlCommand.Parameters.AddWithValue("User_Id" + i, t.UserId);
					}

					sqlCommand.CommandText = query;

					r = sqlCommand.ExecuteNonQuery();
				}

				return r;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int r = -1;
			using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM User_Hall WHERE Id=@Id";
				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				r = sqlCommand.ExecuteNonQuery();
			}

			return r;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				int result = 0;
				if(ids.Count <= maxParamsNumber)
				{
					result = delete(ids);
				}
				else
				{
					int batchSize = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchSize; i++)
					{
						result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += delete(ids.GetRange(batchSize * maxParamsNumber, ids.Count - batchSize * maxParamsNumber));
				}
				return result;
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int r = -1;
				using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM User_Hall WHERE Id IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					r = sqlCommand.ExecuteNonQuery();
				}

				return r;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Data.Entities.Tables.WPL.UserHallEntity> GetByUserId(int userId)
		{
			SqlDataAdapter SelectAdapter = new SqlDataAdapter();
			DataTable dt = new DataTable();
			using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM User_Hall Where User_Id=@UserId";
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
				return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
			}
		}

		public static List<Data.Entities.Tables.WPL.UserHallEntity> GetByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				int maxQuerySize = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.WPL.UserHallEntity>();
				if(usersIds.Count <= maxQuerySize)
				{
					result = getByUsersIds(usersIds);
				}
				else
				{
					int batchSize = usersIds.Count / maxQuerySize;
					result = new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
					for(int i = 0; i < batchSize; i++)
					{
						result.AddRange(getByUsersIds(usersIds.GetRange(i * maxQuerySize, maxQuerySize)));
					}
					result.AddRange(getByUsersIds(usersIds.GetRange(batchSize * maxQuerySize, usersIds.Count - batchSize * maxQuerySize)));
				}
				return result;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
		}
		private static List<Data.Entities.Tables.WPL.UserHallEntity> getByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM User_Hall WHERE User_Id IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
		}

		public static List<Data.Entities.Tables.WPL.UserHallEntity> GetByHallId(int hallId)
		{
			SqlDataAdapter SelectAdapter = new SqlDataAdapter();
			DataTable dt = new DataTable();
			using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM User_Hall Where Hall_Id=@HallId";
				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("HallId", hallId);

				SelectAdapter = new SqlDataAdapter(sqlCommand);
				SelectAdapter.Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.UserHallEntity>();
			}
		}

		public static int DeleteByUserId(int userId)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM User_Hall WHERE User_Id=@UserId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("UserId", userId);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		#endregion

		#region Helpers
		public static List<Data.Entities.Tables.WPL.UserHallEntity> toList(DataTable dataTable)
		{
			var list = new List<Data.Entities.Tables.WPL.UserHallEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				list.Add(new Data.Entities.Tables.WPL.UserHallEntity(dataRow));
			}
			return list;
		}
		#endregion
	}
}
