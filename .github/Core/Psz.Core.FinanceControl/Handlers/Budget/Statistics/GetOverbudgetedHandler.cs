using Infrastructure.Data.Access.Tables.FNC;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetOverbudgetedHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<OverbudgetedResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetOverbudgetedHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<List<OverbudgetedResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var projectEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				var projectIds = projectEntitiesAll?.Select(x => x.Id).ToList();

				// search correspondence between projects and Orders
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProjectIds(projectIds);

				// - Get All Article 
				var articleEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Get();


				// matching article - order

				var orderIds = orderEntities?.Select(x => x.OrderId).ToList();

				var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);

				//Get all Project --> it's useless to call this method again 

				// 5 --Overbudgeted - Total orders amount bigger than projectBudget

				var Overbudgeted = projectEntitiesAll.Select(x => new OverbudgetedResponseModel
				{
					ProjectId = x.Id,
					ProjectName = x.ProjectName,
					ProjectBudget = x.ProjectBudget,
					FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("#,##0.##") + "K",
					CurrencyName = x.CurrencyName,
					NumberOfProject = x.OrderCount,
					SumOfOrder = (orderEntities.Where(order => order.ProjectId == x.Id).ToList()
					.Select(x => x.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase ? articleEntitiesAll?.Where(article => article.OrderId == x.OrderId)?
					.Select(article => article.TotalCostDefaultCurrency ?? 0)?
					.Sum() ?? 0 : ((x.LeasingMonthAmount * x.LeasingNbMonths) ?? 0)).Sum()),
					FormattedSumOfOrder = (orderEntities.Where(order => order.ProjectId == x.Id).ToList()
					.Select(x => x.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase ? articleEntitiesAll?.Where(article => article.OrderId == x.OrderId)?
					.Select(article => article.TotalCostDefaultCurrency ?? 0)?
					.Sum() ?? 0 : ((x.LeasingMonthAmount * x.LeasingNbMonths) ?? 0))
															.Sum() / 1000).ToString("#,##0.##") + "K"




					//FormattedSumOfOrder =  orderEntities.Where(order => order.ProjectId == x.Id).ToList().Select(x => x.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase ? articleEntitiesAll?.Where(article => article.OrderId == x.OrderId)?.Select(article => article.TotalCostDefaultCurrency ?? 0)?.Sum() ?? 0
					//				 : ((x.LeasingMonthAmount * x.LeasingNbMonths) ?? 0))
					//						.Sum()
				}).ToList();


				// -- filtered Overbudgeted and take 5
				var filteredOverbudgeted = Overbudgeted.Where(x => x.ProjectBudget < x.SumOfOrder).Take(5).ToList();

				return ResponseModel<List<OverbudgetedResponseModel>>.SuccessResponse(filteredOverbudgeted);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<OverbudgetedResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<OverbudgetedResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<OverbudgetedResponseModel>>.SuccessResponse();
		}
	}
}
