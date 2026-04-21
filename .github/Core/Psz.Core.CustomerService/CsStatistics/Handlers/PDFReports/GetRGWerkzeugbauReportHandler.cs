using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetRGWerkzeugbauReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private RechnungEndkontrolleReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRGWerkzeugbauReportHandler(Identity.Models.UserModel user, RechnungEndkontrolleReportEntryModel data)
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
				var ReportData = new RechnungModel();
				var RGWerkzeugbauEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRGWerkzeugbau(_data.From, _data.To);
				var RechnungReportParametersEntity = Infrastructure.Data.Access.Tables.CTS.RechnungReportingAccess.GetByLagerIdAndType(0, "RGWerkzeugbau");
				if(RGWerkzeugbauEntity != null && RGWerkzeugbauEntity.Count > 0)
				{
					var _zollatarif = RGWerkzeugbauEntity.Select(z => z.Zolltarif_nr).Distinct().ToList();
					ReportData = new RechnungModel
					{
						ReportParameters = new List<RechnungHeaderModel>
						{
						   new RechnungHeaderModel(RechnungReportParametersEntity,new RechnungEntryModel{ From=_data.From,To=_data.To,RechnungsDatum=_data.RechnungDatum},_data.Rechnungnummer),
						},
						Zollatarif = _zollatarif.OrderBy(s => s).Select(z => new RechnungGroupedZollaTarif
						{
							Zolltarif_nummer = z,
							Gewicht = RGWerkzeugbauEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Gesamtgewicht / 1000) ?? 0,
							Positionen = RGWerkzeugbauEntity.Where(x => x.Zolltarif_nr == z).ToList().Count,
							Lohnleistun = RGWerkzeugbauEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Ausdr3) ?? 0,
							Zusatzkosten = 0,
							Material = RGWerkzeugbauEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Material1) ?? 0,
							Stat_Wert = RGWerkzeugbauEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Material1 + a.Ausdr3) ?? 0,
						}).ToList(),
						Details = RGWerkzeugbauEntity.Select(r => new RechnungDetailsModel(r)).ToList(),
					};
				}
				response = Module.CS_ReportingService.GenerateRechnungReport(Enums.ReportingEnums.ReportType.CTS_RG, ReportData);

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
