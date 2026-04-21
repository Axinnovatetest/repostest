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

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests
{
	public class EditOfferForSingleManufacturerNumberHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private EditOfferForSingleManufacturerNumberModel _data { get; set; }
		public EditOfferForSingleManufacturerNumberHandler(Identity.Models.UserModel user, EditOfferForSingleManufacturerNumberModel emailtoedit)
		{
			this._user = user;
			this._data = emailtoedit;
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
					Graduatedprice1 = _data.Graduatedprice1,
					Graduatedprice2 = _data.Graduatedprice2,
					Graduatedprice3 = _data.Graduatedprice3,
					Graduatedquantity1 = _data.Graduatedquantity1,
					Graduatedquantity2 = _data.Graduatedquantity2,
					Graduatedquantity3 = _data.Graduatedquantity3,
					YearlyQuantity = _data.YearlyQuantity,
					unit = _data.warengrupId,
					OfferRequestArticleDescription = _data.OfferRequestArticleDescription,
					ProjectName = _data.ProjectName,
					EndCustomer = _data.EndCustomer
				};
				
				var offerfamily = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.UpdateOfferForSingleManufacturerNumberWithTransaction(EntityToSave, botransaction.connection, botransaction.transaction);
				var requestUI = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.GetRequestUIById(EntityToSave.Id);
				var updateAllOFProjandCustomer = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.UpdateOfferCommunInformationsWithTransaction(requestUI, EntityToSave.ProjectName,EntityToSave.EndCustomer, botransaction.connection, botransaction.transaction);
				// Log
				var fetchedOffers = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(_data.Id);
				var LogSavingResult = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsAccess.InsertWithTransaction(GenerateLogs(_data.Id, fetchedOffers, EntityToSave), botransaction.connection, botransaction.transaction);

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

		private List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> GenerateLogs(int Id, Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsEntity Olddata, Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsEntity Newdata)
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
			var offerEnities = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(Id);


			if(Olddata.unit!= Newdata.unit)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request unit has been updated from [{Olddata.unit}] to  [{Newdata.unit}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);

			}

			if(Olddata.YearlyQuantity != Newdata.YearlyQuantity)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request YearlyQuantity has been updated from [{Olddata.YearlyQuantity}] to  [{Newdata.YearlyQuantity}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);

			}

			if(Olddata.Graduatedquantity3 != Newdata.Graduatedquantity3)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request Graduatedquantity3 has been updated from [{Olddata.Graduatedquantity3}] to  [{Newdata.Graduatedquantity3}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}

			if(Olddata.Graduatedquantity2 != Newdata.Graduatedquantity2)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request Graduatedquantity2 has been updated from [{Olddata.Graduatedquantity2}] to  [{Newdata.Graduatedquantity2}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}

			if(Olddata.Graduatedquantity1 != Newdata.Graduatedquantity1)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request Graduatedquantity1 has been updated from [{Olddata.Graduatedquantity1}] to  [{Newdata.Graduatedquantity1}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}

			if(Olddata.Graduatedprice3 != Newdata.Graduatedprice3)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request Graduatedprice3 has been updated from [{Olddata.Graduatedprice3}] to  [{Newdata.Graduatedprice3}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}

			if(Olddata.Graduatedprice2 != Newdata.Graduatedprice2)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request Graduatedprice2 has been updated from [{Olddata.Graduatedprice2}] to  [{Newdata.Graduatedprice2}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}

			if(Olddata.Graduatedprice1 != Newdata.Graduatedprice1)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request Graduatedprice1 has been updated from [{Olddata.Graduatedprice1}] to  [{Newdata.Graduatedprice1}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}

			if(Olddata.CustomQuantity != Newdata.CustomQuantity)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request CustomQuantity has been updated from [{Olddata.CustomQuantity}] to  [{Newdata.CustomQuantity}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					); 
			}

			if(Olddata.ProjectName != Newdata.ProjectName)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request ProjectName has been updated from [{Olddata.ProjectName}] to  [{Newdata.ProjectName}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}
			if(Olddata.EndCustomer != Newdata.EndCustomer)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request EndCustomer has been updated from [{Olddata.EndCustomer}] to  [{Newdata.EndCustomer}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}
			if(Olddata.OfferRequestArticleDescription != Newdata.OfferRequestArticleDescription)
			{
				logs.Add(

					new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity()
					{
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LogDescription = $"The Offer request Description has been updated from [{Olddata.OfferRequestArticleDescription}] to  [{Newdata.OfferRequestArticleDescription}]",
						LogObjectId = Id,
						ManufacturerNumber = offerEnities.ManufactuerNumber,
						SupplierContactName = offerEnities.SupplierContactName,
						LogObject = "BSD Offer Request"
					}
					);
			}
			return logs;
		}
	}

}
