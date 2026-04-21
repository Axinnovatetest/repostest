using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Fertigung_FertigungsvorgangEntity
	{
		public bool? ab_buchen { get; set; }
		public decimal? AnfangLagerBestand { get; set; }
		public Single? Anzahl { get; set; }
		public int? Artikel_nr { get; set; }
		public DateTime? Datum { get; set; }
		public decimal? EndeLagerBestand { get; set; }
		public int? Fertigung_Nr { get; set; }
		public int ID { get; set; }
		public int? Lagerort_id { get; set; }
		public bool? Löschen { get; set; }
		public string Mitarbeiter { get; set; }
		public int? Personal_Nr { get; set; }
		public int? Vorgang { get; set; }

		public Fertigung_FertigungsvorgangEntity() { }

		public Fertigung_FertigungsvorgangEntity(DataRow dataRow)
		{
			ab_buchen = (dataRow["ab_buchen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ab_buchen"]);
			AnfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Anzahl"]);
			Artikel_nr = (dataRow["Artikel_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_nr"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
			Fertigung_Nr = (dataRow["Fertigung_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigung_Nr"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter"]);
			Personal_Nr = (dataRow["Personal_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personal_Nr"]);
			Vorgang = (dataRow["Vorgang"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Vorgang"]);
		}
	}
}

