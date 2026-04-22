using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class OffenematbstModel
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
		public OffenematbstModel(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.OffeneMat_BstEntity data)
		{

			Benutzer = data.Benutzer ?? string.Empty;
			Lieferantennr = data.Lieferantennr ?? string.Empty;
			Lieferant = data.Lieferant ?? string.Empty;
			Bestellung_Nr = data.Bestellung_Nr;
			Anzahl = data.Anzahl;
			Mindestbestellmenge = data.Mindestbestellmenge;
			Verpackungseinheit = data.Verpackungseinheit;
			Bezeichnung_1 = data.Bezeichnung_1 ?? string.Empty;
			Artikelnummer = data.Artikelnummer ?? string.Empty;
			Bestellnummer = data.Bestellnummer ?? string.Empty;
			Einzelpreis = data.Einzelpreis;
			Gesamtpreis = data.Gesamtpreis;
			Anlieferung = data.Anlieferung;
			Zahlungsziel_Netto = data.Zahlungsziel_Netto;
			Falligkeit = data.Falligkeit;
			Produktionsstatte = data.Produktionsstatte ?? string.Empty;
			Mandant = data.Mandant ?? string.Empty;
			Bearbeiter = data.Bearbeiter;
			Belegdatum = data.Belegdatum;
			Wunschtermin = data.Wunschtermin;
			Bemerkung_Pos = data.Bemerkung_Pos ?? string.Empty;
			Standardlieferant = data.Standardlieferant;
			RaNumber = data.RaNumber;
			RaNr = data.RaNr;
			TotalCount = data.TotalCount;
		}
	}
	public class GeschlmatbstModel
	{
		public string Benutzer { get; set; } //
		public int TotalCount { get; set; }
		public string Lieferantennr { get; set; }
		public string Lieferant { get; set; } //
		public int Bestellung_Nr { get; set; }
		public double StratAnzhal { get; set; }
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
		public double Preis_Gesamt { get; set; }// 
		public GeschlmatbstModel(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.GeschMat_BstEntity data)
		{

			Benutzer = data.Benutzer ?? string.Empty;
			Lieferantennr = data.Lieferantennr ?? string.Empty;
			Lieferant = data.Lieferant ?? string.Empty;
			Bestellung_Nr = data.Bestellung_Nr;
			StratAnzhal = data.StartAnzhal;
			Mindestbestellmenge = data.Mindestbestellmenge;
			Verpackungseinheit = data.Verpackungseinheit;
			Bezeichnung_1 = data.Bezeichnung_1 ?? string.Empty;
			Artikelnummer = data.Artikelnummer ?? string.Empty;
			Bestellnummer = data.Bestellnummer ?? string.Empty;
			Einzelpreis = data.Einzelpreis;
			Preis_Gesamt = data.Preis_Gesamt;
			Anlieferung = data.Anlieferung;
			Zahlungsziel_Netto = data.Zahlungsziel_Netto;
			Falligkeit = data.Falligkeit;
			Produktionsstatte = data.Produktionsstatte ?? string.Empty;
			Mandant = data.Mandant ?? string.Empty;
			Bearbeiter = data.Bearbeiter;
			Belegdatum = data.Belegdatum;
			Wunschtermin = data.Wunschtermin;
			Standardlieferant = data.Standardlieferant;
			TotalCount = data.TotalCount;
		}
	}
	public class GetUngebuchteMatBstModel
	{
		public DateTime? Bestellung_angelegt { get; set; }//
		public DateTime? Anlieferung { get; set; }//
		public int? von { get; set; }//
		public int TotalCount { get; set; }//
		public string Benutzer { get; set; } //
		public string Lieferantennr { get; set; } //
		public string Lieferant { get; set; } //
		public int Bestellung_Nr { get; set; } //
		public double? Anzahl { get; set; } //
		public string Bezeichnung_1 { get; set; }//
		public string Artikelnummer { get; set; }//
		public string Bestellnummer { get; set; }//
		public double? Einzelpreis { get; set; } //
		public double? Gesamtpreis { get; set; } //
		public int? Zahlungsziel_Netto { get; set; }//
		public string Fertigungsstatte { get; set; }  //

		public GetUngebuchteMatBstModel(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.GetUngebuchteMatBstEntity data)
		{
			Benutzer = data.Benutzer ?? string.Empty;
			Lieferantennr = data.Lieferantennr ?? string.Empty;
			Lieferant = data.Lieferant ?? string.Empty;
			Bestellung_Nr = data.Bestellung_Nr;
			Anzahl = data.Anzahl;
			Bezeichnung_1 = data.Bezeichnung_1 ?? string.Empty;
			Artikelnummer = data.Artikelnummer ?? string.Empty;
			Bestellnummer = data.Bestellnummer ?? string.Empty;
			Einzelpreis = data.Einzelpreis;
			Anlieferung = data.Anlieferung;
			Zahlungsziel_Netto = data.Zahlungsziel_Netto;
			TotalCount = data.TotalCount;
			Bestellung_angelegt = data.Bestellung_angelegt;
			von = data.von;
			Fertigungsstatte = data.Fertigungsstatte;
			Gesamtpreis = data.Gesamtpreis;
		}
	}
	public class PlantsAndLagers
	{
		public string Plant { get; set; }
		public string PlantFull { get; set; }
		public int LagerHaupt { get; set; }
		public int Lager_fert { get; set; }
		public string Lager_fert_2 { get; set; }

		public bool IsValidPlantLager()
		{
			return !string.IsNullOrEmpty(Plant) && LagerHaupt > 0 && Lager_fert > 0;
		}
	}
	public class OffenematbstRequestModel: IPaginatedRequestModel
	{

		[Required]
		public DateTime fromdate { get; set; } = DateTime.Today.AddMonths(-6);
		[Required]
		public DateTime todate { get; set; } = DateTime.Today.AddMonths(+6);

	}
	public class GetUngebuchteMatBstRequestModel: IPaginatedRequestModel
	{
	}
	public class GetArtikelStatisticsModel
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int TotalCount { get; set; }
		public double EK { get; set; }
		public double Bestand { get; set; }
		public double Sicherheitsbestand { get; set; }
		public int Lagerort { get; set; }
		public double Bedarf_1Mo { get; set; }
		public double Gesamtbedarfmax1Jahr { get; set; }
		public double offBest { get; set; }
		public double Entnahme_der_letzen_12_monate { get; set; }
		public GetArtikelStatisticsModel(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.GetArtikelStatisticsEntity data)
		{
			Artikelnummer = data.Artikelnummer ?? string.Empty;
			Bezeichnung_1 = data.Bezeichnung_1 ?? string.Empty;
			EK = data.EK;
			Bestand = data.Bestand;
			Sicherheitsbestand = data.Sicherheitsbestand;
			Lagerort = data.Lagerort;
			Bedarf_1Mo = data.Bedarf_1Mo;
			Gesamtbedarfmax1Jahr = data.Gesamtbedarfmax1Jahr;
			offBest = data.offBest;
			Entnahme_der_letzen_12_monate = data.Entnahme_der_letzen_12_monate;
			TotalCount = data.TotalCount;
		}
	}
	public class GetArtikelStatisticsModelRequestModel: IPaginatedRequestModel
	{
		public int prd { get; set; }
	}
}
