using MoreLinq;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetLierferPlannungPDFHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLierferPlannungPDFHandler(Identity.Models.UserModel user, string data)
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
				var ReportData = new LieferPlannungReportModel();
				ReportData.Weeks = new List<LieferPlannungWeeksReportModel>();
				var lierferPlannungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLieferPlannung(_data);
				if(lierferPlannungEntity != null && lierferPlannungEntity.Count > 0)
				{
					ReportData.Details = lierferPlannungEntity.Select(a => new LieferPlannungDetailseportModel(a)).OrderBy(b => b.Jahr).ThenBy(c => c.KW).
						ThenBy(d => d.Vorname_NameFirma).ToList();
					var _years = lierferPlannungEntity.Select(y => y.Jahr).Distinct().ToList();

					ReportData.Years = _years.Select(y => new LieferPlannungYearsReportModel
					{
						Year = (int)y,
						Count = lierferPlannungEntity.Where(x => x.Jahr == y).ToList().Count
					}
					).ToList();

					var _weeks = lierferPlannungEntity.Select(w => w.KW).Distinct().OrderBy(x => x).ToList();
					foreach(var week in _weeks)
					{
						var _weekYears = lierferPlannungEntity.Where(w => w.KW == week.Value).ToList();
						foreach(var item in _weekYears)
						{
							ReportData.Weeks.Add(new LieferPlannungWeeksReportModel { Week = (int)week, Year = (int)item.Jahr });
						}
					}
					var DistinctWeeks = ReportData.Weeks.GroupBy(d => new { d.Week, d.Year })
									   .Select(m => new LieferPlannungWeeksReportModel { Week = m.Key.Week, Year = m.Key.Year }).ToList();
					ReportData.Weeks = DistinctWeeks;
					response = Module.CS_ReportingService.GenerateLieferPlannungReport(Enums.ReportingEnums.ReportType.CTS_LIEFERPLANNUNG, ReportData);
				}
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
