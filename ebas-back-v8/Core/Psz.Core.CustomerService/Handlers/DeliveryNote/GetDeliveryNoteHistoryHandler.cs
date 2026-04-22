using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.DeliveryNote;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.DeliveryNote
{
	public class GetDeliveryNoteHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DeliveryNoteHistoryModel>>>
	{
		private string _projectNr { get; set; }
		private string _type { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetDeliveryNoteHistoryHandler(Identity.Models.UserModel user, string project_nr, string type)
		{
			this._user = user;
			this._projectNr = project_nr;
			this._type = type;
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
				var historyEntity = Infrastructure.Data.Access.Tables.CTS.DeliveryNoteHistoryAccess.GetDeliveryNoteHistory(this._projectNr, this._type);
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

			return ResponseModel<List<DeliveryNoteHistoryModel>>.SuccessResponse();
		}
	}
}
