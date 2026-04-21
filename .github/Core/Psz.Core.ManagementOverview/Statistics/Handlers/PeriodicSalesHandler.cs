using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Statistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.ManagementOverview.Statistics.Handlers
{
	public class PeriodicSalesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<GetSalesResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public PeriodicSalesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<GetSalesResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				///
				var periodicSaleEntities = Infrastructure.Data.Access.Tables.Statistics.MGO.PeriodicSalesAccess.Get();

				var responseBody = new List<Models.GetSalesResponseModel> { };
				if(periodicSaleEntities is not null)
				{
					foreach(var item in periodicSaleEntities)
					{
						responseBody.Add(new Models.GetSalesResponseModel
						{
							Year = item.Year,
							KW = item.KW,
							InvoiceAmount = item.InvoiceAmount,
							StockFGWoUBGAmount = item.StockFGWoUBGAmount,
							StockFGUBGAmount = item.StockFGUBGAmount,
							StockROHAmount = item.StockROHAmount,
							OrderAmount = item.OrderAmount,
							ProductionOrderFinishedAmount = item.ProductionOrderFinishedAmount,
							ProductionOrderFinishedHours = item.ProductionOrderFinishedHours,
							ProductionOrderPlannedAmount = item.ProductionOrderPlannedAmount,
							ProductionOrderPlannedHours = item.ProductionOrderPlannedHours
						});
					}
				}

				return ResponseModel<List<GetSalesResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<GetSalesResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<GetSalesResponseModel>>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<GetSalesResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<GetSalesResponseModel>>.SuccessResponse();
		}




	}
}
