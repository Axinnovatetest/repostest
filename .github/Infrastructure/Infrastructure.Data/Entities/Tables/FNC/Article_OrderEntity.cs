using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Article_OrderEntity
	{
		public string AB_Nr_Lieferant { get; set; }
		public int? Account_Id { get; set; }
		public string Account_Name { get; set; }
		public decimal? Aktuelle_Anzahl { get; set; }
		public decimal? AnfangLagerBestand { get; set; }
		public decimal? Anzahl { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Bemerkung_Pos { get; set; }
		public bool? Bemerkung_Pos_ID { get; set; }
		public DateTime? Bestätigter_Termin { get; set; }
		public string Bestellnummer { get; set; }
		public int? Bestellung_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public int? BP_zu_RBposition { get; set; }
		public bool? COC_bestatigung { get; set; }
		public DateTime? Confirmation_Date { get; set; }
		public decimal? CUPreis { get; set; }
		public string Currency_Article { get; set; }
		public DateTime? Delivery_Date { get; set; }
		public string Description { get; set; }
		public decimal? Discount { get; set; }
		public string Einheit { get; set; }
		public decimal? Einzelpreis { get; set; }
		public bool? EMPB_Bestatigung { get; set; }
		public decimal? EndeLagerBestand { get; set; }
		public decimal? Erhalten { get; set; }
		public bool? erledigt_pos { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public int Id_AO { get; set; }
		public int Id_Article { get; set; }
		public int? Id_Currency_Article { get; set; }
		public int Id_Order { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public string InfoRahmennummer { get; set; }
		public string Internal_Contact { get; set; }
		public bool? Kanban { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int? Location_Id { get; set; }
		public string Location_Name { get; set; }
		public bool? Löschen { get; set; }
		public DateTime? MhdDatumArtikel { get; set; }
		public int? Position { get; set; }
		public bool? Position_erledigt { get; set; }
		public decimal? Preiseinheit { get; set; }
		public int? Preisgruppe { get; set; }
		public int? Produktionsort { get; set; }
		public decimal? Quantity { get; set; }
		public double? Rabatt { get; set; }
		public double? Rabatt1 { get; set; }
		public decimal? RB_Abgerufen { get; set; }
		public decimal? RB_Offen { get; set; }
		public decimal? RB_OriginalAnzahl { get; set; }
		public string schriftart { get; set; }
		public string sortierung { get; set; }
		public decimal? Start_Anzahl { get; set; }
		public double? TotalCost_Article { get; set; }
		public double? Umsatzsteuer { get; set; }
		public double? Unit_Price { get; set; }
		public decimal? VAT { get; set; }
		public int? WE_Pos_zu_Bestellposition { get; set; }

		public Article_OrderEntity() { }

		public Article_OrderEntity(DataRow dataRow)
		{
			AB_Nr_Lieferant = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
			Account_Id = (dataRow["Account_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Account_Id"]);
			Account_Name = (dataRow["Account_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Account_Name"]);
			Aktuelle_Anzahl = (dataRow["Aktuelle Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aktuelle Anzahl"]);
			AnfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bemerkung_Pos = (dataRow["Bemerkung_Pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Pos"]);
			Bemerkung_Pos_ID = (dataRow["Bemerkung_Pos_ID"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bemerkung_Pos_ID"]);
			Bestätigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellnummer"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			BP_zu_RBposition = (dataRow["BP zu RBposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BP zu RBposition"]);
			COC_bestatigung = (dataRow["COC_bestatigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COC_bestatigung"]);
			Confirmation_Date = (dataRow["Confirmation_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Confirmation_Date"]);
			CUPreis = (dataRow["CUPreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CUPreis"]);
			Currency_Article = (dataRow["Currency_Article"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Currency_Article"]);
			Delivery_Date = (dataRow["Delivery_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delivery_Date"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Discount = (dataRow["Discount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Discount"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelpreis"]);
			EMPB_Bestatigung = (dataRow["EMPB_Bestatigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Bestatigung"]);
			EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
			Erhalten = (dataRow["Erhalten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Erhalten"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			Id_AO = Convert.ToInt32(dataRow["Id_AO"]);
			Id_Article = Convert.ToInt32(dataRow["Id_Article"]);
			Id_Currency_Article = (dataRow["Id_Currency_Article"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency_Article"]);
			Id_Order = Convert.ToInt32(dataRow["Id_Order"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			InfoRahmennummer = (dataRow["InfoRahmennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InfoRahmennummer"]);
			Internal_Contact = (dataRow["Internal_Contact"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Internal_Contact"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Location_Id = (dataRow["Location_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Location_Id"]);
			Location_Name = (dataRow["Location_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Location_Name"]);
			Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			MhdDatumArtikel = (dataRow["MhdDatumArtikel"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MhdDatumArtikel"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Position_erledigt = (dataRow["Position erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Position erledigt"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			Produktionsort = (dataRow["Produktionsort"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionsort"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt"]);
			Rabatt1 = (dataRow["Rabatt1"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt1"]);
			RB_Abgerufen = (dataRow["RB_Abgerufen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Abgerufen"]);
			RB_Offen = (dataRow["RB_Offen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Offen"]);
			RB_OriginalAnzahl = (dataRow["RB_OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_OriginalAnzahl"]);
			schriftart = (dataRow["schriftart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["schriftart"]);
			sortierung = (dataRow["sortierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["sortierung"]);
			Start_Anzahl = (dataRow["Start Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Start Anzahl"]);
			TotalCost_Article = (dataRow["TotalCost_Article"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["TotalCost_Article"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Umsatzsteuer"]);
			Unit_Price = (dataRow["Unit_Price"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Unit_Price"]);
			VAT = (dataRow["VAT"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VAT"]);
			WE_Pos_zu_Bestellposition = (dataRow["WE Pos zu Bestellposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WE Pos zu Bestellposition"]);
		}
	}
}

