using Infrastructure.Services.Reporting.Models.CTS;
using MoreLinq;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetLierferPlannungPDFHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLierferPlannungPDFHandler(Identity.Models.UserModel user, string data)
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
				var ReportData = new Psz.Core.CustomerService.Reporting.Models.LieferPlannungReportModel();
				//ReportData.Weeks = new List<Infrastructure.Services.Reporting.Models.CTS.LieferPlannungWeeksReportModel>();
				var lierferPlannungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLieferPlannung(_data);
				if(lierferPlannungEntity != null && lierferPlannungEntity.Count > 0)
				{
					//ReportData.Details = lierferPlannungEntity.Select(a => new Infrastructure.Services.Reporting.Models.CTS.LieferPlannungDetailseportModel(a)).OrderBy(b => b.Jahr).ThenBy(c => c.KW).
					//	ThenBy(d => d.Vorname_NameFirma).ToList();
					//var _years = lierferPlannungEntity.Select(y => y.Jahr).Distinct().ToList();

					//ReportData.Years = _years.Select(y => new Infrastructure.Services.Reporting.Models.CTS.LieferPlannungYearsReportModel
					//{
					//	Year = (int)y,
					//	Count = lierferPlannungEntity.Where(x => x.Jahr == y).ToList().Count
					//}
					//).ToList();

					//var _weeks = lierferPlannungEntity.Select(w => w.KW).Distinct().OrderBy(x => x).ToList();
					//foreach(var week in _weeks)
					//{
					//	var _weekYears = lierferPlannungEntity.Where(w => w.KW == week.Value).ToList();
					//	foreach(var item in _weekYears)
					//	{
					//		ReportData.Weeks.Add(new Infrastructure.Services.Reporting.Models.CTS.LieferPlannungWeeksReportModel { Week = (int)week, Year = (int)item.Jahr });
					//	}
					//}
					//var DistinctWeeks = ReportData.Weeks.GroupBy(d => new { d.Week, d.Year })
					//				   .Select(m => new Infrastructure.Services.Reporting.Models.CTS.LieferPlannungWeeksReportModel { Week = m.Key.Week, Year = m.Key.Year }).ToList();
					//ReportData.Weeks = DistinctWeeks;
					ReportData = new Psz.Core.CustomerService.Reporting.Models.LieferPlannungReportModel
					{
						Data = lierferPlannungEntity
					   .GroupBy(r => r.Jahr)
					   .Select(g => new Psz.Core.CustomerService.Reporting.Models.DataItem
					   {
						   Year = g.Key ?? 0,
						   Items = g
							   .GroupBy(x => x.KW)
							   .Select(wg => new Psz.Core.CustomerService.Reporting.Models.Item
							   {
								   Week = wg.Key ?? 0,
								   Details = wg.Select(d => new Psz.Core.CustomerService.Reporting.Models.LieferPlannungDetailsReportModel(d)).ToList()
							   }).OrderBy(y => y.Week).ToList()
					   }).ToList()
					};
				}
				response = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = ReportData,
					BodyTemplate = "CTS_LieferPlannung_Body",
					FooterLeftText = DateTime.Now.ToString("dd.MM.yyyy"),
					DocumentTitle = null,
					FooterCenterText = "",
					HasFooter = true,
					FooterData = null,
					FooterTemplate = "CTS_Footer",
					FooterWithCounter = true,
					HasHeader = true,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = true,
					HeaderText = "Übersicht Lieferplannung",
					Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}",
					HeaderFirstPageOnly = true
				});
				//await Psz.Core.CustomerService.Reporting.IText.GetLieferPlannungReport(ReportData);
				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
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
