namespace Psz.Core.BaseData.Models.Article
{
	public class SearchNummerRequestModel
	{
		public string Terms { get; set; }
		public int MaxItemsCount { get; set; } = 20;
	}
	public class SearchNummerResponseModel
	{
		public int ArticleNr { get; set; }
		public string ArticleNumber { get; set; }
		public string Designation1 { get; set; }
		public string Designation2 { get; set; }

		public SearchNummerResponseModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			if(artikelEntity == null)
				return;

			ArticleNr = artikelEntity.ArtikelNr;
			ArticleNumber = artikelEntity.ArtikelNummer;
			Designation1 = artikelEntity.Bezeichnung1;
			Designation2 = artikelEntity.Bezeichnung2;
		}
	}
}
