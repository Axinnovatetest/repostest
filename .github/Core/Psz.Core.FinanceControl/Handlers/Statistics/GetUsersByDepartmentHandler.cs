using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetUsersByDepartmentHandler: IHandle<UserModel, ResponseModel<IEnumerable<KeyValuePair<int, string>>>>
	{
		private readonly UserModel _user;
		private readonly int? _data;
		public GetUsersByDepartmentHandler(UserModel user, int? data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<IEnumerable<KeyValuePair<int, string>>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var users = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(new List<int> { _data ?? -1 });
			var response = users?.Select(x => new KeyValuePair<int, string>(x.Id, x.Name));

			return ResponseModel<IEnumerable<KeyValuePair<int, string>>>.SuccessResponse(response);
		}
		public ResponseModel<IEnumerable<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<IEnumerable<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}
			return ResponseModel<IEnumerable<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}