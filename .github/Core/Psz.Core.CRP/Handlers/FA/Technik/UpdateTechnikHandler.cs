using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Technik
{
	public class UpdateTechnikHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FATechnikModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateTechnikHandler(FATechnikModel data, Identity.Models.UserModel user)
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
				var _oldEntity = Infrastructure.Data.Access.Tables.CTS.Fertigung_PlanungsdetailsAccess.Get(technikEntity.ID);
				var response = Infrastructure.Data.Access.Tables.CTS.Fertigung_PlanungsdetailsAccess.Update(technikEntity);
				//logging
				var Logs = GetLogs(_oldEntity, technikEntity);
				if(Logs != null && Logs.Count > 0)
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(Logs);

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
		public List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.CTS.Fertigung_PlanungsdetailsEntity _old
			, Infrastructure.Data.Entities.Tables.CTS.Fertigung_PlanungsdetailsEntity _new)
		{
			var FAEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get((int)_old.ID_Fertigung);
			var LOG = new Helpers.LogHelper((int)FAEntity.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONTECHNICIEN, "CTS", _user);
			var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			if(_old.Aktion != _new.Aktion)
			{
				_logs.Add(LOG.LogCTS("Aktion", _old.Aktion, _new.Aktion, _old.ID));
			}
			if(_old.Details != _new.Details)
			{
				_logs.Add(LOG.LogCTS("Details", _old.Details, _new.Details, _old.ID));
			}
			if(_old.Mitarbeiter != _new.Mitarbeiter)
			{
				_logs.Add(LOG.LogCTS("Mitarbeiter", _old.Mitarbeiter, _new.Mitarbeiter, _old.ID));
			}
			if(_old.Termin != _new.Termin)
			{
				_logs.Add(LOG.LogCTS("Termin", _old.Termin.ToString(), _new.Termin.ToString(), _old.ID));
			}
			if(_old.Status != _new.Status)
			{
				_logs.Add(LOG.LogCTS("Status", _old.Status.ToString(), _new.Status.ToString(), _old.ID));
			}
			return _logs;
		}
	}
}