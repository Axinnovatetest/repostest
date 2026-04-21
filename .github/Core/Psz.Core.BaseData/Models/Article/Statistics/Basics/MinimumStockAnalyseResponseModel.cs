namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class MinimumStockAnalyseResponseModel
	{
		public decimal? Abweichung { get; set; }
		public string Artikelnummer { get; set; }
		public int? ArtikelNr { get; set; }
		public decimal? Bedarf_nachste_8_Wochen { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? Empfohlener_Mindestbestand { get; set; }
		public decimal? Mindestbestand { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }

		public MinimumStockAnalyseResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_MaterialBestandAnalyse entity)
		{
			if(entity == null)
				return;
			ArtikelNr = entity.ArtikelNr;
			Abweichung = entity.Abweichung;
			Artikelnummer = entity.Artikelnummer;
			Bedarf_nachste_8_Wochen = entity.Bedarf_nachste_8_Wochen;
			Bestand = entity.Bestand;
			Empfohlener_Mindestbestand = entity.Empfohlener_Mindestbestand;
			Mindestbestand = entity.Mindestbestand;
			Wiederbeschaffungszeitraum = entity.Wiederbeschaffungszeitraum;
		}
	}
}
