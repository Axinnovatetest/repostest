using Psz.Core.BaseData.Helpers;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;



namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class CancelOfferRequestHanlder: IHandle<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int _data { get; set; }
	public CancelOfferRequestHanlder(Identity.Models.UserModel user, int Id)
	{
		this._user = user;
		this._data = Id;
	}

	public ResponseModel<int> Handle()
	{
		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

		try
		{


			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			botransaction.beginTransaction();

			var StatusUpdatingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.UpdateOfferRequestOnGoingStatusForSingelManufacturerNuWithTransaction(_data, OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.cancelled, _user), botransaction.connection, botransaction.transaction);
			var LogSavingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.InsertWithTransaction(GenerateLogs(_data, OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.cancelled, _user)), botransaction.connection, botransaction.transaction);
			if(botransaction.commit())
			{
				return ResponseModel<int>.SuccessResponse(StatusUpdatingResult);
			}
			else
			{
				return ResponseModel<int>.FailureResponse("Saving Data Failed !");
			}

		} catch(Exception e)
		{
			botransaction.rollback();
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

	private Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity GenerateLogs(int Id,string Status)
	{
		var offerEnities = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(Id);


		var log = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
		{
			LastUpdateUserId = _user.Id,
			LastUpdateUsername = _user.Username,
			LastUpdateTime = DateTime.Now,
			LastUpdateUserFullName = _user.Name,
			LogDescription = $"The Offer request [ongoingStatus] has been updated to  {Status}",
			LogObjectId = Id,
			ManufacturerNumber = offerEnities.ManufactuerNumber,
			SupplierContactName = offerEnities.SupplierContactName,
			LogObject = "BSD Offer Request"
		};
		return log;
	}
}
