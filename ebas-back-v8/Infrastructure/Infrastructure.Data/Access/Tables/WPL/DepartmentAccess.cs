using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class DepartmentAccess
	{
		public static Boolean GetArchived { get; private set; } = false;

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Departement WHERE Id=@Id AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Departement WHERE Is_Archived=@IsArchived";

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

			sqlCommand.CommandText = "SELECT * FROM Departement WHERE Id IN (" + queryIds + ") AND Is_Archived=@IsArchived";

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO Departement (Creation_Date,Creation_User_Id,Delete_Date,Delete_User_Id,Is_Archived,Last_Edit_User_Id,Last_Edit_Date,Name) " +
									 " VALUES (@Creation_Date,@Creation_User_Id,@Delete_Date,@Delete_User_Id,@IsArchived,@Last_Edit_User_Id,@Last_Edit_Date,@Name);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Name", element.Name);

			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("Delete_Date", element.ArchiveTime == null ? (object)DBNull.Value : element.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", element.ArchiveUserId == null ? (object)DBNull.Value : element.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			var response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			sqlConnection.Close();

			return response;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE Departement SET Creation_Date=@Creation_Date,Creation_User_Id=@Creation_User_Id,Delete_Date=@Delete_Date,Delete_User_Id=@Delete_User_Id," +
				"Last_Edit_Date=@LastEditTime,Last_Edit_User_Id=@LastEditUserId,Name=@Name,Is_Archived=@IsArchived  WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", element.Id);
			sqlCommand.Parameters.AddWithValue("Name", element.Name);

			sqlCommand.Parameters.AddWithValue("LastEditTime", element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Delete_Date", element.ArchiveTime == null ? (object)DBNull.Value : element.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", element.ArchiveUserId == null ? (object)DBNull.Value : element.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity> elements)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity> elements)
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
					query += " UPDATE Departement SET "

						+ "Creation_Date=@Creation_Date" + i + ","
						+ "Creation_User_Id=@Creation_User_Id" + i + ","
						+ "Delete_Date=@Delete_Date" + i + ","
						+ "Delete_User_Id=@Delete_User_Id" + i + ","
						+ "Name=@Name" + i + ","
						+ "Last_Edit_Date=@LastEditTime" + i + ","
						+ "Last_Edit_User_Id=@LastEditUserId" + i + ","
						+ "Is_Archived=@IsArchived" + i + ","
						+ " WHERE Id=@Id" + i
					+ "; ";
					sqlCommand.Parameters.AddWithValue("Id" + i, element.Id);
					sqlCommand.Parameters.AddWithValue("Name" + i, element.Name);

					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, element.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, element.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, element.ArchiveTime == null ? (object)DBNull.Value : element.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, element.ArchiveUserId == null ? (object)DBNull.Value : element.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("IsArchived" + i, element.IsArchived);

				}

				sqlCommand.CommandText = query;

				int response = DbExecution.ExecuteNonQuery(sqlCommand);

				sqlConnection.Close();

				return response;
			}

			return -1;
		}
		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM Departement WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

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

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM Departement WHERE Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				int response = DbExecution.ExecuteNonQuery(sqlCommand);

				sqlConnection.Close();

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity GetByName(string name)
		{
			if(string.IsNullOrEmpty(name))
			{
				return null;
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Departement WHERE Name=@Name AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Name", name);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
