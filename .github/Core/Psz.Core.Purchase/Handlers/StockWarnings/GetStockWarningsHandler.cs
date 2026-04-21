using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;
using System.Globalization;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<StockWarningsResponseModel> GetStockWarnings(UserModel user, StockWarningsRequesteModel data)
		{
			if(user == null)
				return ResponseModel<StockWarningsResponseModel>.AccessDeniedResponse();
			try
			{
				var range = new List<(int Week, int Year)>();
				for(DateTime date = DateTime.Now; date <= DateTime.Now.AddMonths(6); date = date.AddDays(1))
				{
					int weekNumber = ISOWeek.GetWeekOfYear(date);
					int year = date.Month == 12 && date.Year == DateTime.Now.Year && weekNumber == 1
								? date.Year + 1
								: date.Year;
					if(!range.Contains((weekNumber, year)))
						range.Add((weekNumber, year));
				}

				var warehouses = Enums.StockWarningEnums.GetWarehousesFromUnit((Enums.StockWarningEnums.StockWarningsUnits)data.UnitId);
				var _needs = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetFas(((Enums.StockWarningEnums.StockWarningsUnits)data.UnitId).GetDescription(), data.ArtikelNr);
				var _orders = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetPos(((Enums.StockWarningEnums.StockWarningsUnits)data.UnitId).GetDescription(), data.ArtikelNr);
				var cumuls = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetCumuls(((Enums.StockWarningEnums.StockWarningsUnits)data.UnitId).GetDescription(), data.ArtikelNr);


				var currentStock = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetBedarf(data.UnitId, data.ArtikelNr);
				var minimumStock = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetMinimumStock(warehouses, data.ArtikelNr);
				var backlogCumulNeeds = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetBacklogNeeds(((Enums.StockWarningEnums.StockWarningsUnits)data.UnitId).GetDescription(), data.ArtikelNr);
				var backlogCumulOrders = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetBacklogOrders(warehouses, data.ArtikelNr);

				var quantities = cumuls?.Select(q => new WeekValueModel
				{
					Value = q.SumProd_cumul ?? 0m,
					Week = $"{q.Week}/{q.Year}"
				}).ToList();
				if(quantities.Any(x => x.Week == $"{ISOWeek.GetWeekOfYear(DateTime.Now)}/{DateTime.Now.Year}"))
					quantities.FirstOrDefault(x => x.Week == $"{ISOWeek.GetWeekOfYear(DateTime.Now)}/{DateTime.Now.Year}").Value -= minimumStock;
				else
					quantities.Add(new WeekValueModel { Week = $"{ISOWeek.GetWeekOfYear(DateTime.Now)}/{DateTime.Now.Year}", Value = -minimumStock });

				var finalCumuls = Helpers.StockWarningsHelper.ProcessLists(quantities, range, currentStock, backlogCumulOrders, backlogCumulNeeds);

				var backlogWeek = finalCumuls[0].Week.Split("/")[0] == "1"
					? $"{ISOWeek.GetWeekOfYear(new DateTime(DateTime.Now.Year - 1, 12, 31))}/{Convert.ToInt32(finalCumuls[0].Week.Split("/")[1]) - 1}"
					: $"{Convert.ToInt32(finalCumuls[0].Week.Split("/")[0]) - 1}/{finalCumuls[0].Week.Split("/")[1]}";


				var copy = finalCumuls.ConvertAll(x => (WeekValueModel)x.Clone());
				var corrections = Helpers.StockWarningsHelper.CalculateStockCorrections(copy, data.ArtikelNr, data.UnitId, data.SupplierNr, range);
				finalCumuls.ForEach(c =>
				{
					var _v = corrections.Corrections.Where(x => x.Week == c.Week).ToList();
					if(_v == null || _v.Count == 0)
					{
						corrections.Corrections.Add(new WeekValueModel { Week = c.Week, Value = c.Value });
					}
				});
				finalCumuls.Insert(0, new WeekValueModel { Week = backlogWeek, Value = -backlogCumulNeeds });
				corrections.Corrections.Insert(0, new WeekValueModel { Week = backlogWeek, Value = -backlogCumulNeeds });

				var response = new StockWarningsResponseModel
				{
					Quantities = finalCumuls,
					Needs = _needs.Select(x => new WeekValueModel { Week = $"{x.Week}/{x.Year}", Value = x.Qty ?? 0 }).ToList(),
					Orders = _orders.Select(x => new WeekValueModel { Week = $"{x.Week}/{x.Year}", Value = x.Qty ?? 0 }).ToList(),
					Corrections = Helpers.StockWarningsHelper.OrderListByWeekAndYear(corrections.Corrections),
					SuggestedOrdes = corrections.SuggestedOrders,
					CurrentWeek = $"{ISOWeek.GetWeekOfYear(DateTime.Now)}/{DateTime.Now.Year}",
					BacklogNeeds = new List<WeekValueModel> { new WeekValueModel { Week = backlogWeek, Value = backlogCumulNeeds } },
					BacklogOrders = new List<WeekValueModel> { new WeekValueModel { Week = backlogWeek, Value = backlogCumulOrders } },
					CurrentStock = new List<WeekValueModel> { new WeekValueModel { Week = $"{ISOWeek.GetWeekOfYear(DateTime.Now)}/{DateTime.Now.Year}", Value = currentStock } },
					MinimumStock = new List<WeekValueModel> { new WeekValueModel { Week = $"{ISOWeek.GetWeekOfYear(DateTime.Now)}/{DateTime.Now.Year}", Value = minimumStock } },
				};

				return ResponseModel<StockWarningsResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}