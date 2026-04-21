using System.Collections.Generic;

namespace Psz.Core.MaterialManagement.Reporting.Models
{
	public class OrderModel
	{
		public string _Logo { get; set; }
		public string AbEmailAddress { get; set; }
		public bool IsKanban { get; set; }
		public string Lieferanten_Nr { get; set; }
		public string Bestellung_Nr { get; set; }
		public string Typ { get; set; }
		public string Datum { get; set; }
		public string Anrede { get; set; }
		public string Vorname_NameFirma { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Strasse_Postfach { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public string Personal_Nr { get; set; }
		public string Versandart { get; set; }
		public string Zahlungsweise { get; set; }
		public string Konditionen { get; set; }
		public string Zahlungsziel { get; set; }
		public string Bezug { get; set; }
		public string Ihr_Zeichen { get; set; }
		public string Unser_Zeichen { get; set; }
		public string Bestellbestatigung_erbeten_bis { get; set; }
		public string Freitext { get; set; }
		public string Wahrung { get; set; }
		public string Ansprechpartner { get; set; }
		public string Abteilung { get; set; }
		public string Nr { get; set; }
		public string Rahmenbestellung { get; set; }
		public string Mandant { get; set; }
		public string InfoRahmennummer { get; set; }
		public string RahmenBezug { get; set; }
		public string Langtexte { get; set; }

		public byte[] Logo { get; set; }
		public string Text_kopf { get; set; }
		public string Text_fuss { get; set; }

		public string Name { get; set; }
		public string Telefonnummer { get; set; }
		public string Faxnummer { get; set; }
		public string Email { get; set; }

		public string Telefon { get; set; }
		public string Fax { get; set; }

		public string Bestellung { get; set; }
		public string Bestellungen_unten { get; set; }
		public string Lager { get; set; }
		public string Symbol { get; set; }
		public List<PositionModel> PositionItems { get; set; }

		public class PositionModel
		{
			public string Bestellnummer { get; set; }
			public string Rabatt1 { get; set; }
			public string Rabatt2 { get; set; }
			public string Menge { get; set; }
			public string Einzelpreis { get; set; }
			public decimal Gesamtpreis { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public string Einheit { get; set; }
			public string Umsatzsteuer { get; set; }
			public string sortierung { get; set; }
			public string best_art_nr { get; set; }
			public string schrift { get; set; }
			public string Preiseinheit { get; set; }
			public string LT { get; set; }
			public string Lagerort_id { get; set; }
			public string CUPreis { get; set; }
			public string Artikelnummer { get; set; }
			public bool MHD { get; set; }
			public bool COF_Pflichtig { get; set; }
			public string Zeitraum_MHD { get; set; }
			public bool EMPB { get; set; }
			public bool ESD_Schutz { get; set; }
			public string Symbol { get; set; }
			public string LagerortLabel { get; set; }
			public string Position { get; set; }
			public decimal StartAnzahl { get; set; }
			public decimal Anzahl { get; set; }
			public decimal TotalPrice { get; set; }
			public string RahmenBezug { get; set; }
			public PositionModel(Infrastructure.Data.Entities.Joins.MTM.Order.OrderValidationReportEntity orderValidationReportEntity)
			{
				if(orderValidationReportEntity == null)
				{
					return;
				}
				Bestellnummer = orderValidationReportEntity.Bestellnummer;
				Rabatt1 = orderValidationReportEntity.Rabatt1;
				Rabatt2 = orderValidationReportEntity.Rabatt2;
				Menge = orderValidationReportEntity.Menge;
				Einzelpreis = orderValidationReportEntity.Einzelpreis;
				Gesamtpreis = orderValidationReportEntity.Gesamtpreis;
				Bezeichnung_1 = orderValidationReportEntity.Bezeichnung_1;
				Bezeichnung_2 = orderValidationReportEntity.Bezeichnung_2;
				Einheit = orderValidationReportEntity.Einheit;
				Umsatzsteuer = orderValidationReportEntity.Umsatzsteuer;
				sortierung = orderValidationReportEntity.sortierung;
				best_art_nr = orderValidationReportEntity.best_art_nr;
				schrift = orderValidationReportEntity.schrift;
				Preiseinheit = orderValidationReportEntity.Preiseinheit;
				LT = orderValidationReportEntity.LT;
				Lagerort_id = orderValidationReportEntity.Lagerort_id;
				CUPreis = orderValidationReportEntity.CUPreis;
				Artikelnummer = orderValidationReportEntity.Artikelnummer;
				MHD = orderValidationReportEntity.MHD;
				COF_Pflichtig = orderValidationReportEntity.COF_Pflichtig;
				Zeitraum_MHD = orderValidationReportEntity.Zeitraum_MHD;
				EMPB = orderValidationReportEntity.EMPB;
				ESD_Schutz = orderValidationReportEntity.ESD_Schutz;
				Symbol = orderValidationReportEntity.Symbol;
				LagerortLabel = orderValidationReportEntity.LagerortLabel;
				Position = orderValidationReportEntity.Position;
				Anzahl = orderValidationReportEntity.Anzahl;
				StartAnzahl = orderValidationReportEntity.StartAnzahl;
				RahmenBezug = orderValidationReportEntity.RahmenBezug;

			}
		}
		public OrderModel(Infrastructure.Data.Entities.Joins.MTM.Order.OrderValidationReportEntity orderValidationReportEntity)
		{
			if(orderValidationReportEntity == null)
			{
				return;
			}

			// - 
			Lieferanten_Nr = orderValidationReportEntity.Lieferanten_Nr;
			Bestellung_Nr = orderValidationReportEntity.Bestellung_Nr;
			Typ = orderValidationReportEntity.Typ;
			Datum = orderValidationReportEntity.Datum;
			Anrede = orderValidationReportEntity.Anrede;
			Vorname_NameFirma = orderValidationReportEntity.Vorname_NameFirma;
			Name2 = orderValidationReportEntity.Name2;
			Name3 = orderValidationReportEntity.Name3;
			Strasse_Postfach = orderValidationReportEntity.Strasse_Postfach;
			Land_PLZ_Ort = orderValidationReportEntity.Land_PLZ_Ort;
			Personal_Nr = orderValidationReportEntity.Personal_Nr;
			Versandart = orderValidationReportEntity.Versandart;
			Zahlungsweise = orderValidationReportEntity.Zahlungsweise;
			Konditionen = orderValidationReportEntity.Konditionen;
			Zahlungsziel = orderValidationReportEntity.Zahlungsziel;
			Bezug = orderValidationReportEntity.Bezug;
			Ihr_Zeichen = orderValidationReportEntity.Ihr_Zeichen;
			Unser_Zeichen = orderValidationReportEntity.Unser_Zeichen;
			Bestellbestatigung_erbeten_bis = orderValidationReportEntity.Bestellbestatigung_erbeten_bis;
			Freitext = orderValidationReportEntity.Freitext;
			Wahrung = orderValidationReportEntity.Wahrung;
			Ansprechpartner = orderValidationReportEntity.Ansprechpartner;
			Abteilung = orderValidationReportEntity.Abteilung;
			Nr = orderValidationReportEntity.Nr;
			Rahmenbestellung = orderValidationReportEntity.Rahmenbestellung;
			Mandant = orderValidationReportEntity.Mandant;
			InfoRahmennummer = orderValidationReportEntity.InfoRahmennummer;
			RahmenBezug = orderValidationReportEntity.RahmenBezug;
			Langtexte = orderValidationReportEntity.Langtexte;
			Text_kopf = orderValidationReportEntity.Text_kopf;
			Text_fuss = orderValidationReportEntity.Text_fuss;
			Telefon = orderValidationReportEntity.Telefon;
			Fax = orderValidationReportEntity.Fax;
			Name = orderValidationReportEntity.Name;
			Telefonnummer = orderValidationReportEntity.Telefonnummer;
			Faxnummer = orderValidationReportEntity.Faxnummer;
			Email = orderValidationReportEntity.Email;
			Bestellung = orderValidationReportEntity.Bestellung;
			Bestellungen_unten = orderValidationReportEntity.Bestellungen_unten;
			Lager = $"{orderValidationReportEntity.LagerortLabel}{orderValidationReportEntity.Bestellung_Nr}";
			Symbol = orderValidationReportEntity.Symbol;
			Logo = orderValidationReportEntity.Logo;
		}

	}
}
