using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateSupplementHandler
	{
		private Models.Budget.SupplementLandModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateSupplementHandler(Models.Budget.SupplementLandModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var SupplementEntity = new Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity()
				{
					Id_AL = _data.Id_AL,
					Supplement_Budget = _data.Supplement_Budget,
					Creation_Date = _data.Creation_Date,

				};
				var InsertedLand = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.InsertSupplement(SupplementEntity);
				return ResponseModel<int>.SuccessResponse(InsertedLand);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.AssignCreateLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
