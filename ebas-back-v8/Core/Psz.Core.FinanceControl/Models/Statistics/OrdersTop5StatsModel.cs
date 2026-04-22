using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class OrdersTop5StatsModel
	{
		public IEnumerable<OrdersHigestOrSlowestAmountModel> HighestAmounts { get; set; }
		public IEnumerable<OrdersHigestOrSlowestAmountModel> LowestAmounts { get; set; }
		public IEnumerable<FastestOrSlowestOrdersModel> FastestOrders { get; set; }
		public IEnumerable<FastestOrSlowestOrdersModel> SlowestOrders { get; set; }
		public IEnumerable<OrdersHigestDelayModel> HighestDelay { get; set; }
		public IEnumerable<OrdersSlowestBookingModel> SlowestBooking { get; set; }
	}

	public class FastestOrSlowestOrdersModel
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public DateTime? ValidationRequestTime { get; set; }
		public DateTime? Termin { get; set; }
		public string Diff { get; set; }
		public FastestOrSlowestOrdersModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.FastestOrSlowestOrdersEntity entity)
		{
			Id = entity.Id;
			OrderNumber = entity.OrderNumber;
			ValidationRequestTime = entity.ValidationRequestTime;
			Termin = entity.Termin;
			Diff = Helpers.CalculationsHelper.ConvertDaysToYearsMonthsDays(entity.Diff ?? -1);
		}
	}
	public class OrdersHigestOrSlowestAmountModel
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public decimal? Amount { get; set; }
		public OrdersHigestOrSlowestAmountModel(Tuple<int, string, decimal> entity)
		{
			Id = entity.Item1;
			OrderNumber = entity.Item2;
			Amount = entity.Item3;
		}
	}
	public class OrdersHigestDelayModel
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public string Supplier { get; set; }
		public DateTime? DeliveryWishDate { get; set; }
		public DateTime? DeliveryActualDate { get; set; }
		public string Diff { get; set; }
		public OrdersHigestDelayModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersHighestDelayEntity entity)
		{
			Id = entity.Id;
			OrderNumber = entity.OrderNumber;
			Supplier = entity.Supplier;
			DeliveryWishDate = entity.DeliveryWishDate;
			DeliveryActualDate = entity.DeliveryActualDate;
			Diff = Helpers.CalculationsHelper.ConvertDaysToYearsMonthsDays(entity.Diff);
		}
	}
	public class OrdersSlowestBookingModel
	{
		public int? Id { get; set; }
		public string OrderNumber { get; set; }
		public DateTime? MaxDate { get; set; }
		public DateTime? MinDate { get; set; }
		public string Diff { get; set; }
		public OrdersSlowestBookingModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersSlowestBookingEntity entity)
		{
			Id = entity.Id;
			OrderNumber = entity.OrderNumber;
			MaxDate = entity.MaxDate;
			MinDate = entity.MinDate;
			Diff = Helpers.CalculationsHelper.ConvertDaysToYearsMonthsDays(entity.Diff ?? -1);
		}
	}
}