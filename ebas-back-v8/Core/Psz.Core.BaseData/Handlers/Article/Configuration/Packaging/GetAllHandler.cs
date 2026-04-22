using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Packaging
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Packaging.Packaging>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Packaging.Packaging>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var packagingEntites = Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Get();
				if(packagingEntites != null && packagingEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.Packaging.Packaging>>.SuccessResponse(packagingEntites
							.Select(x => new Models.Article.Packaging.Packaging(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Packaging.Packaging>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Packaging.Packaging>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Packaging.Packaging>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Packaging.Packaging>>.SuccessResponse();
		}
	}
}
