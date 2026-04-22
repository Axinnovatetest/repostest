using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class LogEntity
	{
		public string Action { get; set; }
		public int Id { get; set; }
		public int Type { get; set; }
		public int CreationUserId { get; set; }
		public DateTime CreationTime { get; set; }
		public LogEntity() { }

		public LogEntity(DataRow dr)
		{
			Action = dr["Action"].ToString();
			Id = int.Parse(dr["Id"].ToString());
			Type = int.Parse(dr["Type"].ToString());
			CreationUserId = int.Parse(dr["Creation_User_Id"].ToString());
			CreationTime = Convert.ToDateTime(dr["Creation_Date"]);
		}
	}
}
