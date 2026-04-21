using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class GetDispows120ResponseModel
	{
		public string Name1 { get; set; } //
		public string Stucklisten_Artikelnummer { get; set; }//
		public string Bezeichnung { get; set; }//
		public double SummevonBruttobedarf { get; set; } //
		public DateTime? MaxvonTermin_Materialbedarf { get; set; }
		public DateTime? Rahmenauslauf { get; set; }
		public double Bestand { get; set; }//
		public double Differenz { get; set; } //
		public double Mindestbestellmenge { get; set; } //
		public string Lagerort { get; set; }//
		public int Lagerort_id { get; set; } //
		public int TotalCount { get; set; } //
		public int ArtikelNr { get; set; } //
		public string Rahmen_Nr { get; set; } //
		public int? Rahmenmenge { get; set; }
		public Boolean obsolet { get; set; }
		public GetDispows120ResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.Dispows120Entity data)
		{

			Name1 = data.Name1 ?? string.Empty;
			Stucklisten_Artikelnummer = data.Stücklisten_Artikelnummer ?? string.Empty;
			SummevonBruttobedarf = data.SummevonBruttobedarf;
			MaxvonTermin_Materialbedarf = data.MaxvonTermin_Materialbedarf;
			Bestand = data.Bestand;
			Differenz = data.Differenz;
			Mindestbestellmenge = data.Mindestbestellmenge;
			Lagerort = data.Lagerort ?? string.Empty;
			Lagerort_id = data.Lagerort_id;
			Rahmen_Nr = data.Rahmen_Nr ?? string.Empty;
			Rahmenmenge = data.Rahmenmenge;
			obsolet = data.obsolet;
			TotalCount = data.TotalCount;
			obsolet = data.obsolet;
			Bezeichnung = data.Bezeichnung ?? string.Empty;
			Rahmenauslauf = data.Rahmenauslauf;
			ArtikelNr = data.ArtikelNr;
		}

	}
	public class GetDispows120RequestModel: IPaginatedRequestModel
	{
		public string Filter { get; set; }
		public int Dispo { get; set; } = 1;
	}
	// combine two models (Details)
	public class Dispows120DetailsModel
	{

		public Dispows120Details12Model dispows120Details12Models { get; set; }
		public List<Dispows120DetailsBestandModel> dispows120Details15Models { get; set; }

		public Dispows120DetailsModel()
		{
			dispows120Details15Models = new List<Dispows120DetailsBestandModel>();
		}
	}
	public class Dispows120Details12Model
	{
		/*
		1	PSZ#	
		2	Bezeichnung des Bauteils	
		*/
		public string PSZ { get; set; } //
		public string Bezeichnung_des_Bauteils { get; set; }//

		public Dispows120Details12Model(Dispows120DetailsEntity data)
		{
			PSZ = data.PSZ ?? "";
			Bezeichnung_des_Bauteils = data.Bezeichnung_des_Bauteils ?? "";

		}
	}
	public class Dispows120DetailsBestandModel
	{
		/*
		3	Lagerort_id
		4	Lagerort
		5	Lagerort
		*/
		public int Lagerort_id { get; set; } //
		public string Lagerort { get; set; }//
		public double Bestand { get; set; } //

		public Dispows120DetailsBestandModel(Dispows120DetailsBestandEntity data)
		{
			Lagerort_id = data.Lagerort_id;
			Lagerort = data.Lagerort ?? "";
			Bestand = data.Bestand;
		}
	}
	public class Dispows120DetailsLieferantenModel
	{
		/*
		6	Name1
		7	Standardlieferant
		8	Wiederbeschaffungszeitraum
		9	Adresse
		10	Bestell-Nr
		11	Verpackungseinheit
		12	Telefon
		13	Einkaufspreis
		14	Mindestbestellmenge
		*/
		public string Name1 { get; set; }//
		public string Adresse { get; set; }//
		public string Standardlieferant { get; set; }//
		public int Wiederbeschaffungszeitraum { get; set; }//
		public int TotalCount { get; set; }//
		public string Bestell_Nr { get; set; }//
		public int Verpackungseinheit { get; set; }//
		public string Telefon { get; set; }//
		public double Einkaufspreis { get; set; } //
		public double Mindestbestellmenge { get; set; } //
		public int Lieferanten_Nr { get; set; } //
		public int offnenbestellecount { get; set; } //

		public Dispows120DetailsLieferantenModel(Dispows120DetailsLieferantenEntity data)
		{
			Mindestbestellmenge = data.Mindestbestellmenge;
			Einkaufspreis = data.Einkaufspreis;
			Telefon = data.Telefon ?? "";
			Name1 = data.Name1 ?? "";
			Adresse = data.Adresse ?? "";
			Bestell_Nr = data.Bestell_Nr;
			Standardlieferant = data.Standardlieferant ? "YES" : "NO";
			Wiederbeschaffungszeitraum = data.Wiederbeschaffungszeitraum;
			Verpackungseinheit = data.Verpackungseinheit;
			TotalCount = data.TotalCount;
			Lieferanten_Nr = data.Lieferanten_Nr;
		}
	}
	public class Dispows120DetailsLieferantenAndOrderModel
	{
		public Dispows120DetailsLieferantenModel Lieferanten { get; set; }
		public int OrdersCount { get; set; }
		public List<Dispows120DetailsOffenBestellungenModel> OffnenBestellungen { get; set; }
		public Dispows120DetailsLieferantenAndOrderModel()
		{
			OffnenBestellungen = new List<Dispows120DetailsOffenBestellungenModel>();
		}
		public Dispows120DetailsLieferantenAndOrderModel(List<Dispows120DetailsLieferantenModel> suppliers, List<Dispows120DetailsOffenBestellungenModel> orders, int Liefternr)
		{
			Lieferanten = (from supp in suppliers where supp.Lieferanten_Nr == Liefternr select supp).FirstOrDefault();
			OffnenBestellungen = (from order in orders where order.Lieferanten_Nr == Liefternr select order).ToList();
		}

	}
	public class Dispows120DetailsAllLieferantenModel
	{
		public List<Dispows120DetailsLieferantenAndOrderModel> Lieferantens { get; set; }
		public Dispows120DetailsAllLieferantenModel()
		{
			Lieferantens = new List<Dispows120DetailsLieferantenAndOrderModel>();
		}
	}
	public class Dispows120DetailsAllLieferantenAndordersRequestModel
	{
		[Required]
		[MinLength(10, ErrorMessage = "Please Provide Correct ArtikelNummer ")]
		[MaxLength(21, ErrorMessage = "Please Provide Correct ArtikelNummer ")]
		public string artikelnummer { get; set; } = "";
		[Required]
		public int Dispo { get; set; }

	}
	public class Dispows120DetailsOffenBestellungenModel
	{
		/*
		15	Lagerort_id --
		16	Bestellung-Nr --
		17	Rahmenbestellung --
		18	Bestellmenge --
		19	Offen --
		20	Liefertermin --
		21	Bestätigter_Termin --
		22	AB-Nr_Lieferant --
		23	Einzelpreis
		Nr
		*/
		public int Lagerort_id { get; set; } //
		public int Bestellung_Nr { get; set; } //
		public bool Rahmenbestellung { get; set; } //
		public string AB_Nr_Lieferant { get; set; } //
		public int Nr { get; set; }//
		public double Bestellmenge { get; set; } //
		public double Offen { get; set; } //
		public double Einzelpreis { get; set; } //
		public DateTime? Liefertermin { get; set; } //
		public DateTime? Bestätigter_Termin { get; set; } //
		public int Lieferanten_Nr { get; set; }
		public Dispows120DetailsOffenBestellungenModel(Dispows120DetailsOffenBestellungenEntity data)
		{
			Lagerort_id = data.Lagerort_id;
			Bestellung_Nr = data.Bestellung_Nr;
			Rahmenbestellung = data.Rahmenbestellung;
			AB_Nr_Lieferant = data.AB_Nr_Lieferant;
			Nr = data.Nr;
			Bestellmenge = data.Bestellmenge;
			Offen = data.Offen;
			Einzelpreis = data.Offen;
			Liefertermin = data.Liefertermin;
			Bestätigter_Termin = data.Bestätigter_Termin;
			Lieferanten_Nr = data.Lieferanten_Nr;
		}
	}
	public class Dispows120DetailsBedarfeModel
	{
		/*
		24	Termin_Bestätigt1	
		25	Fertigungsnummer	
		26	Artikel_Artikelnummer	
		27	Bezeichnung 1	
		28	Fertigung_Anzahl	
		29	Stücklisten_Anzahl	
		30	Bruttobedarf	
		31	Termin_Materialbedarf	
		32	Laufende Summe	
		33	[Bestand]-[Laufende Summe]	
		*/
		public string Psz { get; set; } //
		public string Artikel_Nr_des_Bauteils { get; set; } //
		public string Artikel_Artikelnummer { get; set; } //
		public string Fertigungsnummer { get; set; } //
		public string Bezeichnung1 { get; set; } //
		public double Fertigung_anzahl { get; set; } //
		public double Stucklisten_Anzahl { get; set; } //
		public double Verfugbar { get; set; } //
		public double Bruttobedarf { get; set; } //
		public double Bestand { get; set; } //
		public double Laufende_Summe { get; set; } //
		public DateTime? Termin_Materialbedarf { get; set; } //
		public DateTime? Termin_Bestatigt1 { get; set; } //
		public int TotalCount { get; set; } //
		public Dispows120DetailsBedarfeModel(Dispows120DetailsBedarfeEntity data)
		{
			Psz = data.Psz;
			Artikel_Nr_des_Bauteils = data.Artikel_Nr_des_Bauteils ?? "";
			Artikel_Artikelnummer = data.Artikel_Artikelnummer ?? "";
			Fertigungsnummer = data.Fertigungsnummer ?? "";
			Bezeichnung1 = data.Bezeichnung1 ?? "";
			Fertigung_anzahl = data.Fertigung_anzahl;
			Stucklisten_Anzahl = data.Stücklisten_Anzahl;
			Bruttobedarf = data.Bruttobedarf;
			Bestand = data.Bestand;
			Laufende_Summe = data.Laufende_Summe;
			Termin_Materialbedarf = data.Termin_Materialbedarf;
			Termin_Bestatigt1 = data.Termin_Bestätigt1;
			Verfugbar = data.Verfügbar;
			TotalCount = data.TotalCount;
		}
	}
	public class GetDispows120DetailsRequestModel
	{
		[Required]

		public int ArtikelNr { get; set; }
		public int Dispo { get; set; }
	}
	public class GetDispows120DetailsLieferantenRequestModel: IPaginatedRequestModel
	{
		[Required]
		public int ArtikelNr { get; set; }
		[Required]
		public int Dispo { get; set; }
	}
	public class GetDispows120DetailsBedarfeRequestModel: IPaginatedRequestModel
	{
		[Required]
		public int ArtikelNr { get; set; }
		[Required]
		public int Dispo { get; set; }
	}

}
