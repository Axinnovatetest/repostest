using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class UpdateFAauftragsbearbeitungHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FADetailsModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateFAauftragsbearbeitungHandler(FADetailsModel data, Identity.Models.UserModel user)
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
				faEntity.Preis = this._data.Preis;
				faEntity.Zeit = this._data.Zeit;
				faEntity.Termin_Fertigstellung = this._data.Termin_Fertigstellung;
				faEntity.Bemerkung = this._data.Bemerkung;
				faEntity.Bemerkung_ohne_statte = this._data.Bemerkung_ohne_statte;
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

			if(!this._data.Fertigungsnummer.HasValue)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please provide FA number");
			}
			if(!this._data.Preis.HasValue)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please provide FA price");
			}
			if(!this._data.Zeit.HasValue)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please provide FA time");
			}
			return ResponseModel<int>.SuccessResponse();
		}
		public List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _old,
		 Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _new)
		{
			var _Log = new Helpers.LogHelper((int)_old.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user);
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			if(_old.Preis != _new.Preis)
			{
				_logs.Add(_Log.LogCTS("Preis", _old.Preis.ToString(), _new.Preis.ToString(), 0));
			}
			if(_old.Zeit != _new.Zeit)
			{
				_logs.Add(_Log.LogCTS("Zeit", _old.Zeit.ToString(), _new.Zeit.ToString(), 0));
			}
			if(_old.Termin_Fertigstellung != _new.Termin_Fertigstellung)
			{
				_logs.Add(_Log.LogCTS("Termin_Fertigstellung", _old.Termin_Fertigstellung.ToString(), _new.Termin_Fertigstellung.ToString(), 0));
			}
			if(_old.Bemerkung != _new.Bemerkung)
			{
				_logs.Add(_Log.LogCTS("Bemerkung", _old.Bemerkung.ToString(), _new.Bemerkung.ToString(), 0));
			}
			if(_old.Bemerkung_ohne_statte != _new.Bemerkung_ohne_statte)
			{
				_logs.Add(_Log.LogCTS("Bemerkung_ohne_statte", _old.Bemerkung_ohne_statte.ToString(), _new.Bemerkung_ohne_statte.ToString(), 0));
			}
			return _logs;
		}
	}
}