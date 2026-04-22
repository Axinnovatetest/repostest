using Geocoding.Microsoft.Json;
using Infrastructure.Data.Access.Tables.FNC;
using Org.BouncyCastle.Ocsp;
using Psz.Core.FinanceControl.Models.Budget.Order.Statistics;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetDataFromLastYearHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.Statistics.CheapestOrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }
		public GetDataFromLastYearHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<List<Models.Budget.Order.Statistics.CheapestOrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//    //-TODO --Friday first hour.
				//// -- 08-02-2024 REM : Review Leasing Calcul ==> separate Purchase calcul and leasing Calcul 

				////-- 09-20-2024  : Done .
				//	var orderEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetDataFromLastYear();

				//	var articlesEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntitiesAll?.Select(x => x.OrderId)?.ToList());

				//	var orderCostAll = orderEntitiesAll?.Select(x => new CheapestOrderModel
				//	{
				//		OrderId = x.OrderId,

				//		OrderNum = x.OrderNumber,

				//		CurrencyName= x.CurrencyName,

				//		OrderCost = x.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase ?
				//		          articlesEntities?.Where(article  => x.OrderId == article.OrderId)?.Select(article => article.TotalCostDefaultCurrency).Sum() ?? 0
				//				   : ((x.LeasingMonthAmount * x.LeasingNbMonths) ?? 0),


				//	});

				//	// tri Order 
				//	var orderedHighestOrders = orderCostAll?.OrderByDescending(x => x.OrderCost)?.ToList();


				//	// retrieved Top 5
				//	var topFiveHighestOrder = orderedHighestOrders?.Take(5)?.Select(x => new CheapestOrderModel
				//	{
				//		OrderId = x.OrderId,
				//		OrderNum = x.OrderNum,
				//		CurrencyName = x.CurrencyName,
				//		OrderCost = x.OrderCost,
				//		OrderCost2 = $"{x.OrderCost} {x.CurrencyName}"
				//	}).ToList();

				//	// we can combinate that by doing
				//	// -- var orderedHighestOrders = orderCostAll?.OrderByDescending(x => x.OrderCost).Take(5)?.ToList();





				//	return ResponseModel <List<Models.Budget.Order.Statistics.CheapestOrderModel>>.SuccessResponse(topFiveHighestOrder);

				return ResponseModel<List<CheapestOrderModel>>.SuccessResponse(
					Infrastructure.Data.Access.Joins.FNC.Budget.StatisticsAccess.GetTopByAmount(false)
					?.Select(x => new CheapestOrderModel(x.Item1, x.Item2))
					?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<Models.Budget.Order.Statistics.CheapestOrderModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.Statistics.CheapestOrderModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Order.Statistics.CheapestOrderModel>>.SuccessResponse();
		}
	}
}
