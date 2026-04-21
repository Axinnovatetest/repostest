using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ProjectClass
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.ProjectClass.AddRequestModel>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.ProjectClass.AddRequestModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var projectCEntites = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.Get();
				if(projectCEntites != null && projectCEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.ProjectClass.AddRequestModel>>.SuccessResponse(
						projectCEntites
							.Select(x => new Models.Article.ProjectClass.AddRequestModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.ProjectClass.AddRequestModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ProjectClass.AddRequestModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ProjectClass.AddRequestModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.ProjectClass.AddRequestModel>>.SuccessResponse();
		}
	}
}
