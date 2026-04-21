using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class WorkPlansEntity
	{
		public string? Hall { get; set; }
		public string? Country { get; set; }
		public string? Article_Nummer { get; set; }
		public bool? Is_Active { get; set; }
		public int? Wp_Id { get; set; }
		public string? Work_Plan { get; set; }
		public int? Hall_Id { get; set; }
		public int? Article_Id { get; set; }
		public int? wpCount { get; set; }

		public DateTime? Creation_Date { get; set; }
		public int? Creation_User_Id { get; set; }
		public DateTime? Last_Edit_Date { get; set; }
		public int? Last_Edit_User_Id { get; set; }
		public WorkPlansEntity(DataRow dataRow) 
		{
			Hall = dataRow["Hall"] == DBNull.Value ? null : Convert.ToString(dataRow["Hall"]);
			Country = dataRow["Country"] == DBNull.Value ? null : Convert.ToString(dataRow["Country"]);
			Article_Nummer = dataRow["Article_Nummer"] == DBNull.Value ? null : Convert.ToString(dataRow["Article_Nummer"]);
			Is_Active = dataRow["Is_Active"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Is_Active"]);
			Wp_Id = dataRow["Wp_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Wp_Id"]);
			Work_Plan = dataRow["Work_Plan"] == DBNull.Value ? null : Convert.ToString(dataRow["Work_Plan"]);
			Hall_Id = dataRow["Hall_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Hall_Id"]);
			Article_Id = dataRow["Article_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Article_Id"]);
			wpCount = dataRow["wpCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["wpCount"]);
			Creation_Date = dataRow["Creation_Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Creation_Date"]);
			Creation_User_Id = dataRow["Creation_User_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Creation_User_Id"]);
			Last_Edit_Date = dataRow["Last_Edit_Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			Last_Edit_User_Id = dataRow["Last_Edit_User_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
		}
	}
}
