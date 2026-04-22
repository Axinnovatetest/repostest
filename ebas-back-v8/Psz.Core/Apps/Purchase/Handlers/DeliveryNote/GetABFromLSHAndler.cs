using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class GetABFromLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<KeyValuePair<int, int>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _Nr { get; set; }

		public GetABFromLSHandler(Identity.Models.UserModel user, int nr)
		{
			_user = user;
			_Nr = nr;
		}
		public ResponseModel<KeyValuePair<int, int>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var LSItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._Nr);
				var ABItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(LSItem.Ab_id ?? -1);
				return ResponseModel<KeyValuePair<int, int>>.SuccessResponse(new KeyValuePair<int, int>(ABItem.Nr, int.TryParse(ABItem.Projekt_Nr, out var val) ? val : 0));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<KeyValuePair<int, int>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<KeyValuePair<int, int>>.AccessDeniedResponse();
			}
			var LSItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._Nr);
			if(LSItem == null)
				return ResponseModel<KeyValuePair<int, int>>.FailureResponse(key: "1", value: $"Delivery note not found");
			var ABItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(LSItem.Ab_id ?? -1);
			if(ABItem == null)
				return ResponseModel<KeyValuePair<int, int>>.FailureResponse(key: "1", value: $"Delivery does not belong to any AB");
			return ResponseModel<KeyValuePair<int, int>>.SuccessResponse();
		}
	}
}
