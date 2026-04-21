using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetNachBerechnungTNReportHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private RechnungEndkontrolleReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetNachBerechnungTNReportHandler(Identity.Models.UserModel user, RechnungEndkontrolleReportEntryModel data)
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
				var ReportData = new NachBerechnungTNReportModel();
				var nachBerechnungTNEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetNachBerechnungTN(_data.From, _data.To);
				if(nachBerechnungTNEntity != null && nachBerechnungTNEntity.Count > 0)
				{
					ReportData = new NachBerechnungTNReportModel
					{
						Header =
						new NachBerechnungTNHeaderReportModel
						{
							From = _data.From.ToString("dd.MM.yyyy"),
							To = _data.To.ToString("dd.MM.yyyy"),
							RechnungDatum = _data.RechnungDatum.ToString("dd.MM.yyyy"),
							Rechnungnummer = _data.Rechnungnummer.ToString(),
							Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetTN()?.Logo)}",
						},
						Details = nachBerechnungTNEntity.Select(n => new NachBerechnungTNReportDetailsModel(n)).OrderBy(x => x.Fertigungsnummer).ToList(),
					};
				}
				response = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
				{
					BodyData = ReportData,
					BodyTemplate = "CTS_NachBerechnungTN_Body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterData = null,
					FooterLeftText = "",
					FooterTemplate = "CTS_Footer",
					FooterWithCounter = false,
					HasFooter = false,
					HasHeader = false,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = null,
					Rotate = false
				});
				//Module.CS_ReportingService.GenerateNachBerechnungTNReporteport(Enums.ReportingEnums.ReportType.CTS_NACHBERECHNUNGTN, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public async Task<ResponseModel<byte[]>> ValidateAsync()
		{
			if(this._user == null)
			{
				return await ResponseModel<byte[]>.AccessDeniedResponseAsync();
			}

			return await ResponseModel<byte[]>.SuccessResponseAsync();
		}
	}
}
