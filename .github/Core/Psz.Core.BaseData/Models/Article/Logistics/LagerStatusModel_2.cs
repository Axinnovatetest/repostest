using Psz.Core.BaseData.Handlers;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Logistics
{
	public class LagerStatusModel_2
	{
		public int? Artikel_Nr { get; set; }
		public int? LagerID { get; set; }
		public string LagerName { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? Bestand_reserviert { get; set; }
		public decimal? GesamtBestand { get; set; }
		public DateTime? letzte_Bewegung { get; set; }
		public decimal? Mindestbestand { get; set; }
		public bool? CCID { get; set; }
		public DateTime? CCID_Datum { get; set; }
		public bool? Bestellvorschlage { get; set; }
		public int? Dispoformel { get; set; }
		public decimal? AB { get; set; }
		public decimal? BW { get; set; }
		public decimal? Durchschnittspreis { get; set; }
		public decimal? Höchstbestand { get; set; }
		public int ID { get; set; }
		public decimal? Meldebestand { get; set; }
		public decimal? Sollbestand { get; set; }

		// - 
		public string KundenIndex { get; set; }
		public LagerStatusModel_2()
		{

		}
		public LagerStatusModel_2(Infrastructure.Data.Entities.Tables.PRS.LagerEntity lagerEntity, string kundenIndex)
		{
			if(lagerEntity != null)
			{
				ID = lagerEntity.ID;
				Artikel_Nr = lagerEntity.Artikel_Nr;
				LagerID = lagerEntity.Lagerort_id;
				Bestand = lagerEntity.Bestand;
				Bestand_reserviert = lagerEntity.Bestand_reserviert;
				GesamtBestand = lagerEntity.GesamtBestand;
				letzte_Bewegung = lagerEntity.letzte_Bewegung;
				CCID = lagerEntity.CCID;
				CCID_Datum = lagerEntity.CCID_Datum;
				Bestellvorschlage = lagerEntity.Bestellvorschläge;
				Dispoformel = lagerEntity.Dispoformel;
				//
				AB = lagerEntity.AB;
				BW = lagerEntity.BW;
				Durchschnittspreis = lagerEntity.Durchschnittspreis;
				Höchstbestand = lagerEntity.Höchstbestand;
				Meldebestand = lagerEntity.Meldebestand;
				Sollbestand = lagerEntity.Sollbestand;
				KundenIndex = kundenIndex;
				Mindestbestand = lagerEntity.Mindestbestand;
			}
		}
		public Infrastructure.Data.Entities.Tables.PRS.LagerEntity ToEntity(
			Infrastructure.Data.Entities.Tables.PRS.LagerEntity prevEntity,
			UserModel user,
			Enums.ObjectLogEnums.Objects objectItem,
			int objectItemId,
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs,
			Enums.ObjectLogEnums.LogType logType = Enums.ObjectLogEnums.LogType.Edit)
		{
			if(prevEntity != null)
			{
				if(objectLogs == null)
					objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				if(prevEntity.Artikel_Nr != Artikel_Nr)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Artikel_Nr", $"{prevEntity.Artikel_Nr}",
									$"{Artikel_Nr}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Lagerort_id != LagerID)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Lagerort_id", $"{prevEntity.Lagerort_id}",
									$"{LagerID}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Bestand != Bestand)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Bestand", $"{prevEntity.Bestand}",
									$"{Bestand}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Bestand_reserviert != Bestand_reserviert)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Bestand_reserviert", $"{prevEntity.Bestand_reserviert}",
									$"{Bestand_reserviert}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.GesamtBestand != GesamtBestand)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"GesamtBestand", $"{prevEntity.GesamtBestand}",
									$"{GesamtBestand}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Mindestbestand != Mindestbestand)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Mindestbestand", $"{prevEntity.Mindestbestand}",
									$"{Mindestbestand}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.CCID != CCID)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"CCID", $"{prevEntity.CCID}",
									$"{CCID}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.CCID_Datum != CCID_Datum)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"CCID_Datum", $"{prevEntity.CCID_Datum}",
									$"{CCID_Datum}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Bestellvorschläge != Bestellvorschlage)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Bestellvorschläge", $"{prevEntity.Bestellvorschläge}",
									$"{Bestellvorschlage}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Dispoformel != Dispoformel)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Dispoformel", $"{prevEntity.Dispoformel}",
									$"{Dispoformel}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.AB != AB)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"AB", $"{prevEntity.AB}",
									$"{AB}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.BW != BW)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"BW", $"{prevEntity.BW}",
									$"{BW}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Durchschnittspreis != Durchschnittspreis)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Durchschnittspreis", $"{prevEntity.Durchschnittspreis}",
									$"{Durchschnittspreis}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Höchstbestand != Höchstbestand)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Höchstbestand", $"{prevEntity.Höchstbestand}",
									$"{Höchstbestand}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Meldebestand != Meldebestand)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Meldebestand", $"{prevEntity.Meldebestand}",
									$"{Meldebestand}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Sollbestand != Sollbestand)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Sollbestand", $"{prevEntity.Sollbestand}",
									$"{Sollbestand}", $"{objectItem.GetDescription()}", logType));
				}
			}
			// -

			return new Infrastructure.Data.Entities.Tables.PRS.LagerEntity
			{
				ID = ID,
				Artikel_Nr = Artikel_Nr,
				Lagerort_id = LagerID,
				Bestand = Bestand,
				Bestand_reserviert = Bestand_reserviert,
				GesamtBestand = GesamtBestand,
				letzte_Bewegung = letzte_Bewegung,
				Mindestbestand = Mindestbestand,
				CCID = CCID,
				CCID_Datum = CCID_Datum,
				Bestellvorschläge = Bestellvorschlage,
				Dispoformel = Dispoformel,
				//
				AB = AB,
				BW = BW,
				Durchschnittspreis = Durchschnittspreis,
				Höchstbestand = Höchstbestand,
				Meldebestand = Meldebestand,
				Sollbestand = Sollbestand
			};
		}
	}
	public class LagerExtensionModel
	{
		public string KundenIndex { get; set; }
		public Infrastructure.Data.Entities.Tables.PRS.LagerEntity LagerEntity { get; set; }
	}
	public class SetLagerMinStockRequestModel
	{
		public int ArticleId { get; set; }
		public int WarehouseNumber { get; set; }
		public decimal WarehouseMinStock { get; set; }
	}
}
