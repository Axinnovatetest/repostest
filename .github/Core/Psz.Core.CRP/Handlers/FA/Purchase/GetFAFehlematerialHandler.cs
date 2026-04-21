using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetFAFehlematerialHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAFehlematerialHandler(Identity.Models.UserModel user, int data)
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

				byte[] responseBody = null;
				var list = new List<Reporting.Models.AnalyseSchneiderei1Model>();
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
				var date = (faEntity.Termin_Bestatigt1.HasValue) ? $"<= '{faEntity.Termin_Bestatigt1.Value.AddDays(10).ToString("yyyyMMdd")}'" : "IS NULL";
				var fehlematerialEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseFehlmaterial(new List<int> { (int)faEntity.Lagerort_id }, new List<int> { (int)faEntity.Lagerort_id }, this._data, date);
				if(fehlematerialEntity != null && fehlematerialEntity.Count > 0)
				{
					list = fehlematerialEntity.Select(x => new Reporting.Models.AnalyseSchneiderei1Model(x)).ToList();
				}
				responseBody = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyTemplate = "CRP_Fehlmaterial_Body",
					BodyData = list,
					DocumentTitle = "",
					FooterCenterText = "",
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
				//Module.CRP_ReportingService.GenerateFAFehlematerialReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FEHLEMATERIAL, list);
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