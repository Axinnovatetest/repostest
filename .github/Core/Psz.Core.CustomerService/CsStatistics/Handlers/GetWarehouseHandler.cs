using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetWarehouseHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetWarehouseHandler(Identity.Models.UserModel user)
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

				var response = new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(-1, "All") };
				response.AddRange(Infrastructure.Data.Access.Joins.CTS.Divers.GetCTSStatsWarehouse(Module.BSD.ProductionLagerIds)
					?? new List<KeyValuePair<int, string>>());
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
