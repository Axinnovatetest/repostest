using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Handlers.Administration.Users
{
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Administration.Users.GetResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Administration.Users.GetResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var userItem = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data);
				var uprofileEntities = Infrastructure.Data.Access.Tables.MTM.AccessProfileUsersAccess.GetByUserId(new List<int> { this._data })
					?.FindAll(x => x.UserId == userItem.Id);
				var profileEntites = Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.Get(uprofileEntities?.Select(x => x.AccessProfileId)?.ToList())
					//?.Where(x => x.ModuleActivated == true)
					?.Where(x => uprofileEntities?.FindIndex(y => y.AccessProfileId == x.Id) >= 0)?.ToList();

				// -
				var companyItem = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(userItem?.CompanyId ?? -1);
				var departmentItem = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userItem?.DepartmentId ?? -1);

				// -
				return ResponseModel<Models.Administration.Users.GetResponseModel>.SuccessResponse(new Models.Administration.Users.GetResponseModel(userItem, companyItem, departmentItem, profileEntites));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<Models.Administration.Users.GetResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Administration.Users.GetResponseModel>.AccessDeniedResponse();
			}

			// - 
			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<Models.Administration.Users.GetResponseModel>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data) == null)
				return ResponseModel<Models.Administration.Users.GetResponseModel>.FailureResponse(key: "2", value: "User not found");

			return ResponseModel<Models.Administration.Users.GetResponseModel>.SuccessResponse();
		}

	}
}
