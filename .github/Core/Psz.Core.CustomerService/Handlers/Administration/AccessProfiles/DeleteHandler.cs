using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Administration.AccessProfiles
{
	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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
				Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.Delete(this._data);
				Infrastructure.Data.Access.Tables.CTS.AccessProfileUsersAccess.DeleteByAccessProfileID(this._data);

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.Get(this._data) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
