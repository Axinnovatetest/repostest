using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Account
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Account.AccountModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Budget.Account.AccountModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var AccountEntities = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_KontenSoll_WKZAccess.Get();
				if(AccountEntities == null)
				{
					return ResponseModel<List<Models.Budget.Account.AccountModel>>.SuccessResponse();
				}

				var response = new List<Models.Budget.Account.AccountModel>();
				foreach(var x in AccountEntities)
				{


					response.Add(new Models.Budget.Account.AccountModel(x));
				}

				return ResponseModel<List<Models.Budget.Account.AccountModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Account.AccountModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<Models.Budget.Account.AccountModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Account.AccountModel>>.SuccessResponse();
		}
	}

}
