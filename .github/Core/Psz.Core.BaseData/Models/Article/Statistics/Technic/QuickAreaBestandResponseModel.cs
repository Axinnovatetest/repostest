namespace Psz.Core.BaseData.Models.Article.Statistics.Technic
{
	public class QuickAreaBestandResponseModel
	{
		public string Artikel_Artikelnummer { get; set; }
		public int? Artikel_Nr_des_Bauteils { get; set; }
		public string Artikelnummer { get; set; }
		public string Artikelnummer_Stucklisten { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int? Fertigungsnummer { get; set; }
		public decimal? SummevonBestand { get; set; }
		public decimal? SummevonBruttobedarf { get; set; }
		public decimal? Verfugbar { get; set; }

		public QuickAreaBestandResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_QuickAreaBestand entity)
		{
			if(entity == null)
				return;

			Artikel_Artikelnummer = entity.Artikel_Artikelnummer;
			Artikel_Nr_des_Bauteils = entity.Artikel_Nr_des_Bauteils;
			Artikelnummer = entity.Artikelnummer;
			Artikelnummer_Stucklisten = entity.Artikelnummer_Stucklisten;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Fertigungsnummer = entity.Fertigungsnummer;
			SummevonBestand = entity.SummevonBestand;
			SummevonBruttobedarf = entity.SummevonBruttobedarf;
			Verfugbar = entity.Verfugbar;
		}
	}
}
