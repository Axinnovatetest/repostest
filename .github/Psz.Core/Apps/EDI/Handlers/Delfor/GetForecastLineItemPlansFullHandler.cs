using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetForecastLineItemPlansFullByHeaderHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Delfor.XMLLineItemPlanFullModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastLineItemPlansFullByHeaderHandler(Identity.Models.UserModel user, int headerId)
		{
			this._user = user;
			this._data = headerId;
		}

		public ResponseModel<List<Models.Delfor.XMLLineItemPlanFullModel>> Handle()
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

				var response = new List<Models.Delfor.XMLLineItemPlanFullModel>();
				foreach(var item in lineItemEntities)
				{
					var plans = lineItemPlansEntities.FindAll(x => x.LineItemId == item.Id);
					response.AddRange(plans?.Select(x => new Models.Delfor.XMLLineItemPlanFullModel(item, x))?.ToList());
				}

				return ResponseModel<List<Models.Delfor.XMLLineItemPlanFullModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Delfor.XMLLineItemPlanFullModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Delfor.XMLLineItemPlanFullModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(this._data) == null)
			{
				return ResponseModel<List<Models.Delfor.XMLLineItemPlanFullModel>>.FailureResponse("Forecast not found");
			}

			return ResponseModel<List<Models.Delfor.XMLLineItemPlanFullModel>>.SuccessResponse();
		}
	}
}