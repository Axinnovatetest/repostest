using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class ArchiveHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public ArchiveHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<int> Handle()
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
				orderEntity.Archived = true;
				orderEntity.ArchiveTime = DateTime.Now;
				orderEntity.ArchiveUserId = this._user.Id;

				// - workflow history
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderEntity, Enums.BudgetEnums.OrderWorkflowActions.Archive, this._user, $"");

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(orderEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var orderExtEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);
			if(orderExtEntity == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			if(orderExtEntity.Level.HasValue && orderExtEntity.Level.Value > 0 && !orderExtEntity.ApprovalTime.HasValue && !orderExtEntity.ApprovalUserId.HasValue)
				return ResponseModel<int>.FailureResponse("Cannot archive in validation workflow");

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(orderExtEntity.OrderId);
			if(orderEntity != null && orderEntity.erledigt != true)
				return ResponseModel<int>.FailureResponse("Cannot archive non-booked order");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
