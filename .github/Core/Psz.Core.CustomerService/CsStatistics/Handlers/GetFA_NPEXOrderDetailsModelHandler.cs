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
	public class GetFA_NPEXOrderDetailsModelHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<FA_NPEX_DetailsModel>>>
	{
		private FA_NPEX_EntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFA_NPEXOrderDetailsModelHandler(Identity.Models.UserModel user, FA_NPEX_EntryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<FA_NPEX_DetailsModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<FA_NPEX_DetailsModel>();
				var query1 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetFA_NPEXQuery1(_data.Lager, _data.Kunde, _data.Order ?? -1);
				var query2 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetFA_NPEXQuery2(_data.Lager, _data.Kunde, _data.Order ?? -1);

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
				response = _details.Select(a => new FA_NPEX_DetailsModel
				{
					Artikelnummer_ROH = a.Stucklisten_Artikelnummer,
					ROH_Description = a.Bezeichnung_des_Bauteils,
					Qty = a.Anzahl,
					Nedded_qty = a.Bedarf,
					Exsisting_qty = a.Bestand,
				}).ToList();

				return ResponseModel<List<FA_NPEX_DetailsModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				throw;
			}
		}
		public ResponseModel<List<FA_NPEX_DetailsModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<FA_NPEX_DetailsModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<FA_NPEX_DetailsModel>>.SuccessResponse();
		}
	}
}
