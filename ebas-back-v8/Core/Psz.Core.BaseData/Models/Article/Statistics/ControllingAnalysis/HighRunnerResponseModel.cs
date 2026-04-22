namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class HighRunnerResponseModel
	{
		public string ArtikelNummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Bezeichnung2 { get; set; }
		public string Bestell_Nr { get; set; }
		public string Name1 { get; set; }
		public string GebuchterWareneingang { get; set; }
		public string MengeGebuchtFA { get; set; }
		public string EinkaufsVolume { get; set; }
		public string Einkaufspreis { get; set; }
		public string Zolltarif_Nr { get; set; }
		public string Getwichte { get; set; }
		public HighRunnerResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_HighRunner entity)
		{
			if(entity == null)
				return;

			ArtikelNummer = entity.ArtikelNummer;
			Bezeichnung1 = entity.Bezeichnung1;
			Bezeichnung2 = entity.Bezeichnung2;
			Bestell_Nr = entity.Bestell_Nr;
			Name1 = entity.Name1;
			GebuchterWareneingang = entity.GebuchterWareneingang;
			MengeGebuchtFA = entity.MengeGebuchtFA;
			EinkaufsVolume = entity.EinkaufsVolume;
			Einkaufspreis = entity.Einkaufspreis;
			Zolltarif_Nr = entity.Zolltarif_Nr;
			Getwichte = entity.Getwichte;
		}
	}
}
