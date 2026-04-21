using System;
using System.ComponentModel;

namespace Psz.Core.Apps.EDI.Helpers
{
	public class LogHelper
	{
		public int Nr { get; set; }
		public int AngebotNr { get; set; }
		public int ProjektNr { get; set; }
		public string LogObject { get; set; }
		public LogType Log_Type { get; set; }
		public string Origin { get; set; }
		public Core.Identity.Models.UserModel User { get; set; }

		public LogHelper(int nr, int angebotNr, int projektNr, string logObject, LogType log_Type, string origin, Core.Identity.Models.UserModel user)
		{
			Nr = nr;
			AngebotNr = angebotNr;
			ProjektNr = projektNr;
			LogObject = logObject;
			Log_Type = log_Type;
			Origin = origin;
			User = user;
		}

		public enum LogType
		{
			[Description("CREATIONOBJECT")]
			CREATIONOBJECT = 0,
			[Description("CREACREATIONPOSTION")]
			CREATIONPOS = 1,
			[Description("MODIFICATIONPOS")]
			MODIFICATIONPOS = 2,
			[Description("MODIFICATIONOBJECT")]
			MODIFICATIONOBJECT = 3,
			[Description("DELETIONOBJECT")]
			DELETIONOBJECT = 4,
			[Description("DELETIONPOS")]
			DELETIONPOS = 5,
			[Description("VALIDATEPRODUCTION")]
			VALIDATEPRODUCTION = 6,
			[Description("CREATIONFA")]
			CREATIONFA = 7,
			[Description("MODIFICATIONSTUCKLIST")]
			MODIFICATIONSTUCKLIST = 8,
			//for fa techniciens
			[Description("CREACREATIONTECHNICIEN")]
			CREACREATIONTECHNICIEN = 9,
			[Description("MODIFICATIONTECHNICIEN")]
			MODIFICATIONTECHNICIEN = 10,
			[Description("DELETIONTECHNICIEN")]
			DELETIONTECHNICIEN = 11,
			[Description("MODIFICATIONFA")]
			MODIFICATIONFA = 12,
			[Description("MODIFICATIONBLANKET")]
			MODIFICATIONBLANKET = 13,
			[Description("MODIFICATIONSTATUSBLANKET")]
			MODIFICATIONSTATUSBLANKET = 14,
		}
		public Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity LogCTS(string column, string oldValue, string newValue, int id)
		{
			string _message = string.Empty;
			string _logTypte = string.Empty;
			switch(Log_Type)
			{
				case LogType.CREATIONOBJECT:
					_message = $"[{LogObject}] Created";
					_logTypte = LogType.CREATIONOBJECT.GetDescription();
					break;
				case LogType.CREATIONPOS:
					_message = $"[{LogObject}] New Position [{id}] Created";
					_logTypte = LogType.CREATIONPOS.GetDescription();
					break;
				case LogType.DELETIONOBJECT:
					_message = $"[{LogObject}] Deleted- [{oldValue}]";
					_logTypte = LogType.DELETIONOBJECT.GetDescription();
					break;
				case LogType.DELETIONPOS:
					_message = $"[{LogObject}] Position [{id}] Deleted-";
					_logTypte = LogType.DELETIONPOS.GetDescription();
					break;
				case LogType.MODIFICATIONPOS:
					_message = $"[{LogObject}] Position [{id}] Modified- [{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				case LogType.MODIFICATIONFA:
					_message = $"[{LogObject}] [{id}] Modified- [{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.MODIFICATIONFA.GetDescription();
					break;
				case LogType.MODIFICATIONOBJECT:
					_message = $"[{LogObject}]  Modified- [{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				case LogType.VALIDATEPRODUCTION:
					_message = $"[{LogObject}] Position [{id}] Validated for production";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				case LogType.CREATIONFA:
					_message = $"[{LogObject}] [{Nr}] Created";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				case LogType.MODIFICATIONSTUCKLIST:
					_message = $"[{LogObject}] [{Nr}] Modified- Stucklist Updated";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				//fa techniciens
				case LogType.CREACREATIONTECHNICIEN:
					_message = $"[{LogObject}] [{Nr}] New Technicien [{id}] Created";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				case LogType.MODIFICATIONTECHNICIEN:
					_message = $"[{LogObject}] Technicien [{id}] Modified- [{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				case LogType.DELETIONTECHNICIEN:
					_message = $"[{LogObject}] [{Nr}] Technicien [{id}] Deleted";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				case LogType.MODIFICATIONBLANKET:
					_message = $"[{LogObject}] [{Nr}] Blanket [{id}] Modified-[{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.MODIFICATIONBLANKET.GetDescription();
					break;
				case LogType.MODIFICATIONSTATUSBLANKET:
					_message = $"[{LogObject}] [{Nr}]  Modified-[{column}] changed from [{oldValue}] to [{newValue}]";
					_logTypte = LogType.MODIFICATIONSTATUSBLANKET.GetDescription();
					break;
			}
			return new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
			{
				Nr = Nr,
				AngebotNr = AngebotNr,
				ProjektNr = ProjektNr,
				LogType = _logTypte,
				LogObject = LogObject,
				DateTime = DateTime.Now,
				UserId = User?.Id,
				Origin = Origin,
				LogText = _message,
				Username = User?.Name,
			};
		}
	}
}
