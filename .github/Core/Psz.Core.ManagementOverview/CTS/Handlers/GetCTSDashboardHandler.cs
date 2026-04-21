using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetCTSDashboardHandler: IHandle<Identity.Models.UserModel, ResponseModel<DashboardResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ManagementOverview.CTS.Models.DashboardRequestModel _data { get; set; }

		public GetCTSDashboardHandler(Identity.Models.UserModel user, ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<DashboardResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var results = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSDashboardAccess(this._data.DateTill ?? DateTime.Today.AddDays(7 * 5), this._data.CustomerName);
				// - 
				var totalAmount = 0m;
				var immAmount = 0m;
				var prodAmount = 0m;
				var responseBody = new List<CTSDashboardItemModel>();
				foreach(var item in results)
				{
					totalAmount += item.ABGesamt ?? 0;
					immAmount += item.ImmediatAmount ?? 0;
					prodAmount += item.ProductionAmount ?? 0;
					responseBody.Add(new CTSDashboardItemModel(item));
				}

				// - 
				return ResponseModel<DashboardResponseModel>.SuccessResponse(new DashboardResponseModel
				{
					Items = responseBody,
					DateTill = this._data.DateTill ?? DateTime.Today.AddDays(7 * 5),
					TotalAmount = totalAmount,
					ImmediatAmount = immAmount,
					ProductionAMount = prodAmount,
					Results = immAmount + prodAmount
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<DashboardResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<DashboardResponseModel>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<DashboardResponseModel>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<DashboardResponseModel>.SuccessResponse();
		}

	}
}
