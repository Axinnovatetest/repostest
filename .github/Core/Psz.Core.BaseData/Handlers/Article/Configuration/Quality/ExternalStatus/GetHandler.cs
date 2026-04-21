using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.ExternalStatus
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel>.SuccessResponse(
						new Models.Article.Configuration.Quality.ExternalStatusModel(Infrastructure.Data.Access.Tables.BSD.ArtikelExternalStatusAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.ArtikelExternalStatusAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel>.ResponseError>
					{
						new ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel>.ResponseError
						{
							Key = "1",
							Value = "Article status not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.Configuration.Quality.ExternalStatusModel>.SuccessResponse();
		}
	}
}
