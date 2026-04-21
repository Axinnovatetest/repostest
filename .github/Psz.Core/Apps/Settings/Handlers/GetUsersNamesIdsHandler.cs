using Infrastructure.Data.Access.Tables.COR;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public class GetUsersNamesIdsHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private string _filter;
		private UserModel _user;
		public GetUsersNamesIdsHandler(string filter, UserModel user)
		{
			_filter = filter;
			_user = user;
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

				var usersList = UserAccess.GetUnarchivedUsesList(_filter).Select(x => new Models.Users.UserModel(x)).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
												(usersList ?? new List<Models.Users.UserModel>())
												?.Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
												.ToList());

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
