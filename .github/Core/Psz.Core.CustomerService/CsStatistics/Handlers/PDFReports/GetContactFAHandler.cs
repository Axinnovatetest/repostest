using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetContactFAHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private IDateRangeModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetContactFAHandler(Identity.Models.UserModel user, IDateRangeModel data)
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
				var ReportData = new CSContactFAReportModel();
				var contactsFAEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetContactFA(_data.From, _data.To);
				if(contactsFAEntity != null && contactsFAEntity.Count > 0)
				{
					var _total = contactsFAEntity.Count;
					var _contacts = contactsFAEntity.Select(c => c.CS_Kontakt).Distinct().ToList();
					ReportData = new CSContactFAReportModel
					{
						Details = _contacts.Select(c => new CSContactFAReportDetailsModel
						{
							Contact = c,
							FACount = contactsFAEntity.Where(a => a.CS_Kontakt == c).ToList().Count,
							Percentage = (decimal)contactsFAEntity.Where(a => a.CS_Kontakt == c).ToList().Count / (decimal)_total,
						}).OrderBy(x => x.Contact).ToList(),
						Header = new List<CSContactFAReportHeaderModel>
						{
							new CSContactFAReportHeaderModel
							{
								DateFrom = _data.From.ToString("dd.MM.yyyy"),
								DateTo = _data.To.ToString("dd.MM.yyyy"),
								Total = _total
							}
						},
					};
				}

				response = Module.CS_ReportingService.GenerateContactsFAReport(Enums.ReportingEnums.ReportType.CTS_CONTACTSFA, ReportData);
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
