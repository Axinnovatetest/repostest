using System;

namespace Psz.Core.Apps.Purchase.Handlers.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetSingleHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.CustomerService.OrderReport.CreateModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.RequestModel _data { get; set; }
		public GetSingleHandler(Identity.Models.UserModel user, Models.RequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<Models.CustomerService.OrderReport.CreateModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var orderReportEntity = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.GetByLanguageAndType(this._data.LanguageId, this._data.TypeId);
				if(orderReportEntity == null)
				{
					orderReportEntity = new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity
					{
						Id = -1,
						CompanyLogoImageId = -1,
						ImportLogoImageId = -1,
						Header = string.Empty,
						ItemsHeader = string.Empty,
						ItemsFooter1 = string.Empty,
						ItemsFooter2 = string.Empty,
						Footer11 = string.Empty,
						Footer12 = string.Empty,
						Footer13 = string.Empty,
						Footer14 = string.Empty,
						Footer15 = string.Empty,
						Footer16 = string.Empty,
						Footer17 = string.Empty,
						Footer21 = string.Empty,
						Footer22 = string.Empty,
						Footer23 = string.Empty,
						Footer24 = string.Empty,
						Footer25 = string.Empty,
						Footer26 = string.Empty,
						Footer27 = string.Empty,

						Footer31 = string.Empty,
						Footer32 = string.Empty,
						Footer33 = string.Empty,
						Footer34 = string.Empty,
						Footer35 = string.Empty,
						Footer36 = string.Empty,
						Footer37 = string.Empty,

						Footer41 = string.Empty,
						Footer42 = string.Empty,
						Footer43 = string.Empty,
						Footer44 = string.Empty,
						Footer45 = string.Empty,
						Footer46 = string.Empty,
						Footer47 = string.Empty,

						Footer51 = string.Empty,
						Footer52 = string.Empty,
						Footer53 = string.Empty,
						Footer54 = string.Empty,
						Footer55 = string.Empty,
						Footer56 = string.Empty,
						Footer57 = string.Empty,

						Footer61 = string.Empty,
						Footer62 = string.Empty,
						Footer63 = string.Empty,
						Footer64 = string.Empty,
						Footer65 = string.Empty,
						Footer66 = string.Empty,
						Footer67 = string.Empty,

						Footer71 = string.Empty,
						Footer72 = string.Empty,
						Footer73 = string.Empty,
						Footer74 = string.Empty,
						Footer75 = string.Empty,
						Footer76 = string.Empty,
						Footer77 = string.Empty,


						// PSZ Address
						Address1 = string.Empty,
						Address2 = string.Empty,
						Address3 = string.Empty,
						Address4 = string.Empty,

						// Document
						OrderNumberPO = string.Empty,
						DocumentType = string.Empty,
						OrderNumber = string.Empty,
						OrderDate = string.Empty,

						// Client
						ClientNumber = string.Empty,
						InternalNumber = string.Empty,
						ShippingMethod = string.Empty,
						PaymentMethod = string.Empty,
						PaymentTarget = string.Empty,

						UST_ID = string.Empty,

						// Items
						Position = string.Empty,
						Article = string.Empty,
						Description = string.Empty,
						Amount = string.Empty,
						PE = string.Empty,
						BasisPrice150 = string.Empty,
						Cu_G = string.Empty,
						Cu_Surcharge = string.Empty,
						UnitPrice = string.Empty,
						Designation = string.Empty,
						Unit = string.Empty,
						TotalPrice150 = string.Empty,
						DEL = string.Empty,
						Cu_Total = string.Empty,
						UnitTotal = string.Empty,

						//
						Bestellt = string.Empty,
						Geliefert = string.Empty,
						Liefertermin = string.Empty,
						Offen = string.Empty,

						Abladestelle = string.Empty,

						LastPageText1 = string.Empty,
						LastPageText2 = string.Empty,
						LastPageText3 = string.Empty,
						LastPageText4 = string.Empty,
						LastPageText5 = string.Empty,
						LastPageText6 = string.Empty,
						LastPageText7 = string.Empty,
						LastPageText8 = string.Empty,
						LastPageText9 = string.Empty,

						SummarySum = string.Empty,
						SummaryTotal = string.Empty,
						SummaryUST = string.Empty,

						LanguageId = this._data.LanguageId,
						OrderTypeId = this._data.TypeId,

						LastUpdateTime = DateTime.Now,
						LastUpdateUserId = this._user.Id,

						Lieferadresse = string.Empty,
						Index_Kunde = string.Empty
					};
					orderReportEntity.Id = Infrastructure.Data.Access.Tables.PRS.OrderReportAccess.Insert(orderReportEntity);
				}

				var responseBody = new Models.CustomerService.OrderReport.CreateModel(orderReportEntity);

				return ResponseModel<Models.CustomerService.OrderReport.CreateModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.CustomerService.OrderReport.CreateModel> Validate()
		{
			return ResponseModel<Models.CustomerService.OrderReport.CreateModel>.SuccessResponse();
		}
	}
}
