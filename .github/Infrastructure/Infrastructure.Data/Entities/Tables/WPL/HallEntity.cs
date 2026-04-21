using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class HallEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Adress { get; set; }
		public int CountryId { get; set; }

		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public Boolean IsArchived { get; set; }


		public int? LagerortId { get; set; }
		public HallEntity() { }
		public HallEntity(DataRow dataRow)
		{
			Id = int.Parse(dataRow["Id"].ToString());
			Name = (string)dataRow["Name"];
			Adress = (string)dataRow["Area"];
			CountryId = int.Parse(dataRow["Country_Id"].ToString());

			CreationTime = (DateTime)dataRow["Creation_Date"];
			CreationUserId = int.Parse(dataRow["Creation_User_Id"].ToString());
			ArchiveTime = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			ArchiveUserId = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			LastEditTime = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			LastEditUserId = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			IsArchived = (Boolean)dataRow["Is_Archived"];
			LagerortId = (dataRow["LagerortId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerortId"]);
		}
	}

	public class HallLagersEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Adress { get; set; }
		public int CountryId { get; set; }
		public int? LagerortId { get; set; }
		public Boolean IsArchived { get; set; }
		public HallLagersEntity() { }
		public HallLagersEntity(DataRow dataRow)
		{
			Id = int.Parse(dataRow["Id"].ToString());
			Name = (string)dataRow["Name"];
			Adress = (string)dataRow["Area"];
			CountryId = int.Parse(dataRow["Country_Id"].ToString());
			LagerortId = (dataRow["LagerortId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerortId"]);
			IsArchived = (Boolean)dataRow["Is_Archived"];
		}
	}
}
