using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.WPL
{
	public class WorkPlanMinimalEntity
	{
		public int Id { get; set; }
		public string Work_Plan { get; set; }
		public int Hall_Id { get; set; }
		public string Hall { get; set; }
		public int Country_Id { get; set; }
		public string Country { get; set; }
		public int Article_Id { get; set; }
		public string Article_Nummer { get; set; }
		public int WpCount { get; set; }
		public int WSCount { get; set; }
		public bool Is_Active { get; set; }
		public DateTime Creation_Date { get; set; }
		public int? Creation_User_Id { get; set; }
		public DateTime? Last_Edit_Date { get; set; }
		public int? Last_Edit_User_Id { get; set; }
		public string Last_Edit_Username { get; set; }

		public WorkPlanMinimalEntity(DataRow dataRow)
		{
			Id = dataRow["Id"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Id"]);
			Work_Plan = dataRow["Work_Plan"] == DBNull.Value ? null : dataRow["Work_Plan"].ToString();
			Hall_Id = dataRow["Hall_Id"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Hall_Id"]);
			Hall = dataRow["Hall"] == DBNull.Value ? null : dataRow["Hall"].ToString();
			Country_Id = dataRow["Country_Id"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Country_Id"]);
			Country = dataRow["Country"] == DBNull.Value ? null : dataRow["Country"].ToString();
			Article_Id = dataRow["Article_Id"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Article_Id"]);
			Article_Nummer = dataRow["Article_Nummer"] == DBNull.Value ? null : dataRow["Article_Nummer"].ToString();
			WpCount = dataRow["wpCount"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["wpCount"]);
			WSCount = dataRow["WSCount"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["WSCount"]);
			Is_Active = dataRow["Is_Active"] != DBNull.Value && Convert.ToBoolean(dataRow["Is_Active"]);
			Creation_Date = dataRow["Creation_Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Creation_Date"]);
			Creation_User_Id = dataRow["Creation_User_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Creation_User_Id"]);
			Last_Edit_Date = dataRow["Last_Edit_Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			Last_Edit_User_Id = dataRow["Last_Edit_User_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			Last_Edit_Username = dataRow["Last_Edit_Username"] == DBNull.Value ? null : dataRow["Last_Edit_Username"].ToString();

		}

	}
}
