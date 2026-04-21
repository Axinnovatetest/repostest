using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.Administration.Handlers.AccessProfiles
{
	public class GetUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private List<int> _data { get; set; }

		public GetUsersHandler(Identity.Models.UserModel user, List<int> ids)
		{
			this._user = user;
			this._data = ids;
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
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(

						System.Linq.Enumerable.DistinctBy(Infrastructure.Data.Access.Tables.MGO.AccessProfileUsersAccess.GetByAccessProfileIds(this._data) ?? null, x => x.UserId)
						?.Select(x => new KeyValuePair<int, string>(x.UserId ?? -1, x.UserName))
						?.ToList());
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
