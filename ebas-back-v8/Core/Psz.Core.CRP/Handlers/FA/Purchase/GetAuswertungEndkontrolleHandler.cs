using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetAuswertungEndkontrolleHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetAuswertungEndkontrolleHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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

				byte[] responseBody = null;
				var auswetungEndkontrolleEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetAuswertingEndkontrolle();
				if(auswetungEndkontrolleEntity != null && auswetungEndkontrolleEntity.Count > 0)
				{
					var ausweruntEndkontrolle = auswetungEndkontrolleEntity.Select(x => new Reporting.Models.AuswertungEndkontrolleReportModel(x)).ToList();
					responseBody = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
					{
						BodyData = ausweruntEndkontrolle,
						BodyTemplate = "CRP_AuswertungEndkontrolle_Body",
						DocumentTitle = "",
						FooterCenterText = "",
						FooterLeftText = $"Printed am {DateTime.Now.ToString("dd.MM.yyyy")}",
						FooterData = null,
						FooterTemplate = "CRP_Footer",
						FooterWithCounter = false,
						HasFooter = true,
						HasHeader = true,
						HeaderFirstPageOnly = true,
						HeaderLogoWithCounter = false,
						HeaderLogoWithText = true,
						HeaderText = "PSZ Electronic GmbH",
						Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}",
						Rotate = false
					});
					//Module.CRP_ReportingService.GenerateAuswerungEndkontrolleReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_AUSWERTUNG_ENDKONTROLLE, ausweruntEndkontrolle);
				}
				return ResponseModel<byte[]>.SuccessResponse(responseBody);
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