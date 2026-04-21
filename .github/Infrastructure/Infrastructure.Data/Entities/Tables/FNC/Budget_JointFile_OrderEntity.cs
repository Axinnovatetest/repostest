using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_JointFile_OrderEntity
	{
		public string Action_File { get; set; }
		public DateTime? File_date { get; set; }
		public int FileId { get; set; }
		public int Id_File { get; set; }
		public int? Id_Order { get; set; }
		public int Id_Order_Version { get; set; }
		public int Id_User { get; set; }

		public Budget_JointFile_OrderEntity() { }

		public Budget_JointFile_OrderEntity(DataRow dataRow)
		{
			Action_File = (dataRow["Action_File"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Action_File"]);
			File_date = (dataRow["File_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["File_date"]);
			FileId = Convert.ToInt32(dataRow["FileId"]);
			Id_File = Convert.ToInt32(dataRow["Id_File"]);
			Id_Order = (dataRow["Id_Order"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Order"]);
			Id_Order_Version = Convert.ToInt32(dataRow["Id_Order_Version"]);
			Id_User = Convert.ToInt32(dataRow["Id_User"]);
		}
	}
}

