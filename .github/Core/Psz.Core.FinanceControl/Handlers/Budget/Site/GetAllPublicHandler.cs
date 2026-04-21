using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Site
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllPublicHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Site.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetAllPublicHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Budget.Site.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(false);

				if(companyEntities == null || companyEntities.Count <= 0)
				{
					return ResponseModel<List<Models.Budget.Site.GetModel>>.SuccessResponse(null);
				}

				var companyExtensionEntities = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompanyIds(companyEntities?.Select(x => x?.Id ?? -1)?.ToList());
				var response = new List<Models.Budget.Site.GetModel>();
				foreach(var companyEntity in companyEntities)
				{
					var companyExtensionEntity = companyExtensionEntities?.Find(x => x.CompanyId == companyEntity.Id);
					response.Add(new Models.Budget.Site.GetModel(companyEntity, companyExtensionEntity));
				}

				return ResponseModel<List<Models.Budget.Site.GetModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Site.GetModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Site.GetModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Site.GetModel>>.SuccessResponse();
		}
	}
}
