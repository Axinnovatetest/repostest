using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class WorstSuppliersStatsModel
	{
		public IEnumerable<OverdueOrdersModel> BiggestOverdueOrders { get; set; }
		public IEnumerable<BookingCountForOneOrderModel> BiggestBookingCountForOneOrder { get; set; }
		public IEnumerable<BiggestCountOfOverdueOrders> BiggestCountOfOverdueOrders { get; set; }
	}
	public class OverdueOrdersModel
	{
		public string OrderNumber { get; set; }
		public int? Id { get; set; }
		public DateTime? DeliveryWishDate { get; set; }
		public DateTime? DeliveryActualDate { get; set; }
		public string Diff { get; set; }
		public string Supplier { get; set; }
		public OverdueOrdersModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.OverdueOrdersEntity entity)
		{
			Id = entity.Id;
			OrderNumber = entity.OrderNumber;
			Supplier = entity.Supplier;
			DeliveryWishDate = entity.DeliveryWishDate;
			DeliveryActualDate = entity.DeliveryActualDate;
			Diff = Helpers.CalculationsHelper.ConvertDaysToYearsMonthsDays(entity.Diff ?? -1);
		}
	}
	public class BookingCountForOneOrderModel
	{
		public int? Id { get; set; }
		public string OrderNumber { get; set; }
		public string Supplier { get; set; }
		public int? BookingCount { get; set; }
		public BookingCountForOneOrderModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.BookingCountForOneOrderEntity entity)
		{
			Id = entity.Id;
			OrderNumber = entity.OrderNumber;
			Supplier = entity.Supplier;
			BookingCount = entity.BookingCount;
		}
	}
	public class BiggestCountOfOverdueOrders
	{
		public string Supplier { get; set; }
		public int? OverdueOrders { get; set; }
		public BiggestCountOfOverdueOrders(KeyValuePair<string, int> entity)
		{
			Supplier = entity.Key;
			OverdueOrders = entity.Value;
		}
	}
}