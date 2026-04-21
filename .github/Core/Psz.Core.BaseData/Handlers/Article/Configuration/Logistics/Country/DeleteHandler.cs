using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.Country
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class DeleteHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public DeleteHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.Delete(this._data));
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

			var country = Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.Get(this._data);
			if(country == null)
			{
				return ResponseModel<int>.FailureResponse($"Country not found");
			}

			var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCountry(country.Land);
			if(articles != null && articles.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Remove country [{country.Land}] from Articles [{string.Join(", ", articles.Take(3).Select(x => x.ArtikelNummer))}] before delete.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
