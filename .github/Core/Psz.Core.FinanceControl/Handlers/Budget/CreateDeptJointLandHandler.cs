using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateDeptJointLandHandler
	{
		private Models.Budget.GetDeptJointLandModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateDeptJointLandHandler(Models.Budget.GetDeptJointLandModel data, Identity.Models.UserModel user)
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
				var DeptJointLandEntity = new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity()
				{
					ID = _data.ID,
					ID_Department = _data.ID_Department,
					ID_Land = _data.ID_Land,
					ID_user = _data.ID_user,
					EmailUser = _data.EmailUser
				};
				//var InsertedDeptJointLand = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.InsertDeptJointLand(DeptJointLandEntity);
				return ResponseModel<int>.SuccessResponse(/*InsertedDeptJointLand*/0);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.ConfigCreateLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			if(ResponseModel<int>.SuccessResponse().Body == -1)
			{
				return ResponseModel<int>.ExistItemResponse();


			}
			else
			{ return ResponseModel<int>.SuccessResponse(); }
		}
	}
}
