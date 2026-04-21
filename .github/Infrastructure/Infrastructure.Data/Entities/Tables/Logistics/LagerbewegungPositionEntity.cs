using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class LagerbewegungPositionEntity
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
		public string? bemerkung { get; set; }
		public DateTime? datum { get; set; }
		// - 2024-02-07 - FormatSoftware
		public decimal? Preiseinheit { get; set; }
		public decimal? ArticleUnitPrice { get; set; } // - Einkaufspreis
		public decimal? ArticleTotalPrice { get; set; } // - Gesamtpreis
		public int? Fertigungsnummer { get; set; }
		public string ArticleWarengruppe { get; set; }
		public decimal? ArticleWeight { get; set; } // - Gewicht
		public string ArticleCustomsNumber { get; set; } // - Zolltarif_nr
		public string ArticleDesignation { get; set; } // - Bezeichnung
		public string Artikelnummer_FG { get; set; }
		public string Zolltariffnummer_FG { get; set; }
		public decimal? Gewicht_FG { get; set; }
		public int ArtikelNr_FG { get; set; }
		public decimal Anzahl_FG { get; set; }
		public string Ursprungsland { get; set; }

		public LagerbewegungPositionEntity()
		{

		}
		public LagerbewegungPositionEntity(DataRow dr)
		{
			id = (dr["ID"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ID"]);
			idLagerbewegung = (dr["Lagerbewegungen_id"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["Lagerbewegungen_id"]);
			artikelNr = (dr["Artikel-nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Artikel-nr"]);
			artikelnummer = (dr["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Artikelnummer"]);
			bezeichnung1 = (dr["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung 1"]);
			einheit = (dr["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Einheit"]);
			anzahl = (dr["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl"]);
			Anzahl_FG = (dr["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl"]);
			lagerVon = (dr["Lager_von"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Lager_von"]);
			artikelNrNach = (dr["Artikel-nr_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Artikel-nr_nach"]);
			artikelnummerNach = (dr["ArtikelnummerNach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArtikelnummerNach"]);
			bezeichnung1Nach = (dr["Bezeichnung 1_nach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung 1_nach"]);
			anzahlNach = (dr["Anzahl_nach"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl_nach"]);
			lagerNach = (dr["Lager_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Lager_nach"]);
			gebuchtVon = (dr["Gebucht von"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Gebucht von"]);
			grund = (dr["Grund"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Grund"]);
			bemerkung = (dr["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bemerkung"]);
			datum = (dr["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Datum"]);
			Preiseinheit = (dr["Preiseinheit"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["Preiseinheit"]);
			ArticleUnitPrice = (dr["ArticleUnitPrice"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["ArticleUnitPrice"]);
			ArticleTotalPrice = (dr["ArticleTotalPrice"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["ArticleTotalPrice"]);
			Fertigungsnummer = (dr["Fertigungsnummer"] == System.DBNull.Value) ? null : Convert.ToInt32(dr["Fertigungsnummer"]);
			ArticleWarengruppe = (dr["ArticleWarengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArticleWarengruppe"]);
			ArticleWeight = (dr["ArticleWeight"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["ArticleWeight"]);
			ArticleCustomsNumber = (dr["ArticleCustomsNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArticleCustomsNumber"]);
			ArticleDesignation = (dr["ArticleDesignation"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArticleDesignation"]);
			Artikelnummer_FG = (dr["Artikelnummer_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Artikelnummer_FG"]);
			Zolltariffnummer_FG = (dr["Zolltariffnummer_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Zolltariffnummer_FG"]);
			Ursprungsland = (dr["Ursprungsland_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Ursprungsland_FG"]);
			Gewicht_FG = (dr["Gewicht_FG"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["Gewicht_FG"]);
			artikelNr = (dr["ArtikelNr_FG"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["ArtikelNr_FG"]);

		}
	}
	public class LagerbewegungPositionFormatEntity
	{
		public long id { get; set; }
		public long idLagerbewegung { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string? bezeichnung1 { get; set; }
		public string? einheit { get; set; }
		public decimal anzahl { get; set; }
		public int lagerVon { get; set; }
		//public int artikelNrNach { get; set; }
		//public string artikelnummerNach { get; set; }
		public string bezeichnung1Nach { get; set; }
		//public decimal anzahlNach { get; set; }
		public int lagerNach { get; set; }
		//public string gebuchtVon { get; set; }
		//public int grund { get; set; }
		//public string? bemerkung { get; set; }
		//public DateTime? datum { get; set; }
		// - 2024-02-07 - FormatSoftware
		public decimal? Preiseinheit { get; set; }
		public decimal? ArticleUnitPrice { get; set; } // - Einkaufspreis
		public decimal? ArticleTotalPrice { get; set; } // - Gesamtpreis
		public int? Fertigungsnummer { get; set; }
		public string ArticleWarengruppe { get; set; }
		public decimal? ArticleWeight { get; set; } // - Gewicht
		public long ArticleCustomsNumber { get; set; } // - Zolltarif_nr
		public string ArticleDesignation { get; set; } // - Bezeichnung
		public string Artikelnummer_FG { get; set; }
		public long Zolltariffnummer_FG { get; set; }
		public decimal? Gewicht_FG { get; set; }
		public int ArtikelNr_FG { get; set; }
		public decimal Anzahl_FG { get; set; }
		public string Ursprungsland { get; set; }
		public decimal? UnitPrice_FG { get; set; }

		public LagerbewegungPositionFormatEntity()
		{

		}
		public LagerbewegungPositionFormatEntity(DataRow dr)
		{
			id = (dr["ID"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ID"]);
			idLagerbewegung = (dr["Lagerbewegungen_id"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["Lagerbewegungen_id"]);
			artikelNr = (dr["Artikel-nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Artikel-nr"]);
			artikelnummer = (dr["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Artikelnummer"]);
			bezeichnung1 = (dr["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung 1"]);
			einheit = (dr["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Einheit"]);
			anzahl = (dr["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl"]);
			Anzahl_FG = (dr["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl"]);
			lagerVon = (dr["Lager_von"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Lager_von"]);
			//artikelNrNach = (dr["Artikel-nr_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Artikel-nr_nach"]);
			//artikelnummerNach = (dr["ArtikelnummerNach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArtikelnummerNach"]);
			bezeichnung1Nach = (dr["Bezeichnung 1_nach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung 1_nach"]);
			//anzahlNach = (dr["Anzahl_nach"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl_nach"]);
			lagerNach = (dr["Lager_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Lager_nach"]);
			//gebuchtVon = (dr["Gebucht von"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Gebucht von"]);
			//grund = (dr["Grund"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Grund"]);
			//bemerkung = (dr["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bemerkung"]);
			//datum = (dr["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Datum"]);
			Preiseinheit = (dr["Preiseinheit"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["Preiseinheit"]);
			ArticleUnitPrice = (dr["ArticleUnitPrice"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["ArticleUnitPrice"]);
			ArticleTotalPrice = (dr["ArticleTotalPrice"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["ArticleTotalPrice"]);
			Fertigungsnummer = (dr["Fertigungsnummer"] == System.DBNull.Value) ? null : Convert.ToInt32(dr["Fertigungsnummer"]);
			ArticleWarengruppe = (dr["ArticleWarengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArticleWarengruppe"]);
			ArticleWeight = (dr["ArticleWeight"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["ArticleWeight"]);
			ArticleCustomsNumber = (dr["ArticleCustomsNumber"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["ArticleCustomsNumber"]);
			ArticleDesignation = (dr["ArticleDesignation"] == System.DBNull.Value) ? "" : Convert.ToString(dr["ArticleDesignation"]);
			Artikelnummer_FG = (dr["Artikelnummer_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Artikelnummer_FG"]);
			Zolltariffnummer_FG = (dr["Zolltariffnummer_FG"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["Zolltariffnummer_FG"]);
			Ursprungsland = (dr["Ursprungsland_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Ursprungsland_FG"]);
			Gewicht_FG = (dr["Gewicht_FG"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["Gewicht_FG"]);
			artikelNr = (dr["ArtikelNr_FG"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["ArtikelNr_FG"]);
			UnitPrice_FG = (dr["UnitPrice_FG"] == System.DBNull.Value) ? null : Convert.ToDecimal(dr["UnitPrice_FG"]);

		}
	}
}
