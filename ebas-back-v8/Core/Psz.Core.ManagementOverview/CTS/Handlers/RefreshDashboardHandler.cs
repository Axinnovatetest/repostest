using Infrastructure.Data.Entities.Joins.MGO;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class RefreshDashboardHandler: IHandle<Identity.Models.UserModel, ResponseModel<DashboardRefreshResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private RefreshDashboardRequestModel _data { get; set; }

		public RefreshDashboardHandler(Identity.Models.UserModel user, RefreshDashboardRequestModel data)
		{
			this._user = user;
			_data = data;
		}


		public ResponseModel<DashboardRefreshResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.RefreshData((int)this._data.statisticType);
				return new ResponseModel<DashboardRefreshResponseModel>
				{
					Success = true,
					Body = new DashboardRefreshResponseModel
					{
						RefreshDate = DateTime.Now
					}
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<DashboardRefreshResponseModel> HandleCheck()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				CTSRefreshEntity Result = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.CheckDataRefreshDate((int)this._data.statisticType);

				return new ResponseModel<DashboardRefreshResponseModel>
				{
					Success = true,
					Body = new DashboardRefreshResponseModel
					{
						RefreshDate = Result.RefreshDate
					}
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<DashboardRefreshResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<DashboardRefreshResponseModel>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<DashboardRefreshResponseModel>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<DashboardRefreshResponseModel>.SuccessResponse();
		}
	}
}
