using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.BaseData.Models.Article.Purchase;

public class GetByArticleAndSupplierIdHandler: IHandle<UserModel, ResponseModel<Models.Article.Purchase.GetModel>>
{
	private UserModel _user { get; set; }
	public GetMinimalRequestModel _data { get; set; }
	public GetByArticleAndSupplierIdHandler(UserModel user, GetMinimalRequestModel filter)
	{
		this._user = user;
		this._data = filter;
	}
	public ResponseModel<Models.Article.Purchase.GetModel> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var purchaseEntities = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticleAndSupplierNr(this._data.ArtikelNr ?? -1, this._data.Lieferanten_Nr ?? -1) ?? new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();

			if(purchaseEntities is null)
				return ResponseModel<Models.Article.Purchase.GetModel>.SuccessResponse(new Models.Article.Purchase.GetModel() { ArtikelNr = null });
			var restoreturn = new Models.Article.Purchase.GetModel(purchaseEntities.FirstOrDefault());

			return ResponseModel<Models.Article.Purchase.GetModel>.SuccessResponse(restoreturn);
		} catch(Exception exception)
		{
			Infrastructure.Services.Logging.Logger.Log(exception);
			throw;
		}
	}

	public ResponseModel<Models.Article.Purchase.GetModel> Validate()
	{
		if(this._user == null/*
                || this._user.Access.____*/)
		{
			return ResponseModel<Models.Article.Purchase.GetModel>.AccessDeniedResponse();
		}

		if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr ?? -1) == null)
			return ResponseModel<Models.Article.Purchase.GetModel>.FailureResponse("Article not found");

		return ResponseModel<Models.Article.Purchase.GetModel>.SuccessResponse();
	}

}