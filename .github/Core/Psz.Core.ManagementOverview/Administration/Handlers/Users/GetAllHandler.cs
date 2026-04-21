using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.Administration.Handlers.Users
{
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Administration.Models.Users.GetResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Administration.Models.Users.GetResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				///
				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get();
				var uprofileEntities = Infrastructure.Data.Access.Tables.MGO.AccessProfileUsersAccess.GetByUserId(userEntities?.Select(x => x.Id)?.ToList());
				var profileEntites = Infrastructure.Data.Access.Tables.MGO.AccessProfileAccess.Get(uprofileEntities?.Select(x => x.AccessProfileId ?? -1)?.ToList())
					?.Where(x => x.ModuleActivated == true)?.ToList();
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get();

				var responseBody = new List<Administration.Models.Users.GetResponseModel> { };
				foreach(var userItem in userEntities)
				{
					var x = 0;
					if(userItem.Username.ToLower() == "souilmi")
					{
						x = 1;
					}
					var uprofileItems = uprofileEntities.FindAll(x => x.UserId == userItem.Id);
					var profileItems = profileEntites.FindAll(x => uprofileItems?.FindIndex(y => y.AccessProfileId == x.Id) >= 0);
					var companyItem = companyEntities.Find(x => x.Id == userItem.CompanyId);
					var departmentItem = departmentEntities.Find(x => x.Id == userItem.CompanyId);
					responseBody.Add(new Administration.Models.Users.GetResponseModel(userItem, companyItem, departmentItem, profileItems));
					;
					;
				}

				return ResponseModel<List<Administration.Models.Users.GetResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Administration.Models.Users.GetResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Administration.Models.Users.GetResponseModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<Administration.Models.Users.GetResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<Administration.Models.Users.GetResponseModel>>.SuccessResponse();
		}
	}
}
