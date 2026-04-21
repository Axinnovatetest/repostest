using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Department
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Settings.Models.Department.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetByUserHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Settings.Models.Department.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var departmentEntities = this._user.IsGlobalDirector
					? Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get()
					: Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(this._user.Id) ?? new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
				var response = new List<Settings.Models.Department.GetModel>();
				foreach(var departmentEntity in departmentEntities)
				{
					response.Add(new Settings.Models.Department.GetModel(departmentEntity));
				}

				return ResponseModel<List<Settings.Models.Department.GetModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Settings.Models.Department.GetModel>> Validate()
		{
			if(this._user == null/* || this._user.Access.____*/)
			{
				return ResponseModel<List<Settings.Models.Department.GetModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Settings.Models.Department.GetModel>>.FailureResponse("user not found");

			return ResponseModel<List<Settings.Models.Department.GetModel>>.SuccessResponse();
		}
	}
}
