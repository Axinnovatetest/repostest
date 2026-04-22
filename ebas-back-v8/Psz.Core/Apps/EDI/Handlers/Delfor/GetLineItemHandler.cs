using System;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetLineItemHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Delfor.XMLLineItemModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetLineItemHandler(Identity.Models.UserModel user, int lineItemId)
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
				var position = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(this._data);
				var nextVersions = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetNextOrPreviousVersion(position.DocumentNumber, position.PositionNumber,
					position.HeaderVersion ?? 0, 1);
				var previousVersions = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetNextOrPreviousVersion(position.DocumentNumber, position.PositionNumber,
					position.HeaderVersion ?? 0, 2);
				var response = new Models.Delfor.XMLLineItemModel(position);

				response.NextVersionId = nextVersions != null && nextVersions.Count > 0
					? (int)nextVersions.FirstOrDefault(x => x.Id == nextVersions.Min(y => y.Id)).Id
					: null;
				response.PreviousVersionId = previousVersions != null && previousVersions.Count > 0
					? (int)previousVersions.FirstOrDefault(x => x.Id == previousVersions.Max(y => y.Id)).Id
					: null;
				return ResponseModel<Models.Delfor.XMLLineItemModel>.SuccessResponse(response);
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