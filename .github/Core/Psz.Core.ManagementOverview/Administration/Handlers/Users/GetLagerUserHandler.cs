using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Administration.Handlers.Users
{
	public class GetLagerUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetLagerUserHandler(Identity.Models.UserModel user)
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

				/// 
				var lagerUser = Infrastructure.Data.Access.Tables.COR.UserAccess.GetLagerUser(this._user.CompanyId);

				// -
				return ResponseModel<int>.SuccessResponse(lagerUser);
				;
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

			// - 
			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
