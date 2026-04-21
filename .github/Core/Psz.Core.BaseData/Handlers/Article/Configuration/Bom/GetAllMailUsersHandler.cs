using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Bom
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllMailUsersHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Configuration.Bom.MailUserModel>>>
	{
		private UserModel _user { get; set; }
		public GetAllMailUsersHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Configuration.Bom.MailUserModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var productionPlaceEntites = Infrastructure.Data.Access.Tables.BSD.BomMailUsersAccess.Get();
				if(productionPlaceEntites != null && productionPlaceEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.Configuration.Bom.MailUserModel>>.SuccessResponse(productionPlaceEntites
							.Select(x => new Models.Article.Configuration.Bom.MailUserModel(x)).Distinct().ToList());
				}

				return ResponseModel<List<Models.Article.Configuration.Bom.MailUserModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.Bom.MailUserModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Configuration.Bom.MailUserModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Configuration.Bom.MailUserModel>>.SuccessResponse();
		}
	}
}
