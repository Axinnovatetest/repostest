using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class LierferPlannungEntity
	{
		public string Dokumentnummer { get; set; }
		public string Vorname_NameFirma { get; set; }
		public string LName2 { get; set; }
		public string LLand_PLZ_Ort { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int? Menge_Offen { get; set; }
		public int? Bestand { get; set; }
		public int? Jahr { get; set; }
		public int? KW { get; set; }
		public Decimal? Gesamtpreis { get; set; }
		public string CSInterneBemerkung { get; set; }

		public LierferPlannungEntity(DataRow dataRow)
		{
			Dokumentnummer = (dataRow["Dokumentnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dokumentnummer"]);
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			LName2 = (dataRow["LName2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName2"]);
			LLand_PLZ_Ort = (dataRow["LLand_PLZ_Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LLand_PLZ_Ort"]);
			Wunschtermin = (dataRow["Wunschtermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wunschtermin"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Angebot_Nr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Menge_Offen = (dataRow["Menge Offen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge Offen"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestand"]);
			Jahr = (dataRow["Jahr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Jahr"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			CSInterneBemerkung = (dataRow["CSInterneBemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CSInterneBemerkung"]);
		}
	}
}
