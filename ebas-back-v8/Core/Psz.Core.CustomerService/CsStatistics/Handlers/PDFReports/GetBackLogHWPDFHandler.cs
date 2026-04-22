using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetBackLogHWPDFHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private BacklogReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBackLogHWPDFHandler(Identity.Models.UserModel user, BacklogReportEntryModel data)
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
				var ReportData = new BacklogHWReportModel();
				var backLogFGEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetBacklogHW(_data.From, _data.To);
				if(backLogFGEntity != null && backLogFGEntity.Count > 0)
				{
					ReportData.Details = backLogFGEntity.OrderBy(y => y.Liefertermin.Value).Select(x => new BacklogHWReportDetailsModel(x)).ToList();
					ReportData.IsAdmin = _user.Access.CustomerService.StatsBacklogHWAdmin;

					response = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
					{
						BodyData = ReportData,
						BodyTemplate = "CTS_BacklogHW_Body",
						DocumentTitle = "",
						FooterCenterText = "",
						FooterData = null,
						FooterLeftText = DateTime.Now.ToString("dd.MM.yyyy"),
						FooterTemplate = "CTS_Footer",
						HasFooter = true,
						FooterWithCounter = true,
						HasHeader = false,
						HeaderFirstPageOnly = false,
						HeaderLogoWithCounter = false,
						HeaderLogoWithText = false,
						HeaderText = "",
						Logo = null,
						Rotate = true
					});
					//_user.Access.CustomerService.StatsBacklogHWAdmin ?
					//Module.CS_ReportingService.GenerateBackLogHWReport(Enums.ReportingEnums.ReportType.CTS_BACKLOGHW1, ReportData)
					//: Module.CS_ReportingService.GenerateBackLogHWReport(Enums.ReportingEnums.ReportType.CTS_BACKLOGHW2, ReportData);
				}
				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
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
