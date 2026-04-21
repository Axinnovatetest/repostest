using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Administration.Users
{
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Administration.Users.GetModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Administration.Users.GetModel> Handle()
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
				var uprofileEntities = Infrastructure.Data.Access.Tables.BSD.AccessProfileUsersAccess.GetByUserId(new List<int> { this._data })
					?.FindAll(x => x.UserId == userItem.Id);
				var profileEntites = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(uprofileEntities?.Select(x => x.AccessProfileId)?.ToList())
					?.Where(x => x.ModuleActivated == true)
					?.Where(x => uprofileEntities?.FindIndex(y => y.AccessProfileId == x.Id) >= 0)?.ToList();
				//TO DO
				/// 
				var bsdLagerEntity = Enum.GetValues(typeof(Identity.Enums.BaseDataEmuns.BaseDataLager)).Cast<Identity.Enums.BaseDataEmuns.BaseDataLager>().ToList();
				var Lagers = bsdLagerEntity
							.Select(x => new KeyValuePair<int, string>((int)x, $"{x.GetDescription()}".Trim())).Distinct().ToList();
				foreach(var item in profileEntites)
				{
					var lagers = Infrastructure.Data.Access.Tables.BSD.BSD_AccessProfileLagerAccess.GetByAccessProfileIds(new List<int> { item.Id });
					List<KeyValuePair<int, string>> profileLagers = new List<KeyValuePair<int, string>>();
					foreach(var l in lagers)
					{
						profileLagers.Add(new KeyValuePair<int, string>(
							(int)l.LagerId,
							Lagers.FirstOrDefault(x => x.Key == l.LagerId).Value
							));
					}
					item.LagerIds = profileLagers;
				}
				//fill profile lager
				var companyItem = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(userItem?.CompanyId ?? -1);
				var departmentItem = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userItem?.DepartmentId ?? -1);

				return ResponseModel<Models.Administration.Users.GetModel>.SuccessResponse(new Models.Administration.Users.GetModel(userItem, companyItem, departmentItem, profileEntites));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<Models.Administration.Users.GetModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Administration.Users.GetModel>.AccessDeniedResponse();
			}

			// - 
			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<Models.Administration.Users.GetModel>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data) == null)
				return ResponseModel<Models.Administration.Users.GetModel>.FailureResponse(key: "2", value: "User not found");

			return ResponseModel<Models.Administration.Users.GetModel>.SuccessResponse();
		}

	}
}
