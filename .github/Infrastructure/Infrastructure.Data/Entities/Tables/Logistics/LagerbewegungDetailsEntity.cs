using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class LagerbewegungDetailsEntity
	{
		public long id { get; set; }
		public long idLagerbewegung { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string? bezeichnung1 { get; set; }
		public string? einheit { get; set; }
		public decimal anzahl { get; set; }
		public int lagerVon { get; set; }
		public int artikelNrNach { get; set; }
		public string artikelnummerNach { get; set; }
		public string bezeichnung1Nach { get; set; }
		public decimal anzahlNach { get; set; }
		public int lagerNach { get; set; }
		public string gebuchtVon { get; set; }
		public int grund { get; set; }
		public long? fertigungsnummer { get; set; }
		public string? bemerkung { get; set; }
		public DateTime? datum { get; set; }
		public LagerbewegungDetailsEntity()
		{

		}
		public LagerbewegungDetailsEntity(DataRow dr)
		{
			id = (dr["ID"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ID"]);
			idLagerbewegung = (dr["Lagerbewegungen_id"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["Lagerbewegungen_id"]);
			artikelNr = (dr["Artikel-nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Artikel-nr"]);
			artikelnummer = (dr["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Artikelnummer"]);
			bezeichnung1 = (dr["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung 1"]);
			einheit = (dr["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Einheit"]);
			anzahl = (dr["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl"]);
			lagerVon = (dr["Lager_von"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Lager_von"]);
			artikelNrNach = (dr["Artikel-nr_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Artikel-nr_nach"]);
			artikelnummerNach = (dr["ArtikelnummerNach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArtikelnummerNach"]);
			bezeichnung1Nach = (dr["Bezeichnung 1_nach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung 1_nach"]);
			anzahlNach = (dr["Anzahl_nach"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl_nach"]);
			lagerNach = (dr["Lager_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Lager_nach"]);
			gebuchtVon = (dr["Gebucht von"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Gebucht von"]);
			grund = (dr["Grund"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Grund"]);
			fertigungsnummer = (dr["Fertigungsnummer"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["Fertigungsnummer"]);
			bemerkung = (dr["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bemerkung"]);
			datum = (dr["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Datum"]);

		}

	}


	public class LagerbewegungDetailsPlantBookingEntity
	{
		public long id { get; set; }
		public long idLagerbewegung { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string? bezeichnung1 { get; set; }
		public string? einheit { get; set; }
		public decimal anzahl { get; set; }
		public int lagerVon { get; set; }
		public int artikelNrNach { get; set; }
		public string artikelnummerNach { get; set; }
		public string bezeichnung1Nach { get; set; }
		public decimal anzahlNach { get; set; }
		public int lagerNach { get; set; }
		public string gebuchtVon { get; set; }
		public int grund { get; set; }
		public long? fertigungsnummer { get; set; }
		public string? bemerkung { get; set; }
		public DateTime? datum { get; set; }
		public int WereingangId { get; set; }
		public int WereingangIdNach { get; set; }
		public decimal TransferableQuantity { get; set; }
		public decimal receivedQuantity { get; set; }
		public decimal TransferbestandNachUmb { get; set; }
		
		public LagerbewegungDetailsPlantBookingEntity()
		{

		}
		public LagerbewegungDetailsPlantBookingEntity(DataRow dr)
		{
			id = (dr["ID"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ID"]);
			idLagerbewegung = (dr["Lagerbewegungen_id"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["Lagerbewegungen_id"]);
			artikelNr = (dr["Artikel-nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Artikel-nr"]);
			artikelnummer = (dr["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Artikelnummer"]);
			bezeichnung1 = (dr["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung 1"]);
			einheit = (dr["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Einheit"]);
			anzahl = (dr["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl"]);
			lagerVon = (dr["Lager_von"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Lager_von"]);
			artikelNrNach = (dr["Artikel-nr_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Artikel-nr_nach"]);
			artikelnummerNach = (dr["ArtikelnummerNach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArtikelnummerNach"]);
			bezeichnung1Nach = (dr["Bezeichnung 1_nach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung 1_nach"]);
			anzahlNach = (dr["Anzahl_nach"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl_nach"]);
			lagerNach = (dr["Lager_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Lager_nach"]);
			gebuchtVon = (dr["Gebucht von"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Gebucht von"]);
			grund = (dr["Grund"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Grund"]);
			fertigungsnummer = (dr["Fertigungsnummer"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["Fertigungsnummer"]);
			bemerkung = (dr["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bemerkung"]);
			datum = (dr["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Datum"]);
			WereingangId = (dr["WereingangId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["WereingangId"]);
			WereingangIdNach = (dr["WereingangIdNach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["WereingangIdNach"]);
			TransferableQuantity = (dr["TransferableQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["TransferableQuantity"]);
			receivedQuantity = (dr["receivedQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["receivedQuantity"]);

		}

	}
}
