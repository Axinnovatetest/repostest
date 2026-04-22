using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class WorkStationMachineAccess
	{
		public static object GetArchived { get; private set; } = false;
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkStationMachine WHERE Id=@Id AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkStationMachine Where Is_Archived=@IsArchived";

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity>();
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

			sqlCommand.CommandText = "SELECT * FROM WorkStationMachine WHERE Id IN (" + queryIds + ")";


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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO WorkStationMachine (Work_Area_Id,Country_Id,Creation_Date,Creation_User_Id,Last_Edit_User_Id,Last_Edit_Date,Name,Hall_Id,Type,Delete_Date,Delete_User_Id,Is_Archived) " +
									 " VALUES (@WorkAreaId,@CountryId,@Creation_Date,@Creation_User_Id,@Last_Edit_User_Id,@Last_Edit_Date,@Name,@Hall_Id,@Type,@DeleteTime,@DeleteUserId,@IsArchived);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Name", element.Name);
			sqlCommand.Parameters.AddWithValue("Hall_Id", element.HallId);
			sqlCommand.Parameters.AddWithValue("Type", element.Type);
			sqlCommand.Parameters.AddWithValue("WorkAreaId", element.WorkAreaId);

			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("DeleteTime", element.DeleteTime == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("DeleteUserId", element.DeleteUserId == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);
			sqlCommand.Parameters.AddWithValue("CountryId", element.CountryId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			var response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			sqlConnection.Close();

			return response;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE WorkStationMachine SET Work_Area_Id=@WorkAreaId,Country_Id=@CountryId,Creation_Date=@Creation_Date,Creation_User_Id=@Creation_User_Id," +
				"Last_Edit_Date=@LastEditTime,Last_Edit_User_Id=@LastEditUserId,Name=@Name,Hall_Id=@Hall_Id,Type=@Type," +
				"Delete_Date=@DeleteTime,Delete_User_Id=@DeleteUserId,Is_Archived=@IsArchived  WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", element.Id);
			sqlCommand.Parameters.AddWithValue("Name", element.Name);
			sqlCommand.Parameters.AddWithValue("Hall_Id", element.HallId);
			sqlCommand.Parameters.AddWithValue("Type", element.Type);
			sqlCommand.Parameters.AddWithValue("WorkAreaId", element.WorkAreaId);

			sqlCommand.Parameters.AddWithValue("LastEditTime", element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("DeleteTime", element.DeleteTime == null ? (object)DBNull.Value : element.DeleteTime);
			sqlCommand.Parameters.AddWithValue("DeleteUserId", element.DeleteUserId == null ? (object)DBNull.Value : element.DeleteUserId);
			sqlCommand.Parameters.AddWithValue("IsArchived", element.IsArchived);
			sqlCommand.Parameters.AddWithValue("CountryId", element.CountryId);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> elements)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> elements)
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
					query += " UPDATE WorkStationMachine SET "

						+ "Country_Id=@CountryId" + i + ","
						+ "Work_Area_Id=@WorkAreaId" + i + ","
						+ "Creation_Date=@Creation_Date" + i + ","
						+ "Creation_User_Id=@Creation_User_Id" + i + ","
						+ "Name=@Name" + i + ","
						+ "Last_Edit_Date=@LastEditTime" + i + ","
						+ "Last_Edit_User_Id=@LastEditUserId" + i + ","
						+ "Hall_Id=@Hall_Id" + i + ","
						+ "Type=@Type" + i + ","
						+ "Delete_Date=@DeleteTime" + i + ","
						+ "Delete_User_Id=@DeleteUserId" + i + ","
						+ "Is_Archived=@IsArchived" + i + ","
						+ " WHERE Id=@Id" + i
					+ "; ";
					sqlCommand.Parameters.AddWithValue("Id" + i, element.Id);
					sqlCommand.Parameters.AddWithValue("Name" + i, element.Name);
					sqlCommand.Parameters.AddWithValue("Hall_Id" + i, element.HallId);
					sqlCommand.Parameters.AddWithValue("Type" + i, element.Type);
					sqlCommand.Parameters.AddWithValue("WorkAreaId" + i, element.WorkAreaId);

					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, element.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, element.CreationUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("DeleteTime" + i, element.DeleteTime == null ? (object)DBNull.Value : element.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, element.DeleteUserId == null ? (object)DBNull.Value : element.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("IsArchived" + i, element.IsArchived);
					sqlCommand.Parameters.AddWithValue("CountryId" + i, element.CountryId);

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

			string query = "DELETE FROM WorkStationMachine WHERE Id=@Id";

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

				string query = "DELETE FROM WorkStationMachine WHERE Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				int response = DbExecution.ExecuteNonQuery(sqlCommand);

				sqlConnection.Close();

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity GetByNameWorkAreaId(string name, int workAreaId)
		{
			if(string.IsNullOrEmpty(name))
			{
				return null;
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkStationMachine WHERE TRIM([Name])=TRIM(@Name) AND Work_Area_Id=@WorkAreaId AND Is_Archived=@IsArchived AND Delete_Date IS NULL";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Name", name);
			sqlCommand.Parameters.AddWithValue("WorkAreaId", workAreaId);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> GetByHallId(int hallId)
		{


			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkStationMachine WHERE Hall_Id=@HallId AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("hallId", hallId);
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
				return new List<Entities.Tables.WPL.WorkStationMachineEntity>();
			}
		}
		public static int CountByHallId(int hallId)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM WorkStationMachine WHERE Hall_Id=@HallId AND Is_Archived=@IsArchived";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("hallId", hallId);
				sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var x) ? x : 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> GetByWorkAreaId(int workAreaId)
		{


			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkStationMachine WHERE Work_Area_Id=@WorkAreaId AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("WorkAreaId", workAreaId);
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
				return new List<Entities.Tables.WPL.WorkStationMachineEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> ArchiveByWorkAreaId(int workAreaId)
		{


			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE WorkStationMachine SET Is_Archived=@IsArchived WHERE Work_Area_Id=@WorkAreaId";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("WorkAreaId", workAreaId);
			sqlCommand.Parameters.AddWithValue("IsArchived", true);

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
				return new List<Entities.Tables.WPL.WorkStationMachineEntity>();
			}
		}

		//
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> GetList(bool IsArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select ws.*, c.Name as CountryName ,h.Name as HallName,wa.Name as WorkAreaName from WorkStationMachine  ws
inner join Countries c on ws.Country_Id = c.Id
inner join Hall h on ws.Hall_Id= h.Id
inner join Work_Area wa on ws.Work_Area_Id = wa.Id WHERE wa.Is_Archived=@IsArchived";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity(x, true)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity>();
			}
		}

		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
