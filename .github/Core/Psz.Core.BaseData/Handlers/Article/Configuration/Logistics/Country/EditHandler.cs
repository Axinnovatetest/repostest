using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.Country
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class EditHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Configuration.Logistics.CountryModel _data { get; set; }
		public EditHandler(UserModel user, Models.Article.Configuration.Logistics.CountryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var data = this._data.ToEntity();
				var country = Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.Get(this._data.ID);
				data.Hinweis = this._data.Hinweis;

				if(country.Land.Trim().ToLower() != this._data.Land.Trim().ToLower())
				{
					data.Land = this._data.Land;
				}
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.Update(data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(string.IsNullOrWhiteSpace(this._data.Land))
				return ResponseModel<int>.FailureResponse("Land cannot be empty");

			var country = Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.Get(this._data.ID);
			if(country == null)
			{
				return ResponseModel<int>.FailureResponse($"Country not found");
			}

			if(country.Land.Trim().ToLower() != this._data.Land.Trim().ToLower())
			{
				if(Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.GetByLand(this._data.Land) != null)
					return ResponseModel<int>.FailureResponse($"Country [{this._data.Land}] exists");

				var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCountry(country.Land);
				if(articles != null && articles.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Remove country [{country.Land}] from Articles [{string.Join(", ", articles.Take(3).Select(x => x.ArtikelNummer))}] before edit.");
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
