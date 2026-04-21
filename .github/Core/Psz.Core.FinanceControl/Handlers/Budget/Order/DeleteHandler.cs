using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int id)
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
				Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.DeleteByOrderId(this._data);
				Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.DeleteByOrderId(this._data);

				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);
				orderEntity.Deleted = true;
				orderEntity.DeleteTime = DateTime.Now;
				orderEntity.DeleteUserId = this._user.Id;

				// - workflow history
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderEntity, Enums.BudgetEnums.OrderWorkflowActions.Delete, this._user, $"");

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

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			if(orderEntity.ApprovalTime != null && orderEntity.ApprovalUserId != null)
				return ResponseModel<int>.FailureResponse("Cannot delete already validated");

			if(orderEntity.Level.HasValue && orderEntity.Level.Value > 0 || orderEntity.Status.HasValue && orderEntity.Status.Value > 0)
				return ResponseModel<int>.FailureResponse("Cannot delete in validation workflow");

			//var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(orderEntity.OrderId);
			//if (articleEntites != null && articleEntites.Count > 0)
			//    return ResponseModel<int>.FailureResponse("Cannot delete Order with Articles");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
