namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class DeliveryOverviewResponseModel
	{
		public string StandardSupplier { get; set; }
		public string Designation1 { get; set; }
		public string ArticleNumber { get; set; }
		public string Designation2 { get; set; }
		public string OrderNumber { get; set; }
		public string PurchasePrice { get; set; }
		public string Name { get; set; }
		public string CustomsTariff { get; set; }
		public string SupplierNumber { get; set; }
		public string OriginCountry { get; set; }
		public string NetWeightinGr { get; set; }
		public DeliveryOverviewResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsDeliveryOverview logisticsDeliveryOverview)
		{
			if(logisticsDeliveryOverview == null)
				return;

			StandardSupplier = logisticsDeliveryOverview.StandardSupplier;
			Designation1 = logisticsDeliveryOverview.Designation1;
			ArticleNumber = logisticsDeliveryOverview.ArticleNumber;
			Designation2 = logisticsDeliveryOverview.Designation2;
			OrderNumber = logisticsDeliveryOverview.OrderNumber;
			PurchasePrice = logisticsDeliveryOverview.PurchasePrice;
			Name = logisticsDeliveryOverview.Name;
			CustomsTariff = logisticsDeliveryOverview.CustomsTariff;
			SupplierNumber = logisticsDeliveryOverview.SupplierNumber;
			OriginCountry = logisticsDeliveryOverview.OriginCountry;
			NetWeightinGr = logisticsDeliveryOverview.NetWeightinGr;
		}
	}
}
