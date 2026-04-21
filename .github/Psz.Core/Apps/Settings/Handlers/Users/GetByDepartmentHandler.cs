using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.User
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByDepartmentHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Settings.Models.Users.UpdateModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }

		public GetByDepartmentHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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
				var userEntites = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(new List<int> { this._data }) ?? new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
				var companyEntites = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(userEntites?.Select(x => x.CompanyId.HasValue ? (long)x.CompanyId.Value : -1)?.ToList());
				var departmentItem = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data);
				var response = new List<Settings.Models.Users.UpdateModel>();

				foreach(var userEntity in userEntites)
				{
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

			if(Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data) == null)
				return ResponseModel<List<Settings.Models.Users.UpdateModel>>.FailureResponse("department not found");

			return ResponseModel<List<Settings.Models.Users.UpdateModel>>.SuccessResponse();
		}
	}
}
