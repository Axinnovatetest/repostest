using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class FAVerschiebungEntity
	{
		public int ID { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Termin_Ursprunglich { get; set; }
		public DateTime? Termin_voranderung { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public string Kennzeichen { get; set; }
		public DateTime? Anderungsdatum { get; set; }
		public DateTime? Zeitraum { get; set; }
		public FAVerschiebungEntity()
		{

		}
		public FAVerschiebungEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"].ToString());
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"].ToString());
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"].ToString());
			Termin_Ursprunglich = (dataRow["Termin_Ursprünglich"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Ursprünglich"].ToString());
			Termin_voranderung = (dataRow["Termin_voränderung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_voränderung"].ToString());
			Termin_Bestatigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"].ToString());
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"].ToString());
			Anderungsdatum = (dataRow["Änderungsdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Änderungsdatum"].ToString());
			Zeitraum = (dataRow["Zeitraum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Zeitraum"].ToString());
		}
	}
}
