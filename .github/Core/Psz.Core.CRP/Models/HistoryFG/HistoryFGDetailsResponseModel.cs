using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.HistoryFG;

namespace Psz.Core.CRP.Models.HistoryFG
{
	public class HistoryDataFGDetailsRequestModel: IPaginatedRequestModel
	{
		public string? SearchValue { get; set; }
		public int? IdSearch { get; set; }

	}
	public class HistoryDataFGDetailsResponseModel
	{
		public string? ArticleDesignation1 { get; set; }
		public string? ArticleDesignation2 { get; set; }
		public string? ArticleNumber { get; set; }
		public string? ArticleReleaseStatus { get; set; }
		public string? CsContact { get; set; }
		public string? CustomerName { get; set; }
		public int? CustomerNumber { get; set; }
		public bool? EdiStandard { get; set; }
		public int? HeaderId { get; set; }
		public long Id { get; set; }
		public decimal? StockQuantity { get; set; }
		public decimal? TotalCostsWithCu { get; set; }
		public decimal? TotalCostsWithoutCu { get; set; }
		public decimal? TotalSalesPrice { get; set; }
		public bool? UBG { get; set; }
		public decimal? UnitSalesPrice { get; set; }
		public int? WarehouseId { get; set; }
		public string? WarehouseName { get; set; }
		public int? ArticleNr { get; set; }

		public DateTime? Datum { get; set; }

		public HistoryDataFGDetailsResponseModel()
		{
		}

		public HistoryDataFGDetailsResponseModel(Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity __HistoryDetailsFGBestandEntity, int? articleNr)
		{
			if(__HistoryDetailsFGBestandEntity == null)
				return;

			Id = __HistoryDetailsFGBestandEntity.Id;
			ArticleDesignation1 = __HistoryDetailsFGBestandEntity.ArticleDesignation1;
			ArticleDesignation2 = __HistoryDetailsFGBestandEntity.ArticleDesignation2;
			ArticleNumber = __HistoryDetailsFGBestandEntity.ArticleNumber;
			ArticleReleaseStatus = __HistoryDetailsFGBestandEntity.ArticleReleaseStatus;
			CsContact = __HistoryDetailsFGBestandEntity.CsContact;
			EdiStandard = __HistoryDetailsFGBestandEntity.EdiStandard;
			HeaderId = __HistoryDetailsFGBestandEntity.HeaderId;
			StockQuantity = __HistoryDetailsFGBestandEntity.StockQuantity;
			TotalCostsWithCu = __HistoryDetailsFGBestandEntity.TotalCostsWithCu;
			TotalCostsWithoutCu = __HistoryDetailsFGBestandEntity.TotalCostsWithoutCu;
			TotalSalesPrice = __HistoryDetailsFGBestandEntity.TotalSalesPrice;
			UBG = __HistoryDetailsFGBestandEntity.UBG;
			UnitSalesPrice = __HistoryDetailsFGBestandEntity.UnitSalesPrice;
			WarehouseId = __HistoryDetailsFGBestandEntity.WarehouseId;
			WarehouseName = __HistoryDetailsFGBestandEntity.WarehouseName;
			CustomerName = __HistoryDetailsFGBestandEntity.CustomerName;
			CustomerNumber = __HistoryDetailsFGBestandEntity.CustomerNumber;
			ArticleNr = (int)articleNr;
			//isRemoved = __HistoryDetailsFGBestandEntity.LogDescription.ToLower().Contains("Delete".ToLower()) ? true : false;
		}
	}
	public class HistoryDataDetailsRequestFGModel
	{
		public List<HistoryDataFGDetailsResponseModel> DataFgXls { get; set; }
		public DateTime? DatumImport { get; set; }

	}
}
public class HistoryFGDetailsResponseModel: IPaginatedResponseModel<HistoryDataFGDetailsResponseModel>
{
}
