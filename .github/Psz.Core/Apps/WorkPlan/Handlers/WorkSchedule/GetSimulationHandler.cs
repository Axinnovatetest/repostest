using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetSimulationHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.WorkSchedule.GetSimulationResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.WorkSchedule.GetSimulationRequestModel _data;

		public GetSimulationHandler(Identity.Models.UserModel user, Models.WorkSchedule.GetSimulationRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<Models.WorkSchedule.GetSimulationResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var details = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByWorkScheduleId(this._data.WorkScheduleId)
					?? new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity>();
				var response = new Models.WorkSchedule.GetSimulationResponseModel
				{
					WorkScheduleId = this._data.WorkScheduleId,
					FASize = this._data.FASize,
					TotalOperationValueAdding = 0,
					TotalOperationTime = 0,
					Ratio = 0
				};

				foreach(var item in details)
				{
					item.LotSizeSTD = Convert.ToInt32(this._data.FASize);
					var _item = Core.Apps.WorkPlan.Helpers.WorkSchedule.setTotalTimeOperation(item);
					_item.OperationTimeValueAdding = Core.Apps.WorkPlan.Helpers.WorkSchedule.GetOperationTimeValueAddng(_item);
					response.TotalOperationValueAdding += Math.Round(_item.OperationTimeValueAdding, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
					response.TotalOperationTime += Math.Round(_item.TotalTimeOperation, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
				}

				response.Ratio = response.TotalOperationValueAdding > 0
						? (decimal)Math.Round(((double)response.TotalOperationTime - response.TotalOperationValueAdding) / response.TotalOperationValueAdding, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS)
						: 0;
				return ResponseModel<Models.WorkSchedule.GetSimulationResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.WorkSchedule.GetSimulationResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.WorkSchedule.GetSimulationResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(this._data.WorkScheduleId) == null)
				return ResponseModel<Models.WorkSchedule.GetSimulationResponseModel>.FailureResponse("work plan not found");

			return ResponseModel<Models.WorkSchedule.GetSimulationResponseModel>.SuccessResponse();
		}
	}
}
