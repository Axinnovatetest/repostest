using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.ProductionWorkload.Models.Data;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.ProductionWorkload.Handlers
{
	public class GetWorkloadHandler: IHandle<Identity.Models.UserModel, ResponseModel<WorkloadResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetWorkloadHandler(Identity.Models.UserModel user, int data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<WorkloadResponseModel> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				// -
				var responseBody = new WorkloadResponseModel();
				var results = Infrastructure.Data.Access.Tables.MGO.ProductionWorkloadAccess.GetLastData(_data);
				//var maxCapacityEntities = Infrastructure.Data.

				if(results != null && results.Count > 0)
				{
					responseBody = new WorkloadResponseModel
					{
						LastEditTime = results[0].RecordSyncTime ?? DateTime.MinValue,
						Items = results?.Select(x => new WorkloadResponseModel.WorkloadItem
						{
							ArticleCount = x.FaArticleCount ?? 0,
							FaCount = x.FaCount ?? 0,
							FaTotalQuantity = x.FaTotalQuantity ?? 0,
							Index = 0,
							TotalHours = x.FaTime ?? 0,
							Week = x.FaWeek ?? 0,
							WeekName = $"{x.FaYear ?? 0}/{x.FaWeek ?? 0}",
							Year = x.FaYear ?? 0,
							MaxCapacity = x.WarehouseMaxCapacity ?? 0,
						})?.OrderBy(x => x.Year)?.ThenBy(x => x.Week)?.ToList()
					};

					for(int i = 0; i < responseBody.Items.Count; i++)
					{
						responseBody.Items[i].Index = i + 1;
					}
				}
				return ResponseModel<WorkloadResponseModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<WorkloadResponseModel> Validate()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<WorkloadResponseModel>.AccessDeniedResponse();
			}

			// -
			if(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data) == null)
			{
				return ResponseModel<WorkloadResponseModel>.FailureResponse($"Warehouse not found");
			}
			// - 
			return ResponseModel<WorkloadResponseModel>.SuccessResponse();
		}
	}
}
