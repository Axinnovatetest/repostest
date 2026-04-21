using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class UserAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.UserEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [User] WHERE Id=@Id AND Is_Archived=@IsArchived";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("IsArchived", false);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.UserEntity> Get(bool includeArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [User] "
					+ (!includeArchived ? " WHERE Is_Archived=0 " : "");

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.UserEntity> Get(List<int> ids, bool includeArchived = false)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var result = new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = get(ids, includeArchived);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), includeArchived));
					}
					result.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), includeArchived));
				}
				return result;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.UserEntity> get(List<int> ids, bool includeArchived)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand
					{
						Connection = sqlConnection
					};

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [User] WHERE Id IN (" + queryIds + ") "
						 + (!includeArchived ? "AND Is_Archived=0 " : "");

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.UserEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "INSERT INTO [User] "
					+ " ([CreationTime],[Name],[Password],[Username],[Creation_User_Id],[Is_Archived], "
					+ " Last_Edit_Date,Last_Edit_User_Id,Delete_Date,Delete_User_Id,IsActivated,AccessProfileId) "
					+ " VALUES "
					+ "(@CreationTime,@Name,@Password,@Username,@CreationUserId,@IsArchived,@Last_Edit_Date,@Last_Edit_User_Id, "
					+ " @Delete_Date,@Delete_User_Id,@IsActivated,@AccessProfileId)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("CreationTime", element.CreationTime);
				sqlCommand.Parameters.AddWithValue("Name", element.Name);
				sqlCommand.Parameters.AddWithValue("Password", element.Password);
				sqlCommand.Parameters.AddWithValue("Username", element.Username);
				sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);
				sqlCommand.Parameters.AddWithValue("Last_Edit_Date", element.LastEditTime.HasValue ? element.LastEditTime.Value : (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", element.LastEditUserId.HasValue ? element.LastEditUserId.Value : (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("Delete_Date", element.DeleteTime.HasValue ? element.DeleteTime.Value : (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("Delete_User_Id", element.DeleteUserId.HasValue ? element.DeleteUserId.Value : (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("IsActivated", element.IsActivated);
				sqlCommand.Parameters.AddWithValue("CreationUserId", element.CreationUserId);

				sqlCommand.Parameters.AddWithValue("AccessProfileId", element.AccessProfileId);

				response = sqlCommand.ExecuteNonQuery();

				if(response > 0)
				{
					query = "SELECT Id FROM [User] WHERE Id = @@IDENTITY";
					sqlCommand = new SqlCommand(query, sqlConnection);
					object insertedId = sqlCommand.ExecuteScalar();

					if(insertedId != null)
					{
						response = Convert.ToInt32(insertedId.ToString());
					}
					else
					{
						response = -1;
					}
				}
				else
				{
					response = -1;
				}
			}

			return response;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.WPL.UserEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE [User] SET [CreationTime]=@CreationTime,[Name]=@Name,[IsActivated]=@IsActivated, "
					+ " [Password]=@Password,[Username]=@Username,[Creation_User_Id]=@CreationUserId,[Is_Archived]=@IsArchived, "
					+ " [Last_Edit_User_Id]=@LastEditUserId,Last_Edit_Date=@LastEditTime,Delete_Date=@DeleteTime,Delete_User_Id=@DeleteUserId, "
					+ " [AccessProfileId]=@AccessProfileId "
					+ " WHERE [Id]=@Id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("Last_Edit_Date", element.LastEditTime ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", element.LastEditUserId ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("CreationTime", element.CreationTime);
				sqlCommand.Parameters.AddWithValue("Name", element.Name ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("Delete_Date", element.DeleteTime ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("IsActivated", element.IsActivated);
				sqlCommand.Parameters.AddWithValue("Delete_User_Id", element.DeleteUserId ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("Password", element.Password);
				sqlCommand.Parameters.AddWithValue("Username", element.Username);
				sqlCommand.Parameters.AddWithValue("CreationUserId", element.CreationUserId);
				sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);
				sqlCommand.Parameters.AddWithValue("LastEditTime", element.LastEditTime ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", element.LastEditUserId ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("DeleteTime", element.DeleteTime ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("DeleteUserId", element.DeleteUserId ?? (object)DBNull.Value);

				sqlCommand.Parameters.AddWithValue("AccessProfileId", element.AccessProfileId);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}

		public static int Delete(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [User] WHERE [Id]=@Id";

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
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int result = 0;
				if(ids.Count <= maxParamsNumber)
				{
					result = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int response = -1;

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand
					{
						Connection = sqlConnection
					};

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [User] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					response = sqlCommand.ExecuteNonQuery();
				}

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static bool CheckExists(string username)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [User] WHERE [Username]=@userName AND Is_Archived=0";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("userName", username);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static Infrastructure.Data.Entities.Tables.WPL.UserEntity GetByUsername(string username)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [User] WHERE [Username]=@username AND Is_Archived=@IsArchived";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("username", username);
				sqlCommand.Parameters.AddWithValue("IsArchived", false);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.WPL.UserEntity> GetLikeName(string searchText)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = " SELECT * FROM [User] WHERE [Name] LIKE @searchText ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");

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
				return new List<Entities.Tables.WPL.UserEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.UserEntity> GetByAccessProfilesIds(List<int> accessProfilesIds, bool includeArchived = false)
		{
			if(accessProfilesIds != null && accessProfilesIds.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var result = new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
				if(accessProfilesIds.Count <= maxQueryNumber)
				{
					result = getByAccessProfilesIds(accessProfilesIds, includeArchived);
				}
				else
				{
					int batchNumber = accessProfilesIds.Count / maxQueryNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByAccessProfilesIds(accessProfilesIds.GetRange(i * maxQueryNumber, maxQueryNumber), includeArchived));
					}
					result.AddRange(getByAccessProfilesIds(accessProfilesIds.GetRange(batchNumber * maxQueryNumber, accessProfilesIds.Count - batchNumber * maxQueryNumber), includeArchived));
				}
				return result;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.UserEntity> getByAccessProfilesIds(List<int> accessProfilesIds, bool includeArchived)
		{
			if(accessProfilesIds != null && accessProfilesIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand
					{
						Connection = sqlConnection
					};

					string queryIds = string.Empty;
					for(int i = 0; i < accessProfilesIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, accessProfilesIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [User] WHERE AccessProfileId IN (" + queryIds + ") "
						 + (!includeArchived ? " AND Is_Archived=0 " : "");

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();
		}
		public static int UpdateLanguage(Infrastructure.Data.Entities.Tables.WPL.UserEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE [User] SET [SelectedLanguage]=@SelectedLanguage "
					+ " WHERE [Id]=@Id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("SelectedLanguage", element.SelectedLanguage);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.UserEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.UserEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
