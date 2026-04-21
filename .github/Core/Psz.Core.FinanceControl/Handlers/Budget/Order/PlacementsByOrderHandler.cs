using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class PlacementByOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.PlaceHistoryModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public PlacementByOrderHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Budget.Order.PlaceHistoryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 

				return ResponseModel<List<Models.Budget.Order.PlaceHistoryModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.FNC.OrderPlacementAccess.GetByOrderId(this._data)
					?.Select(x => new Models.Budget.Order.PlaceHistoryModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.PlaceHistoryModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.PlaceHistoryModel>>.AccessDeniedResponse();
			}

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);
			if(orderEntity == null)
				return ResponseModel<List<Models.Budget.Order.PlaceHistoryModel>>.FailureResponse("Order not found");

			return ResponseModel<List<Models.Budget.Order.PlaceHistoryModel>>.SuccessResponse();
		}
	}
}
