using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class ArticleEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public bool IsArchived { get; set; }
		public int? ArchiveUserId { get; set; }
		public int HallId { get; set; }

		public ArticleEntity() { }

		public ArticleEntity(DataRow dr)
		{
			Id = int.Parse(dr["Id"].ToString());
			Name = (string)dr["Name"];
			CreationTime = Convert.ToDateTime(dr["Creation_Date"]);
			CreationUserId = int.Parse(dr["Creation_User_Id"].ToString());
			HallId = int.Parse(dr["Hall_Id"].ToString());
			LastEditTime = (dr["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Last_Edit_Date"]);
			LastEditUserId = (dr["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["Last_Edit_User_Id"]);
			ArchiveTime = (dr["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Delete_Date"]);
			ArchiveUserId = (dr["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["Delete_User_Id"]);
			IsArchived = Convert.ToBoolean(dr["Is_Archived"]);

		}
	}
}
