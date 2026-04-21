using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Technik
{
	public class DeleteTechnikHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteTechnikHandler(int data, Identity.Models.UserModel user)
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

				//logging
				var technikerEntity = Infrastructure.Data.Access.Tables.CTS.Fertigung_PlanungsdetailsAccess.Get(this._data);
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get((int)technikerEntity.ID_Fertigung);
				var response = Infrastructure.Data.Access.Tables.CTS.Fertigung_PlanungsdetailsAccess.Delete(this._data);
				var _log = new Helpers.LogHelper((int)faEntity.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.DELETIONTECHNICIEN, "CTS", _user)
				 .LogCTS(null, null, null, technikerEntity.ID);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}