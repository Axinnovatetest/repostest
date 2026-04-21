using Psz.Core.Apps.Support.Models.Request;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Apps.Support.Handlers.Request
{
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<ProjectRequest>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public int _id { get; set; }
		public GetHandler(Identity.Models.UserModel user, int id)
		{
			_id = id;
			_user = user;
		}

		public ResponseModel<ProjectRequest> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var ticketEntity = Infrastructure.Data.Access.Tables.Support.Request.RequestAccess.Get(_id);

				return ResponseModel<ProjectRequest>.SuccessResponse(new ProjectRequest(ticketEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<ProjectRequest> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<ProjectRequest>.AccessDeniedResponse();
			}

			return ResponseModel<ProjectRequest>.SuccessResponse();
		}
	}
}
