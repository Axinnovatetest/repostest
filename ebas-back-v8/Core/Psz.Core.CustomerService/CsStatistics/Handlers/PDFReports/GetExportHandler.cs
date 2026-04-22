using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetExportHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private ExportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetExportHandler(Identity.Models.UserModel user, ExportEntryModel data)
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

				var reportData = new ExportReportModel();
				var exportEntity = new List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity>();
				string title = "";
				byte[] responseBody = null;

				int grouping = 0;
				if(_data.SortByArticle.HasValue && _data.SortByArticle.Value)
					grouping = 1;
				if(_data.SortByDate.HasValue && _data.SortByDate.Value)
					grouping = 2;

				switch(_data.Lager)
				{
					case (int)Enums.LagerEnums.KapazitatLager.CZ:
						exportEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetExportCZ(_data.From, _data.To, _data.Artikel, grouping);
						title = Enums.LagerEnums.KapazitatLager.CZ.GetDescription();
						break;
					case (int)Enums.LagerEnums.KapazitatLager.TN:
						exportEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetExportTN(_data.From, _data.To, _data.Artikel);
						title = Enums.LagerEnums.KapazitatLager.TN.GetDescription();
						break;
					case (int)Enums.LagerEnums.KapazitatLager.AL:
						exportEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetExportAL(_data.From, _data.To, _data.Artikel, grouping);
						title = Enums.LagerEnums.KapazitatLager.AL.GetDescription();
						break;
					case (int)Enums.LagerEnums.KapazitatLager.WS:
						exportEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetExportKHTN(_data.From, _data.To, _data.Artikel);
						title = Enums.LagerEnums.KapazitatLager.WS.GetDescription();
						break;
					case (int)Enums.LagerEnums.KapazitatLager.BETN:
						exportEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetExportBETN(_data.From, _data.To, _data.Artikel);
						title = Enums.LagerEnums.KapazitatLager.BETN.GetDescription();
						break;
					case (int)Enums.LagerEnums.KapazitatLager.GZTN:
						exportEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetExportGZTN(_data.From, _data.To, _data.Artikel);
						title = Enums.LagerEnums.KapazitatLager.GZTN.GetDescription();
						break;
					default:
						break;
				}


				if(exportEntity != null && exportEntity.Count > 0)
				{
					reportData.Header = new ExportReportHeaderModel
					{
						Von = _data.From.Value.ToString("dd.MM.yyyy"),
						Bis = _data.To.Value.ToString("dd.MM.yyyy"),
						SummGesamtpreis = exportEntity.Sum(a => a.Gesamtpreis ?? 0),
						Title = title,
					};
					reportData.Details = exportEntity.Select(x => new ExportReportDetailsModel(x)).ToList();
				}
				responseBody = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
				{
					BodyData = reportData,
					BodyTemplate = "CTS_Export_Body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterLeftText = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
					FooterData = null,
					FooterTemplate = "CTS_Footer",
					FooterWithCounter = true,
					HasFooter = true,
					HasHeader = false,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = null,
					Rotate = true
				});
				//Module.CS_ReportingService.GenerateExportReport(Enums.ReportingEnums.ReportType.CTS_EXPORT, reportData);

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
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
