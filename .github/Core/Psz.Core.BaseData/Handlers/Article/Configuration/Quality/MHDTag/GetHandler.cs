using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.Configuration.Quality.MHDTagModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Configuration.Quality.MHDTagModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Configuration.Quality.MHDTagModel>.SuccessResponse(
						new Models.Article.Configuration.Quality.MHDTagModel(Infrastructure.Data.Access.Tables.BSD.ArtikelMHD_TagAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Configuration.Quality.MHDTagModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Configuration.Quality.MHDTagModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.ArtikelMHD_TagAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.Configuration.Quality.MHDTagModel>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.Configuration.Quality.MHDTagModel>.ResponseError>
					{
						new ResponseModel<Models.Article.Configuration.Quality.MHDTagModel>.ResponseError
						{
							Key = "1",
							Value = "Article tag not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.Configuration.Quality.MHDTagModel>.SuccessResponse();
		}
	}
}
