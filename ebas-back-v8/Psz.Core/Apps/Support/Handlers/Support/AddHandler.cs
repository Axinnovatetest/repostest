using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Support.Handlers.Request
{
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Request.ProjectRequest _data { get; set; }

		public AddHandler(Identity.Models.UserModel user, Models.Request.ProjectRequest data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var ticket = new Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity()
				{
					Application = _data.Application,
					Benefits = _data.Benefits,
					BuisnessProcess = _data.BuisnessProcess,
					Consequences = _data.Consequences,
					Date = _data.Date,
					Department = _data.Department,
					Dependencies = _data.Dependencies,
					Goals = _data.Goals,
					OtherApplication = _data.OtherApplications,
					OtherReason = _data.OtherReasons,
					Problem = _data.Problem,
					Reason = _data.Reason,
					Requester = _data.Requester,
					Requirement = _data.Requirement,
					Theme = _data.Theme,
					CreationTime = DateTime.Now,
					CreationUserId = _user.Id,
					Status = 0
				};
				var ticketId = Infrastructure.Data.Access.Tables.Support.Request.RequestAccess.Insert(ticket);

				var userTicket = this._data.Users.Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.User_RequestEntity
				{
					Department = x.Department,
					Email = x.Email,
					Name = x.FullName,
					Phone = x.Phone,
					RequestId = ticketId,
				}).ToList();
				Infrastructure.Data.Access.Tables.Support.Request.User_RequestAccess.Insert(userTicket);

				var requiredProcess = this._data.RequirementProcess.Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.Requirement_ProcessEntity
				{
					RequestId = ticketId,
					Description = x.Description,
					Name = x.Name,
					TestUseCase = x.TestUseCase
				}).ToList();
				Infrastructure.Data.Access.Tables.Support.Request.Requirement_ProcessAccess.Insert(requiredProcess);

				return ResponseModel<int>.SuccessResponse(ticketId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
