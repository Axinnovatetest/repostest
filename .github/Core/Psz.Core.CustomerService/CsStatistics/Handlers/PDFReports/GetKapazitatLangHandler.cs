using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetKapazitatLangHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private KapazitatLangEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetKapazitatLangHandler(KapazitatLangEntryModel data, Identity.Models.UserModel user)
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
				byte[] responseBody = null;
				var reportData = new KapazitatLangReprotModel();
				var lagers = new List<int> { };
				var kapazitatEntity = new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatLangEntity>();
				if(!_data.Warehouse.HasValue || _data.Warehouse.Value <= 0)
				{
					var w = new Handlers.GetWarehouseHandler(this._user).Handle();
					lagers = w.Success ? w.Body.Select(x => x.Key)?.ToList() : new List<int> { _data.Warehouse ?? -1 };
					kapazitatEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetKapazitatLang(_data.From, _data.To, lagers, _data.ClientCode, _data.AT_PD);
				}
				else
				{
					lagers = new List<int> { _data.Warehouse ?? -1 };
					kapazitatEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetKapazitatLang(_data.From, _data.To, lagers, _data.ClientCode, _data.AT_PD);
				}
				if(kapazitatEntity != null && kapazitatEntity.Count > 0)
				{
					var lagerEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForFAStcklist(lagers);
					var _clients = kapazitatEntity.Select(a => a.Kunde).Distinct().ToList();
					var SUMS = new List<KapazitatReportDetailsSumsModel> { };
					reportData.Clients = _clients.Select(x => new KapazitatLangReportClientsModel(x, kapazitatEntity.Where(y => y.Kunde == x).Count())).Distinct().OrderBy(z => z.Kunde).ToList();
					reportData.Details = kapazitatEntity.Select(a => new KapzitatLangDetailsModel(a, _data.AT_PD)).ToList();
					foreach(var item in reportData.Clients)
					{
						var _detialsClient = reportData.Details.Where(x => x.Kunde == item.Kunde).ToList();
						if(_detialsClient != null && _detialsClient.Count > 0)
						{
							var _sumEuro = _detialsClient.Sum(a => double.TryParse(a.LohnkostLohnkosten.Replace(" €", ""), out var x) ? x : 0);
							var _sumZeit = _detialsClient.Sum(a => double.TryParse(a.Auftragszeit, out var x) ? x : 0);
							var _sumMA = _detialsClient.Sum(a => double.TryParse(a.MA, out var x) ? x : 0);
							var _sumtotalZeit = reportData.Details.Sum(a => double.TryParse(a.Auftragszeit, out var x) ? x : 0);
							var _percent = Math.Round((_sumZeit / _sumtotalZeit) * 100, 1);
							SUMS.Add(new KapazitatReportDetailsSumsModel
							{
								Kunde = item.Kunde,
								SumEuro = Convert.ToDecimal(_sumEuro),
								SumMA = Convert.ToDecimal(_sumMA),
								SumZeit = Convert.ToDecimal(_sumZeit),
								Percent = _percent.ToString() + "%",
							});
						}
					}
					//header
					var _sumAllZeit = SUMS.Sum(a => a.SumZeit);
					var _sumAllMA = SUMS.Sum(a => a.SumMA);
					var _sumAllEuro = SUMS.Sum(a => a.SumEuro);
					reportData.Header = new KapzitatLangReportHeaderModel
					{
						Von = _data.From.HasValue ? _data.From.Value.ToString("dd.MM.yyyy") : "",
						Bis = _data.To.HasValue ? _data.To.Value.ToString("dd.MM.yyyy") : "",
						Lagerort = lagerEntity?[0].Lagerort,
						Tage = _data.AT_PD.HasValue ? _data.AT_PD.Value.ToString() : "",
						SumEuro = _sumAllEuro.ToString() + " €",
						SumMA = _sumAllMA.ToString(),
						SumZeit = _sumAllZeit.ToString()
					};
				}
				responseBody = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
				{
					BodyData = reportData,
					BodyTemplate = "CTS_KapazitatLang_Body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterData = null,
					FooterLeftText = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
					HasFooter = true,
					FooterTemplate = "CTS_Footer",
					FooterWithCounter = true,
					HasHeader = true,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = null,
					Rotate = false
				});
				//Module.CS_ReportingService.GenerateKapazitatLangReport(Enums.ReportingEnums.ReportType.CTS_KAPAZITATLANG, reportData);

				return ResponseModel<byte[]>.SuccessResponse(responseBody);

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
			if(_data.AT_PD.HasValue && _data.AT_PD <= 0)
				return await ResponseModel<byte[]>.FailureResponseAsync($"invalid work days");
			return await ResponseModel<byte[]>.SuccessResponseAsync();
		}
	}
}
