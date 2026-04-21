using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.Logistics.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung.PDFReports
{
	public class GetChoosePacking2Handler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private ListeUpdatePacking _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetChoosePacking2Handler(Identity.Models.UserModel user, ListeUpdatePacking data)
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
					ReportData.Details = bestandEntity.Select(a => new PackingReportDetailsModel(a)).OrderBy(a => a.lsNumber).ToList();
					//get ditincts lists
					var _Datums = bestandEntity.Select(a => a.versanddatum_Auswahl).Distinct().ToList();
					var _Kunden = bestandEntity.Select(a => a.lVornameNameFirma).Distinct().ToList();
					var _Verpackungsart = bestandEntity.Select(a => a.versandarten_Auswahl).Distinct().ToList();
					var _Headers = bestandEntity.Select(a => new
					{
						a.versanddatum_Auswahl,
						a.versandarten_Auswahl,
						a.lVornameNameFirma
					}).Distinct().OrderBy(a => a.versanddatum_Auswahl).ThenBy(a => a.versandarten_Auswahl).ThenBy(a => a.lVornameNameFirma).ToList();
					;

					foreach(var Header in _Headers)
					{
						decimal sommeP = bestandEntity.Where(a => a.versanddatum_Auswahl == Header.versanddatum_Auswahl && a.versandarten_Auswahl == Header.versandarten_Auswahl && a.lVornameNameFirma == Header.lVornameNameFirma)
							.Select(a => a.preis * a.anzahl).Sum();
						ReportData.Headers.Add(new HeaderReportModel(Header.versanddatum_Auswahl != null ? Header.versanddatum_Auswahl.Value.ToString("dd-MM-yyyy") : ""
							, Header.versandarten_Auswahl, Header.lVornameNameFirma, sommeP));

					}
				}

				response = Module.Logistic_ReportingService.GenerateVerpachunkReport2(Enums.ReportingEnums.ReportType.VERPACKUNGCHOOSE2, ReportData);

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
