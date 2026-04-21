
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Support.Handlers.Request
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Request.ProjectRequest _data { get; set; }
		private int _ticketStatus { get; set; }

		public EditHandler(Identity.Models.UserModel user, Models.Request.ProjectRequest data, int ticketStatus)
		{
			this._user = user;
			this._data = data;
			this._ticketStatus = ticketStatus;
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
				var ticket = Infrastructure.Data.Access.Tables.Support.Request.RequestAccess.Get(_data.Id);

				ticket.ItConcept = _data.ItConcept;
				ticket.ItConditions = _data.ItConditions;
				ticket.ItConditions = _data.ItConditions;
				ticket.ItEffort = _data.ItEffort;
				ticket.ItFeasibility = _data.ItFeasibility;
				ticket.ItNr = _data.ItNr;
				ticket.LastEditTime = DateTime.Now;
				ticket.LastEditUserId = _user.Id;
				ticket.Status = ticket.Status != _ticketStatus ? _ticketStatus : ticket.Status;
				ticket.Priority = _data.Priority;
				ticket.Application = _data.Application;
				ticket.Benefits = _data.Benefits;
				ticket.BuisnessProcess = _data.BuisnessProcess;
				ticket.Consequences = _data.Consequences;
				ticket.Date = _data.Date;
				ticket.Department = _data.Department;
				ticket.Dependencies = _data.Dependencies;
				ticket.Goals = _data.Goals;
				ticket.OtherApplication = _data.OtherApplications;
				ticket.OtherReason = _data.OtherReasons;
				ticket.Problem = _data.Problem;
				ticket.Reason = _data.Reason;
				ticket.Requester = _data.Requester;
				ticket.Requirement = _data.Requirement;
				ticket.Theme = _data.Theme;
				if(_ticketStatus == 1)
				{
					ticket.Validated = true;
					ticket.ValidationDate = DateTime.Now;
					ticket.ValidationUserId = _user.Id;
				}

				else if(_ticketStatus == 2)
				{
					ticket.Validated = false;
					ticket.ValidationDate = DateTime.Now;
					ticket.ValidationUserId = _user.Id;
				}

				Infrastructure.Data.Access.Tables.Support.Request.Requirement_ProcessAccess.DeleteByRequestd(ticket.Id);
				Infrastructure.Data.Access.Tables.Support.Request.User_RequestAccess.DeleteByRequestId(ticket.Id);
				Infrastructure.Data.Access.Tables.Support.Request.SignatureAccess.DeleteByRequestId(ticket.Id);

				var requiredProcess = this._data.RequirementProcess.Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.Requirement_ProcessEntity
				{
					RequestId = ticket.Id,
					Description = x.Description,
					Name = x.Name,
					TestUseCase = x.TestUseCase
				}).ToList();
				Infrastructure.Data.Access.Tables.Support.Request.Requirement_ProcessAccess.Insert(requiredProcess);


				var userTicket = this._data.Users.Select(x => new Infrastructure.Data.Entities.Tables.Support.Request.User_RequestEntity
				{
					Department = x.Department,
					Email = x.Email,
					Name = x.FullName,
					Phone = x.Phone,
					RequestId = ticket.Id,
				}).ToList();

				Infrastructure.Data.Access.Tables.Support.Request.User_RequestAccess.Insert(userTicket);


				var signatures = new List<Infrastructure.Data.Entities.Tables.Support.Request.SignatureEntity>();


				_data.Signatures.ForEach(x => signatures.Add(new Infrastructure.Data.Entities.Tables.Support.Request.SignatureEntity
				{
					Date = x.Date,
					FirstName = x.FirstName,
					Function = x.Function,
					LastName = x.LastName,
					Signature = "",
					RequestId = ticket.Id,
				}));

				Infrastructure.Data.Access.Tables.Support.Request.SignatureAccess.Insert(signatures);

				Infrastructure.Data.Access.Tables.Support.Request.RequestAccess.Update(ticket);

				return ResponseModel<int>.SuccessResponse(ticket.Id);
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
