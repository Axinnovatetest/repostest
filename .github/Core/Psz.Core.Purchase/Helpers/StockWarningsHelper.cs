using iText.Layout.Font;
using Psz.Core.Purchase.Models.StockWarnings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Purchase.Helpers
{
	public class StockWarningsHelper
	{
		public static List<WeekValueModel> ProcessLists(List<WeekValueModel> cumuls, List<(int Week, int Year)> ranges, decimal currentStock, decimal backlogOrders, decimal backlogNeeds)
		{
			// Sort the first list by period to maintain chronological order
			//cumuls.Sort((a, b) => string.Compare(a.Week, b.Week));
			cumuls = OrderListByWeekAndYear(cumuls);
			foreach(var pair in ranges)
			{
				string rangeToFind = $"{pair.Week}/{pair.Year}";  // Convert week/year pair to period format

				// Find the index where this period should be in the sorted list
				int insertIndex = cumuls.FindInsertionIndex(new WeekValueModel { Week = rangeToFind });

				// Check if period exists
				bool rangeExists = cumuls.Any(x => x.Week == rangeToFind);

				if(!rangeExists)
				{
					// If period doesn't exist, we need to add it
					// Find the previous value to use
					var previousValue = 0m;

					if(insertIndex > 0)
					{
						// Use the value from the previous period
						previousValue = cumuls[insertIndex - 1].Value;
					}
					else if(cumuls.Count > 0)
					{
						// If inserting at the beginning, use the first available value
						previousValue = (currentStock + backlogOrders) - backlogNeeds;
						//cumuls[0].Value;
					}

					// Create new PeriodValue
					var newRangeValue = new WeekValueModel
					{
						Week = rangeToFind,
						Value = previousValue
					};

					// If insertIndex is -1 (meaning it should go at the end)
					// or if the list is empty, add to the end
					if(insertIndex == -1 || cumuls.Count == 0)
					{
						cumuls.Add(newRangeValue);
					}
					else
					{
						cumuls.Insert(insertIndex, newRangeValue);
					}
				}
			}
			return cumuls;
		}
		public static decimal GetProperPoAmount(decimal negativeAmount, decimal minimumAmout)
		{
			var result = negativeAmount;
			var _plus = 0m;
			if(-(negativeAmount) < minimumAmout)
				return minimumAmout;
			while(result <= minimumAmout)
			{
				result += minimumAmout;
				_plus += minimumAmout;
			}
			return _plus;
		}

		public static List<WeekValueModel> RecalculateCumuls(List<WeekValueModel> cumuls, List<(int Week, int Year)> ranges, WeekValueModel suggestedPo)
		{
			//var warehouses = Enums.StockWarningEnums.GetWarehousesFromUnit((Enums.StockWarningEnums.StockWarningsUnits)unit);
			//var qtyInWarehouses = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetQtyInWarehouses(warehouses, artikelNr);

			//var pos = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetPos(((Enums.StockWarningEnums.StockWarningsUnits)unit).GetDescription(), artikelNr);
			//var fas = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetFas(((Enums.StockWarningEnums.StockWarningsUnits)unit).GetDescription(), artikelNr);

			//pos = pos.Union(suggestedPos.Select(p => new Infrastructure.Data.Entities.Joins.PRS.__PRS_StockWarnings_PoStatusEntity
			//{
			//	Qty = p.Value,
			//	Week = Convert.ToInt32(p.Week.Split("/")[0]),
			//	Year = Convert.ToInt32(p.Week.Split("/")[1]),
			//})).ToList();
			//var cumuls = new List<WeekValueModel>();
			//foreach(var r in ranges)
			//{
			//	var cumulInRange = cumuls.Where(c => c.Week == $"{r.Week}/{r.Year}").ToList();
			//	if(cumulInRange == null || cumulInRange.Count == 0)
			//	{
			//		var posInRange = pos.Where(p => p.Week == r.Week && p.Year == r.Year).ToList();
			//		var fasInRange = fas.Where(p => p.Week == r.Week && p.Year == r.Year).ToList();

			//		var sumPos = posInRange.Sum(p => p.Qty ?? 0);
			//		var sumFas = fasInRange.Sum(p => p.Qty ?? 0);
			//		var _value = (qtyInWarehouses + sumPos) - sumFas;

			//		cumuls.Add(new WeekValueModel { Week = $"{r.Week}/{r.Year}", Value = _value });
			//		qtyInWarehouses = _value;
			//	}
			//}
			//return cumuls;
			var poPosition = cumuls.FindIndex(x => x.Week == suggestedPo.Week);
			for(int i = poPosition; i < cumuls.Count; i++)
			{
				cumuls[i].Value += suggestedPo.Value;
			}
			return cumuls;

		}
		public static StockWarningsInternalModel CalculateStockCorrections(List<WeekValueModel> cumuls, int artikelNr, int unit, int? supplierNr, List<(int Week, int Year)> ranges)
		{
			var suggestedPos = new List<WeekValueModel>();
			var suppliers = Infrastructure.Data.Access.Joins.MTM.Order.SupplierAccess.GetSuppliersInfoByArticle(artikelNr);
			var usedSupplier = supplierNr is null ?
				suppliers.FirstOrDefault(s => s.IsDefault)
				: suppliers.FirstOrDefault(s => s.Id == supplierNr);
			var deliveryDays = Convert.ToDouble(usedSupplier.ShippingDays);
			var minimumPoAmount = usedSupplier.MinimumQuantity;
			while(cumuls.Any(c => c.Value < 0))
			{
				var firstNegative = cumuls.FirstOrDefault(c => c.Value < 0) ?? null;
				if(firstNegative != null)
				{

					var FirstNegativeWeek = Convert.ToInt32(firstNegative.Week.Split("/")[0]);
					var FirstNegativeYear = Convert.ToInt32(firstNegative.Week.Split("/")[1]);
					var firstDateOfNegativeWeek = Core.Common.Helpers.DateHelpers.FirstDateOfWeekISO8601(FirstNegativeYear, FirstNegativeWeek);

					var suggestedAmount = GetProperPoAmount(firstNegative.Value, minimumPoAmount);
					var suggestedPo = new WeekValueModel { Week = firstNegative.Week, Value = suggestedAmount };
					suggestedPos.Add(suggestedPo);
					cumuls = RecalculateCumuls(cumuls, ranges, suggestedPo);
				}
			}
			return new StockWarningsInternalModel
			{
				Corrections = cumuls,
				SuggestedOrders = suggestedPos
			};
		}
		public static List<WeekValueModel> OrderListByWeekAndYear(List<WeekValueModel> list)
		{
			return list.OrderBy(x =>
			{
				var parts = x.Week.Split('/');
				var week = int.Parse(parts[0]);
				var year = int.Parse(parts[1]);

				return (year * 100) + week;
			}).ToList();
		}
	}
	public static class Extension
	{
		public static (int week, int year) ParsePeriod(string period)
		{
			var parts = period.Split('/');
			if(parts.Length != 2)
				throw new ArgumentException("Period must be in Week/Year format");

			if(!int.TryParse(parts[0], out int week) ||
				!int.TryParse(parts[1], out int year))
				throw new ArgumentException("Week and Year must be valid numbers");

			return (week, year);
		}
		public static int FindInsertionIndex(this List<WeekValueModel> list, WeekValueModel item)
		{
			// Parse period into comparable components
			(int targetWeek, int targetYear) = ParsePeriod(item.Week);

			for(int i = 0; i < list.Count; i++)
			{
				(int currentWeek, int currentYear) = ParsePeriod(list[i].Week);

				if(targetYear < currentYear ||
					(targetYear == currentYear && targetWeek < currentWeek))
				{
					return i;
				}
			}

			return list.Count;
		}
	}
}
