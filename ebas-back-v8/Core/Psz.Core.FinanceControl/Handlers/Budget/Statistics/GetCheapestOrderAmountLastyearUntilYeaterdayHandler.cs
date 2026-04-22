using Infrastructure.Data.Access.Tables.FNC;
using Psz.Core.FinanceControl.Models.Budget.Order.Statistics;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetCheapestOrderAmountLastyearUntilYeaterdayHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CheapestOrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }
		public GetCheapestOrderAmountLastyearUntilYeaterdayHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}

		public ResponseModel<List<CheapestOrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// get All Enumeration

				////var enumOptionsOrderPaymentTypes = Enum.GetValues(typeof(BestellungenExtensionEnums.OrderPaymentTypes)).Cast<BestellungenExtensionEnums.OrderPaymentTypes>().ToList(); // useless 

				////var orderEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetDataFromLastYear();

				//////---  filtered by PoPayementName == Purchase
				////var filteredPurchaseOrder = orderEntitiesAll?.Where(x => x.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase);

				//////calculation amount according by article 

				////// - 1 get All Article
				////var articleEntitiesAll = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(filteredPurchaseOrder.Select(x => x.OrderId).ToList());

				////// 2- calcul amount for each Order 
				////var orderCostItem = orderEntitiesAll?.Select(x => new CheapestOrderModel
				////{
				////	OrderId = x.OrderId,
				////	OrderNum = x.OrderNumber,
				////	CurrencyName = x.CurrencyName,
				////	OrderCost = x.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Purchase ? articleEntitiesAll?.Where(article => article.OrderId == x.OrderId)?.Select(article => article.TotalCostDefaultCurrency ?? 0)?.Sum() ?? 0
				////									 : ((x.LeasingMonthAmount * x.LeasingNbMonths) ?? 0),
				////});

				////var orderedCheapestOrders = orderCostItem?.OrderBy(order => order.OrderCost)?.ToList();
				////var topFiveCheapestOrders = orderedCheapestOrders?.Take(5)?.ToList();
				////return ResponseModel<List<CheapestOrderModel>>.SuccessResponse(topFiveCheapestOrders);


				// --- filtered by PoPayementName = Leasing

				//var filteredLeasingOrder = orderEntitiesAll?.Where(x => x.PoPaymentType == (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);

				//var OrderLeasingCost = orderEntitiesAll.Select(x => new CheapestOrderModel
				//{
				//	OrderId = x.OrderId,

				//	OrderCost = ((x.LeasingMonthAmount * x.LeasingNbMonths) ?? 0),
				//});

				return ResponseModel<List<CheapestOrderModel>>.SuccessResponse(
					Infrastructure.Data.Access.Joins.FNC.Budget.StatisticsAccess.GetTopByAmount(true)
					?.Select(x => new CheapestOrderModel(x.Item1, x.Item2))
					?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<CheapestOrderModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<CheapestOrderModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<CheapestOrderModel>>.SuccessResponse();
		}
	}
}
