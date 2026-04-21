using Infrastructure.Data.Access.Tables.FNC;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetProjectBiggestSumOfOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ProjectBiggestSumOfOrderResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetProjectBiggestSumOfOrderHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<List<ProjectBiggestSumOfOrderResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				// get project
				var projectEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				// 2 -- Biggest Sum of Order 

				// get Order

				var orderEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Get();

				// matching between Project and Order 

				// -- get Project ids

				var projectIds = projectEntitiesAll?.Select(x => x.Id).ToList();

				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProjectIds(projectIds);

				// filtered order get only whose ProjectId > 0  -- Ask  m.sani, because i have some project whose projectId = 0 that have orders

				var filteredOrderEnties = orderEntities?.Where(x => x.ProjectId > 0).ToList();

				// warning : take only project whose Id > 0 -- check on the data base side 

				// - Get All Article 
				var articleEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Get();


				// matching article - order

				var orderIds = orderEntities?.Select(x => x.OrderId).ToList();

				var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);

				//Get all Project --> it's useless to call this method again 






				var projectSumOfOrder = filteredOrderEnties?
				.GroupBy(x => x.ProjectId)
				.Select(g => new ProjectBiggestSumOfOrderResponseModel
				{
					ProjectId = g.Key,
					ProjectName = projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).ProjectName,
					ProjectBudget = projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).ProjectBudget,
					FormattedProjectBudget = ((projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).ProjectBudget) / 1000).ToString("#,##0.##") + "K",
					CurrencyName = projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).CurrencyName,
					NumberOfOrder = projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).OrderCount,
					TotalAmount = g.Sum(order => order.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? articleEntities?.Where(article => article.OrderId == order.OrderId).ToList().Select(article => article.TotalCostDefaultCurrency).Sum() ?? 0
					: order.LeasingNbMonths * order.LeasingMonthAmount ?? 0),

					FormattedTotalAmount = (g.Sum(order => order.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase
						? articleEntities?.Where(article => article.OrderId == order.OrderId)?.Sum(article => article.TotalCostDefaultCurrency) ?? 0
						: order.LeasingNbMonths * order.LeasingMonthAmount ?? 0) / 1000).ToString("#,##0.##") + "K",
				}).ToList();


				//var projectSumOfOrder = new List<ProjectBiggestSumOfOrderModel>();
				//foreach(var item in filteredOrderEnties)
				//{
				//	try
				//	{
				//		var key = item.ProjectId;
				//		var TotalAmount = (item.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
				//			? articleEntities.Where(article => article.OrderId == item.OrderId).ToList().Select(article => article.TotalCostDefaultCurrency).Sum()??0
				//			: item.LeasingNbMonths * item.LeasingMonthAmount ?? 0;
				//		projectSumOfOrder.Add(new ProjectBiggestSumOfOrderModel { ProjectId = key, TotalAmount = TotalAmount });

				//	} catch(Exception e)
				//	{
				//		Debug.WriteLine(e, $"erreur ici{item.Id}");
				//		throw e;
				//	}
				//}


				//ordered ProjectSumOfOrder

				var projectBiggestSumOfOrder = projectSumOfOrder?.OrderByDescending(x => x.TotalAmount).Take(5).ToList();


				return ResponseModel<List<ProjectBiggestSumOfOrderResponseModel>>.SuccessResponse(projectBiggestSumOfOrder);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<ProjectBiggestSumOfOrderResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<ProjectBiggestSumOfOrderResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<ProjectBiggestSumOfOrderResponseModel>>.SuccessResponse();
		}
	}
}
