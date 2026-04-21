using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class UserCountryAccess
	{
		#region Default Methods
		public static Data.Entities.Tables.WPL.UserCountryEntity Get(int id)
		{
			DataTable dt = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM User_Country WHERE Id=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Data.Entities.Tables.WPL.UserCountryEntity> Get()
		{
			DataTable dt = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM User_Country";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
			}
		}
		public static List<Data.Entities.Tables.WPL.UserCountryEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQuerySize = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.WPL.UserCountryEntity>();
				if(ids.Count <= maxQuerySize)
				{
					result = get(ids);
				}
				else
				{
					int batchSize = ids.Count / maxQuerySize;
					result = new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
					for(int i = 0; i < batchSize; i++)
					{
						result.AddRange(get(ids.GetRange(i * maxQuerySize, maxQuerySize)));
					}
					result.AddRange(get(ids.GetRange(batchSize * maxQuerySize, ids.Count - batchSize * maxQuerySize)));
				}
				return result;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
		}
		private static List<Data.Entities.Tables.WPL.UserCountryEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM User_Country WHERE Id IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
		}

		public static int Insert(Data.Entities.Tables.WPL.UserCountryEntity element)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "INSERT INTO User_Country(Country_Id,User_Id)  VALUES (@Country_Id,@User_Id);";
				query += "SELECT SCOPE_IDENTITY();";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Country_Id", element.CountryId);
				sqlCommand.Parameters.AddWithValue("User_Id", element.UserId);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int Insert(List<Data.Entities.Tables.WPL.UserCountryEntity> elements)
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
		private static int insert(List<Data.Entities.Tables.WPL.UserCountryEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int r = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity t in elements)
					{
						i++;
						query += " INSERT INTO User_Country(Country_Id,User_Id) VALUES( "

							+ "@Country_Id" + i + ","
							+ "@User_Id" + i
							 + "); ";


						sqlCommand.Parameters.AddWithValue("Country_Id" + i, t.CountryId);
						sqlCommand.Parameters.AddWithValue("User_Id" + i, t.UserId);
					}

					sqlCommand.CommandText = query;

					r = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return r;
			}

			return -1;
		}

		public static int Update(Data.Entities.Tables.WPL.UserCountryEntity element)
		{
			int r = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE User_Country SET Country_Id=@Country_Id,User_Id=@User_Id WHERE Id=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("Country_Id", element.CountryId);
				sqlCommand.Parameters.AddWithValue("User_Id", element.UserId);

				r = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return r;
		}
		public static int Update(List<Data.Entities.Tables.WPL.UserCountryEntity> elements)
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
		private static int update(List<Data.Entities.Tables.WPL.UserCountryEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int response = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var element in elements)
					{
						i++;
						query += " UPDATE User_Country SET "

							+ "Country_Id=@Country_Id" + i + ","
							+ "User_Id=@User_Id" + i + " WHERE Id=@Id" + i
							 + "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, element.Id);
						sqlCommand.Parameters.AddWithValue("Country_Id" + i, element.CountryId);
						sqlCommand.Parameters.AddWithValue("User_Id" + i, element.UserId);
					}

					sqlCommand.CommandText = query;

					response = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return response;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM User_Country WHERE Id=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
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
				int response = -1;
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

					string query = "DELETE FROM User_Country WHERE Id IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					response = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Data.Entities.Tables.WPL.UserCountryEntity> GetByUserId(int userId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM User_Country Where User_Id=@UserId";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("UserId", userId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
			}
		}

		public static List<Data.Entities.Tables.WPL.UserCountryEntity> GetByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				int maxQuerySize = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.WPL.UserCountryEntity>();
				if(usersIds.Count <= maxQuerySize)
				{
					result = getByUsersIds(usersIds);
				}
				else
				{
					int batchSize = usersIds.Count / maxQuerySize;
					result = new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
					for(int i = 0; i < batchSize; i++)
					{
						result.AddRange(getByUsersIds(usersIds.GetRange(i * maxQuerySize, maxQuerySize)));
					}
					result.AddRange(getByUsersIds(usersIds.GetRange(batchSize * maxQuerySize, usersIds.Count - batchSize * maxQuerySize)));
				}
				return result;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
		}
		private static List<Data.Entities.Tables.WPL.UserCountryEntity> getByUsersIds(List<int> usersIds)
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

					sqlCommand.CommandText = "SELECT * FROM User_Country WHERE User_Id IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
		}

		public static List<Data.Entities.Tables.WPL.UserCountryEntity> GetByCountryId(int CountryId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM User_Country Where Country_Id=@CountryId";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("CountryId", CountryId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity>();
			}
		}
		public static int CountByCountryId(int CountryId)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM User_Country Where Country_Id=@CountryId";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("CountryId", CountryId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var x) ? x : 0;
			}
		}

		public static int DeleteByUserId(int userId)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM User_Country WHERE User_Id=@UserId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("UserId", userId);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		#endregion

		#region Helpers
		private static List<Data.Entities.Tables.WPL.UserCountryEntity> toList(DataTable dataTable)
		{
			var list = new List<Data.Entities.Tables.WPL.UserCountryEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				list.Add(new Data.Entities.Tables.WPL.UserCountryEntity(dataRow));
			}
			return list;
		}
		#endregion
	}
}
