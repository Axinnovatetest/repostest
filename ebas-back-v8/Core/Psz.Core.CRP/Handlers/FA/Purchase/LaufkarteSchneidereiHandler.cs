using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class LaufkarteSchneidereiHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public LaufkarteSchneidereiHandler(Identity.Models.UserModel user, int data)
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

				var LaufkarteSchneidereiEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetLaufkarteSchneiderei(this._data);
				var LaufkarteSchneiderei = LaufkarteSchneidereiEntity?.Select(x => new Reporting.Models.LaufkarteSchneidereiModel(x)).ToList();
				if(LaufkarteSchneiderei != null && LaufkarteSchneiderei.Count > 0)
				{
					string _temp = string.Empty;
					for(int i = 0; i < LaufkarteSchneiderei.Count; i++)
					{
						for(int j = i + 1; j < LaufkarteSchneiderei.Count; j++)
						{
							_temp = LaufkarteSchneiderei[i].Gewerk;
							if(LaufkarteSchneiderei[j].Gewerk == _temp)
								LaufkarteSchneiderei[j].Gewerk = string.Empty;
							else
								_temp = LaufkarteSchneiderei[j].Gewerk;
						}
					}
				}
				responseBody = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = LaufkarteSchneiderei,
					BodyTemplate = "CRP_LaufkarteSchneiderei_Body",
					DocumentTitle = "",
					FooterBgColor = new iText.Kernel.Colors.DeviceRgb(220, 220, 220),
					FooterCenterText = "",
					FooterCenterText2 = $"Gedrukt am: {DateTime.Now}",
					FooterData = null,
					FooterLeftText = "F-087-01",
					FooterTemplate = "CRP_Footer",
					FooterWithCounter = true,
					HasFooter = true,
					HasHeader = false,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = "",
					Rotate = false
				});
				//Module.CRP_ReportingService.GenerateLaufkarteSchneidereiReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_LAUFKARTE_SCHNEIDEREI, LaufkarteSchneiderei);
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