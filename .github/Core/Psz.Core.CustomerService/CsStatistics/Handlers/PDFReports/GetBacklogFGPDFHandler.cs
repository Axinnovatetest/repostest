using Infrastructure.Data.Entities.Joins.CTS;
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
	public class GetBacklogFGPDFHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private BacklogReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBacklogFGPDFHandler(Identity.Models.UserModel user, BacklogReportEntryModel data)
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
				var ReportData = new Psz.Core.CustomerService.Reporting.Models.BacklogFGReportModel();
				// - 2023-01-04 - allow all - Schremmer
				// - 2023-06-16 - allow none - Heidenreich
				var w = new Handlers.GetWarehouseHandler(this._user).Handle();
				var backLogFGEntity = GetData(this._data.Lager, _data.From, _data.To, w.Success ? w.Body.Select(x => x.Key)?.ToList() : null);


				if(backLogFGEntity != null && backLogFGEntity.Count > 0)
				{
					var _dataFinal = this._data.Lager is null
						? backLogFGEntity
						: backLogFGEntity.Where(a => !string.IsNullOrEmpty(a.Lagerort) && !string.IsNullOrWhiteSpace(a.Lagerort))?.OrderBy(b => b.Liefertermin ?? DateTime.MaxValue)?.ToList();
					var _prort = _dataFinal?.Select(a => a.PRORT)?.Distinct()?.ToList();
					ReportData.Header = _prort?.Select(a => new Psz.Core.CustomerService.Reporting.Models.BackLogReportHeaderModel(a, _dataFinal))?.ToList();
					ReportData.Details = _dataFinal?.Select(x => new Psz.Core.CustomerService.Reporting.Models.BacklogFGReportDetailsModel(x))?.ToList();

					ReportData.IsAdmin = _user.Access.CustomerService.StatsBacklogFGAdmin;
					//response = _user.Access.CustomerService.StatsBacklogFGAdmin ?/*_data.Code == 1 ?*/
					//	Module.CS_ReportingService.GenerateBackLogFGReport(Enums.ReportingEnums.ReportType.CTS_BACKLOGFG1, ReportData)
					//	: Module.CS_ReportingService.GenerateBackLogFGReport(Enums.ReportingEnums.ReportType.CTS_BACKLOGFG2, ReportData);
					response = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
					{
						BodyData = ReportData,
						BodyTemplate = "CTS_BacklogFG_Body",
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
					//await Psz.Core.CustomerService.Reporting.IText.GetBacklogFGReport(ReportData);
				}
				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				//Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				throw;
			}
		}
		public static List<BackLogFGEntity> GetData(int? lager, DateTime from, DateTime to, List<int> lagers)
		{
			if(lager != null && lager <= 0)
			{
				return Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetBacklogFG_Multi2(from, to, lagers);
			}
			else
			{
				return Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetBacklogFG(from, to, lager);
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
