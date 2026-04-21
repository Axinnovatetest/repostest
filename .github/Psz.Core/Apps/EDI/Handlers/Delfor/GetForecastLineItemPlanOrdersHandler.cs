using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetForecastLineItemPlanOrdersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Delfor.LineItemPlanOrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastLineItemPlanOrdersHandler(Identity.Models.UserModel user, int headerId)
		{
			this._user = user;
			this._data = headerId;
		}

		public ResponseModel<List<Models.Delfor.LineItemPlanOrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var response = new List<Models.Delfor.LineItemPlanOrderModel>();
				var lineItemEntity = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(this._data);
				var orderEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByLineItemPlanIds(new List<long> { this._data });
				var orderQuantities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetTotalQuantityByAB(orderEntities?.Select(x => x.Nr)?.ToList());
				foreach(var item in orderEntities)
				{
					response.Add(new Models.Delfor.LineItemPlanOrderModel(lineItemEntity, item, orderQuantities.Where(x => x.Key == item.Nr)?.Sum(x => x.Value) ?? 0));
				}

				return ResponseModel<List<Models.Delfor.LineItemPlanOrderModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Delfor.LineItemPlanOrderModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Delfor.LineItemPlanOrderModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(this._data) == null)
			{
				return ResponseModel<List<Models.Delfor.LineItemPlanOrderModel>>.FailureResponse("Forecast not found");
			}

			return ResponseModel<List<Models.Delfor.LineItemPlanOrderModel>>.SuccessResponse();
		}
	}
}