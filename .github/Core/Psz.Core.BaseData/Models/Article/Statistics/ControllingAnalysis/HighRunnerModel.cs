using Psz.Core.Common.Models;
using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class HighRunnerArticleRequestModel: IPaginatedRequestModel
	{

		public string ArticleNumber { get; set; }
		public DateTime DateFrom { get; set; } = DateTime.Today.AddMonths(-6);
		public DateTime DateTo { get; set; } = DateTime.Today.AddMonths(+6);
	}
	public class HighRunnerArticleResponseModel: IPaginatedResponseModel<HighRunnerArticleItem>
	{
	}
	public class HighRunnerArticleItem
	{
		public string Artikelnummer { get; set; }

		public int? ArtikelNr { get; set; }
		public string Bestell_Nr { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Einkaufsmenge { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Einkaufsvolumen { get; set; }
		public decimal? Gewichte { get; set; }
		public string Name1 { get; set; }
		public string Zolltarif_nr { get; set; }
		public HighRunnerArticleItem(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_HighRunner highRunner)
		{
			if(highRunner == null)
				return;

			// -
			ArtikelNr = highRunner.ArtikelNr;
			Artikelnummer = highRunner.Artikelnummer;
			Bestell_Nr = highRunner.Bestell_Nr;
			Bezeichnung_2 = highRunner.Bezeichnung_2;
			Bezeichnung1 = highRunner.Bezeichnung1;
			Einkaufsmenge = highRunner.Einkaufsmenge;
			Einkaufspreis = highRunner.Einkaufspreis;
			Einkaufsvolumen = highRunner.Einkaufsvolumen;
			Gewichte = highRunner.Gewichte;
			Name1 = highRunner.Name1;
			Zolltarif_nr = highRunner.Zolltarif_nr;
		}
	}
}
