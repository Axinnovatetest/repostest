using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	//public  class SupplementLandBDGHandler : IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.supplementLandBdgModel>>
	public class SupplementLandBDGHandler
	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public SupplementLandBDGHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;

		}
		public ResponseModel<List<Models.Budget.SupplementLandModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.SupplementLandModel>();

				var supplement = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.GetSupplementLandBdg(this._data);
				//var IDLand = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Get(this._data);

				//  responseBody.LandBudget = supplement.LandBudget;
				//  responseBody.Supplement_Land = supplement.Supplement_Land;
				//  responseBody.SOMME_Supplement_Land = supplement.SOMME_Supplement_Land;
				//  responseBody.Land = supplement.Land;
				//  responseBody.Year = supplement.Year;
				//  responseBody.Creation_Date = supplement.Creation_Date;
				//return ResponseModel<Models.Budget.supplementLandBdgModel>.SuccessResponse(responseBody);

				if(supplement != null)
				{
					foreach(var land_supp in supplement)
					{
						responseBody.Add(new Models.Budget.SupplementLandModel(land_supp));
					}
				}

				return ResponseModel<List<Models.Budget.SupplementLandModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.SupplementLandModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.SupplementLandModel>>.SuccessResponse();


		}

	}
}
