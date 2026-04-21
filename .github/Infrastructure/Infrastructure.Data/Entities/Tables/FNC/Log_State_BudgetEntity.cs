using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Log_State_BudgetEntity
	{
		public DateTime? Action_date { get; set; }
		public int Id_LS { get; set; }
		public int Id_proj { get; set; }
		public int Id_state { get; set; }
		public int Id_user { get; set; }

		public Log_State_BudgetEntity() { }

		public Log_State_BudgetEntity(DataRow dataRow)
		{
			Action_date = (dataRow["Action_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Action_date"]);
			Id_LS = Convert.ToInt32(dataRow["Id_LS"]);
			Id_proj = Convert.ToInt32(dataRow["Id_proj"]);
			Id_state = Convert.ToInt32(dataRow["Id_state"]);
			Id_user = Convert.ToInt32(dataRow["Id_user"]);
		}
	}
}

