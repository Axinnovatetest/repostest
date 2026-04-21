using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetForecastLineItemPlansByHeaderHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Delfor.XMLLineItemPlanModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastLineItemPlansByHeaderHandler(Identity.Models.UserModel user, int headerId)
		{
			this._user = user;
			this._data = headerId;
		}

		public ResponseModel<List<Models.Delfor.XMLLineItemPlanModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var lineItemEntities = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetByHeaderId(this._data);
				var lineItemPlansEntities = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(lineItemEntities?.Select(x => x.Id)?.ToList());
				var odrUserIds = lineItemPlansEntities.Select(x => x.OrderUserId ?? -1)?.ToList();
				var prodUserIds = lineItemPlansEntities.Select(x => x.ProductionUserId ?? -1)?.ToList();
				var userIds = new List<int>();
				userIds.AddRange(odrUserIds);
				userIds.AddRange(prodUserIds);
				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userIds);

				var lineItemPlansOrders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByLineItemPlanIds(lineItemPlansEntities?.Select(x => x.Id)?.ToList());
				var lineItemPlansOrdersQty = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetTotalQuantityByAB(lineItemPlansOrders?.Select(x => x.Nr)?.ToList());

				// - 
				var response = new List<Models.Delfor.XMLLineItemPlanModel>();
				foreach(var item in lineItemPlansEntities)
				{
					var odrUId = userEntities.Find(x => x.Id == item.OrderUserId);
					var prodUId = userEntities.Find(x => x.Id == item.ProductionUserId);
					var lt = lineItemPlansOrders?.Where(y => y.nr_dlf == item.Id)?.ToList();
					var absTotalQty = lineItemPlansOrdersQty?.Where(y => lt.Any(z => z.Nr == y.Key))?.Sum(x => x.Value) ?? 0;
					response.Add(new Models.Delfor.XMLLineItemPlanModel(item, odrUId, prodUId, lt?.Count ?? 0, (item.PlanningQuantityQuantity ?? 0) - absTotalQty));
				}

				return ResponseModel<List<Models.Delfor.XMLLineItemPlanModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Delfor.XMLLineItemPlanModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Delfor.XMLLineItemPlanModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(this._data) == null)
			{
				return ResponseModel<List<Models.Delfor.XMLLineItemPlanModel>>.FailureResponse("Forecast not found");
			}

			return ResponseModel<List<Models.Delfor.XMLLineItemPlanModel>>.SuccessResponse();
		}
	}
}