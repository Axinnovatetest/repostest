using System;

namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FADruckHeaderReportModel
	{
		public int ID { get; set; }
		public int ID_Rahmenfertigung { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public Decimal Anzahl { get; set; }
		public string Lagerort { get; set; }
		public int Fertigungsnummer { get; set; }
		public string Datum { get; set; }
		public string Termin_Fertigstellung { get; set; }
		public string Kennzeichen { get; set; }
		public string Bemerkung { get; set; }
		public string EAN { get; set; }
		public Decimal Betrag { get; set; }
		public string Freigabestatus { get; set; }
		public Decimal Produktionszeit { get; set; }
		public string Termin_Bestätigt1 { get; set; }
		public bool Erstmuster { get; set; }
		public string Freigabestatus_TN_intern { get; set; }
		public string Index_Kunde { get; set; }
		public int Lagerort_id_zubuchen { get; set; }
		public string Mandant { get; set; }
		public string Sysmonummer { get; set; }
		public bool UL_Etikett { get; set; }
		public bool Technik { get; set; }
		public string Techniker { get; set; }
		public bool Kanban { get; set; }
		public string Verpackungsart { get; set; }
		public Decimal Verpackungsmenge { get; set; }
		public Decimal Losgroesse { get; set; }
		public bool Quick_Area { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public string Artikelfamilie_Kunde_Detail1 { get; set; }
		public string Artikelfamilie_Kunde_Detail2 { get; set; }
		public string Halle { get; set; }
		//
		public string Artikelfamille { get; set; }
		public string Detail_1 { get; set; }
		public string Detail_2 { get; set; }
		public Decimal H_FA { get; set; }
		public bool ESD { get; set; }
		public string Company { get; set; }
		public string ErstmusterTag { get; set; }
		public FADruckHeaderReportModel()
		{

		}

		public FADruckHeaderReportModel(Infrastructure.Data.Entities.Joins.FADruck.FAReport1HeaderEntity headerEntity)
		{
			var fa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get((int)headerEntity.ID);
			ID = headerEntity.ID ?? 0;
			ID_Rahmenfertigung = headerEntity.ID_Rahmenfertigung ?? 0;
			Artikelnummer = headerEntity.Artikelnummer;
			Bezeichnung_1 = headerEntity.Bezeichnung_1;
			Bezeichnung_2 = headerEntity.Bezeichnung_2;
			Anzahl = headerEntity.Anzahl ?? 0;
			Lagerort = headerEntity.Lagerort;
			Fertigungsnummer = headerEntity.Fertigungsnummer ?? 0;
			Datum = headerEntity.Datum.HasValue ? (headerEntity.Datum.Value.ToString("dd/MM/yyyy")) : "";
			Termin_Fertigstellung = headerEntity.Termin_Fertigstellung.HasValue ? (headerEntity.Termin_Fertigstellung.Value.ToString("dd/MM/yyyy")) : "";
			Kennzeichen = headerEntity.Kennzeichen;
			Bemerkung = headerEntity.Bemerkung;
			EAN = headerEntity.EAN;
			Betrag = headerEntity.Betrag ?? 0;
			Freigabestatus = headerEntity.Freigabestatus;
			Produktionszeit = headerEntity.Produktionszeit ?? 0;
			Termin_Bestätigt1 = headerEntity.Termin_Bestätigt1.HasValue ? (headerEntity.Termin_Bestätigt1.Value.ToString("dd/MM/yyyy")) : "";
			Erstmuster = headerEntity.Erstmuster ?? false;
			Freigabestatus_TN_intern = headerEntity.Freigabestatus_TN_intern;
			Index_Kunde = headerEntity.Index_Kunde;
			Lagerort_id_zubuchen = headerEntity.Lagerort_id_zubuchen ?? 0;
			Mandant = headerEntity.Mandant;
			Sysmonummer = headerEntity.Sysmonummer;
			UL_Etikett = headerEntity.UL_Etikett ?? false;
			Technik = headerEntity.Technik ?? false;
			Techniker = headerEntity.Techniker;
			Kanban = headerEntity.Kanban ?? false;
			Verpackungsart = headerEntity.Verpackungsart;
			Verpackungsmenge = headerEntity.Verpackungsmenge ?? 0;
			Losgroesse = headerEntity.Losgroesse ?? 0;
			Artikelfamilie_Kunde = headerEntity.Artikelfamilie_Kunde;
			Artikelfamilie_Kunde_Detail1 = headerEntity.Artikelfamilie_Kunde_Detail1;
			Artikelfamilie_Kunde_Detail2 = headerEntity.Artikelfamilie_Kunde_Detail2;
			Halle = headerEntity.Halle;
			Artikelfamille = (headerEntity.Artikelnummer.Length >= 3 && headerEntity.Artikelnummer.Substring(0, 3) == "881") ? headerEntity.Artikelfamilie_Kunde : "";
			Detail_1 = (headerEntity.Artikelnummer.Length >= 3 && headerEntity.Artikelnummer.Substring(0, 3) == "881") ? headerEntity.Artikelfamilie_Kunde_Detail1 : "";
			Detail_2 = (headerEntity.Artikelnummer.Length >= 3 && headerEntity.Artikelnummer.Substring(0, 3) == "881") ? headerEntity.Artikelfamilie_Kunde_Detail2 : "";
			H_FA = (headerEntity.Produktionszeit * headerEntity.Anzahl) / 60 ?? 0;
			if(fa.Lagerort_id == 7 || fa.Lagerort_id == 42 || fa.Lagerort_id == 60)
				Company = "Tunisie sarl";
			else if(fa.Lagerort_id == 26)
				Company = "Albania";
			else
				Company = "electronic GmbH";
			ErstmusterTag = (headerEntity.Technik.HasValue && headerEntity.Technik.Value && headerEntity.Erstmuster.HasValue && headerEntity.Erstmuster.Value) ?
				"ERSTMUSTERAUFTRAG" : "";

		}
	}
}
