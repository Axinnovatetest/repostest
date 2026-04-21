using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetLandAssignementCurrentHandler
	{
		public Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }
		//***
		public GetLandAssignementCurrentHandler(Identity.Models.UserModel user, int id_user)
		{
			this._user = user;
			this._data = id_user;
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
				var lands_tableEntities = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetByUser(this._data);

				if(lands_tableEntities != null)
				{
					foreach(var land_tableEntity in lands_tableEntities)
					{
						responseBody.Add(new Models.Budget.GetLandAssignementModel(land_tableEntity));
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

