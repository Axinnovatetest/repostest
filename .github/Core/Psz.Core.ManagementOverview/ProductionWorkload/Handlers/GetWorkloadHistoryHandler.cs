using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.ProductionWorkload.Models.Data;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.ProductionWorkload.Handlers
{
	public class GetWorkloadHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<WorkloadHistoryResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private WorkloadHistoryRequestModel _data { get; set; }

		public GetWorkloadHistoryHandler(Identity.Models.UserModel user, WorkloadHistoryRequestModel data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<List<WorkloadHistoryResponseModel>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				// -
				var results = Infrastructure.Data.Access.Tables.MGO.ProductionWorkloadAccess.GetLastHistoryData(_data.WarehouseId, _data.Week, _data.Year)
					?.Select(x => new WorkloadHistoryResponseModel(x))?.OrderBy(x => x.SyncDate)?.ToList();
				for(int i = 0; i < results.Count; i++)
				{
					results[i].Index = i + 1;
				}
				return ResponseModel<List<WorkloadHistoryResponseModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<WorkloadHistoryResponseModel>> Validate()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<WorkloadHistoryResponseModel>>.AccessDeniedResponse();
			}

			// -
			if(_data.WarehouseId <= 0)
			{
				return ResponseModel<List<WorkloadHistoryResponseModel>>.FailureResponse($"Warehouse ID invalid");
			}
			if(_data.Week <= 0 || _data.Week > 53)
			{
				return ResponseModel<List<WorkloadHistoryResponseModel>>.FailureResponse($"Week [{_data.Week}] invalid");
			}
			if(_data.Year <= 0)
			{
				return ResponseModel<List<WorkloadHistoryResponseModel>>.FailureResponse($"Year [{_data.Year}] invalid");
			}
			if(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.WarehouseId) == null)
			{
				return ResponseModel<List<WorkloadHistoryResponseModel>>.FailureResponse($"Warehouse not found");
			}
			// - 
			return ResponseModel<List<WorkloadHistoryResponseModel>>.SuccessResponse();
		}
	}
}
