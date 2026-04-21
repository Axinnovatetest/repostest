using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class GetWerkUpdateReportHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetWerkUpdateReportHandler(Identity.Models.UserModel user, int data)
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
				var Lists = new Reporting.Models.FAWerkUpdateReportModel();
				var updateEntity = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_UpdateAccess.Get(this._data);
				var UpdateDetails = new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
				var Updated = new List<Reporting.Models.FAUpdatedFromExcelModel>();
				var Notupdated = new List<Reporting.Models.FANotUpdatedFromExcelModel>();
				if(updateEntity != null)
				{
					UpdateDetails = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.GetByUpdateId(this._data);
				}
				if(UpdateDetails != null && UpdateDetails.Count > 0)
				{
					var FANummers = UpdateDetails.Select(x => (int)x.FA).ToList();
					var FAEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFAIds(FANummers);
					var UpdatedOrdersEntity = UpdateDetails?.Where(x => x.updated.Value).ToList();
					var NotupdatedOrdersEntity = UpdateDetails?.Where(x => !x.updated.Value).ToList();

					if(UpdatedOrdersEntity != null && UpdatedOrdersEntity.Count > 0)
					{
						foreach(var item in UpdatedOrdersEntity)
						{
							Updated.Add(new Reporting.Models.FAUpdatedFromExcelModel(1, FAEntities?.FirstOrDefault(x => x.Fertigungsnummer == item.FA)));
						}
					}
					if(NotupdatedOrdersEntity != null && NotupdatedOrdersEntity.Count > 0)
					{
						foreach(var item in NotupdatedOrdersEntity)
						{
							Notupdated.Add(new Reporting.Models.FANotUpdatedFromExcelModel(FAEntities?.FirstOrDefault(x => x.Fertigungsnummer == item.FA), ""));
						}
					}

				}
				Lists = new Reporting.Models.FAWerkUpdateReportModel { IdUpdate = updateEntity.Id, Updated = Updated, NotUpdated = Notupdated, Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}" };
				responseBody = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = Lists,
					BodyTemplate = "CRP_FAUpdateWerk",
					DocumentTitle = "",
					FooterBgColor = new iText.Kernel.Colors.DeviceRgb(220, 220, 220),
					FooterCenterText = "",
					FooterCenterText2 = "",
					FooterData = null,
					FooterLeftText = "",
					FooterTemplate = "CRP_Footer",
					FooterWithCounter = true,
					HasFooter = true,
					HasHeader = false,
					HeaderFirstPageOnly = false,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = false,
					HeaderText = "",
					Logo = null,
					Rotate = false
				});
				//Module.CRP_ReportingService.GenerateWerkTerminUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_WERK_TERMIN, Lists);

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