using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.CustomerService
{

	public class StockStatusSimpleModel
	{
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string Designation1 { get; set; }
		public decimal CurrentQuantity { get; set; }
		public decimal MinStockQuantity { get; set; }
		public decimal ReservedStockQuantity { get; set; }
		public int Site { get; set; }
		public decimal FaNegativeQuantity { get; set; }
		public decimal FaPositiveQuantity { get; set; }
		public decimal AbQuantity { get; set; }
		public StockStatusSimpleModel(
			Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus stockEntity)
		{
			if(stockEntity == null)
				return;

			// -
			ArticleId = stockEntity.ArtikelNr;
			ArticleNumber = stockEntity.ArtikelNummer;
			Designation1 = stockEntity.Bezeichnung1;
			// -
			CurrentQuantity = stockEntity?.Bestand ?? 0;
			MinStockQuantity = stockEntity?.Mindestbestand ?? 0;
			ReservedStockQuantity = stockEntity?.Bestand_reserviert ?? 0;
			Site = stockEntity?.Lagerort_id ?? 0;

			FaNegativeQuantity = stockEntity?.FaNegativeQuantity ?? 0;
			FaPositiveQuantity = stockEntity?.FaPositiveQuantity ?? 0;
			AbQuantity = stockEntity?.AbQuantity ?? 0;
		}
	}
	public class StockStatusItemRequestModel
	{
		public int ArticleId { get; set; }
		public int? LagerId { get; set; }
	}
	public class StockStatusItemResponseModel
	{
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public List<StockItem> StockItems { get; set; }
		public List<ABItem> ABItems { get; set; }
		public List<FAItem> FAItems { get; set; }
		public List<FAItem> FAItemsPositive { get; set; }
		public StockStatusItemResponseModel()
		{
			StockItems = new List<StockItem>();
			ABItems = new List<ABItem>();
			FAItems = new List<FAItem>();
			FAItemsPositive = new List<FAItem>();
		}

		public class StockItem
		{
			public int Id { get; set; }
			public decimal CurrentQuantity { get; set; }
			public decimal MinStockQuantity { get; set; }
			public decimal ReservedStockQuantity { get; set; }
			public int Site { get; set; }
			public decimal ForecastNegative { get; set; }
			public decimal ForecastPositive { get; set; }
			public StockItem() { }
			public StockItem(Infrastructure.Data.Entities.Tables.PRS.LagerEntity lagerEntity)
			{
				if(lagerEntity == null)
					return;

				// -
				Id = lagerEntity.ID;
				CurrentQuantity = lagerEntity.Bestand ?? 0;
				MinStockQuantity = lagerEntity.Mindestbestand ?? 0;
				ReservedStockQuantity = lagerEntity.Bestand_reserviert ?? 0;
				Site = lagerEntity.Lagerort_id ?? 0;
			}
		}
		public class ABItem
		{
			public int ABId { get; set; }
			public bool ABBooked { get; set; }
			public int ABNumber { get; set; }
			public int ABPositionId { get; set; }
			public int? ABPositionNumber { get; set; }
			public decimal ABPostionOpenQuantity { get; set; }
			public decimal ABPostionOriginalQuantity { get; set; }
			public int ABPositionArticleId { get; set; }
			public string ABPositionArticleNumber { get; set; }
			public string ABPositionArticleCustomerIndex { get; set; }
			public int? ABSite { get; set; }
			public ABItem(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
				Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angeboteneArtikelEntity,
				Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
			{
				ABId = angeboteEntity?.Nr ?? 0;
				ABNumber = angeboteEntity?.Angebot_Nr ?? 0;
				ABBooked = angeboteEntity?.Gebucht ?? false;
				ABPositionId = angeboteneArtikelEntity?.Nr ?? 0;
				ABPositionNumber = angeboteneArtikelEntity?.Position;
				ABPostionOpenQuantity = angeboteneArtikelEntity.Anzahl ?? 0;
				ABPostionOriginalQuantity = angeboteneArtikelEntity?.OriginalAnzahl ?? 0;
				ABPositionArticleId = angeboteneArtikelEntity?.ArtikelNr ?? 0;
				ABPositionArticleNumber = artikelEntity?.ArtikelNummer;
				ABPositionArticleCustomerIndex = angeboteneArtikelEntity?.Index_Kunde;
				ABSite = angeboteneArtikelEntity.Lagerort_id;
			}

		}
		public class FAItem
		{
			public int FAId { get; set; }
			public int FANumber { get; set; }
			public int FAArticleId { get; set; }
			public string FAArticleNumber { get; set; }
			public string FAArticleCustomerIndex { get; set; }
			public decimal FAOpenQuantity { get; set; }
			public decimal FAOriginalQuantity { get; set; }
			public DateTime? FADate { get; set; }
			public DateTime? FAWDate { get; set; }
			public int? FASite { get; set; }
			public FAItem()
			{

			}
			public FAItem(
				Infrastructure.Data.Entities.Tables.PRS.FertigungEntity fertigungEntity,
				Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity fertigungPositionenEntity,
				Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
			{
				FAId = fertigungPositionenEntity?.ID_Fertigung ?? 0;
				FANumber = fertigungEntity?.Fertigungsnummer ?? 0;
				FAArticleId = fertigungPositionenEntity?.Artikel_Nr ?? 0;
				FAArticleNumber = artikelEntity?.ArtikelNummer;
				FAArticleCustomerIndex = artikelEntity?.Index_Kunde;
				FAOpenQuantity = ((decimal?)fertigungPositionenEntity?.Anzahl ?? 0) * (fertigungEntity?.Anzahl ?? 0) / ((fertigungEntity?.Originalanzahl ?? 1) == 0 ? 1 : (fertigungEntity?.Originalanzahl ?? 1));
				FAOriginalQuantity = ((decimal?)fertigungPositionenEntity?.Anzahl ?? 0);
				FADate = fertigungEntity?.Termin_Bestatigt1; // - 2022-12-12 - Schremmer fertigungEntity?.Termin_Bestatigt2;
				FAWDate = fertigungEntity?.Termin_Bestatigt2;
				FASite = fertigungEntity?.Lagerort_id;
			}
		}
	}
}
