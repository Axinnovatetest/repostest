using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class TranspotResponseModel
	{
		public int? AnzahlvonFertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public decimal? Kennzahl { get; set; }
		public Single? Produktionszeit { get; set; }
		public decimal? Produktivitat { get; set; }
		public decimal? Stundensatz { get; set; }
		public TranspotResponseModel()
		{

		}
		public TranspotResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Logistics_TransPot entity)
		{
			if(entity == null)
				return;
			// -
			AnzahlvonFertigungsnummer = entity.AnzahlvonFertigungsnummer;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Kennzahl = entity.Kennzahl;
			Produktionszeit = entity.Produktionszeit;
			Produktivitat = entity.Produktivität;
			Stundensatz = entity.Stundensatz;
		}
	}
}
