using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetUpdatedFaPDFHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Reporting.Models.FAUpdateByArticleFinalModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetUpdatedFaPDFHandler(Reporting.Models.FAUpdateByArticleFinalModel data, Identity.Models.UserModel user)
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
				this._data.Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				responseBody = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = this._data,
					BodyTemplate = "CRP_FAUpdate",
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
				//Module.CRP_ReportingService.GenerateFAUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_FA_UPDATE, this._data);
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