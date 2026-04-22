using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class SearchOrderNsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Reception.FilterRequestModel _data { get; set; }

		public SearchOrderNsHandler(Identity.Models.UserModel user, Models.Budget.Reception.FilterRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.FilterReceptionOrderNumbers(this._data.Value, this._data.InProgressOnly, (Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Types)this._data.OrderType) ?? new List<KeyValuePair<int, string>>();
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(bestellungEntities);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
