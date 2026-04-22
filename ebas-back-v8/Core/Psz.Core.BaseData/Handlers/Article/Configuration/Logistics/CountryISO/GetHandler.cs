using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.CountryISO
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel>.SuccessResponse(
						new Models.Article.Configuration.Logistics.CountryISOModel(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel>.ResponseError>
					{
						new ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel>.ResponseError
						{
							Key = "1",
							Value = "Country not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.Configuration.Logistics.CountryISOModel>.SuccessResponse();
		}
	}
}
