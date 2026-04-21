using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetDelforErrorsCountHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetDelforErrorsCountHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var myCustomersIds = new List<string> { };
				var myCustomers = new OrderProcessing.GetMyCustomersHandler(true, _user).Handle();
				if(myCustomers.Success)
				{
					myCustomersIds = myCustomers.Body.Select(x => x.Duns.ToString()).ToList();
				}
				var response = Infrastructure.Data.Access.Tables.CTS.ErrorAccess.GetCountBySendersDuns(myCustomersIds, false);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
