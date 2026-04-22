using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Configuration.Quality.MHDTagModel>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Configuration.Quality.MHDTagModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var tagEntites = Infrastructure.Data.Access.Tables.BSD.ArtikelMHD_TagAccess.Get();
				if(tagEntites != null && tagEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.Configuration.Quality.MHDTagModel>>.SuccessResponse(tagEntites
							.Select(x => new Models.Article.Configuration.Quality.MHDTagModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Configuration.Quality.MHDTagModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.Quality.MHDTagModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Configuration.Quality.MHDTagModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Configuration.Quality.MHDTagModel>>.SuccessResponse();
		}
	}
}
