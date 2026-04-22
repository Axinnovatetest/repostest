using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.CheckStatus
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel>.SuccessResponse(
						new Models.Article.Configuration.Quality.CheckStatusModel(Infrastructure.Data.Access.Tables.BSD.ArtikelCheckStatusAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.ArtikelCheckStatusAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel>.ResponseError>
					{
						new ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel>.ResponseError
						{
							Key = "1",
							Value = "Article status not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.Configuration.Quality.CheckStatusModel>.SuccessResponse();
		}
	}
}
