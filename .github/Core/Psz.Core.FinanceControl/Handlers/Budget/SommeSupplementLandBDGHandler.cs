using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class SommeSupplementLandBDGHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.SupplementLandBdgModel>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public SommeSupplementLandBDGHandler(Identity.Models.UserModel user, int ID)
		{
			this._user = user;
			this._data = ID;

		}
		public ResponseModel<Models.Budget.SupplementLandBdgModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.SupplementLandBdgModel();
				var Sum_supplement = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.SommeSupplementLandBdg(this._data);

				responseBody.ID = Sum_supplement.ID;
				responseBody.Land_Name = Sum_supplement.Land_Name;
				responseBody.budget = Sum_supplement.budget;
				responseBody.SOMME_Supplement_Land_Budget = Sum_supplement.SOMME_Supplement_Land_Budget;
				responseBody.B_year = Sum_supplement.B_year;

				return ResponseModel<Models.Budget.SupplementLandBdgModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.SupplementLandBdgModel> Validate()
		{

			//var suppBDG = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.SommeSupplementLandBdg(this._data);
			////var errors = new List<ResponseModel<int>.ResponseError>();
			//var LandBudget = suppBDG.LandBudget;
			//var SUMSuppBudget = suppBDG.SOMME_Supplement_Land_Budget;
			//var LandName = suppBDG.Land_Name;


			////***do sum
			//var total = LandBudget + SUMSuppBudget;
			//var reste = total - SUMDeptBudget;
			//if (reste < 0)
			//{ reste = 0; }


			return ResponseModel<Models.Budget.SupplementLandBdgModel>.SuccessResponse();
		}


	}
}
