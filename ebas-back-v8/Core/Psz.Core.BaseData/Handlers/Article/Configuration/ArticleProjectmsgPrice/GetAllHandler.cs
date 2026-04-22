using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.ArticleProjectmsgPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Configuration.ArticleProjectmsgPriceModel>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Configuration.ArticleProjectmsgPriceModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var projectTypeEntites = Infrastructure.Data.Access.Tables.BSD.ArticleProjectmsgPriceAccess.Get();
				if(projectTypeEntites != null && projectTypeEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.Configuration.ArticleProjectmsgPriceModel>>.SuccessResponse(projectTypeEntites
							.Select(x => new Models.Article.Configuration.ArticleProjectmsgPriceModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Configuration.ArticleProjectmsgPriceModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.ArticleProjectmsgPriceModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Configuration.ArticleProjectmsgPriceModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Configuration.ArticleProjectmsgPriceModel>>.SuccessResponse();
		}
	}
}
