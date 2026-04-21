using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.DeliveryNote;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.DeliveryNote
{
	public class GetABHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DeliveryNoteHistoryModel>>>
	{
		private string _projectNr { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetABHistoryHandler(Identity.Models.UserModel user, string project_nr)
		{
			this._user = user;
			this._projectNr = project_nr;
		}

		public ResponseModel<List<DeliveryNoteHistoryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var historyEntity = Infrastructure.Data.Access.Tables.CTS.DeliveryNoteHistoryAccess.GetABHistory(this._projectNr);
				List<DeliveryNoteHistoryModel> _response = new List<DeliveryNoteHistoryModel>();
				if(historyEntity != null && historyEntity.Count > 0)
				{
					foreach(var item in historyEntity)
					{
						_response.Add(new DeliveryNoteHistoryModel(item));
					}

				}
				return ResponseModel<List<DeliveryNoteHistoryModel>>.SuccessResponse(_response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<DeliveryNoteHistoryModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<DeliveryNoteHistoryModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByProjectNr(this._projectNr) == null)
				return ResponseModel<List<DeliveryNoteHistoryModel>>.FailureResponse("Document not found.");

			return ResponseModel<List<DeliveryNoteHistoryModel>>.SuccessResponse();
		}
	}
}
