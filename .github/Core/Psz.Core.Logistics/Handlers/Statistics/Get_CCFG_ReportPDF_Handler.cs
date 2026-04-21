using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class Get_CCFG_ReportPDF_Handler
	{
		private Identity.Models.UserModel _user { get; set; }
		private int Lager { get; set; }
		public Get_CCFG_ReportPDF_Handler(Identity.Models.UserModel user, int Lager)
		{
			this._user = user;
			this.Lager = Lager;
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
				var Liste_50_FG_Artikel_Selectione = new List<Liste_50_FG_Artikel_Selectionee_Model>();
				var Liste_50_ROH_Artikel_SelectioneEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.getListe_50_ROH_Artikel_Selectione_CCFG_Entity(Lager);
				if(Liste_50_ROH_Artikel_SelectioneEntity != null && Liste_50_ROH_Artikel_SelectioneEntity.Count > 0)
					Liste_50_FG_Artikel_Selectione = Liste_50_ROH_Artikel_SelectioneEntity.Select(k => new Liste_50_FG_Artikel_Selectionee_Model(k)).ToList();




				var details = Liste_50_FG_Artikel_Selectione?.Select(x => new Liste_50_FG_Artikel_Selectione_Model_Details(x)).ToList();
				var header = new PSZ_CCFG_Artikeltabelle_Header
				{
					Rest = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.Rest_ROH_CCFG_Artikel(Lager),
					Total = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.somme_ROH_CCFG_Artikel(Lager),
					Lagerort_id = Liste_50_FG_Artikel_Selectione.Select(x => x.Lagerort_id).FirstOrDefault(),
					Lagerort = Liste_50_FG_Artikel_Selectione.Select(x => x.Lagerort).FirstOrDefault(),
					DTime = DateTime.Now.ToString("dd/MM/yyyy")
				};

				var ReportData = new PSZ_CCFG_Artikeltabelle_Report { Details = details, Header = new List<PSZ_CCFG_Artikeltabelle_Header> { header } };
				response = Module.Logistic_ReportingService.CCFG_ReportPDFReport(Enums.ReportingEnums.ReportType.CCFG_ReportPDFReport, ReportData);
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
