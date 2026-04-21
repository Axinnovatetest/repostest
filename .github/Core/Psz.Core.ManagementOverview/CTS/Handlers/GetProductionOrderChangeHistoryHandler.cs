using Infrastructure.Data.Entities.Joins.MGO;
using Psz.Core.Common.Helpers;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetProductionOrderChangeHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<GetProductionOrderChangeHistoryResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private GetProductionOrderChangeHistoryRequestModel _data { get; set; }
		public GetProductionOrderChangeHistoryHandler(Identity.Models.UserModel user, GetProductionOrderChangeHistoryRequestModel data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<List<GetProductionOrderChangeHistoryResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				if(!this._user.IsGlobalDirector && !this._user.IsAdministrator &&  this._user.Access.ManagementOverview.CtsPlanning == false)
				{
					return ResponseModel<List<GetProductionOrderChangeHistoryResponseModel>>.SuccessResponse(null);
				}

				var responseBody = new List<GetProductionOrderChangeHistoryResponseModel>();
				// -  brought in FZ
				var inFZ = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetProductionOrderHistoryPerWarehouse(this._data.DateFrom, this._data.DateTo, 0);
				// -  sent out FZ
				var outFZ = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetProductionOrderHistoryPerWarehouse(this._data.DateFrom, this._data.DateTo, 1);
				var warehouseIds = Module.AppSettingsBSD.ProductionLagerIds;

				if(warehouseIds?.Any() == true)
				{
					foreach(var item in warehouseIds)
					{
						var changeItemsIn = (inFZ?.Where(x => x.ProductionOrderWarehouseId == item)) ?? Enumerable.Empty<ProductionOrderChangeHistoryWarehouseEntity>();
						var changeItemsOut = (outFZ?.Where(x => x.ProductionOrderWarehouseId == item)) ?? Enumerable.Empty<ProductionOrderChangeHistoryWarehouseEntity>();
						
						var stornoInFAs = changeItemsIn.FirstOrDefault(x => x.ProductionOrderStatus == "storno")?.ProductionOrderTime;
						var erledigtInFAs = changeItemsIn.FirstOrDefault(x => x.ProductionOrderStatus == "erledigt")?.ProductionOrderTime;
						var offenInFAs = changeItemsIn.FirstOrDefault(x => x.ProductionOrderStatus == "offen")?.ProductionOrderTime;						
						
						var stornoOutFAs = changeItemsOut.FirstOrDefault(x => x.ProductionOrderStatus == "storno")?.ProductionOrderTime;
						var erledigtOutFAs = changeItemsOut.FirstOrDefault(x => x.ProductionOrderStatus == "erledigt")?.ProductionOrderTime;
						var offenInOuts = changeItemsOut.FirstOrDefault(x => x.ProductionOrderStatus == "offen")?.ProductionOrderTime;

						responseBody.Add(new GetProductionOrderChangeHistoryResponseModel
						{
							ProductionOrderWarehouseId = item,

							ProductionOrderCount_CancelledIn = MathHelper.RoundDecimal(stornoInFAs ?? 0, 2) ,
							ProductionOrderCount_DoneIn = MathHelper.RoundDecimal(erledigtInFAs ?? 0, 2),
							ProductionOrderCount_OpenIn = MathHelper.RoundDecimal(offenInFAs ?? 0, 2),

							ProductionOrderCount_CancelledOut = MathHelper.RoundDecimal(stornoOutFAs ?? 0, 2),
							ProductionOrderCount_DoneOut = MathHelper.RoundDecimal(erledigtOutFAs ?? 0, 2),
							ProductionOrderCount_OpenOut = MathHelper.RoundDecimal(offenInOuts ?? 0, 2),
						});
					}
				}

				// -
				return ResponseModel<List<GetProductionOrderChangeHistoryResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<GetProductionOrderChangeHistoryResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<GetProductionOrderChangeHistoryResponseModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<GetProductionOrderChangeHistoryResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<GetProductionOrderChangeHistoryResponseModel>>.SuccessResponse();
		}
	}
}
