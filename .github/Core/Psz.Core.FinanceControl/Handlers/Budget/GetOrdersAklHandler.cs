using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.FinanceControl.Models.Budget;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetOrdersAkl: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetOrdersModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetOrdersAkl(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<GetOrdersModel>> Handle()
		{

			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetOrdersModel>();
				var orders_tableEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Get();


				foreach(var order_tableEntity in orders_tableEntities)
				{
					// responseBody.Add(new Models.Budget.GetOrdersModel(order_tableEntity));
				}

				return ResponseModel<List<Models.Budget.GetOrdersModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

			throw new NotImplementedException();

		}

		public ResponseModel<List<GetOrdersModel>> Validate()
		{
			throw new NotImplementedException();
		}
		//public ResponseModel<List<Models.Budget.GetOrdersModel>> Handle()
		//{
		//    try
		//    {
		//        var validationResponse = this.Validate();
		//        if (!validationResponse.Success)
		//        {
		//            return validationResponse;
		//        }

		//        var responseBody = new List<Models.Budget.GetOrdersModel>();
		//        var orders_tableEntities = Infrastructure.Data.Access.Tables.FNC.Budget_OrderAccess.Get();


		//        foreach (var order_tableEntity in orders_tableEntities)
		//        {
		//            responseBody.Add(new Models.Budget.GetOrdersModel(order_tableEntity));
		//        }

		//        return ResponseModel<List<Models.Budget.GetOrdersModel>>.SuccessResponse(responseBody);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//public ResponseModel<List<Models.Budget.GetOrdersModel>> Validate()
		//{
		//    if (this._user.Access.Purchase.AccessUpdate == true)
		//    {

		//    }
		//    return ResponseModel<List<Models.Budget.GetOrdersModel>>.SuccessResponse();
		//}
	}
}
