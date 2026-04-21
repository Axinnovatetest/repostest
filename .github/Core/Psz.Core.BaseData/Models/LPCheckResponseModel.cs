namespace Psz.Core.BaseData.Models
{
	public class LPCheckResponseModel
	{
		public string ArticleNumber { get; set; }
		public string ArticleDesignation { get; set; }

		public LPCheckResponseModel()
		{

		}

		public LPCheckResponseModel(Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities LPEntity)
		{
			ArticleNumber = LPEntity.Artikelnummer;
			ArticleDesignation = LPEntity.Artikelbezeichnung;
		}
	}
}
