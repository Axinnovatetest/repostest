using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetStateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetStateModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetStateHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.GetStateModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetStateModel>();
				var states_tableEntities = Infrastructure.Data.Access.Tables.FNC.State_BudgetAccess.Get();


				foreach(var state_tableEntity in states_tableEntities)
				{
					responseBody.Add(new Models.Budget.GetStateModel(state_tableEntity));
				}

				return ResponseModel<List<Models.Budget.GetStateModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.GetStateModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.GetStateModel>>.SuccessResponse();
		}
	}
}
