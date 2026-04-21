using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetListWareneingangByLieferantenRapportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DateTime _DateD { get; set; }
		private DateTime _DateF { get; set; }
		private string _name1 { get; set; }
		public GetListWareneingangByLieferantenRapportHandler(Identity.Models.UserModel user, DateTime D1, DateTime D2, string name1)
		{
			_user = user;
			_DateD = D1;
			_DateF = D2;
			_name1 = name1;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] response = null;

				var ReportData = new WareneingangLieferantReportModel();
				ReportData.ListeName1 = new List<HeaderName>() { };
				ReportData.grouping = new List<ListWareneingangRapportDetailsByKundeUndDatumModel>() { };
				var response0 = new List<ListWareneingangRapportDetailsByKundeUndDatumModel>();
				var response01 = new List<WareneingangLieferantRapportDetailsModel>();
				var response00 = new List<HeaderName>();
				var wareneingangEntities = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetListeWareneingangByLieferant(_DateD, _DateF, _name1);
				;
				if(wareneingangEntities != null && wareneingangEntities.Count > 0)
				{
					//filling details list
					//ReportData.Details = bestandEntity.Select(a => new PackingReportDetailsModel(a)).ToList();
					var _groupping = wareneingangEntities.GroupBy(d => new { d.mois, d.annee, d.name1Lower })
				   .Select(m => new Groupping
				   {

					   mois = m.Key.mois,
					   annee = m.Key.annee,
					   name1Lower = m.Key.name1Lower,
					   moisEnLettre = m.Key.annee + " " + returnMois(m.Key.mois),

				   }).ToList();
					foreach(var item in _groupping)
					{
						response0.Add(new ListWareneingangRapportDetailsByKundeUndDatumModel
						{
							name1 = item.name1Lower,
							mois = item.mois,
							annee = item.annee,
							moisEnLettre = item.moisEnLettre,
							details = wareneingangEntities.Where(l => l.name1Lower == item.name1Lower && l.mois == item.mois)?
							.Select(d => new WareneingangLieferantRapportDetailsModel
							{
								liefertermin = d.liefertermin != null ? d.liefertermin.Value.ToString("dd-MM-yyyy") : "",
								name1 = d.name1,
								artikelnummer = d.artikelnummer,
								SummeVonAnzahl = d.SummeVonAnzahl,
								einheit = d.einheit,
								projektNr = d.projektNr,
								typ = d.typ,
							}).ToList()
						});

					}
					response01 = wareneingangEntities.Select(a => new WareneingangLieferantRapportDetailsModel(a)).ToList();
					response00.Add(new HeaderName(this._name1));
					ReportData.ListeName1 = response00;
					ReportData.Details = response01;
					ReportData.grouping = response0.Select(a => new ListWareneingangRapportDetailsByKundeUndDatumModel(a.name1Lower, a.mois, a.annee, a.moisEnLettre, a.details)).ToList();



				}

				response = Module.Logistic_ReportingService.GenerateWareneingangLieferantReport(Enums.ReportingEnums.ReportType.RapportWareneingangLieferant, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

		public class Groupping
		{
			public int mois { get; set; }
			public int annee { get; set; }
			public string moisEnLettre { get; set; }
			public string name1Lower { get; set; }

		}

		public string returnMois(int mois)
		{
			switch(mois)
			{
				case 1:
					return "Januar";
				case 2:
					return "Februar";
				case 3:
					return "März";
				case 4:
					return "April";
				case 5:
					return "Mai";
				case 6:
					return "Juni";
				case 7:
					return "Juli";
				case 8:
					return "August";
				case 9:
					return "September";
				case 10:
					return "Oktober";
				case 11:
					return "November";
				case 12:
					return "Dezember";
				default:
					return "";
			}
		}
	}
}
