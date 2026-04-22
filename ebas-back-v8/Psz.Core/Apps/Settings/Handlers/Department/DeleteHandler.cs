using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Department
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
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

				/// 
				var result = -1;
				var departmentEntiy = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data);
				if(departmentEntiy != null)
				{
					departmentEntiy.IsArchived = true;
					departmentEntiy.ArchiveTime = DateTime.Now;
					departmentEntiy.ArchiveUserId = this._user.Id;
					result = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Update(departmentEntiy);
				}

				return ResponseModel<int>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var departmentEmployees = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(new List<int> { this._data });
			if(departmentEmployees != null && departmentEmployees.Count > 0)
				return ResponseModel<int>.FailureResponse($"Department contains users");

			var deptartmentAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear((int)this._data, DateTime.Today.Year);
			if(deptartmentAllocation != null)
				return ResponseModel<int>.FailureResponse($"Department has budget allocation, please remove it before changing company.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
