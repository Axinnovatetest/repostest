using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class WareneingangReportEntity
	{
		public string NrTransfer { get; set; }
		public string Bestellung_Nr { get; set; }
		public string Lagerort_id { get; set; }
		public string Projekt_Nr { get; set; }
		public string Eingangslieferscheinnr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Bestellnummer { get; set; }
		public string Menge { get; set; }
		public string Einheit { get; set; }
		public string LT { get; set; }
		public string Umsatzsteuer { get; set; }
		public string Einzelpreis { get; set; }
		public string preiseinheit { get; set; }
		public string Rabatt1 { get; set; }
		public string Rabatt2 { get; set; }
		public decimal Gesamtpreis { get; set; }
		public bool EMPB_Bestatigung { get; set; }
		public string Fertigung { get; set; }
		public string best_art_nr { get; set; }
		public string Liefertermin { get; set; }
		public string Vorname_NameFirma { get; set; }
		public bool Kanban { get; set; }
		public string Lagerort_Projekt_Nr { get; set; }
		public string Code { get; set; }
		public string WEK { get; set; }
		public string Lieferantennummer { get; set; }
		public string Kundennummer_Lieferanten { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public WareneingangReportEntity() { }
		public WareneingangReportEntity(DataRow dataRow)
		{

			NrTransfer = (dataRow["NrTransfer"] == System.DBNull.Value) ? "" : dataRow["NrTransfer"].ToString();
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? "" : dataRow["Bestellung-Nr"].ToString();
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : dataRow["Projekt-Nr"].ToString();
			Eingangslieferscheinnr = (dataRow["Eingangslieferscheinnr"] == System.DBNull.Value) ? "" : dataRow["Eingangslieferscheinnr"].ToString();
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
			Lagerort_Projekt_Nr = (dataRow["Lagerort_Projekt_Nr"] == System.DBNull.Value) ? "" : dataRow["Lagerort_Projekt_Nr"].ToString();
			Fertigung = (dataRow["Fertigung"] == System.DBNull.Value) ? "0" : dataRow["Fertigung"].ToString();
			//Fertigung01 = (dataRow["Fertigung01"] == System.DBNull.Value) ? "0" : dataRow["Fertigung01"].ToString();
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? "0" : dataRow["Lagerort_id"].ToString();
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : dataRow["Vorname/NameFirma"].ToString();
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung 1"].ToString();
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung 2"].ToString();
			preiseinheit = (dataRow["preiseinheit"] == System.DBNull.Value) ? "0" : dataRow["preiseinheit"].ToString();
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : dataRow["Bestellnummer"].ToString();
			Rabatt1 = (dataRow["Rabatt1"] == System.DBNull.Value) ? "0" : dataRow["Rabatt1"].ToString();
			Rabatt2 = (dataRow["Rabatt2"] == System.DBNull.Value) ? "0" : dataRow["Rabatt2"].ToString();
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? "0" : dataRow["Menge"].ToString();
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? "0" : dataRow["Einzelpreis"].ToString();
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gesamtpreis"].ToString());
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "0" : dataRow["Einheit"].ToString();
			best_art_nr = (dataRow["best_art_nr"] == System.DBNull.Value) ? "0" : dataRow["best_art_nr"].ToString();
			LT = dataRow["LT"].ToString();
			WEK = dataRow["WEK"].ToString();
			Liefertermin = dataRow["Liefertermin"].ToString();
			EMPB_Bestatigung = (dataRow["EMPB_Bestätigung"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["EMPB_Bestätigung"].ToString());
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Kanban"].ToString());
			Code = (dataRow["Code"] == System.DBNull.Value) ? "0" : dataRow["Code"].ToString();
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? "0" : dataRow["Umsatzsteuer"].ToString();
			Lieferantennummer = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? "0" : dataRow["Lieferantennummer"].ToString();
			Land_PLZ_Ort = (dataRow["Land/PLZ/Ort"] == System.DBNull.Value) ? "0" : dataRow["Land/PLZ/Ort"].ToString();
		}
		private string getLagerLabel(string Lagerortid)
		{
			switch(Lagerortid)
			{
				case "4":
					return "TN-";
				case "41":
					return "WS-";
				case "15":
					return "FD-";
				case "20":
					return "SC-";
				case "8":
					return "HD-";
				case "24":
					return "AL-";
				case "58":
					return "BE-";
				default:
					return "CZ-";
			}
		}
	}
}
