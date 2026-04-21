using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteDeptJointLandHandler: IHandle<Models.Budget.GetDeptJointLandModel, ResponseModel<int>>
	{
		private int _DeptJointLandID { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteDeptJointLandHandler(int deptjointlandid, Identity.Models.UserModel user)
		{
			this._DeptJointLandID = deptjointlandid;
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

				//var deptjointentity = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.Get(_DeptJointLandID);
				//if (deptjointentity == null)
				//{
				//    return ResponseModel<int>.SuccessResponse();
				//}

				return ResponseModel<int>.SuccessResponse(/*Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.Delete(deptjointentity.ID)*/0);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.ConfigDeleteLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
