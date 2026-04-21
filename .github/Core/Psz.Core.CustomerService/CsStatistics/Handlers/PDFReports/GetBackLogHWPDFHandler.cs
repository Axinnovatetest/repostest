using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetBackLogHWPDFHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private BacklogReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBackLogHWPDFHandler(Identity.Models.UserModel user, BacklogReportEntryModel data)
		{
			this._user = user;
			this._data = data;
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

				byte[] response = null;
				var ReportData = new List<BacklogHWReportDetailsModel>();
				var backLogFGEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetBacklogHW(_data.From, _data.To);
				if(backLogFGEntity != null && backLogFGEntity.Count > 0)
				{
					ReportData = backLogFGEntity.OrderBy(y => y.Liefertermin.Value).Select(x => new BacklogHWReportDetailsModel(x)).ToList();
					response = _user.Access.CustomerService.StatsBacklogHWAdmin /*_data.Code == 1*/ ?
						Module.CS_ReportingService.GenerateBackLogHWReport(Enums.ReportingEnums.ReportType.CTS_BACKLOGHW1, ReportData)
						: Module.CS_ReportingService.GenerateBackLogHWReport(Enums.ReportingEnums.ReportType.CTS_BACKLOGHW2, ReportData);
				}
				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
