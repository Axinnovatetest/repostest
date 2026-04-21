using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.ProductionWorkload.Models.Data;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.ProductionWorkload.Handlers
{
	public class GetWorkloadBacklogWeeksHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<WorkloadBacklogResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetWorkloadBacklogWeeksHandler(Identity.Models.UserModel user, int data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<List<WorkloadBacklogResponseModel>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				// -
				var responseBody = Infrastructure.Data.Access.Tables.MGO.ProductionWorkloadAccess.GetBacklogData(_data)
					?.Select(x => new WorkloadBacklogResponseModel
					{
						Index = 0,
						Week = x.Value,
						Year = x.Key
					})?.OrderBy(x => x.Year)?.ThenBy(x => x.Week)?.ToList();

				for(int i = 0; i < responseBody.Count; i++)
				{
					responseBody[i].Index = i + 1;
				}
				return ResponseModel<List<WorkloadBacklogResponseModel>>.SuccessResponse(responseBody);
				;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<WorkloadBacklogResponseModel>> Validate()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<WorkloadBacklogResponseModel>>.AccessDeniedResponse();
			}

			// -
			if(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data) == null)
			{
				return ResponseModel<List<WorkloadBacklogResponseModel>>.FailureResponse($"Warehouse not found");
			}
			// - 
			return ResponseModel<List<WorkloadBacklogResponseModel>>.SuccessResponse();
		}
	}
}
