using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetWorkflowHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Order.WorkflowModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetWorkflowHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Budget.Order.WorkflowModel> Handle()
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

				var issuerEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderEntity.IssuerId);
				var currEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);

				var lastValidationEntity = Infrastructure.Data.Access.Tables.FNC.OrderValidationAccess.GetLastByOrderId(orderEntity.OrderId);
				var lastPlacementEntity = Infrastructure.Data.Access.Tables.FNC.OrderPlacementAccess.GetLastByOrderId(orderEntity.OrderId);

				var startBooking = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetFirstReceptionByOrderId(orderEntity.OrderId);
				var endBooking = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetLastReceptionByOrderId(orderEntity.OrderId);

				return ResponseModel<Models.Budget.Order.WorkflowModel>.SuccessResponse(
					new Models.Budget.Order.WorkflowModel(orderEntity, issuerEntity, currEntity, lastValidationEntity, lastPlacementEntity, startBooking, endBooking));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Order.WorkflowModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Order.WorkflowModel>.AccessDeniedResponse();
			}

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);
			if(orderEntity == null)
				return ResponseModel<Models.Budget.Order.WorkflowModel>.FailureResponse("Order not found");

			return ResponseModel<Models.Budget.Order.WorkflowModel>.SuccessResponse();
		}
	}
}
