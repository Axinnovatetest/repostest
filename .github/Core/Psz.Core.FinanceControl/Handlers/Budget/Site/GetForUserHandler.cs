using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Site
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Site.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetByUserHandler(Identity.Models.UserModel user)
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
				var companyEntities = (this._user.IsGlobalDirector || this._user.Access?.Financial?.Budget?.AssignAllSites == true
					? Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(false)
					: Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { this._user.Id })) ?? new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
				var companyExtensions = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompanyIds(companyEntities.Select(x => x.Id).ToList());
				var response = new List<Models.Budget.Site.GetModel>();
				foreach(var companyEntity in companyEntities)
				{
					var x = companyExtensions.FirstOrDefault(y => y.CompanyId == companyEntity.Id);
					response.Add(new Models.Budget.Site.GetModel(companyEntity, x));
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

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Models.Budget.Site.GetModel>>.FailureResponse("user not found");

			return ResponseModel<List<Models.Budget.Site.GetModel>>.SuccessResponse();
		}
	}
}
