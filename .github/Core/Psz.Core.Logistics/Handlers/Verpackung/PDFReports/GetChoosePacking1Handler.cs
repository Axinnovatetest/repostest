using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.Logistics.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung.PDFReports
{
	public class GetChoosePacking1Handler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private ListeUpdatePacking _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetChoosePacking1Handler(Identity.Models.UserModel user, ListeUpdatePacking data)
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

				var ReportData = new ChoosePackingReportModel();
				ReportData.Headers = new List<HeaderReportModel> { };

				List<long> listeLiefer = this._data.listeLieferscheine;
				var bestandEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetListeChoosePackingByNr(listeLiefer)
	;
				if(bestandEntity != null && bestandEntity.Count > 0)
				{
					//filling details list
					ReportData.Details = bestandEntity.Select(a => new PackingReportDetailsModel(a)).ToList();
					ReportData.Details = (ReportData.Details).OrderBy(x => x.lsNumber).ToList();
					//get ditincts lists
					//var _Datums = bestandEntity.Select(a => a.versanddatum_Auswahl).Distinct().ToList();
					//var _Kunden = bestandEntity.Select(a => a.lVornameNameFirma).Distinct().ToList();
					//var _Verpackungsart = bestandEntity.Select(a => a.versandarten_Auswahl).Distinct().ToList();
					//var _Headers= bestandEntity.Select(a => new {
					//	  a.versanddatum_Auswahl, a.versandarten_Auswahl, a.lVornameNameFirma}).Distinct().ToList();
					//;
					//var _Headers0 = bestandEntity.Select(a => new
					//{
					//	a.versanddatum_Auswahl,
					//	a.versandarten_Auswahl,
					//	a.lVornameNameFirma,

					//}
					//).Distinct().ToList();

					//foreach(var item in _Headers0)
					//{
					//	var resultlStrassePostfach = bestandEntity.Where(x => x.versanddatum_Auswahl == item.versanddatum_Auswahl && x.versandarten_Auswahl == item.versandarten_Auswahl && x.lVornameNameFirma == item.lVornameNameFirma)
					//		.OrderByDescending(x => x.lStrassePostfach).First().lStrassePostfach;
					//	//var resultlStrassePostfach = from r in bestandEntity.Where(r => r.versanddatum_Auswahl == item.versanddatum_Auswahl && r.versandarten_Auswahl == item.versandarten_Auswahl && r.lVornameNameFirma == item.lVornameNameFirma)
					//	//			 select r.lStrassePostfach.Max();
					//	var resultlName2 = bestandEntity.Where(x => x.versanddatum_Auswahl == item.versanddatum_Auswahl && x.versandarten_Auswahl == item.versandarten_Auswahl && x.lVornameNameFirma == item.lVornameNameFirma)
					//		.OrderByDescending(x => x.lName2).First().lName2;
					//	//var resultlName2 = from r in bestandEntity.Where(r => r.versanddatum_Auswahl == item.versanddatum_Auswahl && r.versandarten_Auswahl == item.versandarten_Auswahl && r.lVornameNameFirma == item.lVornameNameFirma)
					//	//				   select r.lName2.Max();
					//	var resultlName3 = bestandEntity.Where(x => x.versanddatum_Auswahl == item.versanddatum_Auswahl && x.versandarten_Auswahl == item.versandarten_Auswahl && x.lVornameNameFirma == item.lVornameNameFirma)
					//		.OrderByDescending(x => x.lName3).First().lName3;
					//	//var resultlName3 = from r in bestandEntity.Where(r => r.versanddatum_Auswahl == item.versanddatum_Auswahl && r.versandarten_Auswahl == item.versandarten_Auswahl && r.lVornameNameFirma == item.lVornameNameFirma)
					//	//				   select r.lName3.Max();
					//	var resultlAnsprechpartner = bestandEntity.Where(x => x.versanddatum_Auswahl == item.versanddatum_Auswahl && x.versandarten_Auswahl == item.versandarten_Auswahl && x.lVornameNameFirma == item.lVornameNameFirma)
					//		.OrderByDescending(x => x.lAnsprechpartner).First().lAnsprechpartner;
					//	//var resultlAnsprechpartner = from r in bestandEntity.Where(r => r.versanddatum_Auswahl == item.versanddatum_Auswahl && r.versandarten_Auswahl == item.versandarten_Auswahl && r.lVornameNameFirma == item.lVornameNameFirma)
					//	//				   select r.lAnsprechpartner.Max();
					//	var resultlAbteilung = bestandEntity.Where(x => x.versanddatum_Auswahl == item.versanddatum_Auswahl && x.versandarten_Auswahl == item.versandarten_Auswahl && x.lVornameNameFirma == item.lVornameNameFirma)
					//		.OrderByDescending(x => x.lAbteilung).First().lAbteilung;
					//	//var resultlAbteilung = from r in bestandEntity.Where(r => r.versanddatum_Auswahl == item.versanddatum_Auswahl && r.versandarten_Auswahl == item.versandarten_Auswahl && r.lVornameNameFirma == item.lVornameNameFirma)
					//	//							 select r.lAbteilung.Max();
					//	ReportData.Headers.Add(new HeaderReportModel(item.versanddatum_Auswahl != null ? item.versanddatum_Auswahl.Value.ToString("dd-MM-yyyy") : ""
					//		, item.versandarten_Auswahl, item.lVornameNameFirma, (string)resultlStrassePostfach
					//		, (string)resultlName2, (string)resultlName3, (string)resultlAnsprechpartner, (string)resultlAbteilung));


					//}
					//filling Header list
					var _Headers1 = bestandEntity.Select(a => new
					{
						a.versanddatum_Auswahl,
						a.versandarten_Auswahl,
						a.lVornameNameFirma,
						a.lStrassePostfach,
						a.lName2,
						a.lName3,
						a.lAnsprechpartner,
						a.lAbteilung,
						a.lLandPLZORT
					}
						  ).Distinct().ToList();

					//filling lager list
					foreach(var Header in _Headers1)
					{
						ReportData.Headers.Add(new HeaderReportModel(Header.versanddatum_Auswahl != null ? Header.versanddatum_Auswahl.Value.ToString("dd-MM-yyyy") : ""
							, Header.versandarten_Auswahl, Header.lVornameNameFirma, Header.lStrassePostfach
							, Header.lName2, Header.lName3, Header.lAnsprechpartner, Header.lAbteilung, Header.lLandPLZORT));

					}
				}

				response = Module.Logistic_ReportingService.GenerateVerpachunkReport(Enums.ReportingEnums.ReportType.VERPACKUNGCHOOSE1, ReportData);

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
