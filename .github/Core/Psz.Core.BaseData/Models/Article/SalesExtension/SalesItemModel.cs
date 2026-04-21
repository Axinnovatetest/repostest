using Psz.Core.BaseData.Handlers;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.SalesExtension
{
	public class SalesItemModel
	{
		public int Id { get; set; }
		public int ArticleId { get; set; }
		public int? TypeId { get; set; }
		public string Type { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? StandardPrice { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public bool? brutto { get; set; }
		public decimal? Aufschlag { get; set; }
		public decimal? Aufschlagsatz { get; set; }
		public int? PricingGroup { get; set; }
		public decimal? ProductionTime { get; set; }
		public decimal? ProductionCost { get; set; }
		public decimal? HourlyRate { get; set; }
		public int? PackagingTypeId { get; set; }
		public int? PackagingQuantity { get; set; }
		public string PackagingType { get; set; }
		public string PackagingTypeDetails { get; set; }
		public string PackagingTypeDetailsFin { get; set; }
		public decimal? DeliveryTimeInWorkingDays { get; set; }
		public int? LotSize { get; set; }
		public string Bemerkung { get; set; }
		public decimal? DBwoCU { get; set; }
		// - 2025-11-28 - show DB/Marge - Khelil/Zipproth
		public decimal? DbWithCU { get; set; }
		public decimal? DbWithoutCU { get; set; }
		public decimal? MargeWithCU { get; set; }
		public decimal? MargeWithoutCU { get; set; }


		public SalesItemModel()
		{

		}
		public SalesItemModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity artikelSalesExtensionEntity,
			Infrastructure.Data.Entities.Tables.BSD.Verpackungseinheiten_DefinitionenEntity verpackungseinheiten_DefinitionenEntity)
		{
			if(artikelSalesExtensionEntity == null)
				return;

			Id = artikelSalesExtensionEntity.Id;
			ArticleId = artikelSalesExtensionEntity.ArticleNr;

			TypeId = artikelSalesExtensionEntity.ArticleSalesTypeId;
			Type = artikelSalesExtensionEntity.ArticleSalesType;
			Quantity = artikelSalesExtensionEntity.MOQ;
			PricingGroup = artikelSalesExtensionEntity.Preisgruppe;
			StandardPrice = artikelSalesExtensionEntity.Verkaufspreis;
			DeliveryTimeInWorkingDays = artikelSalesExtensionEntity.Lieferzeit;
			ProductionTime = artikelSalesExtensionEntity.Profuktionszeit;
			ProductionCost = artikelSalesExtensionEntity.Produktionskosten;
			HourlyRate = artikelSalesExtensionEntity.Stundensatz;
			//
			PackagingTypeId = artikelSalesExtensionEntity.VerpackungsartId;
			PackagingQuantity = (int?)artikelSalesExtensionEntity.Verpackungsmenge;
			LotSize = artikelSalesExtensionEntity.Losgroesse;
			//
			brutto = artikelSalesExtensionEntity.brutto;
			Einkaufspreis = artikelSalesExtensionEntity.Einkaufspreis;
			Bemerkung = artikelSalesExtensionEntity.Bemerkung;
			Aufschlag = artikelSalesExtensionEntity.Aufschlag;
			Aufschlagsatz = (decimal?)artikelSalesExtensionEntity.Aufschlagsatz;
			DBwoCU = artikelSalesExtensionEntity.DBwoCU;

			if(verpackungseinheiten_DefinitionenEntity != null)
			{
				PackagingType = verpackungseinheiten_DefinitionenEntity.Artikelnummer;
				PackagingTypeDetails = $"{verpackungseinheiten_DefinitionenEntity.Artikelnummer} || {verpackungseinheiten_DefinitionenEntity.Packmittel_Karton}".Trim(new char[] { '|', ' ' });
				PackagingTypeDetailsFin = $"{verpackungseinheiten_DefinitionenEntity.Artikelnummer} || {verpackungseinheiten_DefinitionenEntity.Packmittel_Karton} || {verpackungseinheiten_DefinitionenEntity.Masse_LxBxH__in_mm_}".Trim(new char[] { '|', ' ' });
			}
		}

		public SalesItemModel(
			Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity artikelSalesExtensionEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity preisgruppenEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity artikelKalkulatorischeKostenEntity,
			Infrastructure.Data.Entities.Tables.BSD.Verpackungseinheiten_DefinitionenEntity verpackungseinheiten_DefinitionenEntity)
		{

			TypeId = (int)Common.Enums.ArticleEnums.SalesItemType.Serie;
			Type = Common.Enums.ArticleEnums.SalesItemType.Serie.GetDescription();
			PricingGroup = 1; // Standard

			if(artikelSalesExtensionEntity != null)
			{
				Id = artikelSalesExtensionEntity.Id;
				Quantity = artikelSalesExtensionEntity.MOQ;
				PackagingTypeId = artikelSalesExtensionEntity.VerpackungsartId;
				DeliveryTimeInWorkingDays = artikelSalesExtensionEntity.Lieferzeit;
				brutto = artikelSalesExtensionEntity.brutto;
				Einkaufspreis = artikelSalesExtensionEntity.Einkaufspreis;
				ProductionCost = artikelSalesExtensionEntity.Produktionskosten;
				ProductionTime = artikelSalesExtensionEntity.Profuktionszeit;
				LotSize = artikelSalesExtensionEntity.Losgroesse;
				Bemerkung = artikelSalesExtensionEntity.Bemerkung;
				Aufschlag = artikelSalesExtensionEntity.Aufschlag;
				Aufschlagsatz = (decimal?)artikelSalesExtensionEntity.Aufschlagsatz;
				DBwoCU = artikelSalesExtensionEntity.DBwoCU;
			}

			if(artikelEntity != null)
			{
				ArticleId = artikelEntity.ArtikelNr;
				ProductionTime = (decimal?)artikelEntity.Produktionszeit;
				HourlyRate = artikelEntity.Stundensatz;
				PackagingQuantity = artikelEntity.Verpackungsmenge;
				PackagingType = artikelEntity.Verpackungsart;
				LotSize = artikelEntity.Losgroesse;
			}


			if(artikelKalkulatorischeKostenEntity != null)
			{
				ProductionCost = artikelKalkulatorischeKostenEntity.Betrag;
			}

			if(preisgruppenEntity != null)
			{
				StandardPrice = (decimal?)preisgruppenEntity.Verkaufspreis;
				Einkaufspreis = preisgruppenEntity.Einkaufspreis;
				Aufschlagsatz = preisgruppenEntity.Aufschlagsatz;
				Aufschlag = preisgruppenEntity.Aufschlag;
				brutto = preisgruppenEntity.brutto;
				Bemerkung = preisgruppenEntity.Bemerkung;
			}

			if(verpackungseinheiten_DefinitionenEntity != null)
			{
				PackagingType = verpackungseinheiten_DefinitionenEntity.Artikelnummer;
				PackagingTypeDetails = $"{verpackungseinheiten_DefinitionenEntity.Artikelnummer} || {verpackungseinheiten_DefinitionenEntity.Packmittel_Karton}".Trim(new char[] { '|', ' ' });
				PackagingTypeDetailsFin = $"{verpackungseinheiten_DefinitionenEntity.Artikelnummer} || {verpackungseinheiten_DefinitionenEntity.Packmittel_Karton} || {verpackungseinheiten_DefinitionenEntity.Masse_LxBxH__in_mm_}".Trim(new char[] { '|', ' ' });
			}
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity ToEntity(
			Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity prevEntity,
			UserModel user,
			Enums.ObjectLogEnums.Objects objectItem,
			int objectItemId,
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs,
			Enums.ObjectLogEnums.LogType logType = Enums.ObjectLogEnums.LogType.Edit, bool woPrice = false)
		{
			if(prevEntity != null)
			{
				if(objectLogs == null)
					objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				if(prevEntity.ArticleSalesType != Type && !string.IsNullOrWhiteSpace(prevEntity.ArticleSalesType) && !string.IsNullOrWhiteSpace(Type))
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ArticleSalesType", $"{prevEntity.ArticleSalesType}",
									$"{Type}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Lieferzeit != DeliveryTimeInWorkingDays)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Lieferzeit", $"{prevEntity.Lieferzeit}",
									$"{DeliveryTimeInWorkingDays}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Losgroesse != LotSize)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Losgroesse", $"{prevEntity.Losgroesse}",
									$"{LotSize}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.MOQ != Quantity)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"MOQ", $"{prevEntity.MOQ}",
									$"{Quantity}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Preisgruppe != PricingGroup)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Preisgruppe", $"{prevEntity.Preisgruppe}",
									$"{PricingGroup}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Produktionskosten != ProductionCost)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Produktionskosten", $"{prevEntity.Produktionskosten}",
									$"{ProductionCost}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Profuktionszeit != ProductionTime)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Produktionszeit", $"{prevEntity.Profuktionszeit}",
									$"{ProductionTime}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Stundensatz != HourlyRate)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Stundensatz", $"{prevEntity.Stundensatz}",
									$"{HourlyRate}", $"{objectItem.GetDescription()}", logType));
				}
				if(woPrice == false && prevEntity.Verkaufspreis != StandardPrice)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Verkaufspreis", $"{prevEntity.Verkaufspreis}",
									$"{StandardPrice}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Verpackungsart != PackagingType && !string.IsNullOrWhiteSpace(prevEntity.Verpackungsart) && !string.IsNullOrWhiteSpace(PackagingType))
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Verpackungsart", $"{prevEntity.Verpackungsart}",
									$"{PackagingType}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Verpackungsmenge != PackagingQuantity)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Verpackungsmenge", $"{prevEntity.Verpackungsmenge}",
									$"{PackagingQuantity}", $"{objectItem.GetDescription()}", logType));
				}
				if(woPrice == false && prevEntity.Einkaufspreis != Einkaufspreis)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Einkaufspreis", $"{prevEntity.Einkaufspreis}",
									$"{Einkaufspreis}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.brutto != brutto)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"brutto", $"{prevEntity.brutto}",
									$"{brutto}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Aufschlag != Aufschlag)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Aufschlag", $"{prevEntity.Aufschlag}",
									$"{Aufschlag}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Aufschlagsatz != (double?)Aufschlagsatz)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Aufschlagsatz", $"{prevEntity.Aufschlagsatz}",
									$"{Aufschlagsatz}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.DBwoCU != DBwoCU)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"DBwoCU", $"{prevEntity.DBwoCU}",
									$"{DBwoCU}", $"{objectItem.GetDescription()}", logType));
				}
			}
			// -
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity
			{
				Id = Id,
				ArticleNr = ArticleId,
				ArticleSalesTypeId = TypeId,
				ArticleSalesType = Type,
				Lieferzeit = DeliveryTimeInWorkingDays,
				Losgroesse = LotSize,
				MOQ = Quantity,
				Preisgruppe = PricingGroup,
				Produktionskosten = ProductionCost,
				Profuktionszeit = ProductionTime,
				Stundensatz = HourlyRate,
				Verkaufspreis = StandardPrice,
				VerpackungsartId = PackagingTypeId,
				Verpackungsart = PackagingType,
				Verpackungsmenge = PackagingQuantity,
				//added columns
				Einkaufspreis = Einkaufspreis,
				brutto = brutto,
				//kalk_kosten = decimal.TryParse(ProductionCost, out var _betrag) ? _betrag : (decimal?)null,
				Bemerkung = Bemerkung,
				Aufschlag = Aufschlag,
				Aufschlagsatz = (double?)Aufschlagsatz,
				DBwoCU = DBwoCU
			};
		}
		//
		public Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity ToPreisgruppenEntity(
			Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity prevEntity,
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

				if(prevEntity.Einkaufspreis != Einkaufspreis)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Einkaufspreis", $"{prevEntity.Einkaufspreis}",
									$"{Einkaufspreis}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.brutto != brutto)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"brutto", $"{prevEntity.brutto}",
									$"{brutto}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Aufschlag != Aufschlag)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Aufschlag", $"{prevEntity.Aufschlag}",
									$"{Aufschlag}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Aufschlagsatz != Aufschlagsatz)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Aufschlagsatz", $"{prevEntity.Aufschlagsatz}",
									$"{Aufschlagsatz}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.kalk_kosten != ProductionCost)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"kalk_kosten", $"{prevEntity.kalk_kosten}",
									$"{ProductionCost}", $"{objectItem.GetDescription()}", logType));
				}
			}

			// -
			return new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity
			{
				Artikel_Nr = ArticleId,
				Preisgruppe = 1,
				Aufschlag = Aufschlag,
				Aufschlagsatz = Aufschlagsatz,
				brutto = brutto,
				Bemerkung = Bemerkung,
				Einkaufspreis = Einkaufspreis, // - Materialkosten
				Verkaufspreis = StandardPrice,
				kalk_kosten = ProductionCost, // - Arbeitskosten
				letzte_Aktualisierung = DateTime.Now,
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToArtikelEntity(
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity prevEntity,
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

				if(prevEntity.Produktionszeit != ProductionTime)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Produktionszeit", $"{prevEntity.Produktionszeit}",
									$"{ProductionTime}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Stundensatz != HourlyRate)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Stundensatz", $"{prevEntity.Stundensatz}",
									$"{HourlyRate}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Verpackungsart != PackagingType && !string.IsNullOrWhiteSpace(prevEntity.Verpackungsart) && !string.IsNullOrWhiteSpace(PackagingType))
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Verpackungsart", $"{prevEntity.Verpackungsart}",
									$"{PackagingType}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Verpackungsmenge != PackagingQuantity)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Verpackungsmenge", $"{prevEntity.Verpackungsmenge}",
									$"{PackagingQuantity}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Losgroesse != LotSize)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Losgroesse", $"{prevEntity.Losgroesse}",
									$"{LotSize}", $"{objectItem.GetDescription()}", logType));
				}
			}

			// -
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				Produktionszeit = ProductionTime,
				Stundensatz = HourlyRate,
				Verpackungsart = PackagingType,
				Verpackungsmenge = PackagingQuantity,
				Losgroesse = LotSize
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity ToArbeitskostenEntity(
			Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity prevEntity,
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

				if(prevEntity.Artikel_nr != ArticleId)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Artikel_nr", $"{prevEntity.Artikel_nr}",
									$"{ArticleId}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Betrag != ProductionCost)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Betrag", $"{prevEntity.Betrag}",
									$"{ProductionCost}", $"{objectItem.GetDescription()}", logType));
				}
			}

			// -
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelKalkulatorischeKostenEntity
			{
				Artikel_nr = ArticleId,
				Betrag = ProductionCost
			};
		}
	}
}
