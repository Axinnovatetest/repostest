using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetAllDataOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.AllDataOrderModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetAllDataOrderHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.AllDataOrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.AllDataOrderModel>();
				//var alldataorders_tableEntities = Infrastructure.Data.Access.Tables.FNC.Budget_OrderAccess.GetAllData();


				//foreach (var alldataorder_tableEntity in alldataorders_tableEntities)
				//{
				//    responseBody.Add(new Models.Budget.AllDataOrderModel(alldataorder_tableEntity));
				//}

				return ResponseModel<List<Models.Budget.AllDataOrderModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.AllDataOrderModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.AllDataOrderModel>>.SuccessResponse();

		}
	}
}
