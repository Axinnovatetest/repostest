using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class WorkStationMachineEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int HallId { get; set; }
		public int Type { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? DeleteTime { get; set; }
		public int? DeleteUserId { get; set; }
		public bool IsArchived { get; set; }
		public int CountryId { get; set; }
		public int WorkAreaId { get; set; }
		public WorkStationMachineEntity() { }
		public WorkStationMachineEntity(DataRow dataRow)
		{
			Id = int.Parse(dataRow["Id"].ToString());
			Name = (string)dataRow["Name"];
			HallId = int.Parse(dataRow["Hall_Id"].ToString());
			Type = int.Parse(dataRow["Type"].ToString());
			CountryId = int.Parse(dataRow["Country_Id"].ToString());
			CreationTime = (DateTime)dataRow["Creation_Date"];
			CreationUserId = int.Parse(dataRow["Creation_User_Id"].ToString());
			WorkAreaId = int.Parse(dataRow["Work_Area_Id"].ToString());
			LastEditTime = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			LastEditUserId = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			DeleteUserId = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			DeleteTime = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			IsArchived = Convert.ToBoolean(dataRow["Is_Archived"]);
		}
	}
}
