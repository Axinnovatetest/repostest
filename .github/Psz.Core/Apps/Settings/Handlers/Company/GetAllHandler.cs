using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Company
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Settings.Models.Company.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
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
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(false) ?? new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
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

			return ResponseModel<List<Settings.Models.Company.GetModel>>.SuccessResponse();
		}
	}
}
