using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class WorkScheduleDetailsEntity
	{
		public int WorkScheduleId { get; set; }
		public decimal Amount { get; set; }
		public int CountryId { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int DepartementId { get; set; }
		public string FromToolInsert { get; set; }
		public string FromToolInsert2 { get; set; }
		public int HallId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int LotSizeSTD { get; set; }
		public int? OperationDescriptionId { get; set; }
		public int OperationNumber { get; set; }
		public decimal OperationTimeSeconds { get; set; }
		public double OperationTimeValueAdding { get; set; }
		public int PredecessorOperation { get; set; }
		public string PredecessorSubOperation { get; set; }
		public double RelationOperationTime { get; set; }
		public decimal SetupTimeMinutes { get; set; }
		public double StandardOccupancy { get; set; }
		public int StandardOperationId { get; set; }
		public string SubOperationNumber { get; set; }
		public decimal TotalTimeOperation { get; set; }
		public int WorkAreaId { get; set; }
		public string Comment { get; set; }
		public int? WorkStationMachineId { get; set; }
		public bool? OperationValueAdding { get; set; }
		public int OrderDisplayId { get; set; }
		//names
		public string CountryName { get; set; }
		public string HallName { get; set; }
		public string DepartmentName { get; set; }
		public string WorkAreaName { get; set; }
		public string WorkStationMachineName { get; set; }
		public string StandardOperationName { get; set; }
		public string OperationDescriptionName { get; set; }

		public WorkScheduleDetailsEntity() { }
		public WorkScheduleDetailsEntity(DataRow dataRow)
		{
			WorkScheduleId = int.Parse(dataRow["WorkScheduleId"].ToString());
			OrderDisplayId = int.Parse(dataRow["OrderDisplayId"].ToString());
			Amount = decimal.Parse(dataRow["Amount"].ToString());
			CountryId = int.Parse(dataRow["Country_Id"].ToString());
			CreationTime = (DateTime)dataRow["Creation_Date"];
			CreationUserId = int.Parse(dataRow["Creation_User_Id"].ToString());
			DepartementId = int.Parse(dataRow["Departement_Id"].ToString());
			FromToolInsert = (dataRow["FromToolInsert"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FromToolInsert"]);
			FromToolInsert2 = (dataRow["FromToolInsert2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FromToolInsert2"]);
			Comment = (dataRow["Comment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment"]);
			HallId = int.Parse(dataRow["Hall_Id"].ToString());
			Id = int.Parse(dataRow["Id"].ToString());
			LastEditTime = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			LastEditUserId = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			LotSizeSTD = int.Parse(dataRow["LotSizeSTD"].ToString());
			OperationDescriptionId = (dataRow["OperationDescription_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OperationDescription_Id"]);
			OperationNumber = int.Parse(dataRow["OperationNumber"].ToString());
			OperationTimeSeconds = decimal.Parse(dataRow["OperationTimeSeconds"].ToString());
			OperationTimeValueAdding = double.Parse(dataRow["OperationTimeValueAdding"].ToString());
			PredecessorOperation = int.Parse(dataRow["PredecessorOperation"].ToString());
			PredecessorSubOperation = dataRow["PredecessorSubOperation"].ToString();
			RelationOperationTime = double.Parse(dataRow["RelationOperationTime"].ToString());
			SetupTimeMinutes = decimal.Parse(dataRow["SetupTimeMinutes"].ToString());
			StandardOccupancy = double.Parse(dataRow["StandardOccupancy"].ToString());
			StandardOperationId = int.Parse(dataRow["StandardOperation_Id"].ToString());
			SubOperationNumber = dataRow["SubOperationNumber"].ToString();
			TotalTimeOperation = decimal.Parse(dataRow["TotalTimeOperation"].ToString());
			WorkAreaId = int.Parse(dataRow["WorkArea_Id"].ToString());
			WorkStationMachineId = (dataRow["WorkStationMachine_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WorkStationMachine_Id"]);
			OperationValueAdding = (dataRow["Operation_Value_Adding"] == System.DBNull.Value) ? (Boolean?)null : Convert.ToBoolean(dataRow["Operation_Value_Adding"]);
			CountryName = dataRow.Table.Columns.Contains("CountryName") && dataRow["CountryName"] != DBNull.Value ? dataRow["CountryName"].ToString() : "";
			HallName = dataRow.Table.Columns.Contains("HallName") && dataRow["HallName"] != DBNull.Value ? dataRow["HallName"].ToString() : "";
			DepartmentName = dataRow.Table.Columns.Contains("DepartmentName") && dataRow["DepartmentName"] != DBNull.Value ? dataRow["DepartmentName"].ToString() : "";
			WorkAreaName = dataRow.Table.Columns.Contains("WorkAreaName") && dataRow["WorkAreaName"] != DBNull.Value ? dataRow["WorkAreaName"].ToString() : "";
			WorkStationMachineName = dataRow.Table.Columns.Contains("WorkStationMachineName") && dataRow["WorkStationMachineName"] != DBNull.Value ? dataRow["WorkStationMachineName"].ToString() : "";
			StandardOperationName = dataRow.Table.Columns.Contains("StandardOperationName") && dataRow["StandardOperationName"] != DBNull.Value ? dataRow["StandardOperationName"].ToString() : "";
			OperationDescriptionName = dataRow.Table.Columns.Contains("OperationDescriptionName") && dataRow["OperationDescriptionName"] != DBNull.Value ? dataRow["OperationDescriptionName"].ToString() : "";
		}
	}
}

