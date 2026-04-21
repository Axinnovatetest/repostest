using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.BaseData.Models.Article.Purchase;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using MoreLinq.Extensions;
using System.Globalization;

namespace Psz.Core.BaseData.Handlers.Article.Purchase;

public class VerifyOfferDateByArticleAndSupplierIdHandler: IHandle<UserModel, ResponseModel<bool>>
{
	private UserModel _user { get; set; }
	public OfferDateMinimalRequestModel _data { get; set; }
	public VerifyOfferDateByArticleAndSupplierIdHandler(UserModel user, OfferDateMinimalRequestModel filter)
	{
		_user = user;
		_data = filter;
	}
	public ResponseModel<bool> Handle()
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
				return ResponseModel<bool>.SuccessResponse(false);
			var restoreturn = new GetModel(purchaseEntities.FirstOrDefault());

			return ResponseModel<bool>.SuccessResponse(CompareOffersDates(restoreturn.Angebot_Datum, _data.OfferDate));
		} catch(Exception exception)
		{
			Infrastructure.Services.Logging.Logger.Log(exception);
			throw;
		}
	}
	private bool CompareOffersDates(string dateString, DateTime? date2)
	{
		if(date2 is null)
			return true;

		string format = "dd/MM/yyyy";
		if(DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
		{
			return date1 > date2;
		}
		else
		{
			return false;
		}
	}

	public ResponseModel<bool> Validate()
	{
		if(_user == null/*
                || this._user.Access.____*/)
		{
			return ResponseModel<bool>.AccessDeniedResponse();
		}

		if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.ArtikelNr ?? -1) == null)
			return ResponseModel<bool>.FailureResponse("Article not found");

		return ResponseModel<bool>.SuccessResponse();
	}

}
