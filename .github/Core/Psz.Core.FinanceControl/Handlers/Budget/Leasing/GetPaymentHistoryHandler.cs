using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order.Leasing
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetPaymentHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Order.Leasing.PaymentHistoryModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetPaymentHistoryHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Budget.Order.Leasing.PaymentHistoryModel> Handle()
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
				var leasingValidationHistory = Infrastructure.Data.Access.Tables.FNC.OrderValidationLeasingAccess.GetFirstByOrderId(new List<int> { this._data });

				if(leasingValidationHistory == null || leasingValidationHistory.Count <= 0)
				{
					return ResponseModel<Models.Budget.Order.Leasing.PaymentHistoryModel>.SuccessResponse(new Models.Budget.Order.Leasing.PaymentHistoryModel(orderEntity, new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity
					{
						OrderId = this._data,
						ValidationTime = orderEntity.ValidationRequestTime ?? DateTime.MinValue
					}));
				}

				// get last validation date
				var lastValidation = leasingValidationHistory
					?.Where(x => x.OrderId == this._data)
					?.Aggregate((y, z) => y.ValidationTime > z.ValidationTime ? y : z);

				return ResponseModel<Models.Budget.Order.Leasing.PaymentHistoryModel>.SuccessResponse(new Models.Budget.Order.Leasing.PaymentHistoryModel(orderEntity, lastValidation));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Order.Leasing.PaymentHistoryModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Order.Leasing.PaymentHistoryModel>.AccessDeniedResponse();
			}

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data);
			if(orderEntity == null)
				return ResponseModel<Models.Budget.Order.Leasing.PaymentHistoryModel>.FailureResponse("Order not found");

			return ResponseModel<Models.Budget.Order.Leasing.PaymentHistoryModel>.SuccessResponse();
		}
	}
}
