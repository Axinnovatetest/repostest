using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class BacklogHWReportModel
	{
		public List<BacklogHWReportDetailsModel> Details { get; set; }
		public BackLogReportHeaderModel Header { get; set; }

		public BacklogHWReportModel()
		{

		}
	}
	public class BackLogReportHeaderModel
	{
		public string PRORT { get; set; }
		public Decimal TotalGesamt { get; set; }
		public Decimal TotalGesamtPersonalKosten { get; set; }
		public Decimal TotalGesamtKosten { get; set; }
		public Decimal TotalDBI { get; set; }
		public Decimal TotalPercent { get; set; }


		public BackLogReportHeaderModel(string prort, List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity> details)
		{
			PRORT = prort;
			TotalGesamt = details?.Sum(a => a.Gesamtpreis ?? 0) ?? 0;
			TotalGesamtPersonalKosten = details?.Sum(a => a.Gesamtpersonalkosten ?? 0) ?? 0;
			TotalGesamtKosten = details?.Sum(a => a.Gesamtkosten ?? 0) ?? 0;
			TotalDBI = details?.Sum(a => (a.Gesamtpreis ?? 0) - (a.Gesamtkosten ?? 0)) ?? 0;
			if(details != null && details.Count > 0)
			{
				decimal val = 0m;
				foreach(var item in details)
				{
					if(item.Gesamtkosten.HasValue && item.Gesamtkosten.Value > 0)
					{
						val += (item.Gesamtpreis.Value - item.Gesamtkosten.Value) / item.Gesamtkosten.Value;
					}
				}
				TotalPercent = val;
			}
		}
	}
	public class BacklogHWReportDetailsModel
	{
		public string Name1 { get; set; }
		public int Angebot_Nr { get; set; }
		public bool erledigt_pos { get; set; }//
		public bool erledigt { get; set; }//
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
		public Decimal Gesamtkosten { get; set; }
		public bool Stückliste { get; set; }
		public string Kostenart { get; set; }
		public Decimal Betrag { get; set; }
		public Decimal Gesamtpersonalkosten { get; set; }
		public string Artikelnummer { get; set; }
		public int Fertigungsnummer { get; set; }
		public int OriginalAnzahl { get; set; }
		public string Mandant { get; set; }
		public Decimal Kosten { get; set; }
		public int Lagerort_id { get; set; }//
		public string Typ { get; set; }//
		public Decimal Gewicht { get; set; }
		public bool Standardlieferant { get; set; }//
		public Decimal Percent { get; set; }
		public Decimal DBI { get; set; }
		public BacklogHWReportDetailsModel()
		{

		}
		public BacklogHWReportDetailsModel(Infrastructure.Data.Entities.Joins.CTS.BacklogHWEntity entity)
		{
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
			Gesamtkosten = entity.Gesamtkosten ?? 0;
			Stückliste = entity.Stückliste ?? false;
			Kostenart = entity.Kostenart;
			Betrag = entity.Betrag ?? 0;
			Gesamtpersonalkosten = entity.Gesamtpersonalkosten ?? 0;
			Artikelnummer = entity.Artikelnummer;
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			OriginalAnzahl = entity.OriginalAnzahl ?? 0;
			Mandant = entity.Mandant;
			Kosten = entity.Kosten ?? 0;
			Lagerort_id = entity.Lagerort_id ?? 0;
			DBI = entity.Gesamtpreis.HasValue && entity.Gesamtkosten.HasValue && entity.Gesamtkosten.Value < 0.5m ? 0.01m : entity.Gesamtpreis.Value - entity.Gesamtkosten.Value;
			Percent = entity.Gesamtpreis.HasValue && entity.Gesamtkosten.HasValue && entity.Gesamtkosten.Value < 0.5m
				? 0m : ((entity.Gesamtpreis.Value - entity.Gesamtkosten.Value) / entity.Gesamtkosten.Value);
		}
	}
}
