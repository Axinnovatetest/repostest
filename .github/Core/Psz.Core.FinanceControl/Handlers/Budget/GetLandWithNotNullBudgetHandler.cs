using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetLandWithNotNullBudgetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetLandAssignementModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		private int _data2 { get; set; }
		public GetLandWithNotNullBudgetHandler(Identity.Models.UserModel user, string land_name, int year)
		{
			this._user = user;
			this._data = land_name;
			this._data2 = year;
		}
		public ResponseModel<List<Models.Budget.GetLandAssignementModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetLandAssignementModel>();
				var Lands_tableEntities = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Get(this._data, this._data2);

				if(Lands_tableEntities != null)
				{
					foreach(var dep_tableEntity in Lands_tableEntities)
					{
						responseBody.Add(new Models.Budget.GetLandAssignementModel(dep_tableEntity));
					}
				}

				return ResponseModel<List<Models.Budget.GetLandAssignementModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.GetLandAssignementModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.GetLandAssignementModel>>.SuccessResponse();
		}
	}
}
