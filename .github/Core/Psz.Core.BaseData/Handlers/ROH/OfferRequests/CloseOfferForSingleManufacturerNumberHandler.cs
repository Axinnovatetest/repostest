using Psz.Core.BaseData.Helpers;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class CloseOfferForSingleManufacturerNumberHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private CloseOfferRequestModel _data { get; set; }
	public CloseOfferForSingleManufacturerNumberHandler(Identity.Models.UserModel user, CloseOfferRequestModel data)
	{
		this._user = user;
		this._data = data;
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
			var EntityToSave = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsEntity()
			{
				Id = _data.Id,
				CustomQuantity = _data.CustomQuantity,
				YearlyQuantity = _data.YearlyQuantity,
				UnitPrice = _data.UnitPrice,
				RequestStatus = true,
				PriceExpiryDate = _data.PriceExpiryDate,
				OfferNumber = _data.OfferNumber,
				Feedback = _data.Feedback,
				FileId = -1,
				ClosedDate = DateTime.Now,
				ClosedUserId = _user.Id,
				ClosedUserName = _user.Username,
				Graduatedprice1 = _data.Graduatedprice1,
				Graduatedprice2 = _data.Graduatedprice2,
				Graduatedprice3 = _data.Graduatedprice3,

				Graduatedquantity1 = _data.Graduatedquantity1,
				Graduatedquantity2 = _data.Graduatedquantity2,
				Graduatedquantity3 = _data.Graduatedquantity3,

				// 
				MinOrderQuantity = _data.MinOrderQuantity,
				PackagingUnit = _data.PackagingUnit,
				DeliveryTime = _data.DeliveryTime,
				ExportWeight = _data.ExportWeight,
				CustomTariffNumber = _data.CustomTariffNumber,
				CountryOfOrigin = _data.CountryOfOrigin,
				AngebotDatum = _data.AngebotDatum

			};
			var offerfamily = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.UpdateOfferRequestStatusForSingelManufacturerNuWithTransaction(EntityToSave, botransaction.connection, botransaction.transaction);
			Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.UpdateOfferRequestOnGoingStatusForSingelManufacturerNuWithTransaction(EntityToSave.Id, OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.closed, _user), botransaction.connection, botransaction.transaction);

			// Log
			var LogSavingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.InsertWithTransaction(GenerateLogs(EntityToSave.Id, OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.closed, _user)), botransaction.connection, botransaction.transaction);

			if(botransaction.commit())
			{
				return ResponseModel<int>.SuccessResponse(offerfamily);
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

	private Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity GenerateLogs(int Id, string Status)
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
