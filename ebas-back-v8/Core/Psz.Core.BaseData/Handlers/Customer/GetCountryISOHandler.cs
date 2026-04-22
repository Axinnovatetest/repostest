using System;
using System.Collections.Generic;
using System.Linq;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.BaseData.Handlers.Country
{
	public class GetCountryISOHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCountryISOHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;

				var entities = Infrastructure.Data.Access.Tables.BSD.CountryISOAccess.Get();

				var models = entities.Select(e => new Models.Article.Configuration.Logistics.CountryISOModel
				{
					Id = e.Id,
					Name = e.Name,
					NativeName = e.NativeName,
					alpha2Code = e.alpha2Code,
					alpha3Code = e.alpha3Code,
					NumericCode = e.NumericCode,
				}).ToList();

				return ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>.SuccessResponse(models);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>> Validate()
		{
			if(_user == null /* || !_user.Access.HasCountryRead */)
			{
				return ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Configuration.Logistics.CountryISOModel>>.SuccessResponse();
		}
	}
}
