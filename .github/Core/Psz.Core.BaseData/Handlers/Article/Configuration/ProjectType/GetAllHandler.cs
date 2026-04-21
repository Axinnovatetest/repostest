using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ProjectType
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.ProjectType.ProjectType>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.ProjectType.ProjectType>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var projectTypeEntites = Infrastructure.Data.Access.Tables.BSD.ProjectTypeAccess.Get();
				if(projectTypeEntites != null && projectTypeEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.ProjectType.ProjectType>>.SuccessResponse(projectTypeEntites
							.Select(x => new Models.Article.ProjectType.ProjectType(x)).ToList());
				}

				return ResponseModel<List<Models.Article.ProjectType.ProjectType>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ProjectType.ProjectType>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ProjectType.ProjectType>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.ProjectType.ProjectType>>.SuccessResponse();
		}
	}
}
