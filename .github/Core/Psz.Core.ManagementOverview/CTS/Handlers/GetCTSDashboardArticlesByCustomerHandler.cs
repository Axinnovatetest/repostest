using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetCTSDashboardArticlesByCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ManagementOverview.CTS.Models.DashboardRequestModel _data { get; set; }

		public GetCTSDashboardArticlesByCustomerHandler(Identity.Models.UserModel user, ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			this._user = user;
			_data = data;
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

				// - 
				var responseBody = new List<CTSDashboardItemModel>();

				// - 
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSDashboardArticlesByCustomerAccess(this._data.DateTill ?? DateTime.Today.AddDays(7 * 5), this._data.CustomerName));
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
