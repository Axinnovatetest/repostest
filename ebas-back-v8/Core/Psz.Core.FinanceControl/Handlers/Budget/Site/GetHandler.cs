using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Site
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Site.GetModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Budget.Site.GetModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get(this._data);
				var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(companyExtensionEntity.CompanyId);

				return ResponseModel<Models.Budget.Site.GetModel>.SuccessResponse(new Models.Budget.Site.GetModel(companyEntity, companyExtensionEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Site.GetModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Site.GetModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get(this._data) == null)
				return ResponseModel<Models.Budget.Site.GetModel>.FailureResponse("Site Not found");

			return ResponseModel<Models.Budget.Site.GetModel>.SuccessResponse();
		}
	}
}
