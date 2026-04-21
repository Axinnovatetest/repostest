using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetOrderExtensionByIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Order.OrderExtensionModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetOrderExtensionByIdHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Budget.Order.OrderExtensionModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);

				return ResponseModel<Models.Budget.Order.OrderExtensionModel>.SuccessResponse(new Models.Budget.Order.OrderExtensionModel(orderEntity));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Order.OrderExtensionModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Order.OrderExtensionModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data, Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Types.All) == null)
				return ResponseModel<Models.Budget.Order.OrderExtensionModel>.FailureResponse("Order not found");

			return ResponseModel<Models.Budget.Order.OrderExtensionModel>.SuccessResponse();
		}
	}
}
