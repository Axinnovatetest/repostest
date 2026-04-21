using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class AnalyseFibuRequestModel
	{
		public DateTime DateFrom { get; set; }
		public DateTime DateTo { get; set; }
		public string CustomerName { get; set; }
		public string CustomerNumber { get; set; }
		public bool IncludeDetails { get; set; } = false;
	}
	public class AnalyseFibuResponseModel
	{
		public string CustomerName { get; set; }
		public string CustomerNumber { get; set; }
		public List<AnalyseItem> Invoices { get; set; }
		public List<AnalyseItem> Credits { get; set; }

		public decimal? SumInvoices { get; set; }
		public decimal? SumCredits { get; set; }

		public int InvoicesCount { get; set; }
		public int CreditsCount { get; set; }

		public class AnalyseItem
		{
			public string Ausdruck { get; set; }
			public string Debitorenname { get; set; }
			public string Debitorennummer { get; set; }
			public decimal? Gesamtkupferzuschlag { get; set; }
			public DateTime? Rechnungsdatum { get; set; }
			public int? Rechnungsnummer { get; set; }
			public string Typ { get; set; }
			public AnalyseItem(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibu analyseFibu)
			{
				if(analyseFibu == null)
					return;

				Ausdruck = analyseFibu.Ausdruck;
				Debitorenname = analyseFibu.Debitorenname;
				Debitorennummer = analyseFibu.Debitorennummer;
				Gesamtkupferzuschlag = analyseFibu.Gesamtkupferzuschlag;
				Rechnungsdatum = analyseFibu.Rechnungsdatum;
				Rechnungsnummer = analyseFibu.Rechnungsnummer;
				Typ = analyseFibu.Typ;
			}
			public AnalyseItem(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibuHeader analyseFibu)
			{
				if(analyseFibu == null)
					return;

				Ausdruck = analyseFibu.Ausdruck;
				Debitorenname = analyseFibu.Debitorenname;
				Debitorennummer = analyseFibu.Debitorennummer;
				Gesamtkupferzuschlag = analyseFibu.Gesamtkupferzuschlag;
			}
		}
	}
}
