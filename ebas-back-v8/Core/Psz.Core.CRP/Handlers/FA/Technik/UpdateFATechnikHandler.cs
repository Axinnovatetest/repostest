using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Technik
{
	public class UpdateFATechnikHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FADetailsModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateFATechnikHandler(FADetailsModel data, Identity.Models.UserModel user)
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

				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1);
				faEntity.Erstmuster = this._data.Erstmuster;
				faEntity.Technik = this._data.IsTechnik;
				faEntity.Techniker = this._data.Techniker;
				faEntity.Bemerkung_Technik = this._data.Bemerkung_Technik;
				faEntity.Quick_Area = this._data.Quick_Area;
				var _oldEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1);

				var response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(faEntity);
				//logging
				var Logs = GetLogs(_oldEntity, faEntity);
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
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1);
			if(faEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"FA not found");
			return ResponseModel<int>.SuccessResponse();
		}
		public List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _old,
	 Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _new)
		{
			var _Log = new Helpers.LogHelper((int)_old.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user);
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			if(_old.Erstmuster != _new.Erstmuster)
			{
				_logs.Add(_Log.LogCTS("Erstmuster", _old.Erstmuster?.ToString(), _new.Erstmuster?.ToString(), 0));
			}
			if(_old.Technik != _new.Technik)
			{
				_logs.Add(_Log.LogCTS("Technik", _old.Technik?.ToString(), _new.Technik?.ToString(), 0));
			}
			if(_old.Techniker != _new.Techniker)
			{
				_logs.Add(_Log.LogCTS("Techniker", _old.Techniker?.ToString(), _new.Techniker?.ToString(), 0));
			}
			if(_old.Bemerkung_Technik != _new.Bemerkung_Technik)
			{
				_logs.Add(_Log.LogCTS("Bemerkung_Technik", _old.Bemerkung_Technik?.ToString(), _new.Bemerkung_Technik?.ToString(), 0));
			}
			if(_old.Quick_Area != _new.Quick_Area)
			{
				_logs.Add(_Log.LogCTS("Quick_Area", _old.Quick_Area?.ToString(), _new.Quick_Area?.ToString(), 0));
			}
			return _logs;
		}
	}
}