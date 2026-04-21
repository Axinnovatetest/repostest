using Psz.Core.MaterialManagement.Orders.Models.Wareneingang;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Wareneingang
{
	public class CreateReportAllHandler: IHandle<CreateReportRequestModel, ResponseModel<List<byte[]>>>
	{
		private UserModel _user { get; set; }
		public CreateReportRequestModel data { get; set; }
		public CreateReportAllHandler(Identity.Models.UserModel user, CreateReportRequestModel data)
		{
			this._user = user;
			this.data = data;
		}
		public ResponseModel<List<byte[]>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<byte[]> responseBody = new List<byte[]>();
				var wareneingangBestellteArtikels = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByBestellungNr(data.WareneingangId);
				var wareneingangData = new List<Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangReportEntity>();

				switch(data.ReportType)
				{
					case Enums.OrderEnums.WareneingangReportTypes.ESD_SCHUTZ:
						foreach(var item in wareneingangBestellteArtikels)
						{
							responseBody.Add(Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_ESD_SCHUTZ, null, item.Nr.ToString(), DateTime.Now.ToString()));
						}
						break;
					case Enums.OrderEnums.WareneingangReportTypes.PSZ_MHD_Etikett:
						foreach(var item in wareneingangBestellteArtikels)
						{
							//var bestellteArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(item.Nr);
							var artikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.Artikel_Nr ?? -1);
							if(artikel is null)
								continue;
							if(item.MhdDatumArtikel.HasValue)
							{
								data.MhdDate = item.MhdDatumArtikel.Value.ToString("dd.MM.yyyy");
							}
							else
							{
								continue;
							}
							if(!data.Watermark)
								responseBody.Add(Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_PSZ_MHD_Etikett, null, artikel.ArtikelNummer, item.MhdDatumArtikel.Value.ToString("dd.MM.yyyy")));
							else
								responseBody.Add(Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_PSZ_MHD_Etikett_WaterMark, null, artikel.ArtikelNummer, item.MhdDatumArtikel.Value.ToString("dd.MM.yyyy")));
						}
						break;
					case Enums.OrderEnums.WareneingangReportTypes.Wareneingang_Etikett:
						foreach(var item in wareneingangBestellteArtikels)
						{
							wareneingangData = Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.GetWareneingangData(data.WareneingangId, item.Lagerort_id ?? -1, item.Nr);
							responseBody.Add(Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_Etikett, wareneingangData, "", ""));
						}
						break;
					case Enums.OrderEnums.WareneingangReportTypes.Wareneingang_Report:

						wareneingangData = Infrastructure.Data.Access.Joins.MTM.Order.WareneingangAccess.GetWareneingangData(data.WareneingangId, wareneingangBestellteArtikels.Select(x => x.Lagerort_id).ToList(), wareneingangBestellteArtikels.Select(x => x.Nr).ToList());
						responseBody.Add(Module.ReportingService.GenerateWareneingangReport(Infrastructure.Services.Reporting.Helpers.ReportType.MTM_Wareneingang_Report, wareneingangData, "", ""));
						break;
					default:
						break;
				}

				return ResponseModel<List<byte[]>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<byte[]>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<byte[]>>.AccessDeniedResponse();
			}

			return ResponseModel<List<byte[]>>.SuccessResponse();
		}
	}
}
