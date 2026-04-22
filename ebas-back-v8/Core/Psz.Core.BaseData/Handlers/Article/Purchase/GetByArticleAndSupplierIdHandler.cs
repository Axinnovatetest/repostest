using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.BaseData.Models.Article.Purchase;

namespace Psz.Core.BaseData.Handlers.Article.Purchase;

public class GetByArticleAndSupplierIdHandler: IHandle<UserModel, ResponseModel<GetModel>>
{
	private UserModel _user { get; set; }
	public GetMinimalRequestModel _data { get; set; }
	public GetByArticleAndSupplierIdHandler(UserModel user, GetMinimalRequestModel filter)
	{
		_user = user;
		_data = filter;
	}
	public ResponseModel<GetModel> Handle()
	{
		try
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var purchaseEntities = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticleAndSupplierNr(_data.ArtikelNr ?? -1, _data.Lieferanten_Nr ?? -1) ?? new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();

			if(purchaseEntities is null)
				return ResponseModel<GetModel>.SuccessResponse(new GetModel() { ArtikelNr = null });
			var restoreturn = new GetModel(purchaseEntities.FirstOrDefault());

			return ResponseModel<GetModel>.SuccessResponse(restoreturn);
		} catch(Exception exception)
		{
			Infrastructure.Services.Logging.Logger.Log(exception);
			throw;
		}
	}

	public ResponseModel<GetModel> Validate()
	{
		if(_user == null/*
                || this._user.Access.____*/)
		{
			return ResponseModel<GetModel>.AccessDeniedResponse();
		}

		if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.ArtikelNr ?? -1) == null)
			return ResponseModel<GetModel>.FailureResponse("Article not found");

		return ResponseModel<GetModel>.SuccessResponse();
	}

}