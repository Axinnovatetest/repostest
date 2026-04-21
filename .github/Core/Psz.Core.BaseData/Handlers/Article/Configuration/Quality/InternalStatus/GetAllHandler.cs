using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.InternalStatus
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Configuration.Quality.InternalStatusModel>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Configuration.Quality.InternalStatusModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statusEntites = Infrastructure.Data.Access.Tables.BSD.ArtikelInternalStatusAccess.Get();
				if(statusEntites != null && statusEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.Configuration.Quality.InternalStatusModel>>.SuccessResponse(statusEntites
							.Select(x => new Models.Article.Configuration.Quality.InternalStatusModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Configuration.Quality.InternalStatusModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.Quality.InternalStatusModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Configuration.Quality.InternalStatusModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Configuration.Quality.InternalStatusModel>>.SuccessResponse();
		}
	}
}
