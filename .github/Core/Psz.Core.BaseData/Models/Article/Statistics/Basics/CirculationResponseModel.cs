using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class CirculationResponseModel
	{
		public decimal? Anzahl { get; set; }
		public string Artikelnummer_Umlauf { get; set; }
		public decimal? Bestand { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public int? Bestellung_Nr { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int? SummevonBedarf { get; set; }
		public decimal? Transfer_Bestand { get; set; }

		public CirculationResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf entity)
		{
			if(entity == null)
				return;

			Anzahl = entity.Anzahl;
			Artikelnummer_Umlauf = entity.Artikelnummer_Umlauf;
			Bestand = entity.Bestand;
			Bestatigter_Termin = entity.Bestatigter_Termin;
			Bestellung_Nr = entity.Bestellung_Nr;
			Liefertermin = entity.Liefertermin;
			SummevonBedarf = entity.SummevonBedarf;
			Transfer_Bestand = entity.Transfer_Bestand;
		}
	}
}
