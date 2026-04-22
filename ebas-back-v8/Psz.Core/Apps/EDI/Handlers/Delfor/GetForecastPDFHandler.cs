using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.Reporting.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	using System.Threading.Tasks;

	public class GetForecastPDFHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastPDFHandler(Identity.Models.UserModel user, int lineItemId)
		{
			this._user = user;
			this._data = lineItemId;
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

				/// 
				var lineItemEntity = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(this._data);
				var headerEntity = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(lineItemEntity.HeaderId);
				var lineItemPlanEntities = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(new List<long> { this._data });
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(lineItemEntity.SuppliersItemMaterialNumber);
				var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(headerEntity.PSZCustomernumber ?? -1);
				var reportData = new Reporting.Models.DelforRepotModel
				{
					Header = new Psz.Core.Reporting.Models.DLFModels.HeaderModel(headerEntity, adress, article?.Abladestelle),
					LineItem = new Psz.Core.Reporting.Models.DLFModels.LineItemModel(lineItemEntity),
					HeaderLabel = new Psz.Core.Reporting.Models.DLFModels.I18N.HeaderModel(Psz.Core.Reporting.Models.DLFModels.I18N.Language.DE),
					LineItemLabel = new Psz.Core.Reporting.Models.DLFModels.I18N.LineItemModel(Psz.Core.Reporting.Models.DLFModels.I18N.Language.DE),
					LineItemPlan = lineItemPlanEntities?.Select(x => new Psz.Core.Reporting.Models.DLFModels.LineItemPlanModel(x))?.ToList(),
					LineItemPlanLabel = new Psz.Core.Reporting.Models.DLFModels.I18N.LineItemPlanModel(Psz.Core.Reporting.Models.DLFModels.I18N.Language.DE)
				};
				var response = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = reportData,
					BodyTemplate = "CTS_deliveryPlan_Body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterData = new DelforRepotFooterModel
					{
						FooterLine1 = reportData.Header.FooterLine1,
						FooterLine2 = reportData.Header.FooterLine2,
						FooterLine3 = reportData.Header.FooterLine3
					},
					FooterLeftText = "",
					FooterTemplate = "CTS_deliveryPlan_Footer",
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

			var lineItemEntity = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(this._data);
			if(lineItemEntity == null)
				return await ResponseModel<byte[]>.FailureResponseAsync("Forecast Line Item not found");

			if(Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(lineItemEntity.HeaderId) == null)
				return await ResponseModel<byte[]>.FailureResponseAsync("Forecast document not found");

			var lineItemPlanEntities = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(new List<long> { this._data });
			if(lineItemPlanEntities == null || lineItemPlanEntities.Count <= 0)
				return await ResponseModel<byte[]>.FailureResponseAsync("No delivery plans in Forecast");

			return await ResponseModel<byte[]>.SuccessResponseAsync();
		}
	}
}