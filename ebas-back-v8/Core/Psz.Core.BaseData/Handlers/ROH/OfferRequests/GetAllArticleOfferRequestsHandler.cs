using Infrastructure.Services.Reporting.Models.FNC;
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

public class GetAllArticleOfferRequestsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleOfferRequestsModel>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private FilterdOfferRequestModel _data { get; set; }

	public GetAllArticleOfferRequestsHandler(Identity.Models.UserModel user, FilterdOfferRequestModel filter)
	{
		this._user = user;
		_data = filter;
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
			var fetchedOffers = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsEntity>();

			fetchedOffers = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.GetFilterData(_data.loadOpenOnly, GetFilter(_data.status), GetCancelFilter());



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
	public string GenerateRandomColor()
	{
		Random _random = new Random();
		int red = _random.Next(0, 256);
		int green = _random.Next(0, 256);
		int blue = _random.Next(0, 256);
		return $"#{red:X2}{green:X2}{blue:X2}";
	}
	private string GetCancelFilter()
	{
		return "Cancel";
	}
	private string GetFilter(string InputFilter)
	{
		return InputFilter switch
		{
			"Created" => "Create",
			"Sent" => "Sent",
			"Cancelled" => "Cancel",
			"Finished" => "Finish",
			_ => ""
		};
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
