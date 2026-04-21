using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Technik
{
	public class AddTechnikHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FATechnikModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AddTechnikHandler(FATechnikModel data, Identity.Models.UserModel user)
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

				var technikEntity = this._data.ToEntity();
				var response = Infrastructure.Data.Access.Tables.CTS.Fertigung_PlanungsdetailsAccess.Insert(technikEntity);
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get((int)_data.ID_Fertigung);
				//logging
				var _log = new Helpers.LogHelper((int)faEntity.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.CREACREATIONTECHNICIEN, "CTS", _user)
					.LogCTS(null, null, null, response);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
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
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data.ID_Fertigung ?? -1);
			if(faEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"FA not found");
			return ResponseModel<int>.SuccessResponse();
		}
	}
}