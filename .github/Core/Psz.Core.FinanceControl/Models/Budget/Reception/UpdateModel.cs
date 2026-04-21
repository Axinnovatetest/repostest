using System;

namespace Psz.Core.FinanceControl.Models.Budget.Reception
{
	public class UpdateModel
	{
		public string AB_Nr_Lieferant { get; set; }
		public string Abteilung { get; set; }
		public DateTime? Anfrage_Lieferfrist { get; set; }
		public string Anrede { get; set; }
		public string Ansprechpartner { get; set; }
		public int? Bearbeiter { get; set; }
		public int? Belegkreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Benutzer { get; set; }
		public int? best_id { get; set; }
		public DateTime? Bestellbestatigung_erbeten_bis { get; set; }
		public int? Bestellung_Nr { get; set; }
		public string Bezug { get; set; }
		public string Briefanrede { get; set; }
		public bool? datueber { get; set; }
		public DateTime? Datum { get; set; }
		public string Eingangslieferscheinnr { get; set; }
		public string Eingangsrechnungsnr { get; set; }
		public bool? erledigt { get; set; }
		public double? Frachtfreigrenze { get; set; }
		public string Freitext { get; set; }
		public bool? gebucht { get; set; }
		public bool? gedruckt { get; set; }
		public string Ihr_Zeichen { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public bool? Kanban { get; set; }
		public string Konditionen { get; set; }
		public string Kreditorennummer { get; set; }
		public int? Kundenbestellung { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public int? Lieferanten_Nr { get; set; }
		public DateTime? Liefertermin { get; set; }
		public bool? Loschen { get; set; }
		public DateTime? Mahnung { get; set; }
		public string Mandant { get; set; }
		public double? Mindestbestellwert { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public bool? Neu { get; set; }
		public int Nr { get; set; }
		public int? nr_anf { get; set; }
		public int? nr_bes { get; set; }
		public int? nr_gut { get; set; }
		public int? nr_RB { get; set; }
		public int? nr_sto { get; set; }
		public int? nr_war { get; set; }
		public bool? Offnen { get; set; }
		public int? Personal_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public double? Rabatt { get; set; }
		public bool? Rahmenbestellung { get; set; }
		public string Straße_Postfach { get; set; }
		public string Typ { get; set; }
		public string Unser_Zeichen { get; set; }
		public double? USt { get; set; }
		public string Versandart { get; set; }
		public string Vorname_NameFirma { get; set; }
		public int? Wahrung { get; set; }
		public string Zahlungsweise { get; set; }
		public string Zahlungsziel { get; set; }

		// - Extension props
		public int IssuerId { get; set; }
		public string IssuerName { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public int? CompanyId { get; set; }
		public string CompanyName { get; set; }
		public string OrderNumber { get; set; }
		// - 
		public bool CanViewInvoice { get; set; }

		public UpdateModel(Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity bestellungenEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity bestellungenExtension, bool canVIewInvoice)
		{
			AB_Nr_Lieferant = bestellungenEntity.AB_Nr_Lieferant;
			Abteilung = bestellungenEntity.Abteilung;
			Anfrage_Lieferfrist = bestellungenEntity.Anfrage_Lieferfrist;
			Anrede = bestellungenEntity.Anrede;
			Ansprechpartner = bestellungenEntity.Ansprechpartner;
			Bearbeiter = bestellungenEntity.Bearbeiter;
			Belegkreis = bestellungenEntity.Belegkreis;
			Bemerkungen = bestellungenEntity.Bemerkungen;
			Benutzer = bestellungenEntity.Benutzer;
			best_id = bestellungenEntity.best_id;
			Bestellbestatigung_erbeten_bis = bestellungenEntity.Bestellbestatigung_erbeten_bis;
			Bestellung_Nr = bestellungenEntity.Bestellung_Nr;
			Bezug = bestellungenEntity.Bezug;
			Briefanrede = bestellungenEntity.Briefanrede;
			datueber = bestellungenEntity.datueber;
			Datum = bestellungenEntity.Datum;
			Eingangslieferscheinnr = bestellungenEntity.Eingangslieferscheinnr;
			Eingangsrechnungsnr = bestellungenEntity.Eingangsrechnungsnr;
			erledigt = bestellungenEntity.erledigt;
			Frachtfreigrenze = bestellungenEntity.Frachtfreigrenze;
			Freitext = bestellungenEntity.Freitext;
			gebucht = bestellungenEntity.gebucht;
			gedruckt = bestellungenEntity.gedruckt;
			Ihr_Zeichen = bestellungenEntity.Ihr_Zeichen;
			In_Bearbeitung = bestellungenEntity.In_Bearbeitung;
			Kanban = bestellungenEntity.Kanban;
			Konditionen = bestellungenEntity.Konditionen;
			Kreditorennummer = bestellungenEntity.Kreditorennummer;
			Kundenbestellung = bestellungenEntity.Kundenbestellung;
			Land_PLZ_Ort = bestellungenEntity.Land_PLZ_Ort;
			Lieferanten_Nr = bestellungenEntity.Lieferanten_Nr;
			Liefertermin = bestellungenEntity.Liefertermin;
			Loschen = bestellungenEntity.Loschen;
			Mahnung = bestellungenEntity.Mahnung;
			Mandant = bestellungenEntity.Mandant;
			Mindestbestellwert = bestellungenEntity.Mindestbestellwert;
			Name2 = bestellungenEntity.Name2;
			Name3 = bestellungenEntity.Name3;
			Neu = bestellungenEntity.Neu;
			Nr = bestellungenEntity.Nr;
			nr_anf = bestellungenEntity.nr_anf;
			nr_bes = bestellungenEntity.nr_bes;
			nr_gut = bestellungenEntity.nr_gut;
			nr_RB = bestellungenEntity.nr_RB;
			nr_sto = bestellungenEntity.nr_sto;
			nr_war = bestellungenEntity.nr_war;
			Offnen = bestellungenEntity.Offnen;
			Personal_Nr = bestellungenEntity.Personal_Nr;
			Projekt_Nr = bestellungenEntity.Projekt_Nr;
			Rabatt = bestellungenEntity.Rabatt;
			Rahmenbestellung = bestellungenEntity.Rahmenbestellung;
			Straße_Postfach = bestellungenEntity.Straße_Postfach;
			Typ = bestellungenEntity.Typ;
			Unser_Zeichen = bestellungenEntity.Unser_Zeichen;
			USt = bestellungenEntity.USt;
			Versandart = bestellungenEntity.Versandart;
			Vorname_NameFirma = bestellungenEntity.Vorname_NameFirma;
			Wahrung = bestellungenEntity.Wahrung;
			Zahlungsweise = bestellungenEntity.Zahlungsweise;
			Zahlungsziel = bestellungenEntity.Zahlungsziel;

			// - Extension
			IssuerId = bestellungenExtension?.IssuerId ?? -1;
			IssuerName = bestellungenExtension?.IssuerName;
			DepartmentId = bestellungenExtension?.DepartmentId;
			DepartmentName = bestellungenExtension?.DepartmentName;
			CompanyId = bestellungenExtension?.CompanyId;
			CompanyName = bestellungenExtension?.CompanyName;
			OrderNumber = bestellungenExtension.OrderNumber;
			// -
			CanViewInvoice = canVIewInvoice;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity
			{
				AB_Nr_Lieferant = AB_Nr_Lieferant,
				Abteilung = Abteilung,
				Anfrage_Lieferfrist = Anfrage_Lieferfrist,
				Anrede = Anrede,
				Ansprechpartner = Ansprechpartner,
				Bearbeiter = Bearbeiter,
				Belegkreis = Belegkreis,
				Bemerkungen = Bemerkungen,
				Benutzer = Benutzer,
				best_id = best_id,
				Bestellbestatigung_erbeten_bis = Bestellbestatigung_erbeten_bis,
				Bestellung_Nr = Bestellung_Nr,
				Bezug = Bezug,
				Briefanrede = Briefanrede,
				datueber = datueber,
				Datum = Datum,
				Eingangslieferscheinnr = Eingangslieferscheinnr,
				Eingangsrechnungsnr = Eingangsrechnungsnr,
				erledigt = erledigt,
				Frachtfreigrenze = Frachtfreigrenze,
				Freitext = Freitext,
				gebucht = gebucht,
				gedruckt = gedruckt,
				Ihr_Zeichen = Ihr_Zeichen,
				In_Bearbeitung = In_Bearbeitung,
				Kanban = Kanban,
				Konditionen = Konditionen,
				Kreditorennummer = Kreditorennummer,
				Kundenbestellung = Kundenbestellung,
				Land_PLZ_Ort = Land_PLZ_Ort,
				Lieferanten_Nr = Lieferanten_Nr,
				Liefertermin = Liefertermin,
				Loschen = Loschen,
				Mahnung = Mahnung,
				Mandant = Mandant,
				Mindestbestellwert = Mindestbestellwert,
				Name2 = Name2,
				Name3 = Name3,
				Neu = Neu,
				Nr = Nr,
				nr_anf = nr_anf,
				nr_bes = nr_bes,
				nr_gut = nr_gut,
				nr_RB = nr_RB,
				nr_sto = nr_sto,
				nr_war = nr_war,
				Offnen = Offnen,
				Personal_Nr = Personal_Nr,
				Projekt_Nr = Projekt_Nr,
				Rabatt = Rabatt,
				Rahmenbestellung = Rahmenbestellung,
				Straße_Postfach = Straße_Postfach,
				Typ = Typ,
				Unser_Zeichen = Unser_Zeichen,
				USt = USt,
				Versandart = Versandart,
				Vorname_NameFirma = Vorname_NameFirma,
				Wahrung = Wahrung,
				Zahlungsweise = Zahlungsweise,
				Zahlungsziel = Zahlungsziel,
			};
		}
	}
}
