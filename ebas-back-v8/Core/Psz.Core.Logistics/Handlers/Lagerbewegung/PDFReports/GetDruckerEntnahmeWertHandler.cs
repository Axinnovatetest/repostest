using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung.PDFReports;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung.PDFReports
{
	public class GetDruckerEntnahmeWertHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DateTime? _DateD { get; set; }
		private DateTime? _DateF { get; set; }
		private int _lager1 { get; set; }
		private int _lager2 { get; set; }
		private string _lagerort { get; set; }
		private int _type { get; set; }
		private string _artikelnummer { get; set; }
		public GetDruckerEntnahmeWertHandler(Identity.Models.UserModel user, DateTime D1, DateTime D2, int L1, int Type, string artikelnummer)
		{
			this._user = user;
			this._lager1 = L1;
			this._DateD = D1;
			this._DateF = D2;
			this._type = Type;
			this._artikelnummer = artikelnummer;
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

				var ReportData = new EntnahmeWertReportModel();
				ReportData.Headers = new List<HeaderEntnahmeWertReportModel> { };
				ReportData.HeadersGroup = new List<HeaderGroupEntnahmeWertReportModel> { };
				ReportData.HeadersWEK = new List<HeaderEntnahmeWertReportModel> { };
				ReportData.HeadersGroupWEK = new List<HeaderGroupEntnahmeWertReportModel> { };
				var ModelLGT = Psz.Core.Logistics.Module.LGT.LGTList.Where(x => x.Lager_Id == this._lager1).ToList();
				this._lager2 = 0;
				this._lagerort = "";
				if(ModelLGT != null && ModelLGT.Count() > 0)
				{
					this._lager2 = ModelLGT[0].Lager_P_Id;
					this._lagerort = ModelLGT[0].Lager;
				}
				ReportData.Headers.Add(new HeaderEntnahmeWertReportModel(this._lagerort, this._DateD != null ? this._DateD.Value.ToString("dd-MM-yyyy") : "", this._DateF != null ? this._DateF.Value.ToString("dd-MM-yyyy") : ""));
				var entnahmeWertWithEKEntities = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetEntnahmeWertWithEK(this._DateD, this._DateF, this._lager1, this._lager2, this._type, this._artikelnummer);
				;
				if(entnahmeWertWithEKEntities != null && entnahmeWertWithEKEntities.Count > 0)
				{

					ReportData.Details = entnahmeWertWithEKEntities.Select(a => new DetailEntnahmeWertReportModel(a)).ToList();

					var _groupping = entnahmeWertWithEKEntities.GroupBy(d => new { d.datum })
				   .Select(m => new Groupping
				   {
					   datum = m.Key.datum,
					   totalLigne = (from num in entnahmeWertWithEKEntities
									 where num.datum == m.Key.datum
									 select num.datum).Count(),
					   gesmtPreis = Math.Round((from num in entnahmeWertWithEKEntities
												where num.datum == m.Key.datum
												select num.kosten).Sum(), 2),
					   percentPreis = Math.Round((from num in entnahmeWertWithEKEntities
												  where num.datum == m.Key.datum
												  select num.kosten).Sum() /
									(from num in entnahmeWertWithEKEntities
									 select num.kosten).Sum() * 100, 2)
				   }).ToList();

					foreach(var Header in _groupping)
					{
						ReportData.HeadersGroup.Add(new HeaderGroupEntnahmeWertReportModel(Header.datum != null ? Header.datum.Value.ToString("dd-MM-yyyy") : "",
							Header.totalLigne, Header.gesmtPreis, Header.percentPreis));

					}



				}
				//-------------------------Withou EK-----------------------------------------
				var entnahmeWertWithoutEKEntities = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetEntnahmeWertWithoutEK(this._DateD, this._DateF, this._lager1, this._lager2, this._type, this._artikelnummer);
				;
				if(entnahmeWertWithoutEKEntities != null && entnahmeWertWithoutEKEntities.Count > 0)
				{

					ReportData.DetailsWEK = entnahmeWertWithoutEKEntities.Select(a => new DetailEntnahmeWertReportModel(a)).ToList();
					ReportData.HeadersWEK.Add(new HeaderEntnahmeWertReportModel(this._lagerort, this._DateD != null ? this._DateD.Value.ToString("dd-MM-yyyy") : "", this._DateF != null ? this._DateF.Value.ToString("dd-MM-yyyy") : ""));
					var _groupping = entnahmeWertWithoutEKEntities.GroupBy(d => new { d.datum })
				   .Select(m => new Groupping
				   {
					   datum = m.Key.datum,
					   totalLigne = (from num in entnahmeWertWithoutEKEntities
									 where num.datum == m.Key.datum
									 select num.datum).Count(),
					   gesmtPreis = Math.Round((from num in entnahmeWertWithoutEKEntities
												where num.datum == m.Key.datum
												select num.kosten).Sum(), 2),
					   percentPreis = Math.Round((from num in entnahmeWertWithoutEKEntities
												  where num.datum == m.Key.datum
												  select num.kosten).Sum() /
									(from num in entnahmeWertWithoutEKEntities
									 select num.kosten).Sum() * 100, 2)
				   }).ToList();

					foreach(var Header in _groupping)
					{
						ReportData.HeadersGroupWEK.Add(new HeaderGroupEntnahmeWertReportModel(Header.datum != null ? Header.datum.Value.ToString("dd-MM-yyyy") : "",
							Header.totalLigne, Header.gesmtPreis, Header.percentPreis));

					}



				}

				if(this._type == 1)
				{
					response = Module.Logistic_ReportingService.GenerateEntnahmeWertReport(Enums.ReportingEnums.ReportType.EntnahmeWertMitBerechnung, ReportData);
				}
				else if(this._type == 2)
				{
					response = Module.Logistic_ReportingService.GenerateEntnahmeWertReport(Enums.ReportingEnums.ReportType.EntnahmeWert, ReportData);
				}


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
		public class Groupping
		{
			public DateTime? datum { get; set; }
			public int totalLigne { get; set; }
			public decimal gesmtPreis { get; set; }
			public decimal percentPreis { get; set; }
		}
	}
}
