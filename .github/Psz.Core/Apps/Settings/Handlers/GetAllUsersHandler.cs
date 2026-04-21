using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Apps.Settings.Handlers
{
	public class GetAllUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Users.UserModel>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetAllUsersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<List<Models.Users.UserModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<Models.Users.UserModel>();

				var usersEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get();
				var accessProfilesIds = usersEntities.Select(e => e.AccessProfileId).ToList();
				var accessProfilesDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(accessProfilesIds);
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(usersEntities?.Select(x => (long?)x.CompanyId ?? -1)?.ToList());
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(usersEntities?.Select(x => (long?)x.DepartmentId ?? -1)?.ToList());

				foreach(var userDb in usersEntities)
				{
					var companyItem = companyEntities?.Find(x => x.Id == userDb.CompanyId);
					var departmentItem = departmentEntities?.Find(x => x.Id == userDb.DepartmentId);
					var profileEntity = accessProfilesDb.Find(e => e.Id == userDb.AccessProfileId);
					response.Add(new Models.Users.UserModel(userDb, profileEntity, companyItem, departmentItem));
				}

				return ResponseModel<List<Models.Users.UserModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Users.UserModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Users.UserModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Users.UserModel>>.SuccessResponse();
		}

	}
}
