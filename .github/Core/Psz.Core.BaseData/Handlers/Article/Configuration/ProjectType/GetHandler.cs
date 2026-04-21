using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ProjectType
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.ProjectType.ProjectType>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.ProjectType.ProjectType> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.ProjectType.ProjectType>.SuccessResponse(
						new Models.Article.ProjectType.ProjectType(Infrastructure.Data.Access.Tables.BSD.ProjectTypeAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.ProjectType.ProjectType> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ProjectType.ProjectType>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.ProjectTypeAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.ProjectType.ProjectType>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.ProjectType.ProjectType>.ResponseError>
					{
						new ResponseModel<Models.Article.ProjectType.ProjectType>.ResponseError
						{
							Key = "1",
							Value = "Project Type not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.ProjectType.ProjectType>.SuccessResponse();
		}
	}
}
