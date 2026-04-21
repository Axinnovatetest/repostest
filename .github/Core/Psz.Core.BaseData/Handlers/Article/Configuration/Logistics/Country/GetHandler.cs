using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.Country
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.Configuration.Logistics.CountryModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Configuration.Logistics.CountryModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Configuration.Logistics.CountryModel>.SuccessResponse(
						new Models.Article.Configuration.Logistics.CountryModel(Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Configuration.Logistics.CountryModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Configuration.Logistics.CountryModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.Get(this._data) == null)
			{
				return ResponseModel<Models.Article.Configuration.Logistics.CountryModel>.FailureResponse("Country not found");
			}


			return ResponseModel<Models.Article.Configuration.Logistics.CountryModel>.SuccessResponse();
		}
	}
}
