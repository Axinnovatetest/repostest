using CommunityToolkit.HighPerformance.Helpers;
using iText.Kernel.Pdf.Filters;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.BaseData.Helpers;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class CreateArticleOfferRequestsHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
{
	private Identity.Models.UserModel _user { get; set; }
	private OfferrequestVM _data { get; set; }

	public CreateArticleOfferRequestsHandler(Identity.Models.UserModel user, OfferrequestVM data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<string> Handle()
	{

		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
		try
		{

			#region // -- transaction-based logic -- //

			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}


			// 1--> validate input Data
			// 2--> create offer request

			var createDate = DateTime.Now;
			var CommunCreatedUserId = this._user.Id;
			var ccEmail = this._user.Email;
			var CommunCreatedUserName = this._user.Username;
			var CommunRequestUI = Guid.NewGuid().ToString();


			botransaction.beginTransaction();

			int CreatedOfferRequestId = -1;
			foreach(var item in _data.offers)
			{


				foreach(var offrerequest in item.offers)
				{

					var supplier = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(item.SupplierId ?? -1);

					var Createentity = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsEntity()
					{

						CCEmail = _user.Email,
						EmailSubject = GenerateEmailSubjectBasedOnDefaultSupplierLanguage(item.SupplierId ?? -1, item.ProjectName, item.EndCustomer),
						Graduatedquantity1 = offrerequest.Graduatedquantity1,
						Graduatedquantity2 = offrerequest.Graduatedquantity2,
						Graduatedquantity3 = offrerequest.Graduatedquantity3,
						Graduatedprice1 = offrerequest.Graduatedprice1,
						Graduatedprice2 = offrerequest.Graduatedprice2,
						Graduatedprice3 = offrerequest.Graduatedprice3,
						CreatedDate = createDate,
						CreatedUserId = CommunCreatedUserId,
						CreatedUserName = CommunCreatedUserName,
						CustomQuantity = offrerequest.CustomQuantity,
						//EmailId = offrerequest.EmailId,
						//EmailStatus = offrerequest.EmailStatus,
						//Id = offrerequest.Id,
						ManufactuerNumber = offrerequest.ManufactuerNumber,
						RequestStatus = false,
						RequestUI = CommunRequestUI,
						//SentDate = offrerequest.SentDate,
						//SentUserId = offrerequest.SentUserId,
						//SentUserName = offrerequest.SentUserName,
						// email
						SupplierContactEmail = item.SupplierEmail,
						// name
						SupplierContactName = supplier.Name1,
						SupplierId = item.SupplierId,
						EndCustomer = item.EndCustomer,
						ProjectName = item.ProjectName,
						// name
						unit = offrerequest.warengrupId,
						SupplierName = supplier.Name1,
						YearlyQuantity = offrerequest.YearlyQuantity,
						OfferRequestArticleDescription = offrerequest.OfferRequestArticleDescription,
						ongoingStatus = OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.created, _user)
					};
					CreatedOfferRequestId = Createentity.Id;
					Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.InsertWithTransaction(Createentity, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.UpdateOfferRequestOnGoingStatusForSingelManufacturerNrByUIWithTransaction(CommunRequestUI, OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.created, _user), botransaction.connection, botransaction.transaction);
					var LogSavingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.InsertWithTransaction(GenerateLogs(CreatedOfferRequestId, Createentity.ManufactuerNumber, Createentity.SupplierContactName, OfferRequestsManager.GenerateRequestStatus(OfferRequestStatus.created, _user)), botransaction.connection, botransaction.transaction);
				}
			}

			// Log


			#endregion transaction-based logic



			if(botransaction.commit())
			{
				//return (new GenerateEMailForOfferRequestByUIHandler(this._user, CommunRequestUI).Handle());
				return ResponseModel<string>.SuccessResponse(CommunRequestUI);
			}
			else
			{
				return ResponseModel<string>.FailureResponse(key: "1", value: "Transaction error");
			}
			// 4--> Send Email
			// 5--> save sent Email Version
			// 6--> update Request Status
			// 7--> update Email Status to sent

		} catch(Exception e)
		{
			botransaction.rollback();
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}
	private string GenerateEmailSubjectBasedOnDefaultSupplierLanguage(int supplierID, string ProjectName, string Customer)
	{

		if(supplierID == -1)
			return string.Empty;
		var fetchedIds = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.GetDefaultLanguageForSupplier(supplierID);
		if(fetchedIds is null || fetchedIds.ID is null || fetchedIds.ID == 0)
			fetchedIds = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.SupplierDefaultLanguageEntity() { ID = 4 };

		var emailBody = fetchedIds.ID switch
		{
			1 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailSubjetEN(ProjectName, Customer),
			2 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailSubjetFR(ProjectName, Customer),
			3 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailSubjetSP(ProjectName, Customer),
			_ => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailSubjetDE(ProjectName, Customer)
		};
		return emailBody;
	}
	public ResponseModel<string> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<string>.AccessDeniedResponse();
		}
		return ResponseModel<string>.SuccessResponse();
	}

	private Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity GenerateLogs(int Id, string ManNr, string SpplierName, string Status)
	{
		var log = new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
		{
			LastUpdateUserId = _user.Id,
			LastUpdateUsername = _user.Username,
			LastUpdateTime = DateTime.Now,
			LastUpdateUserFullName = _user.Name,
			LogDescription = $"The Offer request [ongoingStatus] has been updated to  {Status}",
			LogObjectId = Id,
			ManufacturerNumber = ManNr,
			SupplierContactName = SpplierName,
			LogObject = "BSD Offer Request"
		};
		return log;
	}
}
