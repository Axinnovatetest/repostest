using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetForecastLineItemsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Delfor.XMLLineItemModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastLineItemsHandler(Identity.Models.UserModel user, int headerId)
		{
			this._user = user;
			this._data = headerId;
		}

		public ResponseModel<List<Models.Delfor.XMLLineItemModel>> Handle()
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

				return ResponseModel<List<Models.Delfor.XMLLineItemModel>>.SuccessResponse(lineItemEntities?.Select(x => new Models.Delfor.XMLLineItemModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Delfor.XMLLineItemModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Delfor.XMLLineItemModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(this._data) == null)
			{
				return ResponseModel<List<Models.Delfor.XMLLineItemModel>>.FailureResponse("Forecast document not found");
			}

			return ResponseModel<List<Models.Delfor.XMLLineItemModel>>.SuccessResponse();
		}
	}
}