using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class StandardOperationDescriptionAccess
	{
		public static Boolean GetArchived { get; private set; } = false;
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE Id=@Id AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

			sqlCommand.CommandText = "SELECT * FROM StandardOperationDescription WHERE Id IN (" + queryIds + ") AND Is_Archived=@IsArchived";

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO StandardOperationDescription (Creation_Date,Creation_User_Id,Delete_Date,Delete_User_Id,Description,StdOperationId,Is_Archived,Last_Edit_User_Id,Last_Edit_Date) " +
										" VALUES (@Creation_Date,@Creation_User_Id,@Delete_Date,@Delete_User_Id,@Description,@StandardOperationId,@IsArchived,@Last_Edit_User_Id,@Last_Edit_Date);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Delete_Date", element.ArchiveTime ?? (object)DBNull.Value);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", element.ArchiveUserId ?? (object)DBNull.Value);
			sqlCommand.Parameters.AddWithValue("Description", element.Description);
			sqlCommand.Parameters.AddWithValue("StandardOperationId", element.StandardOperationId);
			sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", element.LastEditUserId ?? (object)DBNull.Value);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", element.LastEditTime ?? (object)DBNull.Value);

			var result = sqlCommand.ExecuteScalar();
			var response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			sqlConnection.Close();

			return response;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE StandardOperationDescription SET Creation_Date=@Creation_Date,Creation_User_Id=@Creation_User_Id,Delete_Date=@Delete_Date,Delete_User_Id=@Delete_User_Id," +
				"Description=@Description,StdOperationId=@StandardOperationId,Last_Edit_Date=@LastEditTime,Last_Edit_User_Id=@LastEditUserId,Is_Archived=@IsArchived WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", element.Id);
			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Delete_Date", element.ArchiveTime ?? (object)DBNull.Value);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", element.ArchiveUserId ?? (object)DBNull.Value);
			sqlCommand.Parameters.AddWithValue("Description", element.Description);
			sqlCommand.Parameters.AddWithValue("StandardOperationId", element.StandardOperationId);
			sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);
			sqlCommand.Parameters.AddWithValue("LastEditTime", element.LastEditTime ?? (object)DBNull.Value);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", element.LastEditUserId ?? (object)DBNull.Value);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 10; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = update(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += update(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += update(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
				sqlConnection.Open();

				string query = "";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				int i = 0;
				foreach(var element in elements)
				{
					i++;
					query += " UPDATE StandardOperationDescription SET "

						+ "Creation_Date=@Creation_Date" + i + ","
						+ "Creation_User_Id=@Creation_User_Id" + i + ","
						+ "Delete_Date=@Delete_Date" + i + ","
						+ "Delete_User_Id=@Delete_User_Id" + i + ","
						+ "Description=@Description" + i + ","
						+ "StdOperationId=@StandardOperationId" + i + ","
						+ "Last_Edit_Date=@LastEditTime" + i + ","
						+ "Last_Edit_User_Id=@LastEditUserId" + i + ","
						+ "Is_Archived=@IsArchived" + i + ","
						+ " WHERE Id=@Id" + i
					+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, element.Id);
					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, element.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, element.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, element.ArchiveTime ?? (object)DBNull.Value);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, element.ArchiveUserId ?? (object)DBNull.Value);
					sqlCommand.Parameters.AddWithValue("Description" + i, element.Description);
					sqlCommand.Parameters.AddWithValue("StandardOperationId" + i, element.StandardOperationId);
					sqlCommand.Parameters.AddWithValue("IsArchived" + i, element.IsArchived);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, element.LastEditTime ?? (object)DBNull.Value);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, element.LastEditUserId ?? (object)DBNull.Value);
				}

				sqlCommand.CommandText = query;

				int response = sqlCommand.ExecuteNonQuery();

				sqlConnection.Close();

				return response;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM StandardOperationDescription WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConection.Close();

			return response;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

				if(ids.Count <= maxParamsNumber)
				{
					return delete(ids);
				}
				int result = 0;
				int batchNumber = ids.Count / maxParamsNumber;
				for(int i = 0; i < batchNumber; i++)
				{
					result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
				}
				result += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				return result;
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

				string query = "DELETE FROM StandardOperationDescription WHERE Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				int response = sqlCommand.ExecuteNonQuery();

				sqlConnection.Close();

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> GetByOperationId(int StandardOperationId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM StandardOperationDescription WHERE StdOperationId=@StdOperationId AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);
			sqlCommand.Parameters.AddWithValue("StdOperationId", StandardOperationId);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>();
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
