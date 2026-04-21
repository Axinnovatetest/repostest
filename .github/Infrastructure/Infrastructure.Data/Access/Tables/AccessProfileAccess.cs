using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Entities.Tables.AccessProfileEntity Get(int id)
		{
			var dt = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return new Entities.Tables.AccessProfileEntity(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.AccessProfileEntity>();
			}
		}
		public static List<Entities.Tables.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.AccessProfileEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Entities.Tables.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.AccessProfileEntity>();
		}
		private static List<Entities.Tables.AccessProfileEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [_AccessProfile] WHERE [Id] IN (" + queryIds + ")";

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.AccessProfileEntity>();
		}

		public static int Insert(Entities.Tables.AccessProfileEntity element)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "INSERT INTO [_AccessProfile]([ArchiveTime],[ArchiveUserId],[CreationTime],[CreationUserId],[Description],[IsArchived],[LastUpdateId],[LastUpdateTime],[Name])  VALUES (@ArchiveTime,@ArchiveUserId,@CreationTime,@CreationUserId,@Description,@IsArchived,@LastUpdateId,@LastUpdateTime,@Name);";
				query += "SELECT SCOPE_IDENTITY()";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", element.ArchiveTime == null ? (object)DBNull.Value : element.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", element.ArchiveUserId == null ? (object)DBNull.Value : element.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("CreationTime", element.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", element.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Description", element.Description == null ? (object)DBNull.Value : element.Description);
				sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);
				sqlCommand.Parameters.AddWithValue("LastUpdateId", element.LastUpdateId == null ? (object)DBNull.Value : element.LastUpdateId);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", element.LastUpdateTime == null ? (object)DBNull.Value : element.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("Name", element.Name);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int Insert(List<Entities.Tables.AccessProfileEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 10; // Nb params per query
				int response = 0;
				if(elements.Count <= maxParamsNumber)
				{
					response = insert(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						response += insert(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					response += insert(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int insert(List<Entities.Tables.AccessProfileEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int response = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = " INSERT INTO [_AccessProfile] ([ArchiveTime],[ArchiveUserId],[CreationTime],[CreationUserId],[Description],[IsArchived],[LastUpdateId],[LastUpdateTime],[Name]) VALUES ";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(Entities.Tables.AccessProfileEntity t in elements)
					{
						i++;
						query += " ( "

							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@Description" + i + ","
							+ "@IsArchived" + i + ","
							+ "@LastUpdateId" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@Name" + i
								+ "), ";


						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, t.ArchiveTime == null ? (object)DBNull.Value : t.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, t.ArchiveUserId == null ? (object)DBNull.Value : t.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, t.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, t.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, t.Description == null ? (object)DBNull.Value : t.Description);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, t.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastUpdateId" + i, t.LastUpdateId == null ? (object)DBNull.Value : t.LastUpdateId);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, t.LastUpdateTime == null ? (object)DBNull.Value : t.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("Name" + i, t.Name);
					}

					query = query.TrimEnd(',');
					query += ';';
					sqlCommand.CommandText = query;

					response = sqlCommand.ExecuteNonQuery();
				}

				return response;
			}

			return -1;
		}

		public static int Update(Entities.Tables.AccessProfileEntity element)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [_AccessProfile] SET [ArchiveTime]=@ArchiveTime,[ArchiveUserId]=@ArchiveUserId,[CreationTime]=@CreationTime,[CreationUserId]=@CreationUserId,[Description]=@Description,[IsArchived]=@IsArchived,[LastUpdateId]=@LastUpdateId,[LastUpdateTime]=@LastUpdateTime,[Name]=@Name WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", element.ArchiveTime == null ? (object)DBNull.Value : element.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", element.ArchiveUserId == null ? (object)DBNull.Value : element.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("CreationTime", element.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", element.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Description", element.Description == null ? (object)DBNull.Value : element.Description);
				sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);
				sqlCommand.Parameters.AddWithValue("LastUpdateId", element.LastUpdateId == null ? (object)DBNull.Value : element.LastUpdateId);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", element.LastUpdateTime == null ? (object)DBNull.Value : element.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("Name", element.Name);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int Update(List<Entities.Tables.AccessProfileEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 10; // Nb params per query
				int response = 0;
				if(elements.Count <= maxParamsNumber)
				{
					response = update(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						response += update(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					response += update(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int update(List<Entities.Tables.AccessProfileEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int r = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var element in elements)
					{
						i++;
						query += " UPDATE [_AccessProfile] SET "

							+ "[ArchiveTime]=@ArchiveTime" + i + ","
							+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[IsArchived]=@IsArchived" + i + ","
							+ "[LastUpdateId]=@LastUpdateId" + i + ","
							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[Name]=@Name" + i + " WHERE [Id]=@Id" + i
								+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, element.Id);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, element.ArchiveTime == null ? (object)DBNull.Value : element.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, element.ArchiveUserId == null ? (object)DBNull.Value : element.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, element.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, element.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, element.Description == null ? (object)DBNull.Value : element.Description);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, element.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastUpdateId" + i, element.LastUpdateId == null ? (object)DBNull.Value : element.LastUpdateId);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, element.LastUpdateTime == null ? (object)DBNull.Value : element.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("Name" + i, element.Name);
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

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				r = sqlCommand.ExecuteNonQuery();
			}

			return r;
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

					string query = "DELETE FROM [_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					response = sqlCommand.ExecuteNonQuery();
				}

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Entities.Tables.AccessProfileEntity GetByName(string name, bool ignoreArchived = true)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [_AccessProfile] WHERE [Name]=@name "
					+ (ignoreArchived ? " AND IsArchived=0 " : "");

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<KeyValuePair<int, string>> GetIdsNames()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT Id,Name FROM [_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<KeyValuePair<int, string>>();
			}

			var response = new List<KeyValuePair<int, string>>();
			foreach(DataRow dataRow in dataTable.Rows)
			{
				var id = Convert.ToInt32(dataRow["Id"]);
				var name = Convert.ToString(dataRow["Name"]);

				response.Add(new KeyValuePair<int, string>(id, name));
			}
			return response;
		}
		#endregion

		#region Helpers
		private static List<Entities.Tables.AccessProfileEntity> toList(DataTable dataTable)
		{
			var result = new List<Entities.Tables.AccessProfileEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Entities.Tables.AccessProfileEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
