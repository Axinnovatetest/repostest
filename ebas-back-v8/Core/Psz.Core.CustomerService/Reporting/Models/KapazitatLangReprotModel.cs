using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class KapazitatLangReprotModel
	{
		public KapzitatLangReportHeaderModel Header { get; set; }
		public List<KapazitatLangReportClientsModel> Clients { get; set; }
		public List<KapzitatLangDetailsModel> Details { get; set; }

		public KapazitatLangReprotModel()
		{

		}
	}
	public class KapazitatLangEntryModel: IDateRangeNullableModel
	{
		public int? AT_PD { get; set; }
		public int? Warehouse { get; set; }
		public string ClientCode { get; set; }
	}
	public class KapzitatLangReportHeaderModel
	{
		public string Lagerort { get; set; }
		public string Von { get; set; }
		public string Bis { get; set; }
		public string Tage { get; set; }
		public string SumZeit { get; set; }
		public string SumMA { get; set; }
		public string SumEuro { get; set; }

		public KapzitatLangReportHeaderModel()
		{
		}
	}
	public class KapazitatLangReportClientsModel
	{
		public string Kunde { get; set; }
		public string Count { get; set; }
		public KapazitatLangReportClientsModel()
		{

		}
		public KapazitatLangReportClientsModel(string kunde, int count)
		{
			Kunde = kunde;
			Count = count.ToString();
		}
	}
	public class KapzitatLangDetailsModel
	{
		public string Kunde { get; set; }
		public string Termin_Fertigstellung { get; set; }
		public string Termin_Bestatigt1 { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Fertigungsnummer { get; set; }
		public string Bemerkung { get; set; }
		public string Artikelnummer { get; set; }
		public string Anzahl { get; set; }
		public string Auftragszeit { get; set; }
		public string MA { get; set; }
		public string LohnkostLohnkosten { get; set; }
		public string Technik { get; set; }
		public string Erstmuster { get; set; }
		public string Techniker { get; set; }
		public string Freigabestatus { get; set; }

		public KapzitatLangDetailsModel()
		{

		}
		public KapzitatLangDetailsModel(Infrastructure.Data.Entities.Joins.CTS.KapazitatLangEntity entity, int? at)
		{

			Kunde = entity.Kunde;
			Termin_Fertigstellung = entity.Termin_Fertigstellung.HasValue ? entity.Termin_Fertigstellung.Value.ToString("dd.MM.yyyy") : "";
			Termin_Bestatigt1 = entity.Termin_Bestatigt1.HasValue ? entity.Termin_Bestatigt1.Value.ToString("dd.MM.yyyy") : "";
			Bezeichnung_1 = entity.Bezeichnung_1;
			Fertigungsnummer = entity.Fertigungsnummer.HasValue ? entity.Fertigungsnummer.ToString() : "";
			Bemerkung = entity.Bemerkung;
			Artikelnummer = entity.Artikelnummer;
			Anzahl = entity.Anzahl.HasValue ? entity.Anzahl.Value.ToString() : "";
			Auftragszeit = entity.Auftragszeit.HasValue ? Math.Round(entity.Auftragszeit.Value, 0).ToString() : "";
			if(entity.Auftragszeit.HasValue && at.HasValue)
			{
				var value = entity.Auftragszeit.Value / 6 / at.Value;
				var _ma = string.Format("{0:0.0}", Math.Truncate(value * 10) / 10);
				MA = _ma;
			}
			LohnkostLohnkosten = entity.LohnkostLohnkosten.HasValue ? Math.Round(entity.LohnkostLohnkosten.Value, 0).ToString() : "";
			Technik = entity.Technik.HasValue && entity.Technik.Value ? "✓" : "";
			Techniker = entity.Techniker;
			Freigabestatus = entity.Freigabestatus;
		}
	}
	public class KapazitatReportDetailsSumsModel
	{
		public string Kunde { get; set; }
		public Decimal SumZeit { get; set; }
		public Decimal SumMA { get; set; }
		public Decimal SumEuro { get; set; }
		public string Percent { get; set; }
	}
}
