using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Administration.Users
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Administration.Users.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Administration.Users.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.GetActive();
				var uprofileEntities = Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(userEntities?.Select(x => x.Id)?.ToList());
				var profileEntites = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(uprofileEntities?.Select(x => x.AccessProfileId)?.ToList())
					?.Where(x => x.ModuleActivated == true)?.ToList();
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get();

				var responseBody = new List<Models.Administration.Users.GetModel> { };
				foreach(var userItem in userEntities)
				{
					var uprofileItems = uprofileEntities.FindAll(x => x.UserId == userItem.Id);
					var profileItems = profileEntites.FindAll(x => uprofileItems?.FindIndex(y => y.AccessProfileId == x.Id) >= 0);
					var companyItem = companyEntities.Find(x => x.Id == userItem.CompanyId);
					var departmentItem = departmentEntities.Find(x => x.Id == userItem.DepartmentId);
					responseBody.Add(new Models.Administration.Users.GetModel(userItem, companyItem, departmentItem, profileItems));
				}

				return ResponseModel<List<Models.Administration.Users.GetModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Administration.Users.GetModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Administration.Users.GetModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<Models.Administration.Users.GetModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<Models.Administration.Users.GetModel>>.SuccessResponse();
		}
	}
}
