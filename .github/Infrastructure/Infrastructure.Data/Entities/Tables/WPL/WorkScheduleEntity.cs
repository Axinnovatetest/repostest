using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class WorkPlanEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int HallId { get; set; }
		public int ArticleId { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? LastEditTime { get; set; }

		public WorkPlanEntity() { }

		public WorkPlanEntity(DataRow dr)
		{
			Name = dr["Name"].ToString();
			Id = int.Parse(dr["Id"].ToString());
			HallId = int.Parse(dr["Hall_Id"].ToString());
			ArticleId = int.Parse(dr["Article_Id"].ToString());
			IsActive = Convert.ToBoolean(dr["Is_Active"]);
			CreationTime = Convert.ToDateTime(dr["Creation_Date"]);
			CreationUserId = int.Parse(dr["Creation_User_Id"].ToString());
			LastEditTime = (dr["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Last_Edit_Date"]);
			LastEditUserId = (dr["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["Last_Edit_User_Id"]);
		}
	}
}
