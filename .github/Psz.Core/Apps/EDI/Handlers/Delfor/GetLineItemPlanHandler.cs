using System;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;
	using System.Linq;

	public class GetLineItemPlanHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Delfor.XMLLineItemPlanModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetLineItemPlanHandler(Identity.Models.UserModel user, int lineItemId)
		{
			this._user = user;
			this._data = lineItemId;
		}

		public ResponseModel<Models.Delfor.XMLLineItemPlanModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var lineItemPlanEntity = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(this._data);
				var odrUId = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(lineItemPlanEntity.OrderUserId ?? -1);
				var prodUId = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(lineItemPlanEntity.ProductionUserId ?? -1);

				var lineItemPlansOrders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByLineItemPlanIds(new List<long> { this._data });
				var lineItemPlansOrdersQty = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetTotalQuantityByAB(lineItemPlansOrders?.Select(x => x.Nr)?.ToList());
				var absTotalQty = lineItemPlansOrdersQty?.Sum(x => x.Value) ?? 0;
				return ResponseModel<Models.Delfor.XMLLineItemPlanModel>.SuccessResponse(new Models.Delfor.XMLLineItemPlanModel(lineItemPlanEntity, odrUId, prodUId, lineItemPlansOrders?.Count ?? 0, (lineItemPlanEntity.PlanningQuantityQuantity ?? 0) - absTotalQty));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Delfor.XMLLineItemPlanModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Delfor.XMLLineItemPlanModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(this._data) == null)
			{
				return ResponseModel<Models.Delfor.XMLLineItemPlanModel>.FailureResponse("Line Item Plan not found");
			}

			return ResponseModel<Models.Delfor.XMLLineItemPlanModel>.SuccessResponse();
		}
	}
}