using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class HallAuthorizationAccess
	{
		#region Default Methods
		public static Data.Entities.Tables.WPL.HallAuthorizationEntity Get(int id)
		{
			var dt = new DataTable();

			using(SqlConnection sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM User_Hall WHERE Id=@Id";

				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Data.Entities.Tables.WPL.HallAuthorizationEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM User_Hall";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
			}
		}
		public static List<Data.Entities.Tables.WPL.HallAuthorizationEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQuerySize = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.WPL.HallAuthorizationEntity>();
				if(ids.Count <= maxQuerySize)
				{
					result = get(ids);
				}
				else
				{
					int batchSize = ids.Count / maxQuerySize;
					result = new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
					for(int i = 0; i < batchSize; i++)
					{
						result.AddRange(get(ids.GetRange(i * maxQuerySize, maxQuerySize)));
					}
					result.AddRange(get(ids.GetRange(batchSize * maxQuerySize, ids.Count - batchSize * maxQuerySize)));
				}
				return result;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
		}
		private static List<Data.Entities.Tables.WPL.HallAuthorizationEntity> get(List<int> ids)
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
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
		}

		public static int Insert(Data.Entities.Tables.WPL.HallAuthorizationEntity element)
		{
			int InsertedID = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "INSERT INTO User_Hall(Hall_Id,User_Id)  VALUES (@Hall_Id,@User_Id)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Hall_Id", element.HallId);
				sqlCommand.Parameters.AddWithValue("User_Id", element.UserId);

				InsertedID = DbExecution.ExecuteNonQuery(sqlCommand);

				if(InsertedID > 0)
				{
					query = "SELECT Id FROM User_Hall WHERE Id = @@IDENTITY";
					sqlCommand = new SqlCommand(query, sqlConnection);
					object _InsertedID = DbExecution.ExecuteScalar(sqlCommand);

					if(_InsertedID != null)
					{
						InsertedID = Convert.ToInt32(_InsertedID.ToString());
					}
					else
					{
						InsertedID = -1;
					}
				}
				else
				{
					InsertedID = -1;
				}
			}

			return InsertedID;
		}

		public static int Update(Data.Entities.Tables.WPL.HallAuthorizationEntity elements)
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

				r = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return r;
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

				r = DbExecution.ExecuteNonQuery(sqlCommand);
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

					r = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return r;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Data.Entities.Tables.WPL.HallAuthorizationEntity> GetByUserId(int userId)
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
				return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
			}
		}

		public static List<Data.Entities.Tables.WPL.HallAuthorizationEntity> GetByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				int maxQuerySize = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.WPL.HallAuthorizationEntity>();
				if(usersIds.Count <= maxQuerySize)
				{
					result = getByUsersIds(usersIds);
				}
				else
				{
					int batchSize = usersIds.Count / maxQuerySize;
					result = new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
					for(int i = 0; i < batchSize; i++)
					{
						result.AddRange(getByUsersIds(usersIds.GetRange(i * maxQuerySize, maxQuerySize)));
					}
					result.AddRange(getByUsersIds(usersIds.GetRange(batchSize * maxQuerySize, usersIds.Count - batchSize * maxQuerySize)));
				}
				return result;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
		}
		private static List<Data.Entities.Tables.WPL.HallAuthorizationEntity> getByUsersIds(List<int> usersIds)
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
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
		}

		public static List<Data.Entities.Tables.WPL.HallAuthorizationEntity> GetByHallId(int hallId)
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
				return new List<Infrastructure.Data.Entities.Tables.WPL.HallAuthorizationEntity>();
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

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		#endregion

		#region Helpers
		public static List<Data.Entities.Tables.WPL.HallAuthorizationEntity> toList(DataTable dataTable)
		{
			var list = new List<Data.Entities.Tables.WPL.HallAuthorizationEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				list.Add(new Data.Entities.Tables.WPL.HallAuthorizationEntity(dataRow));
			}
			return list;
		}
		#endregion
	}
}
