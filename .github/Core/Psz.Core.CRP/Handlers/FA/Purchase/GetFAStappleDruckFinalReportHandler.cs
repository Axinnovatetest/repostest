using Infrastructure.Services.Reporting.Models.CTS;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFAStappleDruckFinalReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private FAUpdateByArticleFinalModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetFAStappleDruckFinalReportHandler(FAUpdateByArticleFinalModel data, Identity.Models.UserModel user)
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
				List<FAStapelFinalReportModel> finalReport = new List<FAStapelFinalReportModel>();
				var toUpdate = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
				var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Updated?.Select(x => x.Fertigungsnummer)?.ToList());
				foreach(var item in this._data.Updated)
				{
					//var faItem = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(item.Fertigungsnummer);
					var faItem = faEntities.Find(x => x.Fertigungsnummer == item.Fertigungsnummer);
					if(faItem != null)
					{
						var artikelItem = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)faItem.Artikel_Nr);
						finalReport.Add(new FAStapelFinalReportModel(item.Fertigungsnummer, artikelItem.ArtikelNummer, faItem.Termin_Bestatigt1));
						faItem.Gebucht = true;
						faItem.FA_Druckdatum = DateTime.Now;
						//Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(faItem);
						toUpdate.Add(faItem);
					}
				}

				// - 2022-03-16 - refactor 
				Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(toUpdate);


				// -
				response = Module.CRP_ReportingService.GenerateFAStapelFinalReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FASTAPLEREPORT, finalReport);
				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
