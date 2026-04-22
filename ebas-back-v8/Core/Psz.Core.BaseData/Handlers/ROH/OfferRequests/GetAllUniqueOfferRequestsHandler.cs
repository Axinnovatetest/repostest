using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class GetAllUniqueOfferRequestsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleOfferRequestsModel>>>
{
	private Identity.Models.UserModel _user { get; set; }

	public GetAllUniqueOfferRequestsHandler(Identity.Models.UserModel user)
	{
		this._user = user;
	}

	public ResponseModel<List<ArticleOfferRequestsModel>> Handle()
	{

		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedOffers = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get();
			if(fetchedOffers is null || fetchedOffers.Count == 0)
				return ResponseModel<List<ArticleOfferRequestsModel>>.SuccessResponse(new List<ArticleOfferRequestsModel>());

			var restoreturn = fetchedOffers.Select(x => new ArticleOfferRequestsModel(x)).ToList();

			for(int i = 0; i < restoreturn.Count; i++)
			{
				var item = restoreturn[i];
				var numberOfSimilarOffer = restoreturn.Count(x => x.RequestUI == item.RequestUI);
				item.numberofRequestsInTheSameOffer = numberOfSimilarOffer;
			}

			return ResponseModel<List<ArticleOfferRequestsModel>>.SuccessResponse(restoreturn);

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<ArticleOfferRequestsModel>> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<List<ArticleOfferRequestsModel>>.AccessDeniedResponse();
		}
		return ResponseModel<List<ArticleOfferRequestsModel>>.SuccessResponse();
	}
}
