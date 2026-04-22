using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ProductGroup
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.ProductGroup.ProductGroup>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.ProductGroup.ProductGroup>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var WarengruppenEntites = Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get();
				if(WarengruppenEntites != null && WarengruppenEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.ProductGroup.ProductGroup>>.SuccessResponse(WarengruppenEntites
							.Select(x => new Models.Article.ProductGroup.ProductGroup(x)).ToList());
				}

				return ResponseModel<List<Models.Article.ProductGroup.ProductGroup>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ProductGroup.ProductGroup>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ProductGroup.ProductGroup>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.ProductGroup.ProductGroup>>.SuccessResponse();
		}
	}
}
