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
public class GetArtikelCustomerReferencesHandler: IHandle<UserModel, ResponseModel<List<GetArtikelCustomerReferencesModel>>>
{
	private UserModel _user { get; set; }
	public int _data { get; set; }
	public GetArtikelCustomerReferencesHandler(UserModel user, int ArtikelId)
	{
		this._user = user;
		this._data = ArtikelId;
	}
	public ResponseModel<List<GetArtikelCustomerReferencesModel>> Handle()
	{

		var validationResponse = this.Validate();
		if(!validationResponse.Success)
		{
			return validationResponse;
		}

		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
		try
		{

			var data = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.GetByArtikelId(_data);
			var res = data.Select(x => new GetArtikelCustomerReferencesModel(x)).ToList();
			// -
			return ResponseModel<List<GetArtikelCustomerReferencesModel>>.SuccessResponse(res);

		} catch(Exception e)
		{
			botransaction.rollback();
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<GetArtikelCustomerReferencesModel>> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<GetArtikelCustomerReferencesModel>>.AccessDeniedResponse();
		}
		return ResponseModel<List<GetArtikelCustomerReferencesModel>>.SuccessResponse();
	}

}
