using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetStorageLocationsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetStorageLocationsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				var bestellungEntities = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForOrderReception(Module.BookingLagerIds) ?? new List<Infrastructure.Data.Entities.Tables.INV.LagerorteEntity>();
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
						bestellungEntities.Select(x => new KeyValuePair<int, string>(x.LagerortId, $"{x.LagerortId} | {x.Lagerort}"))?.ToList());
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
