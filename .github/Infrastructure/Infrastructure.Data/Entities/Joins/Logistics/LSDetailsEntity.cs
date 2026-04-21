using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class LSDetailsEntity
	{
		public string artikelnummer { get; set; }
		public string ablastelle { get; set; }
		public int position { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung2 { get; set; }
		public DateTime? indexKundeDatum { get; set; }
		public string indexKunde { get; set; }
		public string ursprungsland { get; set; }
		public string zolltarifNummer { get; set; }
		public decimal grosse { get; set; }
		public string einheit { get; set; }
		public decimal gesammtGewicht { get; set; }
		public decimal anzahl { get; set; }
		public decimal ust { get; set; }
		public bool rp { get; set; }
		public string posText { get; set; }
		public decimal anzahlSpezifisch { get; set; }//Wenn([Typ]="Lieferschein";Wenn([LVorname/NameFirma]="Sirona Dental Systems GmbH";"*" & Berichte![PSZ_Packliste_LS_von Versand]!Anzahl & "*";""))


		public LSDetailsEntity()
		{

		}
		public LSDetailsEntity(DataRow dataRow)
		{
			artikelnummer = (dataRow["artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelnummer"]);
			ablastelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
			position = (dataRow["position"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["position"]);
			bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			bezeichnung2 = (dataRow["Bezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung2"]);
			indexKundeDatum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
			indexKunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
			ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
			zolltarifNummer = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			grosse = (dataRow["grosse"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["grosse"]);
			einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			gesammtGewicht = (dataRow["Gesamtgewicht"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gesamtgewicht"]);
			anzahl = (dataRow["anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["anzahl"]);
			ust = (dataRow["ust"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["ust"]);
			rp = (dataRow["rp"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["rp"]);
			posText = (dataRow["POSTEXT"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["POSTEXT"]);




		}
	}
}
