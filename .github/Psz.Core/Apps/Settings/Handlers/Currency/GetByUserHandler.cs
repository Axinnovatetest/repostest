using System;

namespace Psz.Core.Apps.Settings.Handlers.Currency
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<Settings.Models.Currency.GetModel>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetByUserHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<Settings.Models.Currency.GetModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity?.CompanyId ?? -1);
				var currencyEntity = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(companyExtensionEntity?.DefaultCurrencyId ?? -1)
					?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };

				return ResponseModel<Settings.Models.Currency.GetModel>.SuccessResponse(new Models.Currency.GetModel(currencyEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Settings.Models.Currency.GetModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Settings.Models.Currency.GetModel>.AccessDeniedResponse();
			}

			return ResponseModel<Settings.Models.Currency.GetModel>.SuccessResponse();
		}
	}
}
