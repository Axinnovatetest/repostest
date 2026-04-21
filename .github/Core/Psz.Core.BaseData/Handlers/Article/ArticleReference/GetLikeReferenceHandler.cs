using Infrastructure.Data.Entities.Tables.BSD;
using Psz.Core.BaseData.Models.Article.ArticleReference;
using Psz.Core.SharedKernel.Interfaces;
using System;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.ArticleReference;

public class GetLikeReferenceHandler: IHandle<UserModel, ResponseModel<List<GetLikeCustomerArticleReferenceModel>>>
{
	private UserModel _user { get; set; }
	public string _data { get; set; }
	public GetLikeReferenceHandler(UserModel user, string searchtext)
	{
		this._user = user;
		this._data = searchtext;
	}
	public ResponseModel<List<GetLikeCustomerArticleReferenceModel>> Handle()
	{

		var validationResponse = this.Validate();
		if(!validationResponse.Success)
		{
			return validationResponse;
		}



		try
		{
			var dbentity = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.GetLikeReference(_data);
			if(dbentity is null)
				return ResponseModel<List<GetLikeCustomerArticleReferenceModel>>.SuccessResponse(new List<GetLikeCustomerArticleReferenceModel>());


			var res = dbentity.Select(x => new GetLikeCustomerArticleReferenceModel(x)).ToList();

			return ResponseModel<List<GetLikeCustomerArticleReferenceModel>>.SuccessResponse(res);

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<GetLikeCustomerArticleReferenceModel>> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<GetLikeCustomerArticleReferenceModel>>.AccessDeniedResponse();
		}
		return ResponseModel<List<GetLikeCustomerArticleReferenceModel>>.SuccessResponse();
	}

}
