using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Department
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Settings.Models.Department.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
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
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get() ?? new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Settings.Models.Department.GetModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Settings.Models.Department.GetModel>>.SuccessResponse();
		}
	}
}
