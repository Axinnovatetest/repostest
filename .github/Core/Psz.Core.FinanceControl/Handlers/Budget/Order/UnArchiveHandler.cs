using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UnArchiveHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public UnArchiveHandler(Identity.Models.UserModel user, int id)
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
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data, isArchived: true);
				orderEntity.Archived = false;
				orderEntity.ArchiveTime = DateTime.Now;
				orderEntity.ArchiveUserId = this._user.Id;

				// - workflow history
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderEntity, Enums.BudgetEnums.OrderWorkflowActions.Unarchive, this._user, $"");

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

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data, isArchived: true);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse("Archived Order not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
