using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetNachBerechnungTNReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private RechnungEndkontrolleReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetNachBerechnungTNReportHandler(Identity.Models.UserModel user, RechnungEndkontrolleReportEntryModel data)
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
				var ReportData = new NachBerechnungTNReportModel();
				var nachBerechnungTNEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetNachBerechnungTN(_data.From, _data.To);
				if(nachBerechnungTNEntity != null && nachBerechnungTNEntity.Count > 0)
				{
					ReportData = new NachBerechnungTNReportModel
					{
						Header = new List<NachBerechnungTNHeaderReportModel>
					{
						new NachBerechnungTNHeaderReportModel
						{
							From=_data.From.ToString("dd.MM.yyyy"),
							To=_data.To.ToString("dd.MM.yyyy"),
							RechnungDatum=_data.RechnungDatum.ToString("dd.MM.yyyy"),
							Rechnungnummer=_data.Rechnungnummer.ToString(),
						}
					},
						Details = nachBerechnungTNEntity.Select(n => new NachBerechnungTNReportDetailsModel(n)).OrderBy(x => x.Fertigungsnummer).ToList(),
					};
				}
				response = Module.CS_ReportingService.GenerateNachBerechnungTNReporteport(Enums.ReportingEnums.ReportType.CTS_NACHBERECHNUNGTN, ReportData);

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
