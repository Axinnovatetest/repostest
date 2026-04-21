using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.InternalStatus
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel>.SuccessResponse(
						new Models.Article.Configuration.Quality.InternalStatusModel(Infrastructure.Data.Access.Tables.BSD.ArtikelInternalStatusAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.ArtikelInternalStatusAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel>.ResponseError>
					{
						new ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel>.ResponseError
						{
							Key = "1",
							Value = "Article status not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.Configuration.Quality.InternalStatusModel>.SuccessResponse();
		}
	}
}
