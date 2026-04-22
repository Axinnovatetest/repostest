using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Support.Handlers.Request
{
	public class ValidateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Request.ProjectRequest _data { get; set; }
		private int _ticketStatus { get; set; }

		public ValidateHandler(Identity.Models.UserModel user, Models.Request.ProjectRequest data, int ticketStatus)
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
				var request = Infrastructure.Data.Access.Tables.Support.Request.RequestAccess.Get(_data.Id);
				request.ItConcept = _data.ItConcept;
				request.ItConditions = _data.ItConditions;
				request.ItConditions = _data.ItConditions;
				request.ItEffort = _data.ItEffort;
				request.ItFeasibility = _data.ItFeasibility;
				request.ItNr = _data.ItNr;
				request.LastEditTime = DateTime.Now;
				request.LastEditUserId = _user.Id;
				request.Status = _ticketStatus;
				request.Priority = _data.Priority;
				request.ValidationUserId = _user.Id;
				request.ValidationDate = DateTime.Now;
				request.Validated = true;

				var signatures = new List<Infrastructure.Data.Entities.Tables.Support.Request.SignatureEntity>();

				Infrastructure.Data.Access.Tables.Support.Request.SignatureAccess.DeleteByRequestId(request.Id);


				_data.Signatures.ForEach(x => signatures.Add(new Infrastructure.Data.Entities.Tables.Support.Request.SignatureEntity
				{
					Date = x.Date,
					FirstName = x.FirstName,
					Function = x.Function,
					LastName = x.LastName,
					Signature = "",
					RequestId = request.Id,
				}));

				Infrastructure.Data.Access.Tables.Support.Request.SignatureAccess.Insert(signatures);

				Infrastructure.Data.Access.Tables.Support.Request.RequestAccess.Update(request);

				return ResponseModel<int>.SuccessResponse(1);
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
