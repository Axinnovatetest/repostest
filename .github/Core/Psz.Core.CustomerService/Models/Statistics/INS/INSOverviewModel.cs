using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Statistics.INS
{
	public class INSOverviewModel
	{
		public List<DateValueOrderModel> Ruckstandige_Bestellungen { get; set; }
		public List<DateValueOrderModel> Umsatz_Aktuelle_Woche { get; set; }
		public LabelValueModel VK_Summe_Ungebuchte_ABs { get; set; }
		public List<LabelValueModel> Mindesbestand_Auswertung { get; set; }
	}
	public class DateValueOrderModel
	{
		public string Date { get; set; }
		public decimal Value { get; set; }
	}
	public class LabelValueModel
	{
		public string Label { get; set; }
		public decimal Value { get; set; }
	}

	public class Ruckstandige_BestellungenRequestModel
	{
		public int? CustomerNumber { get; set; }
		public int? MitarbeiterId { get; set; }
		public List<string>? Kws { get; set; }
		public List<DateTime>? Dates { get; set; }
	}
	public class Mindesbestand_AuswertungRequestModel
	{
		public int? CustomerNumber { get; set; }
		public int? MitarbeiterId { get; set; }
		public string Artikelnummer { get; set; }
	}

	public class ChartsAndOlderDatesModel
	{
		public decimal OlderDatesButtonValue { get; set; }
		public List<DateValueOrderModel> ChartValues { get; set; }
	}
}