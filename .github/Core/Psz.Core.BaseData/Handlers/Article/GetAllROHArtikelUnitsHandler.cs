using System;
using System.Collections.Generic;
using System.Linq;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.BaseData.Handlers.Article;

public class GetAllROHArtikelUnitsHandler: IHandle<string, ResponseModel<List<string>>>
{
	private Identity.Models.UserModel _user { get; set; }

	public GetAllROHArtikelUnitsHandler(Identity.Models.UserModel user)
	{
		this._user = user;
	}

	public ResponseModel<List<string>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var Fetchedunits = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetROHArticlesUnits();
			if(Fetchedunits == null || Fetchedunits.Count == 0)
				return ResponseModel<List<string>>.SuccessResponse(new List<string>());

			return ResponseModel<List<string>>.SuccessResponse(Fetchedunits.Select(x => x.Unit).ToList());
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<string>> Validate()
	{
		if(this._user == null/*
                || this._user.Access.____*/)
		{
			return ResponseModel<List<string>>.AccessDeniedResponse();
		}

		return ResponseModel<List<string>>.SuccessResponse();
	}
}
