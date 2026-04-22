namespace Psz.Core.BaseData.Models.Article.Statistics.Controlling
{
	public class LangtextResponseModel
	{
		public int ArticleId { get; set; }
		public string Artikelnummer { get; set; }
		public bool Langtext_drucken_AB { get; set; }
		public bool Langtext_drucken_BW { get; set; }
		public string Langtext { get; set; }
		public LangtextResponseModel()
		{

		}
		public LangtextResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingArtikelEntity controlling)
		{
			if(controlling == null)
				return;

			ArticleId = controlling.Artikel_Nr;
			Artikelnummer = controlling.Artikelnummer;
			Langtext = controlling.Langtext;
			Langtext_drucken_AB = controlling.Langtext_drucken_AB ?? false;
			Langtext_drucken_BW = controlling.Langtext_drucken_BW ?? false;
		}
	}
}
