using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class GetArticleHasStandardSupplierdHandler: IHandle<UserModel, ResponseModel<bool>>
{
	private UserModel _user { get; set; }
	public int _data { get; set; }
	public GetArticleHasStandardSupplierdHandler(UserModel user, int ArticleId)
	{
		this._user = user;
		this._data = ArticleId;
	}
	public ResponseModel<bool> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var suppliers = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByStandardSupplier(_data);

			if(suppliers is null || suppliers.Count == 0)
				return ResponseModel<bool>.SuccessResponse(false);

			return ResponseModel<bool>.SuccessResponse(true);
		} catch(Exception exception)
		{
			Infrastructure.Services.Logging.Logger.Log(exception);
			throw;
		}
	}

	public ResponseModel<bool> Validate()
	{
		if(this._user == null/*
                || this._user.Access.____*/)
		{
			return ResponseModel<bool>.AccessDeniedResponse();
		}

		if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
			return ResponseModel<bool>.FailureResponse("Article not found");

		return ResponseModel<bool>.SuccessResponse();
	}

}
