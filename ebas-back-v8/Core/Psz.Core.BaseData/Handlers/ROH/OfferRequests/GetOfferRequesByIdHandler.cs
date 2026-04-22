using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class GetOfferRequesByIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<CloseOfferMinimalModel>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int _data { get; set; }
	public GetOfferRequesByIdHandler(Identity.Models.UserModel user, int Id)
	{
		this._user = user;
		this._data = Id;
	}

	public ResponseModel<CloseOfferMinimalModel> Handle()
	{
		try
		{

			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(_data);


			if(fetchedData is null)
				return ResponseModel<CloseOfferMinimalModel>.FailureResponse("Sorry, Offer Was not found !.");

			var restoreturn = new CloseOfferMinimalModel(fetchedData);

			var data = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.OfferToArticleEKAccess.GetByOfferID(fetchedData.Id);
			if(data is not null && data.Id > 0)
				restoreturn.AlreadyEkCreated = true;

			return ResponseModel<CloseOfferMinimalModel>.SuccessResponse(restoreturn);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<CloseOfferMinimalModel> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<CloseOfferMinimalModel>.AccessDeniedResponse();
		}
		return ResponseModel<CloseOfferMinimalModel>.SuccessResponse();
	}
}
