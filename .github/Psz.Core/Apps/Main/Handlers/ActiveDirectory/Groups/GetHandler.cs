using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Main.Handlers.ActiveDirectory.Groups
{
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHandler: IHandle<Identity.Models.UserModel, Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 

				var usersAd = new List<Core.Models.Main.ActiveDirectory.Groups.GetModel>();

				if(!Core.Program.ActiveDirectoryManager.IsActivated)
				{//if AD not Activated
					for(int i = 0; i < 40; i++)
					{
						usersAd.Add(new Core.Models.Main.ActiveDirectory.Groups.GetModel
						{
							Id = -1,
							Email = "",
							GroupName = "not_ad_user_" + (i + 1),
							Name = "Not AD User " + (i + 1),
						});
					}
				}
				else
				{
					usersAd = Core.Program.ActiveDirectoryManager.GetGroupsInfo()?.Select(x => new Core.Models.Main.ActiveDirectory.Groups.GetModel(x))?.ToList();
				}

				return Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.GetModel>>.SuccessResponse(usersAd);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.GetModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.GetModel>>.AccessDeniedResponse();
			}


			return Core.Common.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.GetModel>>.SuccessResponse();
		}
	}
}
