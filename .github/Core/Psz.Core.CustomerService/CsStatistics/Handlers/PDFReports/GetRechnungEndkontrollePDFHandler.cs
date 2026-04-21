using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetRechnungEndkontrollePDFHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private RechnungEndkontrolleReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungEndkontrollePDFHandler(Identity.Models.UserModel user, RechnungEndkontrolleReportEntryModel data)
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
				var ReportData = new RechnungEndkontrolleReportModel();
				var rechnungEndkontrooleEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnungEndkontrlle(_data.From, _data.To);
				if(rechnungEndkontrooleEntity != null && rechnungEndkontrooleEntity.Count > 0)
				{
					ReportData = new RechnungEndkontrolleReportModel
					{
						Header = new List<RechnungEndkontrolleReportHeaderModel> { new RechnungEndkontrolleReportHeaderModel
					{
						From=_data.From.ToString("dd.MM.yyyy"),
						To=_data.To.ToString("dd.MM.yyyy"),
						DatePlus10=DateTime.Now.AddDays(10).ToString("dd.MM.yyyy"),
						RechnungDatum=_data.RechnungDatum.ToString("dd.MM.yyyy"),
						Rechnungnummer=_data.Rechnungnummer.ToString(),
					}},
						Details = rechnungEndkontrooleEntity.Select(r => new RechnungEndkontrolleReportDetailsModel(r)).ToList(),
					};
				}

				response = Module.CS_ReportingService.GenerateRechnungEndkontrolleReporteport(Enums.ReportingEnums.ReportType.CTS_RECHNUNGENDKONTROLLE, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(response);
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

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
