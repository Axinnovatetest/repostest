using System;
using System.ComponentModel;

namespace Psz.Core.FinanceControl.Helpers
{
	public class LogHelper
	{
		public string LogObject { get; set; }
		public LogType Log_Type { get; set; }
		public Core.Identity.Models.UserModel User { get; set; }

		public LogHelper(string logObject, LogType log_Type, UserModel user)
		{

			LogObject = logObject;
			Log_Type = log_Type;
			User = user;
		}

		public enum LogType
		{
			[Description("CREATENEWITEM")]
			INSERTOBJECT = 0,
			[Description("UPDATEITEM")]
			UPDATEOBJECT = 1,
			[Description("REMOVEITEM")]
			REMOVEOBJECT = 2,
		}
		public Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_Kontenrahmen_LogEntity LogFNC(string column, string oldValue, string newValue, int id)
		{
			string _message = string.Empty;
			string _logTypte = string.Empty;
			switch(Log_Type)
			{

				case LogType.INSERTOBJECT:
					_message = $"[{LogObject}] New Item Created with approximated Id : [{id}]";
					_logTypte = LogType.INSERTOBJECT.GetDescription();
					break;
				case LogType.REMOVEOBJECT:
					_message = $"[{LogObject}] Item with Id :  [{id}] Deleted-";
					_logTypte = LogType.REMOVEOBJECT.GetDescription();
					break;
				case LogType.UPDATEOBJECT:
					_message = $"[{LogObject}] [{id}] Modified- [{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.UPDATEOBJECT.GetDescription();
					break;
			}
			return new Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_Kontenrahmen_LogEntity
			{
				LogType = _logTypte,
				LogObject = LogObject,
				DateTime = DateTime.Now,
				UserId = User?.Id,
				LogText = _message,
				Username = User?.Name,
			};
		}
		public Infrastructure.Data.Entities.Tables.FNC.Adressen_FNC_LogEntity LogFNC2(string column, string oldValue, string newValue, int id)
		{
			string _message = string.Empty;
			string _logTypte = string.Empty;
			switch(Log_Type)
			{

				case LogType.INSERTOBJECT:
					_message = $"[{LogObject}] New Item Created with approximated Id : [{id}]";
					_logTypte = LogType.INSERTOBJECT.GetDescription();
					break;
				case LogType.REMOVEOBJECT:
					_message = $"[{LogObject}] Item with Id :  [{id}] Deleted-";
					_logTypte = LogType.REMOVEOBJECT.GetDescription();
					break;
				case LogType.UPDATEOBJECT:
					_message = $"[{LogObject}] [{id}] Modified- [{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.UPDATEOBJECT.GetDescription();
					break;
			}
			return new Infrastructure.Data.Entities.Tables.FNC.Adressen_FNC_LogEntity
			{
				LogType = _logTypte,
				LogObject = LogObject,
				DateTime = DateTime.Now,
				UserId = User?.Id,
				LogText = _message,
				Username = User?.Name,
			};
		}
		public Infrastructure.Data.Entities.Tables.FNC.ZahlungskonditionenKunden_FNC_LogEntity LogFNC3(string column, string oldValue, string newValue, int id)
		{
			string _message = string.Empty;
			string _logTypte = string.Empty;
			switch(Log_Type)
			{

				case LogType.INSERTOBJECT:
					_message = $"[{LogObject}] New Item Created with approximated Id : [{id}]";
					_logTypte = LogType.INSERTOBJECT.GetDescription();
					break;
				case LogType.REMOVEOBJECT:
					_message = $"[{LogObject}] Item with Id :  [{id}] Deleted-";
					_logTypte = LogType.REMOVEOBJECT.GetDescription();
					break;
				case LogType.UPDATEOBJECT:
					_message = $"[{LogObject}] [{id}] Modified- [{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.UPDATEOBJECT.GetDescription();
					break;
			}
			return new Infrastructure.Data.Entities.Tables.FNC.ZahlungskonditionenKunden_FNC_LogEntity
			{
				LogType = _logTypte,
				LogObject = LogObject,
				DateTime = DateTime.Now,
				UserId = User?.Id,
				LogText = _message,
				Username = User?.Name,
			};
		}
	}

}
