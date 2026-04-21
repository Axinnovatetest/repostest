using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class AuswertungSchneidereiHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AuswertungSchneidereiHandler(Identity.Models.UserModel user, int data)
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
				var ReportData = new AuswertungSchneidereiReportModel();

				var _table = GetTableByLager(_data);
				var AuswertungSchneidereiEntity = _data == -1
					? Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetAuswertungSchneidereiAll()
					: Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetAuswertungSchneiderei(_table);
				if(AuswertungSchneidereiEntity != null && AuswertungSchneidereiEntity.Count > 0)
				{
					ReportData = new AuswertungSchneidereiReportModel
					{
						Header = new AuswertungSchneidereiHeaderReport
						{
							Lager = GetLagerNameByLagerId(_data),
							LagerId = _data,
						},
						Details = AuswertungSchneidereiEntity.Select(a => new AuswertungSchneidereiDetailsReport
						{
							Datum_Bis = a.Datum_Bis,
							KG = a.KG ?? 0,
							KNG = a.KNG ?? 0,
							KGesamt = a.KGesamt ?? 0,
							Woche = a.Woche,
							Percent = a.KGesamt.HasValue ? ((decimal)a.KG.Value / (decimal)a.KGesamt.Value) * 100 : 0m,
						}).OrderBy(x => x.Woche).ToList(),
					};
				}
				response = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
				{
					BodyData = ReportData,
					BodyTemplate = "CTS_Auswertungschneiderei_Body",
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
					HeaderText = "Auswertung Schneiderei",
					Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}",
					Rotate = false
				});
				//Module.CS_ReportingService.GenerateAuswertungSchneidereiReport(Enums.ReportingEnums.ReportType.CTS_AUSWERTUNGSCHNEIDEREI, ReportData);
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

		public string GetTableByLager(int lager)
		{
			switch((Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)lager)
			{
				case Common.Enums.ArticleEnums.ArticleProductionPlace.AL:
					return "Auswertung_Albanien_Schneiderei";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.TN:
					return "Auswertung_Tunesien_Schneiderei";
				//case Common.Enums.ArticleEnums.ArticleProductionPlace.BETN:
				//	return "Auswertung_Schneiderei_BETN";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.WS:
					return "Auswertung_KHTN_Schneiderei";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.DE:
					return "Auswertung_Deutschland_Schneiderei";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.CZ:
					return "Auswertung_Eigenfertigung_Schneiderei";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN:
					return "Auswertung_Schneiderei_GZTN";
				default:
					return "";
			}
		}
		public string GetLagerNameByLagerId(int lager)
		{
			switch((Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)lager)
			{
				case Common.Enums.ArticleEnums.ArticleProductionPlace.AL:
					return "Albanien";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.TN:
					return "Tunesien";
				//case Common.Enums.ArticleEnums.ArticleProductionPlace.BETN:
				//	return "Tunesien";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.WS:
					return "WS";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.DE:
					return "Vohenstrauss";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.CZ:
					return "Eigenfertigung";
				case Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN:
					return "Tunesien";
				default:
					return "";
			}
		}
	}
}
