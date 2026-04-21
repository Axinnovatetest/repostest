using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class StucklistenEntity
	{
		public double? Anzahl { get; set; }
		public int? Artikel_Nr { get; set; }
		public int? Artikel_Nr_des_Bauteils { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_des_Bauteils { get; set; }
		public int Nr { get; set; }
		public string Position { get; set; }
		public string Variante { get; set; }
		public int? Vorgang_Nr { get; set; }

		public StucklistenEntity() { }
		public StucklistenEntity(DataRow dataRow)
		{
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Anzahl"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikel_Nr_des_Bauteils = (dataRow["Artikel-Nr des Bauteils"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr des Bauteils"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
			Variante = (dataRow["Variante"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Variante"]);
			Vorgang_Nr = (dataRow["Vorgang_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Vorgang_Nr"]);
		}
	}
}

