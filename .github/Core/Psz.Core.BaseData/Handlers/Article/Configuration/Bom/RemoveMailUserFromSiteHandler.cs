using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Bom
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class RemoveMailUserFromSiteHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.Bom.MailUserRemoveModel _data { get; set; }
		public RemoveMailUserFromSiteHandler(UserModel user, Models.Article.Configuration.Bom.MailUserRemoveModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.UserId);
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.BomMailUsersAccess.DeleteUserFromSite(this._data.UserId, this._data.SiteId));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.UserId);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse("User not found.");

			var productionPlaceEntites = Enum.GetValues(typeof(Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)).Cast<Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace>().ToList();
			if(productionPlaceEntites.Find(x => ((int)x) == this._data.SiteId) < 0)
				return ResponseModel<int>.FailureResponse("Site not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
