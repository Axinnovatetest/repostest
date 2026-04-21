using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.STG
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Entities.Tables.STG.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [STG_AccessProfile] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.STG.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.STG.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [STG_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.STG.AccessProfileEntity>();
			}
		}
		public static List<Entities.Tables.STG.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.STG.AccessProfileEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Entities.Tables.STG.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.STG.AccessProfileEntity>();
		}
		private static List<Entities.Tables.STG.AccessProfileEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [STG_AccessProfile] WHERE [Id] IN (" + queryIds + ")";

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.STG.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.STG.AccessProfileEntity>();
		}

		public static int Insert(Entities.Tables.STG.AccessProfileEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "INSERT INTO [STG_AccessProfile] "
					+ " ([AccessProfiles],[AccessProfilesCreate],[AccessProfilesDelete],[AccessProfilesUpdate], "
					+ " [Users],[UsersCreate],[UsersDelete],[UsersUpdate],[MainAccessProfileId],[ModuleActivated]) "
					+ " VALUES "
					+ " (@AccessProfiles,@AccessProfilesCreate,@AccessProfilesDelete,@AccessProfilesUpdate, "
					+ " @Users,@UsersCreate,@UsersDelete,@UsersUpdate,@MainAccessProfileId,@ModuleActivated); ";
				query += "SELECT SCOPE_IDENTITY();";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("AccessProfiles", element.AccessProfiles);
				sqlCommand.Parameters.AddWithValue("AccessProfilesCreate", element.AccessProfilesCreate);
				sqlCommand.Parameters.AddWithValue("AccessProfilesDelete", element.AccessProfilesDelete);
				sqlCommand.Parameters.AddWithValue("AccessProfilesUpdate", element.AccessProfilesUpdate);
				sqlCommand.Parameters.AddWithValue("MainAccessProfileId", element.MainAccessProfileId);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", element.ModuleActivated);

				sqlCommand.Parameters.AddWithValue("Users", element.Users);
				sqlCommand.Parameters.AddWithValue("UsersCreate", element.UsersCreate);
				sqlCommand.Parameters.AddWithValue("UsersDelete", element.UsersDelete);
				sqlCommand.Parameters.AddWithValue("UsersUpdate", element.UsersUpdate);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}

		public static int Update(Entities.Tables.STG.AccessProfileEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE [STG_AccessProfile] SET "
					+ " [AccessProfiles]=@AccessProfiles,[AccessProfilesCreate]=@AccessProfilesCreate, "
					+ " [AccessProfilesDelete]=@AccessProfilesDelete,[AccessProfilesUpdate]=@AccessProfilesUpdate, "
					+ " [Users]=@Users,[UsersCreate]=@UsersCreate,[UsersDelete]=@UsersDelete,[UsersUpdate]=@UsersUpdate, "
					+ " [MainAccessProfileId]=@MainAccessProfileId,[ModuleActivated]=@ModuleActivated "
					+ " WHERE [Id]=@Id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfiles", element.AccessProfiles);
				sqlCommand.Parameters.AddWithValue("AccessProfilesCreate", element.AccessProfilesCreate);
				sqlCommand.Parameters.AddWithValue("AccessProfilesDelete", element.AccessProfilesDelete);
				sqlCommand.Parameters.AddWithValue("AccessProfilesUpdate", element.AccessProfilesUpdate);
				sqlCommand.Parameters.AddWithValue("MainAccessProfileId", element.MainAccessProfileId);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", element.ModuleActivated);

				sqlCommand.Parameters.AddWithValue("Users", element.Users);
				sqlCommand.Parameters.AddWithValue("UsersCreate", element.UsersCreate);
				sqlCommand.Parameters.AddWithValue("UsersDelete", element.UsersDelete);
				sqlCommand.Parameters.AddWithValue("UsersUpdate", element.UsersUpdate);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}

		public static int Delete(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [STG_AccessProfile] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
				int response = 0;
				if(ids.Count <= maxParamsNumber)
				{
					response = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						response += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					response += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int response = -1;
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

					string query = "DELETE FROM [STG_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					response = sqlCommand.ExecuteNonQuery();
				}

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Entities.Tables.STG.AccessProfileEntity> GetByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.STG.AccessProfileEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = getByMainAccessProfilesIds(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Entities.Tables.STG.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getByMainAccessProfilesIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(getByMainAccessProfilesIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.STG.AccessProfileEntity>();
		}
		private static List<Entities.Tables.STG.AccessProfileEntity> getByMainAccessProfilesIds(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM STG_AccessProfile WHERE [MainAccessProfileId] IN (" + queryIds + ")";

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.STG.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.STG.AccessProfileEntity>();
		}

		public static int DeleteByMainAccessProfilesId(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [STG_AccessProfile] WHERE [MainAccessProfileId]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		#endregion

		#region Helpers
		private static List<Entities.Tables.STG.AccessProfileEntity> toList(DataTable dataTable)
		{
			var list = new List<Entities.Tables.STG.AccessProfileEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				list.Add(new Entities.Tables.STG.AccessProfileEntity(dataRow));
			}
			return list;
		}
		#endregion
	}
}
