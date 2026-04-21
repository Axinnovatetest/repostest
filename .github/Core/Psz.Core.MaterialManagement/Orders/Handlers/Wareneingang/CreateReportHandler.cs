using Psz.Core.MaterialManagement.Orders.Models.Wareneingang;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Wareneingang
{
	public class CreateReportHandler: IHandle<CreateReportRequestModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		public CreateReportRequestModel data { get; set; }
		public CreateReportHandler(Identity.Models.UserModel user, CreateReportRequestModel data)
		{
			this._user = user;
			this.data = data;
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
				if(data.WareneingangId == 0 && data.WareneingangNr.HasValue)
				{
					var wareneinganag = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.WareneingangNr.Value);
					data.WareneingangId = wareneinganag.Bestellung_Nr.HasValue ? wareneinganag.Bestellung_Nr.Value : 0;
				}
				if(data.WareneingangId == 0)
				{
					return ResponseModel<byte[]>.FailureResponse("Wareneingang not found");
				}
				var wareneingangData = new List<Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangReportEntity>();
				switch(data.ReportType)
				{
					case Enums.OrderEnums.WareneingangReportTypes.ESD_SCHUTZ:
						responseBody = Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_ESD_SCHUTZ, null, data.BestellteArtikelId.ToString(), DateTime.Now.ToString());
						break;
					case Enums.OrderEnums.WareneingangReportTypes.PSZ_MHD_Etikett:
						var bestellteArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.BestellteArtikelId);
						var artikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bestellteArtikel.Artikel_Nr ?? -1);
						data.MhdDate = string.IsNullOrWhiteSpace(data.MhdDate) ?
							bestellteArtikel.MhdDatumArtikel.Value.ToString("dd.MM.yyyy") :
							DateTime.ParseExact(data.MhdDate, "MM/dd/yyyy", null).ToString("dd.MM.yyyy");
						var watermark = false;
						if(bestellteArtikel != null)
						{
							int artikelDays = 0;
							int.TryParse(artikel.Zeitraum_MHD, out artikelDays);
							watermark = DateTime.Today.AddDays(artikelDays) > bestellteArtikel.MhdDatumArtikel;
						}

						if(!data.Watermark && !watermark)
							responseBody = Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_PSZ_MHD_Etikett, null, artikel.ArtikelNummer, data.MhdDate);
						else
							responseBody = Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_PSZ_MHD_Etikett_WaterMark, null, artikel.ArtikelNummer, data.MhdDate);

						break;
					case Enums.OrderEnums.WareneingangReportTypes.Wareneingang_Etikett:
						wareneingangData = Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.GetWareneingangData(data.WareneingangId, data.LagerortId, data.BestellteArtikelId);
						responseBody = Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_Etikett, wareneingangData, "", "");
						break;
					case Enums.OrderEnums.WareneingangReportTypes.Wareneingang_Report:
						wareneingangData = Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.GetWareneingangData(data.WareneingangId, data.LagerortId, data.BestellteArtikelId);
						responseBody = Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_Report, wareneingangData, "", "");
						break;
					default:
						break;
				}

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			switch(data.ReportType)
			{
				case Enums.OrderEnums.WareneingangReportTypes.ESD_SCHUTZ:
					break;
				case Enums.OrderEnums.WareneingangReportTypes.PSZ_MHD_Etikett:
					var bestellteArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.BestellteArtikelId);
					var artikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bestellteArtikel.Artikel_Nr ?? -1);
					if(artikel is null)
						return ResponseModel<byte[]>.FailureResponse("Artikel not found");
					if(string.IsNullOrWhiteSpace(data.MhdDate))
					{
						if(!bestellteArtikel.MhdDatumArtikel.HasValue)
						{
							return ResponseModel<byte[]>.FailureResponse(value: "No MHD Date was found");
						}
					}
					break;
				case Enums.OrderEnums.WareneingangReportTypes.Wareneingang_Etikett:
					break;
				case Enums.OrderEnums.WareneingangReportTypes.Wareneingang_Report:
					break;
				default:
					break;
			}
			//var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data);
			//if(bestellung == null)
			//	return ResponseModel<byte[]>.FailureResponse("Order Not Found");

			//var orderData = Infrastructure.Data.Access.Joins.MTM.Order.OrderValidationAccess.GetReportData(data);
			//if(orderData == null || orderData.Count == 0)
			//	return ResponseModel<byte[]>.FailureResponse("PDF Data Not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
