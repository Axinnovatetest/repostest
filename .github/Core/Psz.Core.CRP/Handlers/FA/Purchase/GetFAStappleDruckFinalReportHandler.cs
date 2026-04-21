using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFAStappleDruckFinalReportHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Reporting.Models.FAUpdateByArticleFinalModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAStappleDruckFinalReportHandler(Reporting.Models.FAUpdateByArticleFinalModel data, Identity.Models.UserModel user)
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
				var finalReport = new List<Reporting.Models.FAStapelFinalReportModel>();
				var toUpdate = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
				var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Updated?.Select(x => x.Fertigungsnummer)?.ToList());
				foreach(var item in this._data.Updated)
				{
					//var faItem = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(item.Fertigungsnummer);
					var faItem = faEntities.Find(x => x.Fertigungsnummer == item.Fertigungsnummer);
					if(faItem != null)
					{
						var artikelItem = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)faItem.Artikel_Nr);
						finalReport.Add(new Reporting.Models.FAStapelFinalReportModel(item.Fertigungsnummer, artikelItem.ArtikelNummer, faItem.Termin_Bestatigt1));
						faItem.Gebucht = true;
						faItem.FA_Druckdatum = DateTime.Now;
						//Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(faItem);
						toUpdate.Add(faItem);
					}
				}

				// - 2022-03-16 - refactor 
				Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(toUpdate);


				// -
				response = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = finalReport,
					BodyTemplate = "CRP_FADruckFinal_Body",
					DocumentTitle = "",
					FooterBgColor = null,
					FooterCenterText = "",
					FooterCenterText2 = "",
					FooterData = null,
					FooterLeftText = "",
					FooterTemplate = "CRP_Footer",
					FooterWithCounter = false,
					HasFooter = false,
					HasHeader = false,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = "",
					Rotate = false
				});
				//Module.CRP_ReportingService.GenerateFAStapelFinalReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FASTAPLEREPORT, finalReport);
				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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