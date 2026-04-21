using System;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetLineItemNextVesionHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Delfor.XMLLineItemModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetLineItemNextVesionHandler(Identity.Models.UserModel user, int lineItemId)
		{
			this._user = user;
			this._data = lineItemId;
		}

		public ResponseModel<Models.Delfor.XMLLineItemModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<Models.Delfor.XMLLineItemModel>.SuccessResponse(new Models.Delfor.XMLLineItemModel(Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetNextVersion(this._data)));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Delfor.XMLLineItemModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Delfor.XMLLineItemModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(this._data) == null)
			{
				return ResponseModel<Models.Delfor.XMLLineItemModel>.FailureResponse("Line Item not found");
			}

			return ResponseModel<Models.Delfor.XMLLineItemModel>.SuccessResponse();
		}
	}
}