namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class MaterialStockProdResponseMode
	{
		public string Artikelnummer { get; set; }
		public int? ArtikleNr { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? Im_Lager { get; set; }
		public decimal? In_Produktion { get; set; }
		public MaterialStockProdResponseMode(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_MaterialBestandProd entity)
		{
			if(entity == null)
				return;
			ArtikleNr = entity.ArtikleNr;
			Artikelnummer = entity.Artikelnummer;
			Bestand = entity.Bestand;
			Im_Lager = entity.Im_Lager;
			In_Produktion = entity.In_Produktion;
		}
	}
}
