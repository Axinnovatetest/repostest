using Psz.Core.Apps.Support.Models.Request;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Support.Handlers.Request
{
	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<ProjectRequest>>>
	{
		public UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<ProjectRequest>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// Filter by user id
				var requestEntities = new List<Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity>();
				if(_user.Id != -2)
					requestEntities = Infrastructure.Data.Access.Tables.Support.Request.RequestAccess.GetByUsers(new List<int> { _user.Id });
				else
					requestEntities = Infrastructure.Data.Access.Tables.Support.Request.RequestAccess.Get();

				return ResponseModel<List<ProjectRequest>>.SuccessResponse(requestEntities.Select(x => new ProjectRequest(x)).ToList().OrderByDescending(x => x.Id).ToList());


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ProjectRequest>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<ProjectRequest>>.AccessDeniedResponse();
			}

			return ResponseModel<List<ProjectRequest>>.SuccessResponse();
		}
	}
}
