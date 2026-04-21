using Infrastructure.Services.Reporting.Models.FNC;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.BaseData.Models.Article.ArticlesPricesHistory;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

   public  class GetLogChangesOfferRequestHandler: IHandle<Identity.Models.UserModel, ResponseModel<IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private Psz.Core.BaseData.Models.ROH.GetOfferRequestLogsRequestModel _data { get; set; }

	public GetLogChangesOfferRequestHandler(Identity.Models.UserModel user, Psz.Core.BaseData.Models.ROH.GetOfferRequestLogsRequestModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>> Handle()
	{

		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}


			var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
			{
				FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
				RequestRows = this._data.PageSize
			};

			var fetchedOffers = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.GetAll(dataPaging,_data.FilterText.ToLower());



			if(fetchedOffers is null || fetchedOffers.Count == 0)
				return ResponseModel<IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>>.SuccessResponse(new IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>());


			var restoreturn = fetchedOffers.Select(x => new GetLogChangesOfferRequestWithTotalCountResponseModel(x)).ToList();


			return ResponseModel<IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>>.SuccessResponse(new IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>
			{
				Items = restoreturn,
				PageRequested = this._data.RequestedPage,
				PageSize = this._data.PageSize,
				TotalCount = restoreturn.FirstOrDefault().TotalCount ?? 0,
				TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(restoreturn.FirstOrDefault().TotalCount > 0 ? restoreturn.FirstOrDefault().TotalCount : 0) / this._data.PageSize)) : 0
			});


		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}
	public ResponseModel<IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>>.SuccessResponse();
	}
}
