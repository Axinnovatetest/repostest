using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class StandardOperationDescriptionEntity
	{
		public string Code { get; set; }
		public DateTime Creation_Date { get; set; }
		public int Creation_User_Id { get; set; }
		public DateTime? Delete_Date { get; set; }
		public int? Delete_User_Id { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public bool Is_Archived { get; set; }
		public DateTime? Last_Edit_Date { get; set; }
		public int? Last_Edit_User_Id { get; set; }
		public string LotPiece { get; set; }
		public string MachineToolInsert { get; set; }
		public string ManuelMachinel { get; set; }
		public bool Operation_Value_Adding { get; set; }
		public string ReationSetup { get; set; }
		public string RelationOperationSetup { get; set; }
		public string Remark { get; set; }
		public string Remark2 { get; set; }
		public string SecondsPerSubOperation { get; set; }
		public string Setup { get; set; }
		public int StdOperationId { get; set; }
		public string TechnologieArea { get; set; }
		public string ValueAdding { get; set; }
		public StandardOperationDescriptionEntity() { }
		public StandardOperationDescriptionEntity(DataRow dataRow)
		{
			Code = (dataRow["Code"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Code"]);
			Creation_Date = Convert.ToDateTime(dataRow["Creation_Date"]);
			Creation_User_Id = Convert.ToInt32(dataRow["Creation_User_Id"]);
			Delete_Date = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			Delete_User_Id = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			Description = Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Is_Archived = Convert.ToBoolean(dataRow["Is_Archived"]);
			Last_Edit_Date = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			Last_Edit_User_Id = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			LotPiece = (dataRow["LotPiece"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LotPiece"]);
			MachineToolInsert = (dataRow["MachineToolInsert"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["MachineToolInsert"]);
			ManuelMachinel = (dataRow["ManuelMachinel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ManuelMachinel"]);
			Operation_Value_Adding = Convert.ToBoolean(dataRow["Operation_Value_Adding"]);
			ReationSetup = (dataRow["ReationSetup"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReationSetup"]);
			RelationOperationSetup = (dataRow["RelationOperationSetup"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RelationOperationSetup"]);
			Remark = (dataRow["Remark"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Remark"]);
			Remark2 = (dataRow["Remark2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Remark2"]);
			SecondsPerSubOperation = (dataRow["SecondsPerSubOperation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SecondsPerSubOperation"]);
			Setup = (dataRow["Setup"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Setup"]);
			StdOperationId = Convert.ToInt32(dataRow["StdOperationId"]);
			TechnologieArea = (dataRow["TechnologieArea"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TechnologieArea"]);
			ValueAdding = (dataRow["ValueAdding"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ValueAdding"]);
		}
	}
}
