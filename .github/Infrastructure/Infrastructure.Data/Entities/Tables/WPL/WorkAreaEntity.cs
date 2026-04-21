using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class WorkAreaEntity
	{
		public int CountryId { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public int? DepartmentId { get; set; }
		public int HallId { get; set; }
		public int Id { get; set; }
		public bool IsArchived { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public string Name { get; set; }

		public WorkAreaEntity() { }

		public WorkAreaEntity(DataRow dataRow)
		{
			CountryId = Convert.ToInt32(dataRow["Country_Id"]);
			CreationTime = Convert.ToDateTime(dataRow["Creation_Date"]);
			CreationUserId = Convert.ToInt32(dataRow["Creation_User_Id"]);
			ArchiveTime = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			ArchiveUserId = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			DepartmentId = (dataRow["Department_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Department_Id"]);
			HallId = Convert.ToInt32(dataRow["Hall_Id"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsArchived = Convert.ToBoolean(dataRow["Is_Archived"]);
			LastEditTime = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			LastEditUserId = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			Name = Convert.ToString(dataRow["Name"]);
		}
	}
}
