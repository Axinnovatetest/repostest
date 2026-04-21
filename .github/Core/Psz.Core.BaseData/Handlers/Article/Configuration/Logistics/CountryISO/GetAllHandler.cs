using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statusEntites = Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.Get();
				if(statusEntites != null && statusEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>.SuccessResponse(statusEntites
							.Select(x => new Models.Article.Configuration.Logistics.CountryISOModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>.SuccessResponse();
		}
	}
}
