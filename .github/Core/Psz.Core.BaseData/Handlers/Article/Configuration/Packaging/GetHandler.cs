using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Packaging
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.Packaging.Packaging>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Packaging.Packaging> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Packaging.Packaging>.SuccessResponse(
						new Models.Article.Packaging.Packaging(Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Packaging.Packaging> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Packaging.Packaging>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.Packaging.Packaging>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.Packaging.Packaging>.ResponseError>
					{
						new ResponseModel<Models.Article.Packaging.Packaging>.ResponseError
						{
							Key = "1",
							Value = "Packaging Type not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.Packaging.Packaging>.SuccessResponse();
		}
	}
}
