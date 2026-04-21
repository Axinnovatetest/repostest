using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateDeptJointLandHandler: IHandle<Models.Budget.GetDeptJointLandModel, ResponseModel<int>>
	{
		private Models.Budget.GetDeptJointLandModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateDeptJointLandHandler(Models.Budget.GetDeptJointLandModel data, Identity.Models.UserModel user)
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

				var JointedDepartmententity = new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity()
				{
					ID = _data.ID,
					ID_Department = _data.ID_Department,
					ID_Land = _data.ID_Land,
					ID_user = _data.ID_user,
					EmailUser = _data.EmailUser
				};
				//var updatedJointedDepartment = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.Update(JointedDepartmententity);
				return ResponseModel<int>.SuccessResponse(/*updatedJointedDepartment*/0);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.ConfigEditArtikel)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			//var JointID = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.Get(this._data.ID);
			var errors = new List<ResponseModel<int>.ResponseError>();
			//if (JointID == null)
			//{
			//    return new ResponseModel<int>()
			//    {
			//        Errors = new List<ResponseModel<int>.ResponseError>() {
			//            new ResponseModel<int>.ResponseError {Key = "", Value = "Jointed Department not found"}
			//        }
			//    };
			//}
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
