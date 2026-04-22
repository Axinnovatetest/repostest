using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetRechnungROHHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private RechnungEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungROHHandler(Identity.Models.UserModel user, RechnungEntryModel data)
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
				var ReportData = new RechnungROHModel();
				var _lager = Enums.OrderEnums.GetLagerNumber((Enums.OrderEnums.KapazitatLager)_data.Lager);
				var roh1 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnungROH_1(_data.From, _data.To, _lager);
				var roh2 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnungROH_2(_data.From, _data.To, _lager);

				var rohEntity = roh1?.Concat(roh2 ?? new List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHEntity> { }).ToList();

				if(rohEntity != null && rohEntity.Count > 0)
				{
					var _zollatarif = rohEntity.Select(z => z.Zolltarif_nr).Distinct().ToList();
					ReportData = new RechnungROHModel
					{
						Header = new List<RechnungROHHeaderModel>
						{
							new RechnungROHHeaderModel
							{
								From=_data.From.ToString("dd-MM-yyyy"),
								To=_data.To.ToString("dd-MM-yyyy"),
								RechnungDatum=_data.RechnungsDatum.HasValue?_data.RechnungsDatum.Value.ToString("dd-MM-yyyy"):"",
							}
						},
						Details = _zollatarif.OrderBy(x => x).Select(a => new RechnungROHDetailsModel
						{
							Warenummer = a,
							Anzahl = rohEntity.Where(x => x.Zolltarif_nr == a).Sum(y => y.Eingangsmenge ?? 0),
							Gewicht = rohEntity.Where(x => x.Zolltarif_nr == a).Sum(y => y.Gewicht ?? 0),
							WarenWert = rohEntity.Where(x => x.Zolltarif_nr == a).Sum(y => y.Wert ?? 0),
						}).ToList(),
					};
				}
				response = Module.CS_ReportingService.GenerateRechnungROHReport(Enums.ReportingEnums.ReportType.CTS_RECHNUNGROH, ReportData);
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
