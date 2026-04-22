using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class NeededQuantityOrderedQuantityAnalysisSummaryHandler: IHandle<NeededQuantityOrderedQuantityAnalysisRequestModel, ResponseModel<List<NeedStockSummaryItemModel>>>
	{
		private NeededQuantityOrderedQuantityAnalysisRequestModel data { get; set; }
		private UserModel user { get; set; }

		public NeededQuantityOrderedQuantityAnalysisSummaryHandler(UserModel user, NeededQuantityOrderedQuantityAnalysisRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<NeedStockSummaryItemModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}
				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<List<NeedStockSummaryItemModel>> Perform(UserModel user, NeededQuantityOrderedQuantityAnalysisRequestModel data)
		{
			DateTime date;
			var lagerIds = Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(null, null)?.ToList();

			var responseBody = new List<NeedStockSummaryItemModel>();
			//// - 1 KW
			//date = DateTime.Today.AddDays(1 * 7);
			//responseBody.Add(new NeedStockSummaryItemModel(date, date.Year, CRP.Helpers.DateTimeHelper.GetIso8601WeekOfYear(date), Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysisSummary(date, lagerIds)));

			//// - 2 KW
			//date = DateTime.Today.AddDays(2 * 7);
			//responseBody.Add(new NeedStockSummaryItemModel(date, date.Year, CRP.Helpers.DateTimeHelper.GetIso8601WeekOfYear(date), Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysisSummary(date, lagerIds)));

			//// - 3 KW
			//date = DateTime.Today.AddDays(3 * 7);
			//responseBody.Add(new NeedStockSummaryItemModel(date, date.Year, CRP.Helpers.DateTimeHelper.GetIso8601WeekOfYear(date), Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysisSummary(date, lagerIds)));

			//// - 4 KW
			//date = DateTime.Today.AddDays(4 * 7);
			//responseBody.Add(new NeedStockSummaryItemModel(date, date.Year, CRP.Helpers.DateTimeHelper.GetIso8601WeekOfYear(date), Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysisSummary(date, lagerIds)));

			//// - 5 KW
			//date = DateTime.Today.AddDays(5 * 7);
			//responseBody.Add(new NeedStockSummaryItemModel(date, date.Year, CRP.Helpers.DateTimeHelper.GetIso8601WeekOfYear(date), Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysisSummary(date, lagerIds)));

			//// - 6 KW
			//date = DateTime.Today.AddDays(6 * 7);
			//responseBody.Add(new NeedStockSummaryItemModel(date, date.Year, CRP.Helpers.DateTimeHelper.GetIso8601WeekOfYear(date), Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysisSummary(date, lagerIds)));

			// -
			var dates = new List<DateTime>();
			for(int i = 1; i <= 6; i++)
			{
				dates.Add(DateTime.Today.AddDays(7 * i));
			}
			responseBody.AddRange(Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysisSummary(dates, lagerIds)
				?.Select(x => new NeedStockSummaryItemModel(x))?.ToList());

			return ResponseModel<List<NeedStockSummaryItemModel>>.SuccessResponse(responseBody);
		}
		public ResponseModel<List<NeedStockSummaryItemModel>> Validate()
		{
			if(this.user is null)
			{
				return ResponseModel<List<NeedStockSummaryItemModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<NeedStockSummaryItemModel>>.SuccessResponse();
		}
	}
}
