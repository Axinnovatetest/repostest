using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class BacklogFGReportModel
	{
		public List<BacklogFGReportDetailsModel> Details { get; set; }
		public List<BackLogReportHeaderModel> Header { get; set; }
		public bool IsAdmin { get; set; }
		public BacklogFGReportModel()
		{

		}
	}
	public class BacklogFGReportDetailsModel
	{
		public string Name1 { get; set; }
		public int Angebot_Nr { get; set; }
		public int Nr { get; set; }
		public int Artikel_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung1 { get; set; }
		public int Anzahl { get; set; }
		public string Liefertermin { get; set; }
		public Decimal Einzelpreis { get; set; }
		public Decimal Preiseinheit { get; set; }
		public Decimal Gesamtpreis { get; set; }
		public string Ausdr2 { get; set; }
		public string Ausdr1 { get; set; }
		public Decimal GesamtkostenValue { get; set; }
		public string Gesamtkosten { get; set; }
		public bool Stückliste { get; set; }
		public string Kostenart { get; set; }
		public string Betrag { get; set; }
		public Decimal GesamtpersonalkostenValue { get; set; }
		public string Gesamtpersonalkosten { get; set; }
		public string Artikelnummer { get; set; }
		public int Fertigungsnummer { get; set; }
		public int OriginalAnzahl { get; set; }
		public string Mandant { get; set; }
		public string Kosten { get; set; }
		public string Lagerort { get; set; }
		public string PRORT { get; set; }
		public Decimal Percent { get; set; }
		public string DOC_Number { get; set; }

		public BacklogFGReportDetailsModel()
		{

		}
		public BacklogFGReportDetailsModel(Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity entity)
		{
			if(entity == null)
				return;
			Name1 = !string.IsNullOrEmpty(entity.Name1) && !string.IsNullOrWhiteSpace(entity.Name1) && entity.Name1.Length > 18 ?
				entity.Name1.Substring(0, 18) : entity.Name1;
			Angebot_Nr = entity.Angebot_Nr ?? 0;
			Nr = entity.Nr ?? 0;
			Artikel_Nr = entity.Artikel_Nr ?? 0;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Bezeichnung1 = entity.Bezeichnung1;
			Anzahl = entity.Anzahl ?? 0;
			Liefertermin = entity.Liefertermin.HasValue ? entity.Liefertermin.Value.ToString("dd.MM.yyyy") : "";
			Einzelpreis = entity.Einzelpreis ?? 0;
			Preiseinheit = entity.Preiseinheit ?? 0;
			Gesamtpreis = entity.Gesamtpreis ?? 0;
			Ausdr2 = entity.Ausdr2.HasValue ? entity.Ausdr2.Value.ToString("dd.MM.yyyy") : "";
			Ausdr1 = entity.Ausdr1.HasValue ? entity.Ausdr1.Value.ToString("dd.MM.yyyy") : "";
			GesamtkostenValue = entity.Gesamtkosten ?? 0;
			Gesamtkosten = entity.Gesamtkosten.Value.ToString("F2");
			Stückliste = entity.Stückliste ?? false;
			Kostenart = entity.Kostenart;
			Betrag = entity.Betrag.Value.ToString("F2");
			GesamtpersonalkostenValue = entity.Gesamtpersonalkosten ?? 0;
			Gesamtpersonalkosten = entity.Gesamtpersonalkosten.Value.ToString("F2");
			Artikelnummer = entity.Artikelnummer;
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			OriginalAnzahl = entity.OriginalAnzahl ?? 0;
			Mandant = entity.Mandant;
			Kosten = entity.Kosten.Value.ToString("F2");
			Lagerort = entity.Lagerort;
			PRORT = entity.PRORT;
			DOC_Number = entity.DOC_Number;
			Percent = entity.Gesamtpreis.HasValue && entity.Gesamtkosten.HasValue && entity.Gesamtkosten.Value > 0
				? ((entity.Gesamtpreis.Value - entity.Gesamtkosten.Value) / entity.Gesamtkosten.Value) * 100 : 0;
		}
	}
}