using Geocoding.Microsoft.Json;
using Infrastructure.Data.Access.Joins.FNC.Budget;
using Infrastructure.Data.Access.Tables.FNC;
using Infrastructure.Data.Entities.Tables.FNC;
using Infrastructure.Services.Reporting.Models.MTM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MoreLinq.Extensions;
using Psz.Core.FinanceControl.Handlers.Budget.Order;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


#region >> one principe to be  respected

//least acces to the bdatabase if possible  

#endregion

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{

	#region
	//public class GetOrderAmountHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>>
	//{

	//	Psz.Core.Identity.Models.UserModel _user { get; set; }


	//	public GetOrderAmountHandler(UserModel user)
	//	{
	//		_user = user;
	//	}

	//	public ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>> Handle()
	//	{
	//		try
	//		{
	//			var validationResponse = this.Validate();

	//			if(!validationResponse.Success)
	//			{
	//				return validationResponse;
	//			}

	//			// retrieve => check + Access to DataBase 
	//			var projectEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

	//			//--
	//			var statisticAmount = new List<StatisticAmounts>();

	//			//--check

	//				//List of Order According to Project
	//				var orderEntitiesAll = (Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess
	//				.GetByProjectIds(projectEntities.Select(x => x.Id).ToList()))
	//				.Where(order => order.ApprovalTime != null && order.ApprovalUserId != null);

	//				// List od Article according to order 
	//				var articleEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntitiesAll.Select(x => x.OrderId).ToList());
	//			//

	#region >>> Comment Section 
	//			//foreach(var projectEntity in projectEntities) // --optimize this loop used c# LINQ lambda expression (filter by projects that have got only commands )
	//			//{										      // use forEach method on yours project-List??


	//			//	//filter order
	//			//		var orderEntities = orderEntitiesAll.FindAll(x => (int)x.ProjectId == projectEntity.Id); // don't apply ToList() method if findAll 




	//			//		int countLeasing = 0;
	//			//		int countPurchase = 0;
	//			//		decimal leasingOnlyAmount = 0;
	//			//	    decimal PurchaseOnlyAmount = 0;

	//			//	//if(orderEntities is null || orderEntities.Count > 0)
	//			//	//{
	//			//	//	decimal totalAmountArticle = 0;


	//			//	//	//We have different order type purchase and leasing order, so checked 
	//			//	//	foreach(var orderEntity in orderEntities)
	//			//	//	{


	//			//	//		//We have different order type purchase and leasing order, so cheched 
	//			//	//		 //I don't know if it is worth it to checked wether validate order or no ?? review this after
	//			//	//		if(orderEntity.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
	//			//	//		{
	//			//	//			 countPurchase += 1;
	//			//	//			 //get all Article related to current command 
	//			//	//			 var articleEntities = articleEntitiesAll.FindAll(x => x.OrderId == orderEntity.OrderId);

	//			//	//				//=> Check
	//			//	//				if(articleEntities?.Count > 0 || articleEntities != null)
	//			//	//				{
	//			//	//				totalAmountArticle += articleEntities.Select(x => x.TotalCost).Sum() ?? 0;
	//			//	//				}

	//			//	//			PurchaseOnlyAmount += articleEntities.Select(x => x.TotalCost).Sum() ?? 0;

	//			//	//		}
	//			//	//		else // leasing
	//			//	//		{

	//			//	//			totalAmountArticle += (orderEntity.LeasingTotalAmount ?? 0) ;
	//			//	//			 countLeasing += 1;
	//			//	//			leasingOnlyAmount += (orderEntity.LeasingTotalAmount ?? 0);
	//			//	//		}



	//			//	//	}

	//			//	//	statisticAmount.Add(new StatisticAmounts(projectEntity, projectEntity.ProjectBudget, totalAmountArticle, orderEntities.Count, countLeasing, countPurchase, PurchaseOnlyAmount, leasingOnlyAmount));


	//			//	//}

	//			//}
	#endregion

	//			#region >> refactoring code 


	//			// I will apply group by fonction to perform this logic

	//			var orderGroupList = orderEntitiesAll.GroupBy(x => x.ProjectId).ToList();
	//			var responseModel = (orderGroupList.Select(group =>
	//			{
	//				// Recherche le projet correspondant à l'ProjectId du groupe
	//				var project = projectEntities?.FirstOrDefault(p => p.Id == group.Key);

	//				// Si aucun projet correspondant n'est trouvé, retourne null
	//				if(project is null)
	//					return null;

	//				// Calcule le montant total des commandes pour ce projet
	//				decimal totalOrderAmount = group.Sum(order => order.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase ?
	//				(articleEntitiesAll?.Where(article => article.OrderId == order.OrderId).ToList().Select(art => art.TotalCostDefaultCurrency).Sum() ?? 0)
	//				: (order.LeasingNbMonths * order.LeasingMonthAmount ?? 0));

	//				return new AmountOfOrderAssociatedWithProjectResponseModel(
	//							projectEntity: project,               // another way to pass an argument has a constructor; check well, because it's an constructor 
	//							orderAmount: totalOrderAmount);

	//			}).Where(model => model != null).ToList());

	//			responseModel = responseModel   // FIX : I will perform refactoring afterward			
	//			.OrderByDescending(model => model.AmountOfOrderAssociated)
	//		    .Take(5) 
	//		    .ToList();


	//			return ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>.SuccessResponse(responseModel);



	//			#endregion




	//		} catch(Exception ex)
	//		{
	//			Infrastructure.Services.Logging.Logger.Log(ex);

	//			throw;
	//		}
	//	}

	//	public ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>> Validate()
	//	{
	//		if(this._user == null)
	//		{
	//			ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>.AccessDeniedResponse();
	//		}

	//		return ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>.SuccessResponse();
	//	}
	//}
	#endregion



	#region --- New version with fixing problem

	public class GetOrderAmountHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>>
	{

		Psz.Core.Identity.Models.UserModel _user { get; set; }
		int _data { get; set; }

		public GetOrderAmountHandler(UserModel user, int year)
		{
			_user = user;
			_data = year;

		}

		public ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();

				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// Get All Project whose Order already validated, no deleted and no Archived
				var ProjectWhoseOrderAlreadyValidated = StatisticsAccess.GetAllProjectWhoseOrderAlreadyValidated(this._data);

				//  -- List Project and Expense
				List<AmountOfOrderAssociatedWithProjectResponseModel> projectWithExpenseResponseModel = new List<AmountOfOrderAssociatedWithProjectResponseModel>();

				var projIds = ProjectWhoseOrderAlreadyValidated.Select(x => x.Id).ToList().Distinct();

				foreach(var proj in projIds)
				{
					var orderListEntity = ProjectWhoseOrderAlreadyValidated.Where(x => x.Id == proj).ToList();



					#region

					// that mean this order has already validated
					//var orderListEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(proj.Id);

					//decimal? TotalOrderAmountBeforeDiscount = orderListEntity.Sum(order => order.PoPaymentType ==(int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase ?
					//				Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(order.OrderId), false) :
					//				order.LeasingTotalAmount ?? 0);


					//var TotalAmount = orderListEntity?.Select(order =>
					//	order.Discount.HasValue && order.Discount.Value > 0
					//		? (1 - order.Discount.Value / 100) *
					//		  (order.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					//			? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(order.OrderId), false)
					//			: order.LeasingTotalAmount ?? 0)
					//		: (order.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					//			? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(order.OrderId), false)
					//			: order.LeasingTotalAmount ?? 0)).ToList().Sum();

					//decimal? TotalAmountDefaultCurrencyBeforeDiscount = orderListEntity
					//	.Sum(order => order.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					//	? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(order.OrderId), false, true)
					//	: order.LeasingTotalAmount ?? 0);
					#endregion



					decimal? TotalAmountDefaultCurrency = orderListEntity.Sum(order =>
						order.Discount.HasValue && order.Discount.Value > 0
							? (1 - order.Discount.Value / 100) *
							  (order.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
								? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(order.OrderId), false, true, order.Discount.Value)
								: order.LeasingTotalAmount ?? 0)
							: (order.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
								? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(order.OrderId), false, true)
								: order.LeasingTotalAmount ?? 0));

					var item = new OrderValidatedByProjectEntity()
					{
						Id = proj,
						OrderCount = orderListEntity.FirstOrDefault().OrderCount,
						ProjectBudget = orderListEntity.FirstOrDefault().ProjectBudget,
						ProjectName = orderListEntity.FirstOrDefault().ProjectName,
						CurrencyName = orderListEntity.FirstOrDefault().CurrencyName,

					};

					projectWithExpenseResponseModel.Add(new AmountOfOrderAssociatedWithProjectResponseModel(projectEntity: item, orderAmount: TotalAmountDefaultCurrency));

				}

				// Get Top Five 
				return ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>.SuccessResponse(projectWithExpenseResponseModel.OrderByDescending(n => n.AmountOfOrderAssociated).Take(5).ToList());


			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);

				throw;
			}
		}

		public ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>.SuccessResponse();
		}
	}

	#endregion

}
