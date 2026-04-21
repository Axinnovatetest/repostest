using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Psz.Core.Logistics.Helpers
{
	public class LogHelper
	{

		public int AngebotNr { get; set; }
		public int ProjektNr { get; set; }
		public string LogObject { get; set; }
		public LogType Log_Type { get; set; }
		public string Origin { get; set; }
		public Core.Identity.Models.UserModel User { get; set; }
		public LogHelper(int angebotNr, int projektNr, string logObject, LogType log_Type, string origin, UserModel user)
		{
			AngebotNr = angebotNr;
			ProjektNr = projektNr;
			LogObject = logObject;
			Log_Type = log_Type;
			Origin = origin;
			User = user;
		}
		public enum LogType
		{

			[Description("MODIFICATIONPRINTVERSAND")]
			MODIFICATIONPRINTVERSAND = 0,
			[Description("MODIFICATIONPRINTLS")]
			MODIFICATIONPRINTLS = 1,
			[Description("CREATEVDA")]
			CREATEVDA = 2,

		}
		public Infrastructure.Data.Entities.Tables.Logistics.Logistics_LogEntity LogLGT(string column, string oldValue, string newValue, int id)
		{
			string _message = string.Empty;
			string _logTypte = string.Empty;
			switch(Log_Type)
			{
				case LogType.MODIFICATIONPRINTVERSAND:
					_message = $"[{LogObject}] Change (NrAngeboteArtikel) Versand [{oldValue}] to printed";
					_logTypte = LogType.MODIFICATIONPRINTVERSAND.GetDescription();
					break;
				case LogType.MODIFICATIONPRINTLS:
					_message = $"[{LogObject}] Change LS [{oldValue}] to printed";
					_logTypte = LogType.MODIFICATIONPRINTLS.GetDescription();
					break;
				case LogType.CREATEVDA:
					_message = $"[{LogObject}] Create VDA of customer  [{oldValue}] ";
					_logTypte = LogType.CREATEVDA.GetDescription();
					break;



			}
			return new Infrastructure.Data.Entities.Tables.Logistics.Logistics_LogEntity
			{

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

	public class PlantBookingLogHelper
	{
		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity> GenerateLogForUpdates
			(
			Identity.Models.UserModel _user,
			Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity CurrentValue,
			Psz.Core.Logistics.Models.PlantBookings.PlantBookingUpdateModel newValues 
			)
		{
			var restoReturn = new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity>();
			if(newValues?.Menge != null && CurrentValue.Menge != newValues?.Menge  )
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity() 
					{ 
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ?  CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the menge has been updated from [{CurrentValue.Menge}] to [{newValues.Menge}]"
					});
			}
			if(CurrentValue.Laufende_Nummer != newValues.Laufende_Nummer && newValues.Laufende_Nummer != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Laufende_Nummer has been updated from [{CurrentValue.Laufende_Nummer}] to [{newValues.Laufende_Nummer}]"
					});
			}
			if(CurrentValue.Inspektor != newValues.Inspektor && newValues.Inspektor != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Inspektor has been updated from [{CurrentValue.Inspektor}] to [{newValues.Inspektor}]"
					});
			}
			if(CurrentValue.Akzeptierte_Menge != newValues.Akzeptierte_Menge && newValues.Akzeptierte_Menge != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] |: the Akzeptierte_Menge has been updated from [{CurrentValue.Akzeptierte_Menge}] to [{newValues.Akzeptierte_Menge}]"
					});
			}
			if(CurrentValue.Prufmenge != newValues.Prufmenge && newValues.Prufmenge != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Prufmenge has been updated from [{CurrentValue.Prufmenge}] to [{newValues.Prufmenge}]"
					});
			}
			if(CurrentValue.Pruftiefe != newValues.Pruftiefe && newValues.Pruftiefe != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Pruftiefe has been updated from [{CurrentValue.Pruftiefe}] to [{newValues.Pruftiefe}]"
					});
			}
			if(CurrentValue.Clock_Number != newValues.Clock_Number && newValues.Clock_Number != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Clock_Number has been updated from [{CurrentValue.Clock_Number}] to [{newValues.Clock_Number}]"
					});
			}
			if(CurrentValue.Artikelnummer != newValues.Artikelnummer && newValues.Artikelnummer != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Artikelnummer  has been updated from [{CurrentValue.Artikelnummer}] to [{newValues.Artikelnummer}]"
					});
			}
			if(CurrentValue.Geprufte_Prufmenge != newValues.Geprufte_Prufmenge && newValues.Geprufte_Prufmenge != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Geprufte_Prufmenge   has been updated from [{CurrentValue.Geprufte_Prufmenge}] to [{newValues.Geprufte_Prufmenge}]"
					});
			}
			if(CurrentValue.Gesamtmenge != newValues.Gesamtmenge && newValues.Gesamtmenge != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Gesamtmenge    has been updated from [{CurrentValue.Gesamtmenge}] to [{newValues.Gesamtmenge}]"
					});
			}
			if(CurrentValue.Prufentscheid != newValues.Prufentscheid && newValues.Prufentscheid != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Prufentscheid     has been updated from [{CurrentValue.Prufentscheid}] to [{newValues.Prufentscheid}]"
					});
			}
			if(CurrentValue.Reklamierte_Menge != newValues.Reklamierte_Menge && newValues.Reklamierte_Menge != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Reklamierte_Menge has been updated from [{CurrentValue.Reklamierte_Menge}] to [{newValues.Reklamierte_Menge}]"
					});
			}
			if(CurrentValue.Verpackungsnr != newValues.Verpackungsnr && newValues.Verpackungsnr != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Verpackungsnr  has been updated from [{CurrentValue.Verpackungsnr}] to [{newValues.Verpackungsnr}]"
					});
			}
			if(CurrentValue.Resultat != newValues.Resultat && newValues.Resultat != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Resultat   has been updated from [{CurrentValue.Resultat}] to [{newValues.Resultat}]"
					});
			}
			if(CurrentValue.Kunde != newValues.Kunde && newValues.Kunde != null)
			{
				restoReturn.Add(
					new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Update]",
						LogObjectId = CurrentValue.Nummer_Verpackung == 0 ? CurrentValue.Verpackungsnr : CurrentValue.Nummer_Verpackung,
						LogDescription = $"| Update || Lager: [{CurrentValue.LagerortID}] | : the Kunde    has been updated from [{CurrentValue.Kunde}] to [{newValues.Kunde}]"
					});
			}


			return restoReturn;
		}

		public static Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity GenerateLogForDelete
			(
			Identity.Models.UserModel _user,
			int LagerId,
			int Nummer_Verpackung
		
			)
		{

			return new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
					{
						LastUpdateTime = DateTime.Now,
						LastUpdateUsername = _user.Username,
						LastUpdateUserId = _user.Id,
						LastUpdateUserFullName = _user.Name,
						LogObject = "[LGT][Plant Booking][Delete]",
						LogObjectId = Nummer_Verpackung,
						LogDescription = $"| Delete |: The Object with Nummer Verpackung [{Nummer_Verpackung}] has been deleted From Lager [{LagerId}] "
					};
		}

		public static Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity GenerateLogForCreate
			(
			Identity.Models.UserModel _user,
				
			int Nummer_Verpackung,
			string ArtikelNummer,
			int LagerId
			)
		{
			string AdditionalLogTest = (
				ArtikelNummer == null || ArtikelNummer =="" || ArtikelNummer == string.Empty
				)?
				"  has been Created " :
				$" and  ArtikelNummer [{ArtikelNummer}] has been Created ";

			return new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity()
			{
				LastUpdateTime = DateTime.Now,
				LastUpdateUsername = _user.Username,
				LastUpdateUserId = _user.Id,
				LastUpdateUserFullName = _user.Name,
				LogObject = "[LGT][Plant Booking][Create]",
				LogObjectId = Nummer_Verpackung,
				LogDescription = $"| Create | | Lager:{LagerId} | : The Object with Nummer Verpackung [{Nummer_Verpackung}]" + AdditionalLogTest
			};


		}

	}
}
