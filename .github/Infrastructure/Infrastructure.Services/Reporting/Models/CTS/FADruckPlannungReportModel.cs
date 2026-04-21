using System;

namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FADruckPlannungReportModel
	{
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
		public string Klassifizierung { get; set; }
		public string Bezeichnung { get; set; }
		public string Nummernkreis { get; set; }
		public string Kupferzahl { get; set; }
		public int ID { get; set; }
		public string Gewerk { get; set; }

		public FADruckPlannungReportModel(Infrastructure.Data.Entities.Joins.FADruck.FAReport1PlannungEntity plannungEntity)
		{
			Artikelnummer = plannungEntity.Artikelnummer;
			Bezeichnung_1 = plannungEntity.Bezeichnung_1;
			Bezeichnung_2 = plannungEntity.Bezeichnung_2;
			Anzahl = (plannungEntity.Anzahl.HasValue) ? plannungEntity.Anzahl.Value : 0;
			Lagerort = plannungEntity.Lagerort;
			Fertigungsnummer = plannungEntity.Fertigungsnummer.HasValue ? plannungEntity.Fertigungsnummer.Value : 0;
			Datum = plannungEntity.Datum.HasValue ? plannungEntity.Datum.Value.ToString("dd/MM/yyyy") : "";
			Termin_Fertigstellung = plannungEntity.Termin_Fertigstellung.HasValue ? plannungEntity.Termin_Fertigstellung.Value.ToString("dd/MM/yyyy") : "";
			Kennzeichen = plannungEntity.Kennzeichen;
			Bemerkung = plannungEntity.Bemerkung;
			EAN = plannungEntity.EAN;
			Betrag = plannungEntity.Betrag ?? 0;
			Freigabestatus = plannungEntity.Freigabestatus;
			Produktionszeit = plannungEntity.Produktionszeit.HasValue ? plannungEntity.Produktionszeit.Value : 0;
			Termin_Bestätigt1 = plannungEntity.Termin_Bestätigt1.HasValue ? plannungEntity.Termin_Bestätigt1.Value.ToString("dd/MM/yyyy") : "";
			Erstmuster = plannungEntity.Erstmuster ?? false;
			Freigabestatus_TN_intern = plannungEntity.Freigabestatus_TN_intern;
			Index_Kunde = plannungEntity.Index_Kunde;
			Lagerort_id_zubuchen = plannungEntity.Lagerort_id_zubuchen ?? 0;
			Mandant = plannungEntity.Mandant;
			Sysmonummer = plannungEntity.Sysmonummer;
			UL_Etikett = plannungEntity.UL_Etikett ?? false;
			Technik = plannungEntity.Technik ?? false;
			Techniker = plannungEntity.Techniker;
			Kanban = plannungEntity.Kanban ?? false;
			Verpackungsart = plannungEntity.Verpackungsart;
			Verpackungsmenge = plannungEntity.Verpackungsmenge ?? 0;
			Losgroesse = plannungEntity.Losgroesse ?? 0;
			Quick_Area = plannungEntity.Quick_Area ?? false;
			Artikelfamilie_Kunde = plannungEntity.Artikelfamilie_Kunde;
			Artikelfamilie_Kunde_Detail1 = plannungEntity.Artikelfamilie_Kunde_Detail1;
			Artikelfamilie_Kunde_Detail2 = plannungEntity.Artikelfamilie_Kunde_Detail2;
			Klassifizierung = plannungEntity.Klassifizierung;
			Bezeichnung = plannungEntity.Bezeichnung;
			Nummernkreis = plannungEntity.Nummernkreis;
			Kupferzahl = plannungEntity.Kupferzahl;
			ID = plannungEntity.ID ?? 0;
			Gewerk = plannungEntity.Gewerk;
		}
	}
}
