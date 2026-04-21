using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Reception
{
	public class HistoryModel
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

		public List<Article.GetModel> Articles { get; set; }

		public HistoryModel(Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity bestellungenEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> artikelEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> bestellte_ArtikelEntities)
		{
			AB_Nr_Lieferant = bestellungenEntity?.AB_Nr_Lieferant;
			Abteilung = bestellungenEntity?.Abteilung;
			Anfrage_Lieferfrist = bestellungenEntity?.Anfrage_Lieferfrist;
			Anrede = bestellungenEntity?.Anrede;
			Ansprechpartner = bestellungenEntity?.Ansprechpartner;
			Bearbeiter = bestellungenEntity?.Bearbeiter;
			Belegkreis = bestellungenEntity?.Belegkreis;
			Bemerkungen = bestellungenEntity?.Bemerkungen;
			Benutzer = bestellungenEntity?.Benutzer;
			best_id = bestellungenEntity?.best_id;
			Bestellbestatigung_erbeten_bis = bestellungenEntity?.Bestellbestatigung_erbeten_bis;
			Bestellung_Nr = bestellungenEntity?.Bestellung_Nr;
			Bezug = bestellungenEntity?.Bezug;
			Briefanrede = bestellungenEntity?.Briefanrede;
			datueber = bestellungenEntity?.datueber;
			Datum = bestellungenEntity?.Datum;
			Eingangslieferscheinnr = bestellungenEntity?.Eingangslieferscheinnr;
			Eingangsrechnungsnr = bestellungenEntity?.Eingangsrechnungsnr;
			erledigt = bestellungenEntity?.erledigt;
			Frachtfreigrenze = bestellungenEntity?.Frachtfreigrenze;
			Freitext = bestellungenEntity?.Freitext;
			gebucht = bestellungenEntity?.gebucht;
			gedruckt = bestellungenEntity?.gedruckt;
			Ihr_Zeichen = bestellungenEntity?.Ihr_Zeichen;
			In_Bearbeitung = bestellungenEntity?.In_Bearbeitung;
			Kanban = bestellungenEntity?.Kanban;
			Konditionen = bestellungenEntity?.Konditionen;
			Kreditorennummer = bestellungenEntity?.Kreditorennummer;
			Kundenbestellung = bestellungenEntity?.Kundenbestellung;
			Land_PLZ_Ort = bestellungenEntity?.Land_PLZ_Ort;
			Lieferanten_Nr = bestellungenEntity?.Lieferanten_Nr;
			Liefertermin = bestellungenEntity?.Liefertermin;
			Loschen = bestellungenEntity?.Loschen;
			Mahnung = bestellungenEntity?.Mahnung;
			Mandant = bestellungenEntity?.Mandant;
			Mindestbestellwert = bestellungenEntity?.Mindestbestellwert;
			Name2 = bestellungenEntity?.Name2;
			Name3 = bestellungenEntity?.Name3;
			Neu = bestellungenEntity?.Neu;
			Nr = bestellungenEntity?.Nr ?? -1;
			nr_anf = bestellungenEntity?.nr_anf;
			nr_bes = bestellungenEntity?.nr_bes;
			nr_gut = bestellungenEntity?.nr_gut;
			nr_RB = bestellungenEntity?.nr_RB;
			nr_sto = bestellungenEntity?.nr_sto;
			nr_war = bestellungenEntity?.nr_war;
			Offnen = bestellungenEntity?.Offnen;
			Personal_Nr = bestellungenEntity?.Personal_Nr;
			Projekt_Nr = bestellungenEntity?.Projekt_Nr;
			Rabatt = bestellungenEntity?.Rabatt;
			Rahmenbestellung = bestellungenEntity?.Rahmenbestellung;
			Straße_Postfach = bestellungenEntity?.Straße_Postfach;
			Typ = bestellungenEntity?.Typ;
			Unser_Zeichen = bestellungenEntity?.Unser_Zeichen;
			USt = bestellungenEntity?.USt;
			Versandart = bestellungenEntity?.Versandart;
			Vorname_NameFirma = bestellungenEntity?.Vorname_NameFirma;
			Wahrung = bestellungenEntity?.Wahrung;
			Zahlungsweise = bestellungenEntity?.Zahlungsweise;
			Zahlungsziel = bestellungenEntity?.Zahlungsziel;

			Articles = new List<Models.Budget.Reception.Article.GetModel>();
			if(bestellte_ArtikelEntities != null)
			{
				foreach(var item in bestellte_ArtikelEntities)
				{
					var artikelItem = artikelEntities?.Find(x => x.Artikel_Nr == item.Artikel_Nr);
					Articles.Add(new Models.Budget.Reception.Article.GetModel(item, artikelItem, bestellungenEntity));
				}
			}
		}

	}
}
