using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetLastVersionOrdersAklHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.ArtikelOrderParamsModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetLastVersionOrdersAklHandler(Identity.Models.UserModel user, int Id_Order)
		{
			this._user = user;
			this._data = Id_Order;
		}
		public ResponseModel<List<Models.Budget.ArtikelOrderParamsModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.ArtikelOrderParamsModel>();
				//var orders_tableEntities = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.GetLastVersionOrder(this._data);


				//foreach (var order_tableEntity in orders_tableEntities)
				//{
				//    responseBody.Add(new Models.Budget.ArtikelOrderParamsModel(order_tableEntity));
				//}

				return ResponseModel<List<Models.Budget.ArtikelOrderParamsModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.ArtikelOrderParamsModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.ArtikelOrderParamsModel>>.SuccessResponse();
		}
	}
}
