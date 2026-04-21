using Microsoft.AspNetCore.Http;
using Psz.Core.BaseData.Helpers;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests
{
	public class AddOfferToEKPriceRelationHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private OfferToArticleEKModel _data { get; set; }
		public AddOfferToEKPriceRelationHandler(Identity.Models.UserModel user, OfferToArticleEKModel data)
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
				var OfferToEkMappingEntity = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity()
				{
					ArtikelNr = _data.ArtikelNr,
					LastUpdate = DateTime.Now,
					OfferId = _data.OfferId,
					SupplierId = _data.SupplierId,
					BestellnummernNr = _data.BestellnummernNr,
				};
				
				var insert = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.OfferToArticleEKAccess.InsertWithTransaction(OfferToEkMappingEntity, botransaction.connection, botransaction.transaction);
				
				// Logs 
				var OfferData = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(_data.OfferId ?? -1);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.ArtikelNr ?? -1);
				var LogSavingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.InsertWithTransaction(GenerateLogs(_data.OfferId ?? -1, articleEntity.ArticleNumber, OfferData.SupplierContactName,OfferData.ManufactuerNumber), botransaction.connection, botransaction.transaction);


				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insert);
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

		private Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity GenerateLogs(int OfferId,string  Artikelnummer,string SupplierName,string ManNumber)
		{
			var offerEnities = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(OfferId);


			var log = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
			{
				LastUpdateUserId = _user.Id,
				LastUpdateUsername = _user.Username,
				LastUpdateTime = DateTime.Now,
				LastUpdateUserFullName = _user.Name,
				LogDescription = $"The Purchase (EK) price of the Artikel {Artikelnummer}     has been updated  from the offer of  the Supplier {SupplierName} and ManNumber [{ManNumber}] ",
				LogObjectId = OfferId,
				ManufacturerNumber = offerEnities.ManufactuerNumber,
				SupplierContactName = offerEnities.SupplierContactName,
				LogObject = "BSD Offer Request"
			};
			return log;
		}
	}
}
