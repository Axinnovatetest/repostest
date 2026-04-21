using Psz.Core.Identity.Models;
using System;
using System.ComponentModel;

namespace Psz.Core.CustomerService.Helpers
{
	public class BestellungenLogHelper
	{
		public int Nr { get; set; }
		public int BestellungenNr { get; set; }
		public int ProjektNr { get; set; }
		public string LogObject { get; set; }
		public LogType Log_Type { get; set; }
		public string Origin { get; set; }
		public UserModel User { get; set; }

		public BestellungenLogHelper(int nr, int bestellungNr, int projektNr, string logObject, LogType log_Type, string origin, UserModel user)
		{
			Nr = nr;
			BestellungenNr = bestellungNr;
			ProjektNr = projektNr;
			LogObject = logObject;
			Log_Type = log_Type;
			Origin = origin;
			User = user;
		}

		public enum LogType
		{
			[Description("Order Creation")]
			CREATIONORDER = 0,
			[Description("Position Creation")]
			CREATIONPOS = 1,
			[Description("Position Modification")]
			MODIFICATIONPOS = 2,
			[Description("Order Modification")]
			MODIFICATIONORDER = 3,
			[Description("Order Delete")]
			DELETIONORDER = 4,
			[Description("Posistion Delete")]
			DELETEPOS = 5,
			[Description("Order Validate")]
			VALIDATEORDER = 6,
			[Description("Order Unvalidate")]
			UNVALIDATEORDER = 7,
		}
		public Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity LogMTM(int id)
		{
			string _message = string.Empty;
			string _logTypte = string.Empty;
			switch(Log_Type)
			{
				case LogType.CREATIONORDER:
					_message = $"[{LogObject}] Created";
					_logTypte = LogType.CREATIONORDER.GetDescription();
					break;
				case LogType.CREATIONPOS:
					_message = $"[{LogObject}] New Position [{id}] Created";
					_logTypte = LogType.CREATIONPOS.GetDescription();
					break;
				case LogType.DELETIONORDER:
					_message = $"[{LogObject}] Deleted";
					_logTypte = LogType.DELETIONORDER.GetDescription();
					break;
				case LogType.DELETEPOS:
					_message = $"[{LogObject}] Position [{id}] Deleted";
					_logTypte = LogType.DELETEPOS.GetDescription();
					break;
				case LogType.MODIFICATIONPOS:
					_message = $"[{LogObject}] Position [{id}] Modified";
					_logTypte = LogType.MODIFICATIONPOS.GetDescription();
					break;
				case LogType.MODIFICATIONORDER:
					_message = $"[{LogObject}] Modified";
					_logTypte = LogType.MODIFICATIONORDER.GetDescription();
					break;
				case LogType.VALIDATEORDER:
					_message = $"[{LogObject}] Validated";
					_logTypte = LogType.VALIDATEORDER.GetDescription();
					break;
				case LogType.UNVALIDATEORDER:
					_message = $"[{LogObject}] UnValidated";
					_logTypte = LogType.UNVALIDATEORDER.GetDescription();
					break;
			}
			return new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity
			{
				Nr = Nr,
				BestellungenNr = BestellungenNr,
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
