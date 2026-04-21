using Psz.Core.Common.Models;
using Psz.Core.Logistics.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetLSDruckHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private long _ls { get; set; }
		private int _languageId { get; set; }
		private int _orderTypeId { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLSDruckHandler(Identity.Models.UserModel user, int ls, int languageId, int orderTypeId)
		{
			this._user = user;
			this._ls = ls;
			this._languageId = languageId;
			this._orderTypeId = orderTypeId;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] responseBody = null;
				var lieferscheineHeaderEntity = Infrastructure.Data.Access.Tables.Logistics.LSAccess.GetLS(this._ls);
				var lieferscheineDetailsEntity = Infrastructure.Data.Access.Tables.Logistics.LSAccess.GetDetailsLS(this._ls);
				if(lieferscheineDetailsEntity != null && lieferscheineDetailsEntity.Count() > 0)
				{
					lieferscheineHeaderEntity.ablastelle = lieferscheineDetailsEntity[0].ablastelle;
					lieferscheineHeaderEntity.ust = lieferscheineDetailsEntity[0].ust;
					lieferscheineHeaderEntity.rp = lieferscheineDetailsEntity[0].rp;
				}

				var lieferscheineHeadersEntity = new List<Infrastructure.Data.Entities.Tables.Logistics.LSEntity>();
				lieferscheineHeadersEntity.Add(lieferscheineHeaderEntity);
				//---------------------Footer----------------------------------------------------------
				// get order template data
				var rechnungReportEntity = Infrastructure.Data.Access.Tables.Logistics.OrderReportAccess.GetByLanguageAndType(this._languageId, _orderTypeId);
				if(rechnungReportEntity == null)
				{
					rechnungReportEntity = new Infrastructure.Data.Entities.Tables.Logistics.OrderReportEntity
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

						ShippingMethod = string.Empty,


						// Items
						Position = string.Empty,
						Article = string.Empty,
						Description = string.Empty,




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

						LanguageId = this._languageId,
						OrderTypeId = this._orderTypeId,

						LastUpdateTime = DateTime.Now,
						LastUpdateUserId = this._user.Id,

						Lieferadresse = string.Empty,

					};
					rechnungReportEntity.Id = Infrastructure.Data.Access.Tables.Logistics.OrderReportAccess.Insert(rechnungReportEntity);
				}
				var rechnungReportModel = new LSDruckFooterReportModel(rechnungReportEntity);
				var listRechnungReportModel = new List<LSDruckFooterReportModel>();
				listRechnungReportModel.Add(rechnungReportModel);
				//---------------------Fin Footer------------------------------------------------------

				var order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByAngebotNr((int)lieferscheineHeaderEntity.angeboteNr);
				var customer = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(order?.Kunden_Nr ?? -1);
				bool poBarCode = lieferscheineHeaderEntity?.lVornameNameFirma?.Trim()?.ToLower() == "sirona dental systems gmbh" || (customer?.CodeTypeInLSId.HasValue == true && customer?.CodeTypeInLSId.Value == (int)Psz.Core.BaseData.Enums.AddressEnums.LSCodeTypes.Barcode);
				bool docNoBarCode = lieferscheineHeaderEntity?.lVornameNameFirma?.Trim()?.ToLower() == "sirona dental systems gmbh" || customer?.LsBarCodeDocumentNumber == true;
				var HEADER = lieferscheineHeadersEntity?.Select(x => new Infrastructure.Services.Reporting.Models.Logistics.LSDruckHeaderReportModel(x, poBarCode, docNoBarCode)).ToList();
				var POSITIONS = lieferscheineDetailsEntity?.Select(x => new Infrastructure.Services.Reporting.Models.Logistics.LSDruckDetailsReportModel(x)).ToList();
				var INVOICEFIELDS = listRechnungReportModel.ToList();




				responseBody = Module.Logistic_ReportingService.GenerateLSDruckReport(Enums.ReportingEnums.ReportType.LSDRUCKER,
			 new LSReportingModel() { Headers = HEADER, Details = POSITIONS, InvoiceFields = INVOICEFIELDS });


				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.Logistics.LSAccess.GetLS(this._ls) == null)
				return ResponseModel<byte[]>.FailureResponse("LS not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
		public Bitmap FromBytesToBitmap(byte[] src)
		{
			Bitmap bmp;
			using(var ms = new MemoryStream(src))
			{
				bmp = new Bitmap(ms);
			}
			return bmp;
		}
	}
}