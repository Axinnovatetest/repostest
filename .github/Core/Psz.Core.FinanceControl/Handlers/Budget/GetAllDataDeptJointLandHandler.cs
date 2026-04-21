using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetAllDataDeptJointLandHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.AllDataDeptJointLandModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetAllDataDeptJointLandHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.AllDataDeptJointLandModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.AllDataDeptJointLandModel>();
				//var alldatajointDeptsLand_tableEntities = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.GetAllDataJointDeptLand();


				//foreach (var alldatajointdeptLand_tableEntity in alldatajointDeptsLand_tableEntities)
				//{
				//    responseBody.Add(new Models.Budget.AllDataDeptJointLandModel(alldatajointdeptLand_tableEntity));
				//}

				return ResponseModel<List<Models.Budget.AllDataDeptJointLandModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.AllDataDeptJointLandModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.AllDataDeptJointLandModel>>.SuccessResponse();

		}
	}
}
