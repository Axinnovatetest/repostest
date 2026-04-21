using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Technic
{
	public class ProdTNResponseModel
	{
		public int? AnzahlvonFertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public int? ArtikelNr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_3 { get; set; }
		public decimal? Kennzahl { get; set; }
		public Single? Produktionszeit { get; set; }
		public decimal? Produktivitat { get; set; }
		public decimal? Stundensatz { get; set; }
		public ProdTNResponseModel()
		{

		}
		public ProdTNResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_ProdTN entity)
		{
			if(entity == null)
				return;
			// -
			ArtikelNr = entity.ArtikelNr;
			AnzahlvonFertigungsnummer = entity.AnzahlvonFertigungsnummer;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Bezeichnung_3 = entity.Bezeichnung_3;
			Kennzahl = entity.Kennzahl;
			Produktionszeit = entity.Produktionszeit;
			Produktivitat = entity.Produktivität;
			Stundensatz = entity.Stundensatz;
		}
	}
}
