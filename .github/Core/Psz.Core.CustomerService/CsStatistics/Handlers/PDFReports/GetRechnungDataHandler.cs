using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetRechnungDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<RechnungModel>>
	{
		private RechnungEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungDataHandler(Identity.Models.UserModel user, RechnungEntryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<RechnungModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var ReportData = new RechnungModel();
				var _lager = GetRechnungPDFHandler.GetWarehouse(_data);
				var rechnungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnung(_data.From, _data.To, _lager);
				if(rechnungEntity != null && rechnungEntity.Count > 0)
				{
					var _zollatarif = rechnungEntity.Select(z => z.Zolltarif_nr).Distinct().ToList();
					var _lagerNummer = Enums.OrderEnums.GetLagerNumber((Enums.OrderEnums.KapazitatLager)_data.Lager);
					var RechnungReportParametersEntity = Infrastructure.Data.Access.Tables.CTS.RechnungReportingAccess.GetByLagerIdAndType(_lagerNummer, "Rechnung");
					ReportData = new RechnungModel
					{
						ReportParameters = new RechnungHeaderModel(RechnungReportParametersEntity, _data, GetRechnungPDFHandler.PrepareRechnungNummer(_data)),

						Zollatarif = _zollatarif.OrderBy(s => s).Select(z => new RechnungGroupedZollaTarif
						{
							Zolltarif_nummer = z,
							Gewicht = rechnungEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Gesamtgewicht / 1000) ?? 0,
							Positionen = rechnungEntity.Where(x => x.Zolltarif_nr == z).ToList().Count,
							Lohnleistun = rechnungEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Ausdr3) ?? 0,
							Zusatzkosten = rechnungEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Zusatzkosten_Produktion) ?? 0,
							Material = rechnungEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Material1) ?? 0,
							Stat_Wert = rechnungEntity.Where(x => x.Zolltarif_nr == z).Sum(a => a.Material1 + a.Ausdr3 + a.Zusatzkosten_Produktion) ?? 0,
						}).ToList(),
					};
				}
				var Details = rechnungEntity.Select(r => new RechnungDetailsModel(r)).ToList();
				ReportData.Details = SetRepeated(Details);

				return ResponseModel<RechnungModel>.SuccessResponse(ReportData);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<RechnungModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<RechnungModel>.AccessDeniedResponse();
			}

			return ResponseModel<RechnungModel>.SuccessResponse();
		}
		public static List<RechnungDetailsModel> SetRepeated(List<RechnungDetailsModel> data)
		{
			var result = new List<RechnungDetailsModel>();
			var FAList = data.Select(f => f.Fertigungsnummer).ToList();
			var _repeatedFA = FAList.GroupBy(x => x)
			   .Where(g => g.Count() > 1)
			   .Select(y => y.Key)
			   .ToList();
			result = data.Select(d => new RechnungDetailsModel
			{
				Ausdr3 = d.Ausdr3,
				Datum = d.Datum,
				Fertigungsnummer = d.Fertigungsnummer,
				Originalanzahl = d.Originalanzahl,
				Anzahl_erledigt = d.Anzahl_erledigt,
				Anzahl = d.Anzahl,
				Artikelnummer = d.Artikelnummer,
				Bezeichnung1 = d.Bezeichnung1,
				Betrag = d.Betrag,
				Preis = d.Preis,
				Bemerkung = d.Bemerkung,
				Bezfeld = d.Bezfeld,
				Erstmuster = d.Erstmuster,
				Zolltarif_nr = d.Zolltarif_nr,
				Material1 = d.Material1,
				Größe = d.Größe,
				Gesamtgewicht = d.Gesamtgewicht,
				Stundensatz = d.Stundensatz,
				MinutenKosten = d.MinutenKosten,
				Zusatzkosten_FA_Basis_30_Min = d.Zusatzkosten_FA_Basis_30_Min,
				PREIS_Mit_Zusatzkosten_Pro_Stück = d.PREIS_Mit_Zusatzkosten_Pro_Stück,
				Zusatzkosten_Produktion = d.Zusatzkosten_Produktion,
				Preis_Total_Mit_Zusatzkosten = d.Preis_Total_Mit_Zusatzkosten,
				Repeated = _repeatedFA != null && _repeatedFA.Count > 0 && _repeatedFA.Contains(d.Fertigungsnummer) ? 1 : 0,
			}).ToList();

			return result;
		}

	}
}
