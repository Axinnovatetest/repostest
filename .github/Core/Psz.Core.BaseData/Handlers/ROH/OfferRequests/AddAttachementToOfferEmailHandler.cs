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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public  class AddAttachementToOfferEmailHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private AddAttachementToEmailRequestModel _data { get; set; }
	public AddAttachementToOfferEmailHandler(Identity.Models.UserModel user, AddAttachementToEmailRequestModel data)
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

			var Ids = SpecialHelper.ParseIds(_data.Ids);
			int fileId = -1;
			byte[] fileBytes;
			using(var ms = new MemoryStream())
			{
				_data.file.CopyTo(ms);
				fileBytes = ms.ToArray();
				 fileId = Program.FilesManager.NewFile(fileBytes, Path.GetExtension(_data.file.FileName), 10);
			}
			if(fileId == -1 )
				return ResponseModel<int>.FailureResponse("Problem Occured !");


			var attachment = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.GetAttachmentIById(Ids[0]);

			var Attachmententity = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity()
			{
				FileId = fileId,
				FileName = _data.file.FileName,
				OfferId = Ids[0]
			};

			botransaction.beginTransaction();
			//GetAttachmentIById
			
			Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsAccess.InsertWithTransaction(Attachmententity, botransaction.connection, botransaction.transaction);

			var LogSavingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.InsertWithTransaction(GenerateLogs(Ids, OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.cancelled, _user)), botransaction.connection, botransaction.transaction);
			if(botransaction.commit())
			{
				return ResponseModel<int>.SuccessResponse(fileId);
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
			if(_data.Ids is null || _data.Ids.Count() ==0)
				return  ResponseModel<int>.FailureResponse("Invalid Offer !");
			if(_data.file is null)
				return ResponseModel<int>.FailureResponse("Invalid File !");

			return ResponseModel<int>.AccessDeniedResponse();
		}
		return ResponseModel<int>.SuccessResponse();
	}

	private List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> GenerateLogs(List<int> Ids, string Status)
	{
		var logs = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> ();
		var offerEnities = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(Ids);

		foreach(var item in offerEnities)
		{
			logs.Add(new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
			{
				LastUpdateUserId = _user.Id,
				LastUpdateUsername = _user.Username,
				LastUpdateTime = DateTime.Now,
				LastUpdateUserFullName = _user.Name,
				LogDescription = $"New Attachment added to the offer request email",
				LogObjectId = item.Id,
				ManufacturerNumber = item.ManufactuerNumber,
				SupplierContactName = item.SupplierContactName,
				LogObject = "BSD Offer Request"
			});
		}
		
		return logs;
	}
}
