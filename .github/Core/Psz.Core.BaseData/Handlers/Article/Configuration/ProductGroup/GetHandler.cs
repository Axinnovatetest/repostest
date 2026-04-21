using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ProductGroup
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.ProductGroup.ProductGroup>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.ProductGroup.ProductGroup> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.ProductGroup.ProductGroup>.SuccessResponse(
						new Models.Article.ProductGroup.ProductGroup(Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.ProductGroup.ProductGroup> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ProductGroup.ProductGroup>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.ProductGroup.ProductGroup>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.ProductGroup.ProductGroup>.ResponseError>
					{
						new ResponseModel<Models.Article.ProductGroup.ProductGroup>.ResponseError
						{
							Key = "1",
							Value = "Product Group not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.ProductGroup.ProductGroup>.SuccessResponse();
		}
	}
}
