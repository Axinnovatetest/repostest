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

public class DeleteSingleArticleOfferRequestbyIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int _data { get; set; }

	public DeleteSingleArticleOfferRequestbyIdHandler(Identity.Models.UserModel user, int Id)
	{
		this._user = user;
		this._data = Id;
	}

	public ResponseModel<int> Handle()
	{

		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedOffer = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(_data);
			
			var removedOffer = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Delete(_data);
			var LogSavingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.Insert(GenerateLogs(_data,fetchedOffer.SupplierContactName,fetchedOffer.ManufactuerNumber ));

			return ResponseModel<int>.SuccessResponse(removedOffer);

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<int> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<int>.AccessDeniedResponse();
		}
		return ResponseModel<int>.SuccessResponse();
	}

	private Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity GenerateLogs(int Id, string SupplierName,string ManNumber)
	{
		var offerEnities = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(Id);

		var log = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
		{
			LastUpdateUserId = _user.Id,
			LastUpdateUsername = _user.Username,
			LastUpdateTime = DateTime.Now,
			LastUpdateUserFullName = _user.Name,
			LogDescription = $"The Offer request  for the Supplier [{SupplierName}] and ManNumber [{ManNumber}] has been Removed ",
			LogObjectId = Id,
			ManufacturerNumber = offerEnities.ManufactuerNumber,
			SupplierContactName = offerEnities.SupplierContactName,
			LogObject = "BSD Offer Request"
		};
		return log;
	}
}

