using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetCustomerProjectHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetCustomerProjectModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetCustomerProjectHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.GetCustomerProjectModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetCustomerProjectModel>();
				var kunden_project_tableEntities = Infrastructure.Data.Access.Tables.FNC.KundenAccess.GetCustomerProject();


				foreach(var kunden_project_tableEntity in kunden_project_tableEntities)
				{
					responseBody.Add(new Models.Budget.GetCustomerProjectModel(kunden_project_tableEntity));
				}

				return ResponseModel<List<Models.Budget.GetCustomerProjectModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.GetCustomerProjectModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.GetCustomerProjectModel>>.SuccessResponse();
		}
	}
}
