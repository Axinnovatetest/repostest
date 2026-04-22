using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Fertigung_PlanungsdetailsEntity
	{
		public string Aktion { get; set; }
		public string Details { get; set; }
		public int ID { get; set; }
		public int? ID_Fertigung { get; set; }
		public string Mitarbeiter { get; set; }
		public bool? Status { get; set; }
		public DateTime? Termin { get; set; }

		public Fertigung_PlanungsdetailsEntity() { }

		public Fertigung_PlanungsdetailsEntity(DataRow dataRow)
		{
			Aktion = (dataRow["Aktion"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Aktion"]);
			Details = (dataRow["Details"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Details"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Fertigung = (dataRow["ID_Fertigung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Fertigung"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Status"]);
			Termin = (dataRow["Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin"]);
		}
	}
}

