using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class WorkScheduleDetailsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails";

			var sqlCommand = new SqlCommand(query, sqlConnection);


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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
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

			sqlCommand.CommandText = "SELECT * FROM WorkScheduleDetails WHERE Id IN (" + queryIds + ")";


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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO WorkScheduleDetails (OrderDisplayId,Comment,FromToolInsert2,Country_Id,Creation_Date,Creation_User_Id,Departement_Id,FromToolInsert,Hall_Id,Amount,Last_Edit_Date,Last_Edit_User_Id,LotSizeSTD,Operation_Value_Adding,OperationDescription_Id,OperationNumber,OperationTimeSeconds,OperationTimeValueAdding,PredecessorOperation,PredecessorSubOperation,RelationOperationTime,SetupTimeMinutes,StandardOccupancy,StandardOperation_Id,SubOperationNumber,TotalTimeOperation,WorkArea_Id,WorkScheduleId,WorkStationMachine_Id)" +
										" VALUES (@OrderDisplayId,@Comment,@FromToolInsert2,@Country_Id,@Creation_Date,@Creation_User_Id,@Departement_Id,@FromToolInsert,@Hall_Id,@Amount,@Last_Edit_Date,@Last_Edit_User_Id,@LotSizeSTD,@Operation_Value_Adding,@OperationDescription_Id,@OperationNumber,@OperationTimeSeconds,@OperationTimeValueAdding,@PredecessorOperation,@PredecessorSubOperation,@RelationOperationTime,@SetupTimeMinutes,@StandardOccupancy,@StandardOperation_Id,@SubOperationNumber,@TotalTimeOperation,@WorkArea_Id,@WorkScheduleId,@WorkStationMachine_Id);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("OrderDisplayId", element.OrderDisplayId);
			sqlCommand.Parameters.AddWithValue("Country_Id", element.CountryId);
			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Departement_Id", element.DepartementId);
			sqlCommand.Parameters.AddWithValue("Hall_Id", element.HallId);
			sqlCommand.Parameters.AddWithValue("Amount", element.Amount);
			sqlCommand.Parameters.AddWithValue("LotSizeSTD", element.LotSizeSTD);
			sqlCommand.Parameters.AddWithValue("Operation_Value_Adding", element.OperationValueAdding == null ? (object)DBNull.Value : element.OperationValueAdding);
			sqlCommand.Parameters.AddWithValue("OperationNumber", element.OperationNumber);
			sqlCommand.Parameters.AddWithValue("OperationTimeSeconds", element.OperationTimeSeconds);
			sqlCommand.Parameters.AddWithValue("OperationTimeValueAdding", element.OperationTimeValueAdding);
			sqlCommand.Parameters.AddWithValue("PredecessorOperation", element.PredecessorOperation);
			sqlCommand.Parameters.AddWithValue("PredecessorSubOperation", element.PredecessorSubOperation);
			sqlCommand.Parameters.AddWithValue("RelationOperationTime", element.RelationOperationTime);
			sqlCommand.Parameters.AddWithValue("SetupTimeMinutes", element.SetupTimeMinutes);
			sqlCommand.Parameters.AddWithValue("StandardOccupancy", element.StandardOccupancy);
			sqlCommand.Parameters.AddWithValue("StandardOperation_Id", element.StandardOperationId);
			sqlCommand.Parameters.AddWithValue("SubOperationNumber", element.SubOperationNumber);
			sqlCommand.Parameters.AddWithValue("TotalTimeOperation", element.TotalTimeOperation);
			sqlCommand.Parameters.AddWithValue("WorkArea_Id", element.WorkAreaId);
			sqlCommand.Parameters.AddWithValue("WorkScheduleId", element.WorkScheduleId);
			sqlCommand.Parameters.AddWithValue("WorkStationMachine_Id", element.WorkStationMachineId == null ? (object)DBNull.Value : element.WorkStationMachineId);
			sqlCommand.Parameters.AddWithValue("FromToolInsert", element.FromToolInsert == null ? (object)DBNull.Value : element.FromToolInsert);
			sqlCommand.Parameters.AddWithValue("FromToolInsert2", element.FromToolInsert2 == null ? (object)DBNull.Value : element.FromToolInsert2);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("OperationDescription_Id", element.OperationDescriptionId == null ? (object)DBNull.Value : element.OperationDescriptionId);
			sqlCommand.Parameters.AddWithValue("Comment", element.Comment == null ? (object)DBNull.Value : element.Comment);

			var result = sqlCommand.ExecuteScalar();
			var response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			sqlConnection.Close();

			return response;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity element)
		{
			var sqlConnection = new SqlConnection(Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = " UPDATE WorkScheduleDetails SET "
				+ " OrderDisplayId=@OrderDisplayId,Amount=@Amount,Comment=@Comment,FromToolInsert2=@FromToolInsert2, "
				+ " Country_Id=@Country_Id,Creation_Date=@Creation_Date,Creation_User_Id=@Creation_User_Id, "
				+ " Departement_Id=@Departement_Id,FromToolInsert=@FromToolInsert,Hall_Id=@Hall_Id, "
				+ " Last_Edit_Date=@Last_Edit_Date,Last_Edit_User_Id=@Last_Edit_User_Id,LotSizeSTD=@LotSizeSTD, "
				+ " Operation_Value_Adding=@OperationValueAdding,OperationDescription_Id=@OperationDescription_Id, "
				+ " OperationNumber=@OperationNumber,OperationTimeSeconds=@OperationTimeSeconds, "
				+ " OperationTimeValueAdding=@OperationTimeValueAdding,PredecessorOperation=@PredecessorOperation, "
				+ " PredecessorSubOperation=@PredecessorSubOperation,RelationOperationTime=@RelationOperationTime, "
				+ " SetupTimeMinutes=@SetupTimeMinutes,StandardOccupancy=@StandardOccupancy, "
				+ " StandardOperation_Id=@StandardOperation_Id,SubOperationNumber=@SubOperationNumber, "
				+ " TotalTimeOperation=@TotalTimeOperation,WorkArea_Id=@WorkArea_Id, "
				+ " WorkScheduleId=@WorkScheduleId,WorkStationMachine_Id=@WorkStationMachine_Id "
				+ " WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", element.Id);
			sqlCommand.Parameters.AddWithValue("OrderDisplayId", element.OrderDisplayId);
			sqlCommand.Parameters.AddWithValue("OperationValueAdding", element.OperationValueAdding == null ? (object)DBNull.Value : element.OperationValueAdding);
			sqlCommand.Parameters.AddWithValue("WorkScheduleId", element.WorkScheduleId);
			sqlCommand.Parameters.AddWithValue("Amount", element.Amount);
			sqlCommand.Parameters.AddWithValue("Country_Id", element.CountryId);
			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Departement_Id", element.DepartementId);
			sqlCommand.Parameters.AddWithValue("Hall_Id", element.HallId);
			sqlCommand.Parameters.AddWithValue("LotSizeSTD", element.LotSizeSTD);
			sqlCommand.Parameters.AddWithValue("OperationNumber", element.OperationNumber);
			sqlCommand.Parameters.AddWithValue("OperationTimeSeconds", element.OperationTimeSeconds);
			sqlCommand.Parameters.AddWithValue("OperationTimeValueAdding", element.OperationTimeValueAdding);
			sqlCommand.Parameters.AddWithValue("PredecessorOperation", element.PredecessorOperation);
			sqlCommand.Parameters.AddWithValue("PredecessorSubOperation", element.PredecessorSubOperation);
			sqlCommand.Parameters.AddWithValue("RelationOperationTime", element.RelationOperationTime);
			sqlCommand.Parameters.AddWithValue("SetupTimeMinutes", element.SetupTimeMinutes);
			sqlCommand.Parameters.AddWithValue("StandardOccupancy", element.StandardOccupancy);
			sqlCommand.Parameters.AddWithValue("StandardOperation_Id", element.StandardOperationId);
			sqlCommand.Parameters.AddWithValue("SubOperationNumber", element.SubOperationNumber);
			sqlCommand.Parameters.AddWithValue("TotalTimeOperation", element.TotalTimeOperation);
			sqlCommand.Parameters.AddWithValue("WorkArea_Id", element.WorkAreaId);
			sqlCommand.Parameters.AddWithValue("WorkStationMachine_Id", element.WorkStationMachineId == null ? (object)DBNull.Value : element.WorkStationMachineId);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("FromToolInsert", element.FromToolInsert == null ? (object)DBNull.Value : element.FromToolInsert);
			sqlCommand.Parameters.AddWithValue("FromToolInsert2", element.FromToolInsert2 == null ? (object)DBNull.Value : element.FromToolInsert2);
			sqlCommand.Parameters.AddWithValue("OperationDescription_Id", element.OperationDescriptionId == null ? (object)DBNull.Value : element.OperationDescriptionId);
			sqlCommand.Parameters.AddWithValue("Comment", element.Comment == null ? (object)DBNull.Value : element.Comment);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> elements)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> elements)
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
					query += " UPDATE WorkScheduleDetails SET "

							+ "OrderDisplayId=@OrderDisplayId" + i + ","
							+ "Amount=@Amount" + i + ","
							+ "Comment=@Comment" + i + ","
							+ "Operation_Value_Adding=@OperationValueAdding" + i + ","
							+ "WorkScheduleId=@WorkScheduleId" + i + ","
							+ "Country_Id=@Country_Id" + i + ","
							+ "Creation_Date=@Creation_Date" + i + ","
							+ "Creation_User_Id=@Creation_User_Id" + i + ","
							+ "Departement_Id=@Departement_Id" + i + ","
							+ "FromToolInsert=@FromToolInsert" + i + ","
							+ "FromToolInsert2=@FromToolInsert2" + i + ","
							+ "Hall_Id=@Hall_Id" + i + ","
							+ "Last_Edit_Date=@Last_Edit_Date" + i + ","
							+ "Last_Edit_User_Id=@Last_Edit_User_Id" + i + ","
							+ "LotSizeSTD=@LotSizeSTD" + i + ","
							+ "OperationDescription_Id=@OperationDescription_Id" + i + ","
							+ "OperationNumber=@OperationNumber" + i + ","
							+ "OperationTimeSeconds=@OperationTimeSeconds" + i + ","
							+ "OperationTimeValueAdding=@OperationTimeValueAdding" + i + ","
							+ "PredecessorOperation=@PredecessorOperation" + i + ","
							+ "PredecessorSubOperation=@PredecessorSubOperation" + i + ","
							+ "RelationOperationTime=@RelationOperationTime" + i + ","
							+ "SetupTimeMinutes=@SetupTimeMinutes" + i + ","
							+ "StandardOccupancy=@StandardOccupancy" + i + ","
							+ "StandardOperation_Id=@StandardOperation_Id" + i + ","
							+ "SubOperationNumber=@SubOperationNumber" + i + ","
							+ "TotalTimeOperation=@TotalTimeOperation" + i + ","
							+ "WorkArea_Id=@WorkArea_Id" + i + ","
							+ "WorkStationMachine_Id=@WorkStationMachine_Id" + i + " WHERE Id=@Id" + i
							+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, element.Id);
					sqlCommand.Parameters.AddWithValue("OrderDisplayId" + i, element.OrderDisplayId);
					sqlCommand.Parameters.AddWithValue("OperationValueAdding" + i, element.OperationValueAdding == null ? (object)DBNull.Value : element.OperationValueAdding);
					sqlCommand.Parameters.AddWithValue("WorkScheduleId" + i, element.WorkScheduleId);
					sqlCommand.Parameters.AddWithValue("Amount" + i, element.Amount);
					sqlCommand.Parameters.AddWithValue("Country_Id" + i, element.CountryId);
					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, element.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, element.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Departement_Id" + i, element.DepartementId);
					sqlCommand.Parameters.AddWithValue("Hall_Id" + i, element.HallId);
					sqlCommand.Parameters.AddWithValue("LotSizeSTD" + i, element.LotSizeSTD);
					sqlCommand.Parameters.AddWithValue("OperationNumber" + i, element.OperationNumber);
					sqlCommand.Parameters.AddWithValue("OperationTimeSeconds" + i, element.OperationTimeSeconds);
					sqlCommand.Parameters.AddWithValue("OperationTimeValueAdding" + i, element.OperationTimeValueAdding);
					sqlCommand.Parameters.AddWithValue("PredecessorOperation" + i, element.PredecessorOperation);
					sqlCommand.Parameters.AddWithValue("PredecessorSubOperation" + i, element.PredecessorSubOperation);
					sqlCommand.Parameters.AddWithValue("RelationOperationTime" + i, element.RelationOperationTime);
					sqlCommand.Parameters.AddWithValue("SetupTimeMinutes" + i, element.SetupTimeMinutes);
					sqlCommand.Parameters.AddWithValue("StandardOccupancy" + i, element.StandardOccupancy);
					sqlCommand.Parameters.AddWithValue("StandardOperation_Id" + i, element.StandardOperationId);
					sqlCommand.Parameters.AddWithValue("SubOperationNumber" + i, element.SubOperationNumber);
					sqlCommand.Parameters.AddWithValue("TotalTimeOperation" + i, element.TotalTimeOperation);
					sqlCommand.Parameters.AddWithValue("WorkArea_Id" + i, element.WorkAreaId);
					sqlCommand.Parameters.AddWithValue("WorkStationMachine_Id" + i, element.WorkStationMachineId == null ? (object)DBNull.Value : element.WorkStationMachineId);
					sqlCommand.Parameters.AddWithValue("FromToolInsert" + i, element.FromToolInsert == null ? (object)DBNull.Value : element.FromToolInsert);
					sqlCommand.Parameters.AddWithValue("FromToolInsert2" + i, element.FromToolInsert2 == null ? (object)DBNull.Value : element.FromToolInsert2);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("OperationDescription_Id" + i, element.OperationDescriptionId == null ? (object)DBNull.Value : element.OperationDescriptionId);
					sqlCommand.Parameters.AddWithValue("Comment" + i, element.Comment == null ? (object)DBNull.Value : element.Comment);

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

			string query = "DELETE FROM WorkScheduleDetails WHERE Id=@Id";

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

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM WorkScheduleDetails WHERE Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				int response = sqlCommand.ExecuteNonQuery();

				sqlConnection.Close();

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> GetByWorkScheduleId(int WorkScheduleId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE WorkScheduleId=@WorkScheduleId";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("WorkScheduleId", WorkScheduleId);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static int CountByWorkScheduleId(int WorkScheduleId)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM WorkScheduleDetails WHERE WorkScheduleId=@WorkScheduleId";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("WorkScheduleId", WorkScheduleId);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> GetByStandardOperationId(int StdOperationId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE StandardOperation_Id=@StandardOperation_Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("StandardOperation_Id", StdOperationId);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> GetByOperationDescriptionId(int OperationDescription_Id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE OperationDescription_Id=@OperationDescription_Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("OperationDescription_Id", OperationDescription_Id);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> GetByWorkStationMachineId(int WorkStationMachine_Id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE WorkStationMachine_Id=@WorkStationMachine_Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("WorkStationMachine_Id", WorkStationMachine_Id);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static int CountByWorkStationMachineId(int WorkStationMachine_Id)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM WorkScheduleDetails WHERE WorkStationMachine_Id=@WorkStationMachine_Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("WorkStationMachine_Id", WorkStationMachine_Id);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> GetByWorkAreaId(int WorkArea_Id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE WorkArea_Id=@WorkArea_Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("WorkArea_Id", WorkArea_Id);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static int CountByWorkAreaId(int WorkArea_Id)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM WorkScheduleDetails WHERE WorkArea_Id=@WorkArea_Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("WorkArea_Id", WorkArea_Id);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}

		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> GetByDepartementId(int Departement_Id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE Departement_Id=@Departement_Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Departement_Id", Departement_Id);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static int CountByDepartementId(int Departement_Id)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM WorkScheduleDetails WHERE Departement_Id=@Departement_Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Departement_Id", Departement_Id);
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> GetByHallId(int Hall_Id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE Hall_Id=@Hall_Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Hall_Id", Hall_Id);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> GetByCountryId(int Country_Id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkScheduleDetails WHERE Country_Id=@Country_Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Country_Id", Country_Id);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
			}
		}
		public static int CountByCountryId(int Country_Id)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM WorkScheduleDetails WHERE Country_Id=@Country_Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Country_Id", Country_Id);
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static int CountByHallId(int Hall_Id)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM WorkScheduleDetails WHERE Hall_Id=@Hall_Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Hall_Id", Hall_Id);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}

		public static int UpdateOrderDisplayId(int id, int orderDisplayId)
		{
			var sqlConnection = new SqlConnection(Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = " UPDATE WorkScheduleDetails SET "
				+ " OrderDisplayId=@OrderDisplayId "
				+ " WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("OrderDisplayId", orderDisplayId);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}

		public static int Update_OperationNumber_PredecessorOperation(int id,
			int operationNumber,
			int predecessorOperation)
		{
			var sqlConnection = new SqlConnection(Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = " UPDATE WorkScheduleDetails SET "
				+ " OperationNumber=@operationNumber, PredecessorOperation=@predecessorOperation "
				+ " WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("operationNumber", operationNumber);
			sqlCommand.Parameters.AddWithValue("predecessorOperation", predecessorOperation);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
