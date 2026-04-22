using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Main.Handlers.ActiveDirectory.Groups
{
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetUsersHandler: IHandle<Identity.Models.UserModel, Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.UserModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }

		public GetUsersHandler(Identity.Models.UserModel user, string groupName)
		{
			this._user = user;
			this._data = groupName;
		}

		public Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.UserModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 

				var usersAd = new List<Core.Models.Main.ActiveDirectory.Groups.UserModel>();

				//if AD not Activated
				if(!Core.Program.ActiveDirectoryManager.IsActivated)
				{
					for(int i = 0; i < 40; i++)
					{
						usersAd.Add(new Core.Models.Main.ActiveDirectory.Groups.UserModel
						{
							Id = -1,
							Email = "",
							UserName = "not_ad_user_" + (i + 1),
							FirstName = "Not AD User " + (i + 1),
							LastName = "Not AD User " + (i + 1),
						});
					}
				}
				else
				{
					usersAd = Core.Program.ActiveDirectoryManager.GetUsersByGroup(this._data)?.Select(x => new Core.Models.Main.ActiveDirectory.Groups.UserModel(x))?.ToList();
				}

				return Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.UserModel>>.SuccessResponse(usersAd);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.UserModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.UserModel>>.AccessDeniedResponse();
			}


			return Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.UserModel>>.SuccessResponse();
		}
	}
}
