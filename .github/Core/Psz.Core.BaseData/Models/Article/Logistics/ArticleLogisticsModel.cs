using Psz.Core.BaseData.Handlers;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Logistics
{
	public class ArticleLogisticsModel
	{
		public int ArticleID { get; set; }
		public int Id { get; set; }
		public string ZolltarifNummer { get; set; }
		public Decimal? Exportgewicht { get; set; }
		public bool? VDALabel { get; set; } // VDA_E / VDA_1
		public bool? ULLabel { get; set; }
		// - 
		public string Abladeort { get; set; }
		public string Anlieferadresse { get; set; }
		public string UrsprungslandName { get; set; }
		// Stage v2
		public bool? VDA_P { get; set; } // VDA_2
		public decimal? Grosse { get; set; }

		public string Praeferenz_Aktuelles_jahr { get; set; }
		public string Praeferenz_Folgejahr { get; set; }
		public string Artikelkurztext { get; set; }
		public string Halle { get; set; }
		public bool aktiv { get; set; }
		public int TeamsId { get; set; }
		public int TeamsSiteId { get; set; }
		public string TeamsName { get; set; }
		public ArticleLogisticsModel()
		{

		}
		public ArticleLogisticsModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity articleLogisticsEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			if(artikelEntity != null)
			{
				// - 
				ZolltarifNummer = artikelEntity.Zolltarif_nr;
				UrsprungslandName = artikelEntity.Ursprungsland;
				Exportgewicht = artikelEntity.Exportgewicht;
				ULLabel = artikelEntity.ULEtikett;
				VDALabel = artikelEntity.VDA_1;
				VDA_P = artikelEntity.VDA_2;
				Grosse = artikelEntity.Größe;
				Praeferenz_Aktuelles_jahr = artikelEntity.Praeferenz_Aktuelles_jahr;
				Praeferenz_Folgejahr = artikelEntity.Praeferenz_Folgejahr;
				Artikelkurztext = artikelEntity.Artikelkurztext;
				TeamsName = artikelEntity.Artikelkurztext;
				Halle = artikelEntity.Halle;
				aktiv = artikelEntity.aktiv ?? false;
			}

			if(articleLogisticsEntity != null)
			{
				Id = articleLogisticsEntity.Id;
				ArticleID = articleLogisticsEntity.ArticleId;
				VDALabel = articleLogisticsEntity.VDALabel;
				Abladeort = articleLogisticsEntity.Abladeort;
				Anlieferadresse = articleLogisticsEntity.Anlieferadresse;
			}
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity ToEntity(
			Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity logisticsExtensionEntity,
			UserModel user,
			Enums.ObjectLogEnums.Objects objectItem,
			int objectItemId,
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs)
		{
			if(logisticsExtensionEntity != null)
			{
				if(objectLogs == null)
					objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				if(logisticsExtensionEntity.VDALabel != VDALabel)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"VDALabel", $"{logisticsExtensionEntity.VDALabel}",
									$"{VDALabel}",
									$"{objectItem.GetDescription()}",
									Enums.ObjectLogEnums.LogType.Edit));
				}
				if(logisticsExtensionEntity.Abladeort != Abladeort)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Abladeort", $"{logisticsExtensionEntity.Abladeort}",
									$"{Abladeort}",
									$"{objectItem.GetDescription()}",
									Enums.ObjectLogEnums.LogType.Edit));
				}
				if(logisticsExtensionEntity.Anlieferadresse != Anlieferadresse)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Anlieferadresse", $"{logisticsExtensionEntity.Anlieferadresse}",
									$"{Anlieferadresse}",
									$"{objectItem.GetDescription()}",
									Enums.ObjectLogEnums.LogType.Edit));
				}
			}

			// -
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity
			{
				Id = Id,
				ArticleId = ArticleID,
				VDALabel = VDALabel,
				Abladeort = Abladeort,
				Anlieferadresse = Anlieferadresse,
			};
		}

		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToArtikelEntity(
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity,
			UserModel user,
			Enums.ObjectLogEnums.Objects objectItem,
			int objectItemId,
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs)
		{
			if(articleEntity != null)
			{
				if(objectLogs == null)
					objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				if(articleEntity.Zolltarif_nr != ZolltarifNummer)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Zolltarif_nr", $"{articleEntity.Zolltarif_nr}",
									$"{ZolltarifNummer}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.Ursprungsland != UrsprungslandName)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Ursprungsland", $"{articleEntity.Ursprungsland}",
									$"{UrsprungslandName}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.Exportgewicht != Exportgewicht)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Exportgewicht", $"{articleEntity.Exportgewicht}",
									$"{Exportgewicht}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.ULEtikett != ULLabel)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ULEtikett", $"{articleEntity.ULEtikett}",
									$"{ULLabel}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.VDA_1 != VDALabel)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"VDALabel", $"{articleEntity.VDA_1}",
									$"{VDALabel}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.VDA_2 != VDA_P)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"VDA_P", $"{articleEntity.VDA_2}",
									$"{VDA_P}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.Größe != Grosse)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Größe", $"{articleEntity.Größe}",
									$"{Grosse}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.Praeferenz_Aktuelles_jahr != Praeferenz_Aktuelles_jahr)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Praeferenz_Aktuelles_jahr", $"{articleEntity.Praeferenz_Aktuelles_jahr}",
									$"{Praeferenz_Aktuelles_jahr}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.Praeferenz_Folgejahr != Praeferenz_Folgejahr)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Praeferenz_Folgejahr", $"{articleEntity.Praeferenz_Folgejahr}",
									$"{Praeferenz_Folgejahr}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.Artikelkurztext != Artikelkurztext)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Artikelkurztext", $"{articleEntity.Artikelkurztext}",
									$"{Artikelkurztext}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.Halle != Halle)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Halle", $"{articleEntity.Halle}",
									$"{Halle}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				if(articleEntity.Artikelkurztext != TeamsName)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Artikelkurztext", $"{articleEntity.Artikelkurztext}",
									$"{TeamsName}", $"{objectItem.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
			}

			// -
			if(articleEntity == null)
				articleEntity = new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity();

			//articleEntity.ArtikelNr = ArticleID;
			articleEntity.Zolltarif_nr = ZolltarifNummer;
			articleEntity.Ursprungsland = UrsprungslandName;
			articleEntity.Exportgewicht = Exportgewicht;
			articleEntity.ULEtikett = ULLabel;
			articleEntity.VDA_1 = VDALabel;
			articleEntity.VDA_2 = VDA_P;
			articleEntity.Größe = Grosse;
			articleEntity.Praeferenz_Aktuelles_jahr = Praeferenz_Aktuelles_jahr;
			articleEntity.Praeferenz_Folgejahr = Praeferenz_Folgejahr;
			articleEntity.Artikelkurztext = TeamsName;
			articleEntity.Halle = Halle;

			return articleEntity;
		}
	}
}
