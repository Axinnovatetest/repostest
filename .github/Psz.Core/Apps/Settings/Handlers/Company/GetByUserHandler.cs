using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Company
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Settings.Models.Company.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetByUserHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Settings.Models.Company.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var companyEntities = this._user.IsGlobalDirector || this._user.IsCorporateDirector
					? Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get()
					: Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { this._user.Id }) ?? new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
				var response = new List<Settings.Models.Company.GetModel>();
				foreach(var companyEntity in companyEntities)
				{
					response.Add(new Settings.Models.Company.GetModel(companyEntity));
				}

				return ResponseModel<List<Settings.Models.Company.GetModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Settings.Models.Company.GetModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Settings.Models.Company.GetModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Settings.Models.Company.GetModel>>.FailureResponse("user not found");

			return ResponseModel<List<Settings.Models.Company.GetModel>>.SuccessResponse();
		}
	}
}
