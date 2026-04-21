using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetCTSDashboardSummeryByCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<DashboardSummaryResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ManagementOverview.CTS.Models.DashboardRequestModel _data { get; set; }

		public GetCTSDashboardSummeryByCustomerHandler(Identity.Models.UserModel user, ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<DashboardSummaryResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var date = this._data.DateTill ?? DateTime.Today.AddDays(7 * 5);
				return ResponseModel<DashboardSummaryResponseModel>.SuccessResponse(new DashboardSummaryResponseModel(
					Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSDashboardSummaryByCustomerAccess(date, this._data.CustomerName), date, this._data.CustomerName));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<DashboardSummaryResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<DashboardSummaryResponseModel>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<DashboardSummaryResponseModel>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<DashboardSummaryResponseModel>.SuccessResponse();
		}
	}
}
