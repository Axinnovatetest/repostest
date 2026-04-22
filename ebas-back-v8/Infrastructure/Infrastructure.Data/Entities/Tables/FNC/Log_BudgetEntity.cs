using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Log_BudgetEntity
	{
		public string Action { get; set; }
		public DateTime Creation_Date { get; set; }
		public int Creation_User_Id { get; set; }
		public int Id { get; set; }
		public int Type { get; set; }

		public Log_BudgetEntity() { }

		public Log_BudgetEntity(DataRow dataRow)
		{
			Action = Convert.ToString(dataRow["Action"]);
			Creation_Date = Convert.ToDateTime(dataRow["Creation_Date"]);
			Creation_User_Id = Convert.ToInt32(dataRow["Creation_User_Id"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Type = Convert.ToInt32(dataRow["Type"]);
		}
	}
}

