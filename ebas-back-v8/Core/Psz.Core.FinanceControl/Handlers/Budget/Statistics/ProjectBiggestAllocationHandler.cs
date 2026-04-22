using Infrastructure.Data.Access.Tables.FNC;
using Infrastructure.Data.Entities.Tables.FNC;
using Microsoft.CodeAnalysis.VisualBasic;
using Psz.Core.FinanceControl.Enums;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Psz.Core.FinanceControl.Enums.BudgetEnums;
using static Psz.Core.FinanceControl.Helpers.Processings.Budget;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{

	//13-02-2024
	public class ProjectBiggestAllocationHandler: IHandle<Identity.Models.UserModel, ResponseModel<ProjectOverViewModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }
		public ProjectBiggestAllocationHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<ProjectOverViewModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// 1 --Biggest Allocation

				// get project
				var projectEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				//filtered project by Biggest Allocation

				var filteredBiggestAllocationProject = projectEntitiesAll?.OrderByDescending(x => x.ProjectBudget);

				// take five top biggest budget 
				var topFiveBiggestAllocation = filteredBiggestAllocationProject?.Take(5).ToList(); //--

				var topFiveBiggestAllocation_1 = topFiveBiggestAllocation.Select(x => new BiggestAllocationModel
				{
					ProjectId = x.Id,

					ProjectName = x.ProjectName,

					BudgetAllocatedForProject = x.ProjectBudget,

					FormattedBudgetAllocatedForProject = (x.ProjectBudget / 1000).ToString("0. ##") + "K",

					CurrencyName = x.CurrencyName,

				}).ToList();

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
				.Select(g => new ProjectBiggestSumOfOrderModel
				{
					ProjectId = g.Key,
					ProjectName = projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).ProjectName,
					ProjectBudget = projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).ProjectBudget,
					FormattedProjectBudget = ((projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).ProjectBudget) / 1000).ToString("0.##") + "K",
					CurrencyName = projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).CurrencyName,
					NumberOfOrder = projectEntitiesAll.FirstOrDefault(p => p.Id == g.Key).OrderCount,
					TotalAmount = g.Sum(order => order.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? articleEntities?.Where(article => article.OrderId == order.OrderId).ToList().Select(article => article.TotalCostDefaultCurrency).Sum() ?? 0
					: order.LeasingNbMonths * order.LeasingMonthAmount ?? 0)
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

				var projectBiggestSumOfOrder = projectSumOfOrder?.OrderByDescending(x => x.TotalAmount).Take(5).ToList();  //--



				// 3 --Oldest - oldest first approval time for currently open projects


				//filtered - choose only open project

				var openProject = projectEntitiesAll.Where(x => x.Closed == false).ToList();

				var oldestFirstApproval = openProject.OrderBy(project => project.ApprovalTime).ToList();

				var oldestFirstApprovalTimeSpan = oldestFirstApproval?.Where(a => a.ApprovalTime is not null).Select(x => new OldestFirstApprovalModel
				{
					ProjectId = x.Id,
					ProjectName = x.ProjectName,
					Date = (((TimeSpan)(DateTime.Now - x.ApprovalTime))),
					Date_Time = $"{(int)(((TimeSpan)(DateTime.Now - x.ApprovalTime))).TotalDays} jours {(int)(((TimeSpan)(DateTime.Now - x.ApprovalTime))).Hours} heures {(((TimeSpan)(DateTime.Now - x.ApprovalTime))).Minutes} minutes",
				}).ToList();

				//(((TimeSpan)(DateTime.Now - x.ApprovalTime))).ToString("dd\\:hh\\:mm"),

				// Take Five Top 

				var topFiveOldestFirstApprovalTimeSpan = oldestFirstApprovalTimeSpan.Take(5).ToList();


				// 4 --Best external - biggest profit = PSZOffer - ProjectBudget

				var biggestProfitEntities = projectEntitiesAll.Where(x => x.Id_Type == (int)BudgetEnums.ProjectTypes.External && x.PSZOffer is not null).ToList();


				var biggestProfit = biggestProfitEntities?
					.Select(x => new BiggestOfferModel
					{
						ProjectId = x.Id,
						ProjectName = x.ProjectName,
						PSZOffer = x.PSZOffer,
						ProjectBudget = x.ProjectBudget,
						FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("0.##") + "K",
						CurrencyName = x.CurrencyName,
						Profit = ((decimal)(x.PSZOffer - x.ProjectBudget)),
						FormattedProfit = (((decimal)(x.PSZOffer - x.ProjectBudget)) / 1000).ToString("0.##") + "K",

					}).ToList();

				//take five top

				var topFiveBiggestProfit = biggestProfit?.Take(5).ToList();


				// 5 --Overbudgeted - Total orders amount bigger than projectBudget

				var Overbudgeted = projectEntitiesAll.Select(x => new OverbudgetedModel
				{
					ProjectId = x.Id,
					ProjectName = x.ProjectName,
					ProjectBudget = x.ProjectBudget,
					FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("0.##") + "K",
					CurrencyName = x.CurrencyName,
					NumberOfProject = x.OrderCount,
					SumOfOrder = orderEntities.Where(order => order.ProjectId == x.Id).ToList().Select(x => x.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase ? articleEntitiesAll?.Where(article => article.OrderId == x.OrderId)?.Select(article => article.TotalCostDefaultCurrency ?? 0)?.Sum() ?? 0
													 : ((x.LeasingMonthAmount * x.LeasingNbMonths) ?? 0))
															.Sum()

				}).ToList();


				// -- filtered Overbudgeted and take 5
				var filteredOverbudgeted = Overbudgeted.Where(x => x.ProjectBudget < x.SumOfOrder).Take(5).ToList();


				// 6 --BudgetLeak: closed prjects with remaining budget >> take 5

				var closedProjectWithRemainingBudget = projectEntitiesAll.Where(x => x.Closed == true && x.ProjectBudget > 0 && x.ProjectStatusName.ToLower() == "closed").Take(5).ToList();

				var BudgetLeak = closedProjectWithRemainingBudget.Select(x => new BudgetLeakModel
				{

					ProjectId = x.Id,
					ProjectName = x.ProjectName,
					Status = x.ProjectStatusName,
					ProjectBudget = x.ProjectBudget,
					FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("0.##") + "K",
					CurrencyName = x.CurrencyName,

				}).ToList();



				//7- Best Custommers Biggest Profils in External Project

				var bestCustommer = biggestProfitEntities?.Where(x => x.CustomerId > 0).ToList();



				var Customer = bestCustommer.Select(x => new BiggestOfferModel
				{
					ProjectId = x.Id,

					ProjectName = x.ProjectName,

					CustomerName = x.CustomerName,

					CustomerNr = x.CustomerNr,
					PSZOffer = x.PSZOffer,
					FormattedPSZOffer = ((decimal)x.PSZOffer / 1000).ToString("0.##") + "K",
					ProjectBudget = x.ProjectBudget,
					FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("0.##") + "K",
					CurrencyName = x.CurrencyName,
					Profit = ((decimal)(x.PSZOffer - x.ProjectBudget)),
					FormattedProfit = (((decimal)(x.PSZOffer - x.ProjectBudget)) / 1000).ToString("0.##") + "K",


				}).ToList();

				//filtered 

				var filteredBestCustomer = Customer.OrderByDescending(x => x.Profit).Take(5).ToList();



				// 8- worst custommer :Smallest Profil in external Order

				var filteredWorstCustomer = Customer.OrderBy(x => x.Profit).Take(5).ToList();


				//9 - Biggest customers: Highest PSZOffer in external projects

				var highestPSZOffer = biggestProfitEntities.OrderByDescending(x => x.PSZOffer).Take(5).ToList();

				var transformedHighestPSZOffer = highestPSZOffer.Select(x => new BiggestOfferModel
				{
					ProjectId = x.Id,
					ProjectName = x.ProjectName,
					CustomerName = x.CustomerName,
					CustomerNr = x.CustomerNr,
					PSZOffer = x.PSZOffer,
					FormattedPSZOffer = ((decimal)x.PSZOffer / 1000).ToString("0.##") + "K",
					ProjectBudget = x.ProjectBudget,
					FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("0.##") + "K",
					CurrencyName = x.CurrencyName,


				}).ToList();


				//10 - Smallest customers: lowest PSZOffer in external projects

				var lowestPSZOffer = biggestProfitEntities.OrderBy(x => x.PSZOffer).Take(5).ToList();

				//--transformed data

				var tansformedLowestPSZOffer = lowestPSZOffer.Select(x => new BiggestOfferModel
				{
					ProjectId = x.Id,

					ProjectName = x.ProjectName,

					CustomerName = x.CustomerName,

					CustomerNr = x.CustomerNr,

					PSZOffer = x.PSZOffer,

					FormattedPSZOffer = ((decimal)x.PSZOffer / 1000).ToString("0.##") + "K",

					ProjectBudget = x.ProjectBudget,

					FormattedProjectBudget = (x.ProjectBudget / 1000).ToString("0.##") + "K",
					CurrencyName = x.CurrencyName,


				}).ToList();



				var response = new ProjectOverViewModel
				{
					BiggestAllocation = topFiveBiggestAllocation_1,

					ProjectBiggestWithSumOfOrder = projectBiggestSumOfOrder,

					OldestFirstApproval = topFiveOldestFirstApprovalTimeSpan,

					BiggestOffer = topFiveBiggestProfit,

					Overbudgeted = filteredOverbudgeted,

					ClosedProjectWithRemainingBudget = BudgetLeak,

					BestCustommer = filteredBestCustomer,

					WorstCustommer = filteredWorstCustomer,

					BiggestCustomer = transformedHighestPSZOffer,

					SmallestCustomer = tansformedLowestPSZOffer
				};


				//	return ResponseModel<ProjectOverViewModel>.SuccessResponse(response);

				return null;

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<ProjectOverViewModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<ProjectOverViewModel>.AccessDeniedResponse();
			}

			return ResponseModel<ProjectOverViewModel>.SuccessResponse();
		}
	}
}
