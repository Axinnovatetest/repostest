using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.Industry
{
	public class GetAllIndustries: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Industry.IndustryModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllIndustries(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Industry.IndustryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var industryEntities = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get();

				var response = new List<Models.Industry.IndustryModel>();

				foreach(var industryEntity in industryEntities)
				{
					response.Add(new Models.Industry.IndustryModel(industryEntity));
				}

				return ResponseModel<List<Models.Industry.IndustryModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Industry.IndustryModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Industry.IndustryModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Industry.IndustryModel>>.SuccessResponse();
		}
	}
}
