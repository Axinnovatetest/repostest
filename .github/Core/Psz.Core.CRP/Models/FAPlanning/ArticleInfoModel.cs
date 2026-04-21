
namespace Psz.Core.CRP.Models.FAPlanning
{
	public class ArticleInfoModel
	{
		public decimal? Produktionslosgrosse { get; set; }
		public decimal? Losgrosse { get; set; }
		public decimal? VPE { get; set; }
		public decimal? VGZ { get; set; }
		public decimal? Mindestbestand { get; set; }
		public int ResidueFaCount { get; set; } = 0;
		public decimal ResidueFaQuantity { get; set; } = 0;
		public string ExternalStatus { get; set; }
		public string ProductionSite { get; set; }
		public string BemerkungCRPPlanung { get; set; }
		public ArticleInfoModel()
		{

		}
		public ArticleInfoModel(Psz.Core.BaseData.Models.Article.SalesExtension.SalesItemModel entity, decimal mindestbestand, decimal productionLotsize, int rfaCount, decimal rfaQty, string externalStatus, string productionSite, string bemerkungCRPPlanung)
		{
			Losgrosse = entity?.LotSize;
			VPE = entity?.PackagingQuantity;
			VGZ = entity?.ProductionTime;
			Mindestbestand = mindestbestand;
			Produktionslosgrosse = productionLotsize;
			ResidueFaCount = rfaCount;
			ResidueFaQuantity = rfaQty;
			ExternalStatus = externalStatus;
			ProductionSite = productionSite;
			BemerkungCRPPlanung = bemerkungCRPPlanung;
		}
	}
}
