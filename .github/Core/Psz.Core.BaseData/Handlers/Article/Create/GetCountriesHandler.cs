using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public partial class GetCountriesHandler: IHandle<string, ResponseModel<List<Models.Article.CountryResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCountriesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Article.CountryResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var results = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				// -
				return ResponseModel<List<Models.Article.CountryResponseModel>>.SuccessResponse(
					results?.Select(x => new Models.Article.CountryResponseModel(x))?.Distinct()?.ToList());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.CountryResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.CountryResponseModel>>.AccessDeniedResponse();
			}

			// -
			return ResponseModel<List<Models.Article.CountryResponseModel>>.SuccessResponse();
		}
	}
}
