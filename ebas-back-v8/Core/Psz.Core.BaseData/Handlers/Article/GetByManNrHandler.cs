using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article;

public class GetByManNrHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.ArticleModel>>
{
	private Identity.Models.UserModel _user { get; set; }
	private string _data { get; set; }


	public GetByManNrHandler(Identity.Models.UserModel user, string id)
	{
		this._user = user;
		this._data = id;
	}

	public ResponseModel<Models.Article.ArticleModel> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			// get  angebote -- angebote-nr
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArtikelByManNr(this._data);
			if(articleEntity is null)
				return ResponseModel<Models.Article.ArticleModel>.SuccessResponse(new Models.Article.ArticleModel() { ArtikelNr = -1 });

			var result = new Models.Article.ArticleModel(articleEntity);

			return ResponseModel<Models.Article.ArticleModel>.SuccessResponse(result);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<Models.Article.ArticleModel> Validate()
	{
		if(this._user == null/*
                || this._user.Access.____*/)
		{
			return ResponseModel<Models.Article.ArticleModel>.AccessDeniedResponse();
		}
		return ResponseModel<Models.Article.ArticleModel>.SuccessResponse();
	}
}

