using iText.Kernel.XMP.Impl;
using Microsoft.AspNetCore.Http;
using Psz.Core.BaseData.Helpers;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public  class RemoveAttachementFromOfferEmailHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private RemoveSingleAttachementFromEmailRequestModel _data { get; set; }
	public RemoveAttachementFromOfferEmailHandler(Identity.Models.UserModel user, RemoveSingleAttachementFromEmailRequestModel data)
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

			//
			var StatusUpdatingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsAccess.DeleteMultipleWithTransaction(_data.FiledId, botransaction.connection, botransaction.transaction);

			var LogSavingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.InsertWithTransaction(GenerateLogs(_data.Ids, OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.cancelled, _user)), botransaction.connection, botransaction.transaction);
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

	private List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> GenerateLogs(List<int> Ids, string Status)
	{
		var logs = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();

		var offerEnities = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(Ids);

		foreach(var item in offerEnities)
		{
			logs.Add(new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
			{
				LastUpdateUserId = _user.Id,
				LastUpdateUsername = _user.Username,
				LastUpdateTime = DateTime.Now,
				LastUpdateUserFullName = _user.Name,
				LogDescription = $" Attachment Removed from  the offer request email",
				LogObjectId = item.Id,
				ManufacturerNumber = item.ManufactuerNumber,
				SupplierContactName = item.SupplierContactName,
				LogObject = "BSD Offer Request"
			});
		}

		return logs;
	}
}
