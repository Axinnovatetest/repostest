using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ArtikelhistorieEntity
	{
		public string ArtikelHistorie { get; set; }
		public string Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string COF_Pflichtig_Geändert_auf { get; set; }
		public DateTime? Datum { get; set; }
		public string Geänderte_COC { get; set; }
		public string Geänderte_MHD { get; set; }
		public string Gelöschte_Artikel { get; set; }
		public int ID { get; set; }
		public string MHD_geändert_Auf { get; set; }
		public string Mitarbeiter { get; set; }
		public string Zeitraum_MHD { get; set; }

		public ArtikelhistorieEntity() { }

		public ArtikelhistorieEntity(DataRow dataRow)
		{
			ArtikelHistorie = (dataRow["ArtikelHistorie"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelHistorie"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			COF_Pflichtig_Geändert_auf = (dataRow["COF_Pflichtig_Geändert_auf"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["COF_Pflichtig_Geändert_auf"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Geänderte_COC = (dataRow["Geänderte_COC"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Geänderte_COC"]);
			Geänderte_MHD = (dataRow["Geänderte_MHD"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Geänderte_MHD"]);
			Gelöschte_Artikel = (dataRow["Gelöschte_Artikel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gelöschte_Artikel"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			MHD_geändert_Auf = (dataRow["MHD_geändert_Auf"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["MHD_geändert_Auf"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter"]);
			Zeitraum_MHD = (dataRow["Zeitraum_MHD"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeitraum_MHD"]);
		}
	}
}

