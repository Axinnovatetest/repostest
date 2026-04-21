using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Project_Log_BudgetEntity
	{
		public string Description { get; set; }
		public int? Id_Currency { get; set; }
		public int? Id_Customer { get; set; }
		public int? Id_Dept { get; set; }
		public int? Id_Land { get; set; }
		public int Id_proj { get; set; }
		public int Id_Responsable { get; set; }
		public int Id_State { get; set; }
		public int Id_Type { get; set; }
		public string Name_proj { get; set; }
		public string Nr_Customer { get; set; }
		public double Proj_Budget { get; set; }
		public DateTime? Action_date { get; set; }
		public int Id_LS { get; set; }
		public int Id_state { get; set; }
		public int Id_user { get; set; }



		public Project_Log_BudgetEntity() { }

		public Project_Log_BudgetEntity(DataRow dataRow)
		{
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id_Currency = (dataRow["Id_Currency"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency"]);
			//Id_Customer = Convert.ToInt32(dataRow["Id_Customer"]);
			Id_Customer = (dataRow["Id_Customer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Customer"]);
			Id_Dept = (dataRow["Id_Dept"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept"]);
			//Id_Dept = Convert.ToInt32(dataRow["Id_Dept"]);
			Id_Land = (dataRow["Id_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Land"]);
			Id_proj = Convert.ToInt32(dataRow["Id_proj"]);
			Id_Responsable = Convert.ToInt32(dataRow["Id_Responsable"]);
			Id_State = Convert.ToInt32(dataRow["Id_State"]);
			Id_Type = Convert.ToInt32(dataRow["Id_Type"]);
			Name_proj = Convert.ToString(dataRow["Name_proj"]);
			Nr_Customer = (dataRow["Nr_Customer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nr_Customer"]);
			//Nr_Customer = Convert.ToString(dataRow["Nr_Customer"]);
			//Proj_Budget = Convert.ToDouble(dataRow["Proj_Budget"]);
			Proj_Budget = double.Parse(dataRow["Proj_Budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			Action_date = (dataRow["Action_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Action_date"]);
			Id_LS = Convert.ToInt32(dataRow["Id_LS"]);
			Id_state = Convert.ToInt32(dataRow["Id_state"]);
			Id_user = Convert.ToInt32(dataRow["Id_user"]);

		}
	}
}

