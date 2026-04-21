using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.ArticleEmployeeAV
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Configuration.ArticleContactAVModel>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Configuration.ArticleContactAVModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var entities = Infrastructure.Data.Access.Tables.CTS.AV_Abteilung_MitarbeiterAccess.Get();
				if(entities != null && entities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Configuration.ArticleContactAVModel>>.SuccessResponse(entities
							.Select(x => new Models.Article.Configuration.ArticleContactAVModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Configuration.ArticleContactAVModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.ArticleContactAVModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Configuration.ArticleContactAVModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Configuration.ArticleContactAVModel>>.SuccessResponse();
		}
	}
}
