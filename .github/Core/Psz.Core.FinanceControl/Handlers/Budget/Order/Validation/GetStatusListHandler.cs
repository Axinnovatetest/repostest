using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order.Validation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetStatusListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetStatusListHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var response = new List<KeyValuePair<int, string>>();
				for(int i = 0; i < Enums.BudgetEnums.MaxOrderValidationStep + 1; i++)
				{
					response.Add(new KeyValuePair<int, string>(i, Enums.BudgetEnums.GetOrderValidationStatus(i)));
				}

				// -- Completed
				response.Add(new KeyValuePair<int, string>(Enums.BudgetEnums.CompletedOrderValidationStep, Enums.BudgetEnums.GetOrderValidationStatus(Enums.BudgetEnums.CompletedOrderValidationStep)));

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
