using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class OffeneMat_BstEntity
	{
		public string Benutzer { get; set; } //
		public int TotalCount { get; set; }
		public string Lieferantennr { get; set; }
		public string Lieferant { get; set; } //
		public int Bestellung_Nr { get; set; }
		public double Anzahl { get; set; }
		public double Mindestbestellmenge { get; set; }
		public double Verpackungseinheit { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Artikelnummer { get; set; }//
		public string Bestellnummer { get; set; }//
		public double Einzelpreis { get; set; }//
		public double Gesamtpreis { get; set; }// 
		public DateTime? Anlieferung { get; set; }// 
		public int Zahlungsziel_Netto { get; set; }//
		public DateTime? Falligkeit { get; set; }// 
		public string Produktionsstatte { get; set; }// 
		public string Mandant { get; set; }// 
		public int Bearbeiter { get; set; }// 
		public DateTime? Belegdatum { get; set; }// 
		public DateTime? Wunschtermin { get; set; }// 
		public string Bemerkung_Pos { get; set; } //
		public bool Standardlieferant { get; set; } //
		public string RaNumber { get; set; }
		public int? RaNr { get; set; }
		public OffeneMat_BstEntity(DataRow dataRow)
		{
			Benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : dataRow["Benutzer"].ToString();
			Lieferantennr = (dataRow["Lieferantennr"] == System.DBNull.Value) ? "" : dataRow["Lieferantennr"].ToString();
			Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : dataRow["Lieferant"].ToString();
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Bestellung-Nr"].ToString());
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["Anzahl"].ToString());
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["Mindestbestellmenge"].ToString());
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["Verpackungseinheit"].ToString());
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung 1"].ToString();
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : dataRow["Bestellnummer"].ToString();
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Einzelpreis"].ToString());
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gesamtpreis"].ToString());
			Anlieferung = (dataRow["Anlieferung"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Anlieferung"].ToString());
			Zahlungsziel_Netto = (dataRow["Zahlungsziel Netto"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Zahlungsziel Netto"].ToString());
			Falligkeit = (dataRow["Fälligkeit"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Fälligkeit"].ToString());
			Produktionsstatte = (dataRow["Produktionsstätte"] == System.DBNull.Value) ? "" : dataRow["Produktionsstätte"].ToString();
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : dataRow["Mandant"].ToString();
			Bearbeiter = (dataRow["Bearbeiter"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Bearbeiter"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Belegdatum = (dataRow["Belegdatum"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Belegdatum"].ToString());
			Wunschtermin = (dataRow["Wünschtermin"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Wünschtermin"].ToString());
			Bemerkung_Pos = (dataRow["Bemerkung_Pos"] == System.DBNull.Value) ? "" : dataRow["Bemerkung_Pos"].ToString();
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"].ToString());
			RaNumber = (dataRow["RaNumber"] == System.DBNull.Value) ? "" : dataRow["RaNumber"].ToString();
			RaNr = (dataRow["RaNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["RaNr"].ToString());
		}
	}
	public class GeschMat_BstEntity
	{
		public string Benutzer { get; set; } //
		public int TotalCount { get; set; }
		public string Lieferantennr { get; set; }
		public string Lieferant { get; set; } //
		public int Bestellung_Nr { get; set; }
		public double StartAnzhal { get; set; }
		public double Mindestbestellmenge { get; set; }
		public double Verpackungseinheit { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Artikelnummer { get; set; }//
		public string Bestellnummer { get; set; }//
		public double Einzelpreis { get; set; }//
		public double Preis_Gesamt { get; set; }// 
		public DateTime? Anlieferung { get; set; }// 
		public int Zahlungsziel_Netto { get; set; }//
		public DateTime? Falligkeit { get; set; }// 
		public string Produktionsstatte { get; set; }// 
		public string Mandant { get; set; }// 
		public int Bearbeiter { get; set; }// 
		public DateTime? Belegdatum { get; set; }// 
		public DateTime? Wunschtermin { get; set; }// 
		public bool Standardlieferant { get; set; } //
		public GeschMat_BstEntity(DataRow dataRow)
		{

			Benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : dataRow["Benutzer"].ToString();
			Lieferantennr = (dataRow["Lieferantennr"] == System.DBNull.Value) ? "" : dataRow["Lieferantennr"].ToString();
			Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : dataRow["Lieferant"].ToString();
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Bestellung-Nr"].ToString());
			StartAnzhal = (dataRow["Start Anzahl"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["Start Anzahl"].ToString());
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["Mindestbestellmenge"].ToString());
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["Verpackungseinheit"].ToString());
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung 1"].ToString();
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : dataRow["Bestellnummer"].ToString();
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Einzelpreis"].ToString());
			Preis_Gesamt = (dataRow["Preis_Gesamt"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Preis_Gesamt"].ToString());
			Anlieferung = (dataRow["Anlieferung"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Anlieferung"].ToString());
			Zahlungsziel_Netto = (dataRow["Zahlungsziel Netto"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Zahlungsziel Netto"].ToString());
			Falligkeit = (dataRow["Fälligkeit"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Fälligkeit"].ToString());
			Produktionsstatte = (dataRow["Produktionsstätte"] == System.DBNull.Value) ? "" : dataRow["Produktionsstätte"].ToString();
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : dataRow["Mandant"].ToString();
			Bearbeiter = (dataRow["Bearbeiter"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Bearbeiter"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Belegdatum = (dataRow["Belegdatum"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Belegdatum"].ToString());
			Wunschtermin = (dataRow["Wünschtermin"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Wünschtermin"].ToString());
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"].ToString());
		}
	}
	public class GetUngebuchteMatBstEntity
	{
		public DateTime? Bestellung_angelegt { get; set; }//
		public DateTime? Anlieferung { get; set; }//
		public int von { get; set; }//
		public int TotalCount { get; set; }//
		public string Benutzer { get; set; } //
		public string Lieferantennr { get; set; } //
		public string Lieferant { get; set; } //
		public int Bestellung_Nr { get; set; } //
		public double Anzahl { get; set; } //
		public string Bezeichnung_1 { get; set; }//
		public string Artikelnummer { get; set; }//
		public string Bestellnummer { get; set; }//
		public double Einzelpreis { get; set; } //
		public double Gesamtpreis { get; set; } //
		public int Zahlungsziel_Netto { get; set; }//
		public string Fertigungsstatte { get; set; }  //

		public GetUngebuchteMatBstEntity(DataRow dataRow)
		{
			Bestellung_angelegt = (dataRow["Bestellung angelegt"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Bestellung angelegt"].ToString());
			Anlieferung = (dataRow["Anlieferung"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Anlieferung"].ToString());
			von = (dataRow["von"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["von"].ToString());
			Benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : dataRow["Benutzer"].ToString();
			Lieferantennr = (dataRow["Lieferantennr"] == System.DBNull.Value) ? "" : dataRow["Lieferantennr"].ToString();
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : dataRow["Lieferant"].ToString();
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Bestellung-Nr"].ToString());
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Anzahl"].ToString());
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung 1"].ToString();
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : dataRow["Bestellnummer"].ToString();
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Einzelpreis"].ToString());
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gesamtpreis"].ToString());
			Fertigungsstatte = (dataRow["Fertigungsstätte"] == System.DBNull.Value) ? "" : dataRow["Fertigungsstätte"].ToString();
			Zahlungsziel_Netto = (dataRow["Zahlungsziel Netto"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Zahlungsziel Netto"].ToString());

		}
	}
	public class BestellungohneFAEntity
	{
		public string Artikelnummer { get; set; }
		public string Lieferant { get; set; }
		public int Bestellung_Nr { get; set; }
		public double Anzahl { get; set; }
		public DateTime? Wunschtermin { get; set; }//
		public DateTime? Bestatigter_Termin { get; set; }//
		public BestellungohneFAEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Art"] == System.DBNull.Value) ? "" : dataRow["Art"].ToString();
			Lieferant = (dataRow["Lief"] == System.DBNull.Value) ? "" : dataRow["Lief"].ToString();
			Bestellung_Nr = (dataRow["Be"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Be"].ToString());
			Anzahl = (dataRow["Anz"] == System.DBNull.Value) ? -1 : Convert.ToDouble(dataRow["Anz"].ToString());
			Wunschtermin = (dataRow["WunchT"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["WunchT"].ToString());
			Bestatigter_Termin = (dataRow["BesTermin"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["BesTermin"].ToString());
		}
	}
	public class GetArtikelStatisticsEntity
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public double EK { get; set; }
		public double Bestand { get; set; }
		public double Sicherheitsbestand { get; set; }
		public int Lagerort { get; set; }
		public int TotalCount { get; set; }
		public double Bedarf_1Mo { get; set; }
		public double Gesamtbedarfmax1Jahr { get; set; }
		public double offBest { get; set; }
		public double Entnahme_der_letzen_12_monate { get; set; }
		public GetArtikelStatisticsEntity(DataRow dataRow)
		{

			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung 1"].ToString();
			Sicherheitsbestand = (dataRow["Sicherheitsbestand"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Sicherheitsbestand"].ToString());
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerort"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			EK = (dataRow["EK"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["EK"].ToString());
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Bestand"].ToString());
			Bedarf_1Mo = (dataRow["Bedarf +1Mo"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Bedarf +1Mo"].ToString());
			offBest = (dataRow["off Best"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["off Best"].ToString());
			Gesamtbedarfmax1Jahr = (dataRow["Gesamtbedarf max  1 Jahr"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gesamtbedarf max  1 Jahr"].ToString());
			Entnahme_der_letzen_12_monate = (dataRow["Entnahme der letzen 12 monate"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Entnahme der letzen 12 monate"].ToString());
		}
	}
	public class StdSupplierViolationEntity
	{
		public string Artikelnummer { get; set; }
		public int? Bestellung_Nr { get; set; }
		public DateTime? Datum { get; set; }
		public decimal? Diff { get; set; }
		public decimal? EK_Price_Second_Source { get; set; }
		public string Lieferant { get; set; }
		public int? Position { get; set; }
		public decimal? Preis_von_Standardlieferant { get; set; }
		public string Standardlieferant { get; set; }
		public decimal? Start_Anzahl { get; set; }
		public decimal? Total_Second_Source { get; set; }
		public decimal? Total_von_Standardlieferant { get; set; }
		public StdSupplierViolationEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Diff = (dataRow["Diff"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Diff"]);
			EK_Price_Second_Source = (dataRow["EK Price Second Source"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK Price Second Source"]);
			Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferant"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Preis_von_Standardlieferant = (dataRow["Preis von Standardlieferant"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis von Standardlieferant"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardlieferant"]);
			Start_Anzahl = (dataRow["Start Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Start Anzahl"]);
			Total_Second_Source = (dataRow["Total Second Source"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Total Second Source"]);
			Total_von_Standardlieferant = (dataRow["Total von Standardlieferant"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Total von Standardlieferant"]);
		}
	}
}
