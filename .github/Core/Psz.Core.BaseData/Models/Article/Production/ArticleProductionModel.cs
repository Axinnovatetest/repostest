using Psz.Core.BaseData.Handlers;
using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Production
{
	public class ArticleProductionModel
	{
		public int Id { get; set; }
		public int ArticleID { get; set; }
		public bool? AlternativeProductionPlace { get; set; }
		public int? ProductionPlace1_Id { get; set; }
		public int? ProductionPlace2_Id { get; set; }
		public int? ProductionPlace3_Id { get; set; }
		public string ProductionPlace1_Name { get; set; }
		public string ProductionPlace2_Name { get; set; }
		public string ProductionPlace3_Name { get; set; }
		public string ExternalStatus { get; set; }
		public string Prufstatus { get; set; }
		public string InternalStatus { get; set; }

		public string Artikelkurztext { get; set; }
		public string Halle { get; set; }
		public bool aktiv { get; set; }
		// - 2023-06-26
		public int TeamsId { get; set; }
		public int TeamsSiteId { get; set; }
		public string TeamsName { get; set; }
		// -2024-03-08
		public decimal ProductionLotSize { get; set; }
		public ArticleProductionModel()
		{

		}
		public ArticleProductionModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity productionExtensionEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity,
			Infrastructure.Data.Entities.Tables.BSD.TeamsEntity teamsEntity)
		{
			if(productionExtensionEntity != null)
			{
				Id = productionExtensionEntity.Id;
				ProductionPlace1_Id = productionExtensionEntity.ProductionPlace1_Id;
				ProductionPlace2_Id = productionExtensionEntity.ProductionPlace2_Id;
				ProductionPlace3_Id = productionExtensionEntity.ProductionPlace3_Id;

				ProductionPlace1_Name = productionExtensionEntity.ProductionPlace1_Name;
				ProductionPlace2_Name = productionExtensionEntity.ProductionPlace2_Name;
				ProductionPlace3_Name = productionExtensionEntity.ProductionPlace3_Name;

				AlternativeProductionPlace = productionExtensionEntity.AlternativeProductionPlace;
			}
			if(articleEntity != null)
			{
				ArticleID = articleEntity.ArtikelNr;
				ExternalStatus = articleEntity.Freigabestatus;
				Prufstatus = articleEntity.PrufstatusTNWare;
				InternalStatus = articleEntity.FreigabestatusTNIntern;
				Artikelkurztext = articleEntity.Artikelkurztext;
				TeamsName = articleEntity.Artikelkurztext;
				Halle = articleEntity.Halle;
				aktiv = articleEntity.aktiv ?? false;
				ProductionLotSize = articleEntity.ProductionLotSize ?? 0;
			}

			if(teamsEntity is not null)
			{
				TeamsId = teamsEntity.Id;
				TeamsName = teamsEntity.Name;
				TeamsSiteId = teamsEntity.SiteId ?? -1;
			}

		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToArtikelEntity(
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity prevEntity,
			UserModel user = null,
			Enums.ObjectLogEnums.Objects objectItem = Enums.ObjectLogEnums.Objects.Article,
			int objectItemId = -1,
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs = null,
			Enums.ObjectLogEnums.LogType logType = Enums.ObjectLogEnums.LogType.Edit)
		{
			if(prevEntity != null)
			{
				if(objectLogs == null)
					objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				if(prevEntity.Artikelkurztext != TeamsName)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Artikelkurztext", $"{prevEntity.Artikelkurztext}",
									$"{TeamsName}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Halle != Halle)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Halle", $"{prevEntity.Halle}",
									$"{Halle}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ProductionLotSize != ProductionLotSize)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ProductionLotSize", $"{prevEntity.ProductionLotSize}",
									$"{ProductionLotSize}", $"{objectItem.GetDescription()}", logType));
				}
			}
			// -

			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				ArtikelNr = ArticleID,
				Artikelkurztext = TeamsName,
				Halle = Halle,
				ProductionLotSize = ProductionLotSize
			};
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity ToEntity(
			Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity prevEntity,
			UserModel user = null,
			Enums.ObjectLogEnums.Objects objectItem = Enums.ObjectLogEnums.Objects.Article,
			int objectItemId = -1,
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs = null,
			Enums.ObjectLogEnums.LogType logType = Enums.ObjectLogEnums.LogType.Edit)
		{
			if(prevEntity != null)
			{
				if(objectLogs == null)
					objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				if(prevEntity.ArticleId != ArticleID)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ArticleId", $"{prevEntity.ArticleId}",
									$"{ArticleID}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.AlternativeProductionPlace != AlternativeProductionPlace)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"AlternativeProductionPlace", $"{prevEntity.AlternativeProductionPlace}",
									$"{AlternativeProductionPlace}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ProductionPlace1_Id != ProductionPlace1_Id)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ProductionPlace1_Id", $"{prevEntity.ProductionPlace1_Id}",
									$"{ProductionPlace1_Id}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ProductionPlace2_Id != ProductionPlace2_Id)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ProductionPlace2_Id", $"{prevEntity.ProductionPlace2_Id}",
									$"{ProductionPlace2_Id}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ProductionPlace3_Id != ProductionPlace3_Id)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ProductionPlace3_Id", $"{prevEntity.ProductionPlace3_Id}",
									$"{ProductionPlace3_Id}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ProductionPlace1_Name != ProductionPlace1_Name)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ProductionPlace1_Name", $"{prevEntity.ProductionPlace1_Name}",
									$"{ProductionPlace1_Name}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ProductionPlace2_Name != ProductionPlace2_Name)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ProductionPlace2_Name", $"{prevEntity.ProductionPlace2_Name}",
									$"{ProductionPlace2_Name}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ProductionPlace3_Name != ProductionPlace3_Name)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ProductionPlace3_Name", $"{prevEntity.ProductionPlace3_Name}",
									$"{ProductionPlace3_Name}", $"{objectItem.GetDescription()}", logType));
				}
			}
			// -

			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity
			{
				Id = Id,
				ArticleId = ArticleID,
				AlternativeProductionPlace = AlternativeProductionPlace,
				ProductionPlace1_Id = ProductionPlace1_Id,
				ProductionPlace2_Id = ProductionPlace2_Id,
				ProductionPlace3_Id = ProductionPlace3_Id,

				ProductionPlace1_Name = ProductionPlace1_Name,
				ProductionPlace2_Name = ProductionPlace2_Name,
				ProductionPlace3_Name = ProductionPlace3_Name,
			};
		}
	}
}
