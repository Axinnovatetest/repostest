using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetRGSpritzgussReportHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private RechnungEndkontrolleReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRGSpritzgussReportHandler(Identity.Models.UserModel user, RechnungEndkontrolleReportEntryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<byte[]>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] response = null;
				var ReportData = new RechnungModel();
				var RGSpritzgussEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRGSpritzguss(_data.From, _data.To);
				var RechnungReportParametersEntity = Infrastructure.Data.Access.Tables.CTS.RechnungReportingAccess.GetByLagerIdAndType(0, "RGSpritzguss");
				if(RGSpritzgussEntity != null && RGSpritzgussEntity.Count > 0)
				{
					var _zollatarif = RGSpritzgussEntity.Select(z => z.Zolltarif_nr).Distinct().ToList();
					ReportData = new RechnungModel
					{
						ReportParameters = new RechnungHeaderModel(RechnungReportParametersEntity, new RechnungEntryModel { From = _data.From, To = _data.To, RechnungsDatum = _data.RechnungDatum }, _data.Rechnungnummer),
						Zollatarif = _zollatarif.OrderBy(s => s).Select(z => new RechnungGroupedZollaTarif
						{
							Zolltarif_nummer = z,
							Gewicht = RGSpritzgussEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Gesamtgewicht / 1000) ?? 0,
							Positionen = RGSpritzgussEntity.Where(x => x.Zolltarif_nr == z).ToList().Count,
							Lohnleistun = RGSpritzgussEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Ausdr3) ?? 0,
							Zusatzkosten = 0,
							Material = RGSpritzgussEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Material1) ?? 0,
							Stat_Wert = RGSpritzgussEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Material1 + a.Ausdr3) ?? 0,
						}).ToList(),
						Details = RGSpritzgussEntity.Select(r => new RechnungDetailsModel(r)).ToList(),
					};
				}
				response = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
				{
					BodyData = ReportData,
					BodyTemplate = "CTS_RGSpritzguss_Body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterLeftText = "",
					FooterData = new Reporting.Models.CTS_StatsRG_DocFooterModel
					{
						Footer1 = ReportData.ReportParameters.Footer1,
						Footer2 = ReportData.ReportParameters.Footer2,
						Footer3 = ReportData.ReportParameters.Footer3,
						Footer4 = ReportData.ReportParameters.Footer4,
						Footer5 = ReportData.ReportParameters.Footer5,
						Footer6 = ReportData.ReportParameters.Footer6,
						Footer7 = ReportData.ReportParameters.Footer7,
						Footer8 = ReportData.ReportParameters.Footer8,
						Footer9 = ReportData.ReportParameters.Footer9,
						Footer10 = ReportData.ReportParameters.Footer10,
						Footer11 = ReportData.ReportParameters.Footer11,
						Footer12 = ReportData.ReportParameters.Footer12,
					},
					FooterTemplate = "CTS_STATS_RG_Footer",
					FooterWithCounter = false,
					HasFooter = true,
					HasHeader = false,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = null,
				});
				//Module.CS_ReportingService.GenerateRechnungReport(Enums.ReportingEnums.ReportType.CTS_RG, ReportData);
				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public async Task<ResponseModel<byte[]>> ValidateAsync()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return await ResponseModel<byte[]>.AccessDeniedResponseAsync();
			}

			return await ResponseModel<byte[]>.SuccessResponseAsync();
		}
	}
}