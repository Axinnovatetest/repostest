using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{

	public class DeliveryListRequestModel
	{
		public string searchTerm { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }

	}
	public class DeliveryListResponseModel
	{
		public List<LogisticsDeliveryListModel> DeliveryLists { get; set; }
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

	}
	public class LogisticsDeliveryListModel
	{
		public int? ArticleNr { get; set; }
		public string ArticleNumber { get; set; }
		public string CustomsTariff { get; set; }
		public string EAN { get; set; }
		public string OriginCountry { get; set; }
		public string Designation1 { get; set; }
		public string Designation2 { get; set; }
		public string SupplierNumber { get; set; }
		public string Name1 { get; set; }
		public string ULCertificated { get; set; }
		public string OrderNumber { get; set; }
		public string ReplacementPeriod { get; set; }
		public string StandardSupplier { get; set; }
		public string PurchasingPrice { get; set; }
		public string SalesPrice { get; set; }
		public string MinimumOrderQuantity { get; set; }
		public string DrawingNumber { get; set; }
		public LogisticsDeliveryListModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsDeliveryList logisticsDeliveryList)
		{
			if(logisticsDeliveryList == null)
				return;
			ArticleNr = logisticsDeliveryList.ArticleNr;
			ArticleNumber = logisticsDeliveryList.ArticleNumber;
			CustomsTariff = logisticsDeliveryList.CustomsTariff;
			EAN = logisticsDeliveryList.EAN;
			OriginCountry = logisticsDeliveryList.OriginCountry;
			Designation1 = logisticsDeliveryList.Designation1;
			Designation2 = logisticsDeliveryList.Designation2;
			SupplierNumber = logisticsDeliveryList.SupplierNumber;
			Name1 = logisticsDeliveryList.Name1;
			ULCertificated = logisticsDeliveryList.ULCertificated;
			OrderNumber = logisticsDeliveryList.OrderNumber;
			ReplacementPeriod = logisticsDeliveryList.ReplacementPeriod;
			StandardSupplier = logisticsDeliveryList.StandardSupplier;
			PurchasingPrice = logisticsDeliveryList.PurchasingPrice;
			SalesPrice = logisticsDeliveryList.SalesPrice;
			MinimumOrderQuantity = logisticsDeliveryList.MinimumOrderQuantity;
			DrawingNumber = logisticsDeliveryList.DrawingNumber;
		}
	}
}
