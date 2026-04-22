using Infrastructure.Data.Entities.Joins.MTM.Order;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{


	public class NeededAndOrderedQunatityWithStockResponseModel
	{
		public List<MergedQuantityAndOrdersResponseModel> mergedQuantityAndOrders { get; set; }
		public decimal? stock { get; set; }
		public decimal? minStock { get; set; }
		public decimal? minRequiredQuantity { get; set; }
		public decimal? shippingTime { get; set; }
		public NeededAndOrderedQunatityWithStockResponseModel(List<MergedQuantityAndOrdersResponseModel> data, decimal? mystock, decimal? myminstock, float myQuantity, int shippingPeriod)
		{
			mergedQuantityAndOrders = data ?? default;
			stock = mystock ?? 0;
			minStock = myminstock ?? 0;
			minRequiredQuantity = Convert.ToDecimal(myQuantity);
			shippingTime = shippingPeriod;
		}
	}
	public class MergedQuantityAndOrdersResponseModel: IComparable<MergedQuantityAndOrdersResponseModel>
	{
		public decimal? NeededQuantity { get; set; }
		public decimal? orderedQuantity { get; set; }
		public string Week { get; set; }
		public MergedQuantityAndOrdersResponseModel(NeededQuantityEntity need)
		{

			orderedQuantity = 0;
			NeededQuantity = need.NeededQuantity;
			Week = need.Week;
		}
		public MergedQuantityAndOrdersResponseModel(OrderedQuantityEntity orders)
		{
			NeededQuantity = 0;
			orderedQuantity = orders.orderedQuantity;
			Week = orders.Week;
		}
		public MergedQuantityAndOrdersResponseModel(DateTime date)
		{
			NeededQuantity = 0;
			orderedQuantity = 0;
			Week = string.Concat(Psz.Core.MaterialManagement.Helpers.SpecialHelper.ExtractIsoWeek(date), "/", date.Year);
		}

		public int CompareTo(MergedQuantityAndOrdersResponseModel other)
		{
			return Psz.Core.MaterialManagement.Helpers.SpecialHelper.CompareWeekPatternDiff(Week, other.Week) ? 1 : -1;
		}
	}
	public class NeededQuantityOrderedQuantityRequestModel
	{
		[Required]
		[Range(0, 6, ErrorMessage = "Please enter a valid timespan  3 to 6 months !")]
		public int Months { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid article number !")]
		public int ArtikelNr { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a location !")]
		public int CountryID { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid location !")]
		public int UnitId { get; set; }
	}
	public class NeededQuantityOrderedQuantityAnalysisRequestModel: IPaginatedRequestModel
	{
		[Required]
		[Range(0, 6, ErrorMessage = "Please enter a valid timespan  3 to 6 months !")]
		public int Months { get; set; }
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a location !")]
		public int? CountryID { get; set; }
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid location !")]
		public int? UnitId { get; set; }
		public bool? IsExtra { get; set; }
		public string FilterTerms { get; set; } = "";
	}

	#region Analyse Need
	public class NeedStockPerTypeResponseModel: IPaginatedResponseModel<NeedStockItemModel>
	{
		public DateTime DateTill { get; set; }
	}
	public class NeedStockResponseModel
	{
		public List<NeedStockItemModel> ItemsExtraROH { get; set; }
		public List<NeedStockItemModel> ItemsMissingROH { get; set; }
		public DateTime DateTill { get; set; }
	}
	public class NeedStockItemModel
	{
		public string Artikelnummer { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal? DiffPrice { get; set; }
		public decimal? DiffQuantity { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public string Name1 { get; set; }
		public decimal? ROH_Stock { get; set; }
		public decimal? ROH_Quantity { get; set; }
		public decimal? Wert_LagerStockNeed { get; set; }
		public NeedStockItemModel(Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			Artikelnummer = entity.Artikelnummer;
			Bestell_Nr = entity.Bestell_Nr;
			DiffPrice = entity.DiffPrice;
			DiffQuantity = entity.DiffQuantity;
			Einkaufspreis = entity.Einkaufspreis;
			Gesamtpreis = entity.Gesamtpreis;
			Name1 = entity.Name1;
			ROH_Stock = entity.ROH_Bestand;
			ROH_Quantity = entity.ROH_Quantity;
			Wert_LagerStockNeed = entity.Wert_LagerBestandBedarf;
		}
	}
	public class NeedStockSummaryItemModel
	{
		public decimal TotalAmount { get; set; }
		public decimal PeriodAmount { get; set; }
		public decimal UnnecessaryAmount { get; set; }
		public DateTime DateTill { get; set; }
		public int Year { get; set; }
		public int KW { get; set; }
		public NeedStockSummaryItemModel(Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedSummaryEntity entity)
		{
			//DateTill = date;
			Year = entity?.Year ?? 0;
			KW = entity?.KW ?? 0;
			TotalAmount = entity?.TotalAmount ?? 0;
			PeriodAmount = entity?.PeriodAmount ?? 0;
			UnnecessaryAmount = (entity?.TotalAmount ?? 0) - (entity?.PeriodAmount ?? 0);
		}
		public NeedStockSummaryItemModel(DateTime date, int year, int kw, Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedSummaryEntity entity)
		{
			DateTill = date;
			Year = year;
			KW = kw;
			TotalAmount = entity?.TotalAmount ?? 0;
			PeriodAmount = entity?.PeriodAmount ?? 0;
			UnnecessaryAmount = (entity?.TotalAmount ?? 0) - (entity?.PeriodAmount ?? 0);
		}
	}
	#endregion Analyse Need
}
