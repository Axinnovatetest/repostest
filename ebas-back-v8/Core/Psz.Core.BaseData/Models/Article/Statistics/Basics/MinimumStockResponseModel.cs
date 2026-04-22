namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class MinimumStockResponseModel
	{
		public decimal? Aktueller_Bestand { get; set; }
		public decimal? Bestandskosten { get; set; }
		public string Bezeichnung { get; set; }
		public decimal? EK { get; set; }
		public string Lagerort { get; set; }
		public decimal? Mindestbestand { get; set; }
		public decimal? Mindestbestandskosten { get; set; }
		public string PSZ_Nummer { get; set; }

		public int? ArtikleNr { get; set; }
		public MinimumStockResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Mindesbestand entity)
		{
			if(entity == null)
				return;
			ArtikleNr = entity.ArtikleNr;
			Aktueller_Bestand = entity.Aktueller_Bestand;
			Bestandskosten = entity.Bestandskosten;
			Bezeichnung = entity.Bezeichnung;
			EK = entity.EK;
			Lagerort = entity.Lagerort;
			Mindestbestand = entity.Mindestbestand;
			Mindestbestandskosten = entity.Mindestbestandskosten;
			PSZ_Nummer = entity.PSZ_Nummer;
		}
	}
}
