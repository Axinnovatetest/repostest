using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetFA_NPEX_reprotHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private FA_NPEX_ReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFA_NPEX_reprotHandler(Identity.Models.UserModel user, FA_NPEX_ReportEntryModel data)
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
				var ReportData = new FA_NPEX_ReportModel();
				ReportData.Articles = new List<FA_NPEX_ReportArticleModel>();
				ReportData.Orders = new List<FA_NPEX_ReportOrderModel>();
				var lagers = new List<int> { _data.Lager };
				if(_data.Lager <= 0)
				{
					var w = new Handlers.GetWarehouseHandler(_user).Handle();
					if(w.Success)
					{
						lagers = w.Body.Select(x => x.Key)?.ToList();
					}
				}
				var query1 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetFA_NPEXQuery1(lagers, _data.Kunde, _data.Articles);
				var query2 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetFA_NPEXQuery2(lagers, _data.Kunde, _data.Articles);

				var join = from q1 in query1
						   join q2 in query2
						   on q1.Artikelnummer equals q2.Artikelnummer
						   select new FA_NPEX_ReportDetailsModel
						   {
							   Fertigungsnummer = q1.Fertigungsnummer ?? 0,
							   Kunde = q1.Kunde,
							   Artikelnummer = q1.Artikelnummer,
							   Bezeichnung_1 = q1.Bezeichnung_1,
							   Termin_Fertigstellung = q1.Termin_Fertigstellung ?? DateTime.Now,
							   Bemerkung = q1.Bemerkung,
							   Preis = q1.Preis ?? 0,
							   Stucklisten_Artikelnummer = q2.Artikelnummer_ROH,
							   Bezeichnung_des_Bauteils = q2.Bezeichnung_des_Bauteils,
							   Stucklisten_Anzahl = q2.Anzahl_ROH ?? 0,
							   Bedarf = q2.Bedarf ?? 0,
							   Bestand = q2.Bestand ?? 0,
							   Freigabestatus = q1.Freigabestatus,
							   Termin_Bestätigt1 = q1.Termin_Bestätigt1 ?? DateTime.Now,
							   Erstmuster = q1.Erstmuster ?? false,
							   Anzahl = q1.Anzahl ?? 0,
						   };
				//details
				var _details = join.Where(a => a.Termin_Bestätigt1 <= DateTime.Now.AddDays(60)).OrderBy(a => a.Kunde).ThenBy(b => b.Artikelnummer).ThenBy(c => c.Fertigungsnummer).ThenBy(d => d.Termin_Bestätigt1).ThenBy(e => e.Anzahl).ToList();
				ReportData.Details = _details;

				//kunden
				var _customers = _details.Select(a => a.Kunde).Distinct().ToList();
				ReportData.Kunden = _customers.Select(a => new FA_NPEX_ReportCustomersModel(a, 0)).ToList();

				//articles
				var _articles = _details.Select(a => a.Artikelnummer).Distinct().ToList();
				foreach(var item in _articles)
				{
					var articleOrders = _details.Where(a => a.Artikelnummer == item).Select(b => b.Fertigungsnummer).Distinct().ToList();
					foreach(var order in articleOrders)
					{
						ReportData.Articles.Add(new FA_NPEX_ReportArticleModel(
							_details.FirstOrDefault(a => a.Artikelnummer == item).Kunde,
						   item,
							_details.FirstOrDefault(a => a.Artikelnummer == item).Bezeichnung_1,
							_details.FirstOrDefault(a => a.Artikelnummer == item).Freigabestatus,
							order
							));
					}
				}
				//orders
				var _orders = _details.Select(a => a.Fertigungsnummer).Distinct().ToList();
				foreach(var item in _orders)
				{
					var orderArticles = _details.Where(a => a.Fertigungsnummer == item).Select(b => b.Artikelnummer).Distinct().ToList();
					foreach(var article in orderArticles)
					{
						ReportData.Orders.Add(new FA_NPEX_ReportOrderModel(
							_details.FirstOrDefault(a => a.Fertigungsnummer == item).Kunde,
							article,
							item,
							_details.FirstOrDefault(a => a.Fertigungsnummer == item).Anzahl,
							_details.FirstOrDefault(a => a.Fertigungsnummer == item).Termin_Fertigstellung,
							_details.FirstOrDefault(a => a.Fertigungsnummer == item).Termin_Bestätigt1,
							_details.FirstOrDefault(a => a.Fertigungsnummer == item).Bemerkung,
							_details.FirstOrDefault(a => a.Fertigungsnummer == item).Erstmuster,
							_details.FirstOrDefault(a => a.Fertigungsnummer == item).Preis
							));
					}
				}
				response = await Reporting.IText.GetItextPDF(new ITextHeaderFooterProps
				{
					BodyData = ReportData,
					BodyTemplate = "CTS_FANPEX_Body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterData = null,
					FooterLeftText = "",
					FooterTemplate = "CTS_Footer",
					FooterWithCounter = false,
					HasFooter = false,
					HasHeader = false,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = null,
					Rotate = false
				});
				//Module.CS_ReportingService.GenerateFA_NPEXReport(Enums.ReportingEnums.ReportType.CTS_FANPEX, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
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