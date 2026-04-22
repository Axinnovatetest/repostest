using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetFullWorkflowHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.WorkflowFullModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetFullWorkflowHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Budget.Order.WorkflowFullModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var historyEntities = Infrastructure.Data.Access.Tables.FNC.OrderWorkflowHistoryAccess.GetByOrderId(this._data);

				return ResponseModel<List<Models.Budget.Order.WorkflowFullModel>>.SuccessResponse(
					historyEntities?.Select(x => new Models.Budget.Order.WorkflowFullModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.WorkflowFullModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.WorkflowFullModel>>.AccessDeniedResponse();
			}

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);
			if(orderEntity == null)
				return ResponseModel<List<Models.Budget.Order.WorkflowFullModel>>.FailureResponse("Order not found");

			return ResponseModel<List<Models.Budget.Order.WorkflowFullModel>>.SuccessResponse();
		}
	}
}
