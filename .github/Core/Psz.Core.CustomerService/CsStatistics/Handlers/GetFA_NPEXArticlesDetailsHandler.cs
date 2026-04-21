using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetFA_NPEXArticlesDetailsHandler: IHandle<Identity.Models.UserModel, ResponseModel<FA_NPEX_ResponseModel>>
	{
		private FA_NPEX_EntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFA_NPEXArticlesDetailsHandler(Identity.Models.UserModel user, FA_NPEX_EntryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<FA_NPEX_ResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new FA_NPEX_ResponseModel();
				var lagers = new List<int> { _data.Lager };
				if(_data.Lager <= 0)
				{
					var w = new Handlers.GetWarehouseHandler(_user).Handle();
					if(w.Success)
					{
						lagers = w.Body.Select(x => x.Key)?.ToList();
					}
				}
				var query1 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetFA_NPEXQuery1(lagers, _data.Kunde);
				var query2 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetFA_NPEXQuery2(lagers, _data.Kunde);

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
				//paging
				var FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0;
				var RequestRows = this._data.ItemsPerPage;
				var _articles = _details.Select(a => a.Artikelnummer).Distinct().ToList();

				response = new FA_NPEX_ResponseModel
				{
					Articles = _articles.Select(a => new FA_NPEX_ArticlesModel
					{
						Artikelnummer = a,
						Bezeichnung_1 = _details.FirstOrDefault(x => x.Artikelnummer == a).Bezeichnung_1,
						Freigabestatus = _details.FirstOrDefault(x => x.Artikelnummer == a).Freigabestatus,
						Orders = _details.Where(o => o.Artikelnummer == a).Select(y => new FA_NPEX_OrderModel
						{
							Fertigungsnummer = y.Fertigungsnummer,
							Anzahl = y.Anzahl,
							Termin_Fertigstellung = y.Termin_Fertigstellung,
							Termin_Bestatigt1 = y.Termin_Bestätigt1,
							Bemerkung = y.Bemerkung,
							Erstmuster = y.Erstmuster,
							Preis = y.Preis
						}).DistinctBy(z => z.Fertigungsnummer).ToList(),
					}).ToList(),
					Kunde = _data.Kunde,
					Lager = $"{string.Join(" | ", Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(lagers)?.Select(x => x.Lagerort))}",
					RequestedPage = _data.RequestedPage,
					ItemsPerPage = _data.ItemsPerPage,
				};
				response.AllCount = response.Articles.Count;
				//filtering
				if(!string.IsNullOrEmpty(_data.SearchTerms) && !string.IsNullOrWhiteSpace(_data.SearchTerms))
					response.Articles = response.Articles.Where(a => a.Artikelnummer.ToLower().Contains(_data.SearchTerms.Trim().ToLower())).ToList();
				//pagination
				response.Articles = response.Articles.Skip(FirstRowNumber).Take(RequestRows).ToList();
				response.AllPagesCount = (int)Math.Ceiling((decimal)response.AllCount / this._data.ItemsPerPage);

				return ResponseModel<FA_NPEX_ResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				throw;
			}
		}
		public ResponseModel<FA_NPEX_ResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<FA_NPEX_ResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<FA_NPEX_ResponseModel>.SuccessResponse();
		}
	}
}
