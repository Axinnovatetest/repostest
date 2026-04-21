using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetForecastPDFHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastPDFHandler(Identity.Models.UserModel user, int lineItemId)
		{
			this._user = user;
			this._data = lineItemId;
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

				/// 
				var lineItemEntity = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(this._data);
				var headerEntity = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(lineItemEntity.HeaderId);
				var lineItemPlanEntities = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(new List<long> { this._data });
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(lineItemEntity.SuppliersItemMaterialNumber);
				var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(headerEntity.PSZCustomernumber ?? -1);

				var reportData = Program.ReportingService.Generate_CTS_DeliveryPlan(
					 new Infrastructure.Services.Reporting.Models.CTS.DLFModels.HeaderModel(headerEntity, adress, article?.Abladestelle),
					 new Infrastructure.Services.Reporting.Models.CTS.DLFModels.LineItemModel(lineItemEntity),
					 lineItemPlanEntities?.Select(x => new Infrastructure.Services.Reporting.Models.CTS.DLFModels.LineItemPlanModel(x))?.ToList(),
					 new Infrastructure.Services.Reporting.Models.CTS.DLFModels.I18N.HeaderModel(Infrastructure.Services.Reporting.Models.CTS.DLFModels.I18N.Language.DE),
					 new Infrastructure.Services.Reporting.Models.CTS.DLFModels.I18N.LineItemModel(Infrastructure.Services.Reporting.Models.CTS.DLFModels.I18N.Language.DE),
					 new Infrastructure.Services.Reporting.Models.CTS.DLFModels.I18N.LineItemPlanModel(Infrastructure.Services.Reporting.Models.CTS.DLFModels.I18N.Language.DE)
					 );
				return ResponseModel<byte[]>.SuccessResponse(reportData);
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

			var lineItemEntity = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(this._data);
			if(lineItemEntity == null)
				return ResponseModel<byte[]>.FailureResponse("Forecast Line Item not found");

			if(Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(lineItemEntity.HeaderId) == null)
				return ResponseModel<byte[]>.FailureResponse("Forecast document not found");

			var lineItemPlanEntities = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(new List<long> { this._data });
			if(lineItemPlanEntities == null || lineItemPlanEntities.Count <= 0)
				return ResponseModel<byte[]>.FailureResponse("No delivery plans in Forecast");

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}