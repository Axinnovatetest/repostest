using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class ProductivityRequestModel
	{
		public string ArticleNumber { get; set; }
		public string Site { get; set; }
	}
	public class ProductivityResponseModel
	{
		public Item CZ { get; set; }
		public Item TN { get; set; }
		public Item AL { get; set; }
		public Item WS { get; set; }
		public Item GZ { get; set; }
		public class Item
		{

			public string Artikel_Kunde { get; set; }
			public string Artikelnummer { get; set; }
			public decimal? Artikelzeit { get; set; }
			public int? Fertigungen { get; set; }
			public decimal? Prod_Artikelzeit { get; set; }
			public decimal? Prod_FA_Zeit { get; set; }
			public decimal? Std_Satz_aktuell { get; set; }
			public Item(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity productivity)
			{
				if(productivity == null)
					return;

				// -
				Artikel_Kunde = productivity.Artikel_Kunde;
				Artikelnummer = productivity.Artikelnummer;
				Artikelzeit = productivity.Artikelzeit;
				Fertigungen = productivity.Fertigungen;
				Prod_Artikelzeit = productivity.Prod_Artikelzeit;
				Prod_FA_Zeit = productivity.Prod_FA_Zeit;
				Std_Satz_aktuell = productivity.Std_Satz_aktuell;
			}
		}
	}
	public class ProductivityDetailsResponseModel
	{
		public int? Anzahl { get; set; }
		public decimal? Artikelzeit { get; set; }
		public int? FA { get; set; }
		public decimal? FA_Zeit { get; set; }
		public decimal? Prod_Artikelzeit { get; set; }
		public decimal? Prod_FA_Zeit { get; set; }
		public DateTime? Termin { get; set; }
		public ProductivityDetailsResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails productivityDetails)
		{
			if(productivityDetails == null)
				return;

			// -
			Anzahl = productivityDetails.Anzahl;
			Artikelzeit = productivityDetails.Artikelzeit;
			FA = productivityDetails.FA;
			FA_Zeit = productivityDetails.FA_Zeit;
			Prod_Artikelzeit = productivityDetails.Prod_Artikelzeit;
			Prod_FA_Zeit = productivityDetails.Prod_FA_Zeit;
			Termin = productivityDetails.Termin;
		}
	}
}
