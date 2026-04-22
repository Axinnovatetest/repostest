using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.User
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByDepartmentDirectorHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Settings.Models.Users.UpdateModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetByDepartmentDirectorHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Settings.Models.Users.UpdateModel>> Handle()
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
					: Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(this._user.Id);
				var userEntites = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(departmentEntities?.Select(x => (int)x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
				var companyEntites = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(departmentEntities?.Select(x => x.CompanyId)?.ToList());
				var response = new List<Settings.Models.Users.UpdateModel>();
				foreach(var userEntity in userEntites)
				{
					var departmentItem = departmentEntities?.Find(x => x.Id == userEntity.DepartmentId);
					var companyItem = companyEntites?.Find(x => x.Id == userEntity.CompanyId);
					response.Add(new Settings.Models.Users.UpdateModel(userEntity, departmentItem, companyItem));
				}

				return ResponseModel<List<Settings.Models.Users.UpdateModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Settings.Models.Users.UpdateModel>> Validate()
		{
			if(this._user == null/* || this._user.Access.____*/)
			{
				return ResponseModel<List<Settings.Models.Users.UpdateModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Settings.Models.Users.UpdateModel>>.FailureResponse("user not found");

			return ResponseModel<List<Settings.Models.Users.UpdateModel>>.SuccessResponse();
		}
	}
}
