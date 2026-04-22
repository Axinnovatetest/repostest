using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetContactFAHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private IDateRangeModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetContactFAHandler(Identity.Models.UserModel user, IDateRangeModel data)
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
							Percentage = ((decimal)contactsFAEntity.Where(a => a.CS_Kontakt == c).ToList().Count / (decimal)_total) * 100,
						}).OrderBy(x => x.Contact).ToList(),
						Header = new CSContactFAReportHeaderModel
						{
							DateFrom = _data.From.ToString("dd.MM.yyyy"),
							DateTo = _data.To.ToString("dd.MM.yyyy"),
							Total = _total
						},
					};
				}

				response = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
				{
					BodyData = ReportData,
					BodyTemplate = "CTS_ContactFA_Body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterData = null,
					FooterLeftText = DateTime.Now.ToString("dd.MM.yyyy"),
					FooterTemplate = "CTS_Footer",
					FooterWithCounter = true,
					HasFooter = true,
					HasHeader = true,
					HeaderFirstPageOnly = true,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = true,
					HeaderText = "Erfasste / erstellte  Fertigung áufträge nack Mitarbeiter",
					Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}",
					Rotate = false
				});
				//Module.CS_ReportingService.GenerateContactsFAReport(Enums.ReportingEnums.ReportType.CTS_CONTACTSFA, ReportData);
				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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
