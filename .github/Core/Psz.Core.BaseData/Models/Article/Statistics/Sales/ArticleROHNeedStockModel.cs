using Geocoding.Microsoft.Json;
using iText.Layout.Element;
using Psz.Core.BaseData.Helpers;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.Sales
{
	public class ArticleROHNeedStockRequestModel: IPaginatedRequestModel
	{
		public string Stufe { get; set; }
		public string ArticleNumber { get; set; }
		public int? AdressenNr { get; set; }
		public bool OnlyPrioSupplier { get; set; } = false;
		public bool Fa30Postive { get; set; } = true;
		public string ArticleClassification { get; set; }
	}
	public class ArticleROHNeedStockResponseModel
	{
		#region props
		public string Artikelnummer { get; set; }
		public decimal BedarfPO { get; set; }
		public string Bestellung { get; set; }
		public int BsQuantityArtikelNr { get; set; }
		public decimal EK { get; set; }
		public int FaNeedsArtikelNr { get; set; }
		public decimal GesamtbedarfOffeneFA360 { get; set; }
		public string ArticleClassification { get; set; }
		public string Description2 { get; set; }
		public decimal Lager { get; set; }
		public decimal Lager_CW1 { get; set; }
		public decimal Lager_CW10 { get; set; }
		public decimal Lager_CW11 { get; set; }
		public decimal Lager_CW12 { get; set; }
		public decimal Lager_CW13 { get; set; }
		public decimal Lager_CW14 { get; set; }
		public decimal Lager_CW15 { get; set; }
		public decimal Lager_CW16 { get; set; }
		public decimal Lager_CW17 { get; set; }
		public decimal Lager_CW18 { get; set; }
		public decimal Lager_CW19 { get; set; }
		public decimal Lager_CW2 { get; set; }
		public decimal Lager_CW20 { get; set; }
		public decimal Lager_CW21 { get; set; }
		public decimal Lager_CW22 { get; set; }
		public decimal Lager_CW23 { get; set; }
		public decimal Lager_CW24 { get; set; }
		public decimal Lager_CW25 { get; set; }
		public decimal Lager_CW26 { get; set; }
		public decimal Lager_CW27 { get; set; }
		public decimal Lager_CW28 { get; set; }
		public decimal Lager_CW29 { get; set; }
		public decimal Lager_CW3 { get; set; }
		public decimal Lager_CW30 { get; set; }
		public decimal Lager_CW31 { get; set; }
		public decimal Lager_CW32 { get; set; }
		public decimal Lager_CW33 { get; set; }
		public decimal Lager_CW34 { get; set; }
		public decimal Lager_CW35 { get; set; }
		public decimal Lager_CW36 { get; set; }
		public decimal Lager_CW37 { get; set; }
		public decimal Lager_CW38 { get; set; }
		public decimal Lager_CW39 { get; set; }
		public decimal Lager_CW4 { get; set; }
		public decimal Lager_CW40 { get; set; }
		public decimal Lager_CW41 { get; set; }
		public decimal Lager_CW42 { get; set; }
		public decimal Lager_CW43 { get; set; }
		public decimal Lager_CW44 { get; set; }
		public decimal Lager_CW45 { get; set; }
		public decimal Lager_CW46 { get; set; }
		public decimal Lager_CW47 { get; set; }
		public decimal Lager_CW48 { get; set; }
		public decimal Lager_CW49 { get; set; }
		public decimal Lager_CW5 { get; set; }
		public decimal Lager_CW50 { get; set; }
		public decimal Lager_CW51 { get; set; }
		public decimal Lager_CW52 { get; set; }
		public decimal Lager_CW53 { get; set; }
		public decimal Lager_CW6 { get; set; }
		public decimal Lager_CW7 { get; set; }
		public decimal Lager_CW8 { get; set; }
		public decimal Lager_CW9 { get; set; }
		public string LieferantArtikelnummer { get; set; }
		public string Lieferzeit { get; set; }
		public decimal Min_Lagerbestand { get; set; }
		public decimal Need_CW1 { get; set; }
		public decimal Need_CW10 { get; set; }
		public decimal Need_CW11 { get; set; }
		public decimal Need_CW12 { get; set; }
		public decimal Need_CW13 { get; set; }
		public decimal Need_CW14 { get; set; }
		public decimal Need_CW15 { get; set; }
		public decimal Need_CW16 { get; set; }
		public decimal Need_CW17 { get; set; }
		public decimal Need_CW18 { get; set; }
		public decimal Need_CW19 { get; set; }
		public decimal Need_CW2 { get; set; }
		public decimal Need_CW20 { get; set; }
		public decimal Need_CW21 { get; set; }
		public decimal Need_CW22 { get; set; }
		public decimal Need_CW23 { get; set; }
		public decimal Need_CW24 { get; set; }
		public decimal Need_CW25 { get; set; }
		public decimal Need_CW26 { get; set; }
		public decimal Need_CW27 { get; set; }
		public decimal Need_CW28 { get; set; }
		public decimal Need_CW29 { get; set; }
		public decimal Need_CW3 { get; set; }
		public decimal Need_CW30 { get; set; }
		public decimal Need_CW31 { get; set; }
		public decimal Need_CW32 { get; set; }
		public decimal Need_CW33 { get; set; }
		public decimal Need_CW34 { get; set; }
		public decimal Need_CW35 { get; set; }
		public decimal Need_CW36 { get; set; }
		public decimal Need_CW37 { get; set; }
		public decimal Need_CW38 { get; set; }
		public decimal Need_CW39 { get; set; }
		public decimal Need_CW4 { get; set; }
		public decimal Need_CW40 { get; set; }
		public decimal Need_CW41 { get; set; }
		public decimal Need_CW42 { get; set; }
		public decimal Need_CW43 { get; set; }
		public decimal Need_CW44 { get; set; }
		public decimal Need_CW45 { get; set; }
		public decimal Need_CW46 { get; set; }
		public decimal Need_CW47 { get; set; }
		public decimal Need_CW48 { get; set; }
		public decimal Need_CW49 { get; set; }
		public decimal Need_CW5 { get; set; }
		public decimal Need_CW50 { get; set; }
		public decimal Need_CW51 { get; set; }
		public decimal Need_CW52 { get; set; }
		public decimal Need_CW53 { get; set; }
		public decimal Need_CW6 { get; set; }
		public decimal Need_CW7 { get; set; }
		public decimal Need_CW8 { get; set; }
		public decimal Need_CW9 { get; set; }
		public int Need_CYear { get; set; }
		public decimal Order_CW1 { get; set; }
		public decimal Order_CW10 { get; set; }
		public decimal Order_CW11 { get; set; }
		public decimal Order_CW12 { get; set; }
		public decimal Order_CW13 { get; set; }
		public decimal Order_CW14 { get; set; }
		public decimal Order_CW15 { get; set; }
		public decimal Order_CW16 { get; set; }
		public decimal Order_CW17 { get; set; }
		public decimal Order_CW18 { get; set; }
		public decimal Order_CW19 { get; set; }
		public decimal Order_CW2 { get; set; }
		public decimal Order_CW20 { get; set; }
		public decimal Order_CW21 { get; set; }
		public decimal Order_CW22 { get; set; }
		public decimal Order_CW23 { get; set; }
		public decimal Order_CW24 { get; set; }
		public decimal Order_CW25 { get; set; }
		public decimal Order_CW26 { get; set; }
		public decimal Order_CW27 { get; set; }
		public decimal Order_CW28 { get; set; }
		public decimal Order_CW29 { get; set; }
		public decimal Order_CW3 { get; set; }
		public decimal Order_CW30 { get; set; }
		public decimal Order_CW31 { get; set; }
		public decimal Order_CW32 { get; set; }
		public decimal Order_CW33 { get; set; }
		public decimal Order_CW34 { get; set; }
		public decimal Order_CW35 { get; set; }
		public decimal Order_CW36 { get; set; }
		public decimal Order_CW37 { get; set; }
		public decimal Order_CW38 { get; set; }
		public decimal Order_CW39 { get; set; }
		public decimal Order_CW4 { get; set; }
		public decimal Order_CW40 { get; set; }
		public decimal Order_CW41 { get; set; }
		public decimal Order_CW42 { get; set; }
		public decimal Order_CW43 { get; set; }
		public decimal Order_CW44 { get; set; }
		public decimal Order_CW45 { get; set; }
		public decimal Order_CW46 { get; set; }
		public decimal Order_CW47 { get; set; }
		public decimal Order_CW48 { get; set; }
		public decimal Order_CW49 { get; set; }
		public decimal Order_CW5 { get; set; }
		public decimal Order_CW50 { get; set; }
		public decimal Order_CW51 { get; set; }
		public decimal Order_CW52 { get; set; }
		public decimal Order_CW53 { get; set; }
		public decimal Order_CW6 { get; set; }
		public decimal Order_CW7 { get; set; }
		public decimal Order_CW8 { get; set; }
		public decimal Order_CW9 { get; set; }
		public int Order_CYear { get; set; }
		public string PRIO1_Lieferant { get; set; }
		public decimal SummePO { get; set; }
		public decimal Verfugbarbestand { get; set; }
		public Single VPE_Losgroesse { get; set; }
		public decimal? Mindestbestellmenge { get; set; }
		public string Label_CW1 { get; set; }
		public string Label_CW10 { get; set; }
		public string Label_CW11 { get; set; }
		public string Label_CW12 { get; set; }
		public string Label_CW13 { get; set; }
		public string Label_CW14 { get; set; }
		public string Label_CW15 { get; set; }
		public string Label_CW16 { get; set; }
		public string Label_CW17 { get; set; }
		public string Label_CW18 { get; set; }
		public string Label_CW19 { get; set; }
		public string Label_CW2 { get; set; }
		public string Label_CW20 { get; set; }
		public string Label_CW21 { get; set; }
		public string Label_CW22 { get; set; }
		public string Label_CW23 { get; set; }
		public string Label_CW24 { get; set; }
		public string Label_CW25 { get; set; }
		public string Label_CW26 { get; set; }
		public string Label_CW27 { get; set; }
		public string Label_CW28 { get; set; }
		public string Label_CW29 { get; set; }
		public string Label_CW3 { get; set; }
		public string Label_CW30 { get; set; }
		public string Label_CW31 { get; set; }
		public string Label_CW32 { get; set; }
		public string Label_CW33 { get; set; }
		public string Label_CW34 { get; set; }
		public string Label_CW35 { get; set; }
		public string Label_CW36 { get; set; }
		public string Label_CW37 { get; set; }
		public string Label_CW38 { get; set; }
		public string Label_CW39 { get; set; }
		public string Label_CW4 { get; set; }
		public string Label_CW40 { get; set; }
		public string Label_CW41 { get; set; }
		public string Label_CW42 { get; set; }
		public string Label_CW43 { get; set; }
		public string Label_CW44 { get; set; }
		public string Label_CW45 { get; set; }
		public string Label_CW46 { get; set; }
		public string Label_CW47 { get; set; }
		public string Label_CW48 { get; set; }
		public string Label_CW49 { get; set; }
		public string Label_CW5 { get; set; }
		public string Label_CW50 { get; set; }
		public string Label_CW51 { get; set; }
		public string Label_CW52 { get; set; }
		public string Label_CW53 { get; set; }
		public string Label_CW6 { get; set; }
		public string Label_CW7 { get; set; }
		public string Label_CW8 { get; set; }
		public string Label_CW9 { get; set; }
		#endregion props
		#region Min To Order
		public decimal MinToOrder_CW1 { get; set; }
		public decimal MinToOrder_CW10 { get; set; }
		public decimal MinToOrder_CW11 { get; set; }
		public decimal MinToOrder_CW12 { get; set; }
		public decimal MinToOrder_CW13 { get; set; }
		public decimal MinToOrder_CW14 { get; set; }
		public decimal MinToOrder_CW15 { get; set; }
		public decimal MinToOrder_CW16 { get; set; }
		public decimal MinToOrder_CW17 { get; set; }
		public decimal MinToOrder_CW18 { get; set; }
		public decimal MinToOrder_CW19 { get; set; }
		public decimal MinToOrder_CW2 { get; set; }
		public decimal MinToOrder_CW20 { get; set; }
		public decimal MinToOrder_CW21 { get; set; }
		public decimal MinToOrder_CW22 { get; set; }
		public decimal MinToOrder_CW23 { get; set; }
		public decimal MinToOrder_CW24 { get; set; }
		public decimal MinToOrder_CW25 { get; set; }
		public decimal MinToOrder_CW26 { get; set; }
		public decimal MinToOrder_CW27 { get; set; }
		public decimal MinToOrder_CW28 { get; set; }
		public decimal MinToOrder_CW29 { get; set; }
		public decimal MinToOrder_CW3 { get; set; }
		public decimal MinToOrder_CW30 { get; set; }
		public decimal MinToOrder_CW31 { get; set; }
		public decimal MinToOrder_CW32 { get; set; }
		public decimal MinToOrder_CW33 { get; set; }
		public decimal MinToOrder_CW34 { get; set; }
		public decimal MinToOrder_CW35 { get; set; }
		public decimal MinToOrder_CW36 { get; set; }
		public decimal MinToOrder_CW37 { get; set; }
		public decimal MinToOrder_CW38 { get; set; }
		public decimal MinToOrder_CW39 { get; set; }
		public decimal MinToOrder_CW4 { get; set; }
		public decimal MinToOrder_CW40 { get; set; }
		public decimal MinToOrder_CW41 { get; set; }
		public decimal MinToOrder_CW42 { get; set; }
		public decimal MinToOrder_CW43 { get; set; }
		public decimal MinToOrder_CW44 { get; set; }
		public decimal MinToOrder_CW45 { get; set; }
		public decimal MinToOrder_CW46 { get; set; }
		public decimal MinToOrder_CW47 { get; set; }
		public decimal MinToOrder_CW48 { get; set; }
		public decimal MinToOrder_CW49 { get; set; }
		public decimal MinToOrder_CW5 { get; set; }
		public decimal MinToOrder_CW50 { get; set; }
		public decimal MinToOrder_CW51 { get; set; }
		public decimal MinToOrder_CW52 { get; set; }
		public decimal MinToOrder_CW53 { get; set; }
		public decimal MinToOrder_CW6 { get; set; }
		public decimal MinToOrder_CW7 { get; set; }
		public decimal MinToOrder_CW8 { get; set; }
		public decimal MinToOrder_CW9 { get; set; }
		#endregion
		public int SyncId { get; set; }
		public DateTime SyncDate { get; set; }
		public decimal PoDelay { get; set; }
		public decimal FaDelay { get; set; }
		public int Year { get; set; }

		public ArticleROHNeedStockResponseModel()
		{

		}
		public ArticleROHNeedStockResponseModel(Infrastructure.Data.Entities.Functions.ArticleROHNeedStockEntity entity)
		{
			if(entity is null)
			{
				return;
			}

			// -
			Artikelnummer = entity.Artikelnummer;
			BedarfPO = entity.BedarfPO;
			Bestellung = entity.Bestellung;
			BsQuantityArtikelNr = entity.BsQuantityArtikelNr;
			EK = entity.EK ?? 0;
			FaNeedsArtikelNr = entity.FaNeedsArtikelNr;
			GesamtbedarfOffeneFA360 = entity.GesamtbedarfOffeneFA360;
			Lager = entity.Lager ?? 0;
			Lager_CW1 = entity.Lager_CW1 ?? 0;
			Lager_CW10 = entity.Lager_CW10 ?? 0;
			Lager_CW11 = entity.Lager_CW11 ?? 0;
			Lager_CW12 = entity.Lager_CW12 ?? 0;
			Lager_CW13 = entity.Lager_CW13 ?? 0;
			Lager_CW14 = entity.Lager_CW14 ?? 0;
			Lager_CW15 = entity.Lager_CW15 ?? 0;
			Lager_CW16 = entity.Lager_CW16 ?? 0;
			Lager_CW17 = entity.Lager_CW17 ?? 0;
			Lager_CW18 = entity.Lager_CW18 ?? 0;
			Lager_CW19 = entity.Lager_CW19 ?? 0;
			Lager_CW2 = entity.Lager_CW2 ?? 0;
			Lager_CW20 = entity.Lager_CW20 ?? 0;
			Lager_CW21 = entity.Lager_CW21 ?? 0;
			Lager_CW22 = entity.Lager_CW22 ?? 0;
			Lager_CW23 = entity.Lager_CW23 ?? 0;
			Lager_CW24 = entity.Lager_CW24 ?? 0;
			Lager_CW25 = entity.Lager_CW25 ?? 0;
			Lager_CW26 = entity.Lager_CW26 ?? 0;
			Lager_CW27 = entity.Lager_CW27 ?? 0;
			Lager_CW28 = entity.Lager_CW28 ?? 0;
			Lager_CW29 = entity.Lager_CW29 ?? 0;
			Lager_CW3 = entity.Lager_CW3 ?? 0;
			Lager_CW30 = entity.Lager_CW30 ?? 0;
			Lager_CW31 = entity.Lager_CW31 ?? 0;
			Lager_CW32 = entity.Lager_CW32 ?? 0;
			Lager_CW33 = entity.Lager_CW33 ?? 0;
			Lager_CW34 = entity.Lager_CW34 ?? 0;
			Lager_CW35 = entity.Lager_CW35 ?? 0;
			Lager_CW36 = entity.Lager_CW36 ?? 0;
			Lager_CW37 = entity.Lager_CW37 ?? 0;
			Lager_CW38 = entity.Lager_CW38 ?? 0;
			Lager_CW39 = entity.Lager_CW39 ?? 0;
			Lager_CW4 = entity.Lager_CW4 ?? 0;
			Lager_CW40 = entity.Lager_CW40 ?? 0;
			Lager_CW41 = entity.Lager_CW41 ?? 0;
			Lager_CW42 = entity.Lager_CW42 ?? 0;
			Lager_CW43 = entity.Lager_CW43 ?? 0;
			Lager_CW44 = entity.Lager_CW44 ?? 0;
			Lager_CW45 = entity.Lager_CW45 ?? 0;
			Lager_CW46 = entity.Lager_CW46 ?? 0;
			Lager_CW47 = entity.Lager_CW47 ?? 0;
			Lager_CW48 = entity.Lager_CW48 ?? 0;
			Lager_CW49 = entity.Lager_CW49 ?? 0;
			Lager_CW5 = entity.Lager_CW5 ?? 0;
			Lager_CW50 = entity.Lager_CW50 ?? 0;
			Lager_CW51 = entity.Lager_CW51 ?? 0;
			Lager_CW52 = entity.Lager_CW52 ?? 0;
			Lager_CW53 = entity.Lager_CW53 ?? 0;
			Lager_CW6 = entity.Lager_CW6 ?? 0;
			Lager_CW7 = entity.Lager_CW7 ?? 0;
			Lager_CW8 = entity.Lager_CW8 ?? 0;
			Lager_CW9 = entity.Lager_CW9 ?? 0;
			LieferantArtikelnummer = entity.LieferantArtikelnummer;
			Lieferzeit = entity.Lieferzeit;
			Min_Lagerbestand = entity.Min_Lagerbestand;
			Need_CW1 = entity.Need_CW1 ?? 0;
			Need_CW10 = entity.Need_CW10 ?? 0;
			Need_CW11 = entity.Need_CW11 ?? 0;
			Need_CW12 = entity.Need_CW12 ?? 0;
			Need_CW13 = entity.Need_CW13 ?? 0;
			Need_CW14 = entity.Need_CW14 ?? 0;
			Need_CW15 = entity.Need_CW15 ?? 0;
			Need_CW16 = entity.Need_CW16 ?? 0;
			Need_CW17 = entity.Need_CW17 ?? 0;
			Need_CW18 = entity.Need_CW18 ?? 0;
			Need_CW19 = entity.Need_CW19 ?? 0;
			Need_CW2 = entity.Need_CW2 ?? 0;
			Need_CW20 = entity.Need_CW20 ?? 0;
			Need_CW21 = entity.Need_CW21 ?? 0;
			Need_CW22 = entity.Need_CW22 ?? 0;
			Need_CW23 = entity.Need_CW23 ?? 0;
			Need_CW24 = entity.Need_CW24 ?? 0;
			Need_CW25 = entity.Need_CW25 ?? 0;
			Need_CW26 = entity.Need_CW26 ?? 0;
			Need_CW27 = entity.Need_CW27 ?? 0;
			Need_CW28 = entity.Need_CW28 ?? 0;
			Need_CW29 = entity.Need_CW29 ?? 0;
			Need_CW3 = entity.Need_CW3 ?? 0;
			Need_CW30 = entity.Need_CW30 ?? 0;
			Need_CW31 = entity.Need_CW31 ?? 0;
			Need_CW32 = entity.Need_CW32 ?? 0;
			Need_CW33 = entity.Need_CW33 ?? 0;
			Need_CW34 = entity.Need_CW34 ?? 0;
			Need_CW35 = entity.Need_CW35 ?? 0;
			Need_CW36 = entity.Need_CW36 ?? 0;
			Need_CW37 = entity.Need_CW37 ?? 0;
			Need_CW38 = entity.Need_CW38 ?? 0;
			Need_CW39 = entity.Need_CW39 ?? 0;
			Need_CW4 = entity.Need_CW4 ?? 0;
			Need_CW40 = entity.Need_CW40 ?? 0;
			Need_CW41 = entity.Need_CW41 ?? 0;
			Need_CW42 = entity.Need_CW42 ?? 0;
			Need_CW43 = entity.Need_CW43 ?? 0;
			Need_CW44 = entity.Need_CW44 ?? 0;
			Need_CW45 = entity.Need_CW45 ?? 0;
			Need_CW46 = entity.Need_CW46 ?? 0;
			Need_CW47 = entity.Need_CW47 ?? 0;
			Need_CW48 = entity.Need_CW48 ?? 0;
			Need_CW49 = entity.Need_CW49 ?? 0;
			Need_CW5 = entity.Need_CW5 ?? 0;
			Need_CW50 = entity.Need_CW50 ?? 0;
			Need_CW51 = entity.Need_CW51 ?? 0;
			Need_CW52 = entity.Need_CW52 ?? 0;
			Need_CW53 = entity.Need_CW53 ?? 0;
			Need_CW6 = entity.Need_CW6 ?? 0;
			Need_CW7 = entity.Need_CW7 ?? 0;
			Need_CW8 = entity.Need_CW8 ?? 0;
			Need_CW9 = entity.Need_CW9 ?? 0;
			Need_CYear = entity.Need_CYear;
			Order_CW1 = entity.Order_CW1 ?? 0;
			Order_CW10 = entity.Order_CW10 ?? 0;
			Order_CW11 = entity.Order_CW11 ?? 0;
			Order_CW12 = entity.Order_CW12 ?? 0;
			Order_CW13 = entity.Order_CW13 ?? 0;
			Order_CW14 = entity.Order_CW14 ?? 0;
			Order_CW15 = entity.Order_CW15 ?? 0;
			Order_CW16 = entity.Order_CW16 ?? 0;
			Order_CW17 = entity.Order_CW17 ?? 0;
			Order_CW18 = entity.Order_CW18 ?? 0;
			Order_CW19 = entity.Order_CW19 ?? 0;
			Order_CW2 = entity.Order_CW2 ?? 0;
			Order_CW20 = entity.Order_CW20 ?? 0;
			Order_CW21 = entity.Order_CW21 ?? 0;
			Order_CW22 = entity.Order_CW22 ?? 0;
			Order_CW23 = entity.Order_CW23 ?? 0;
			Order_CW24 = entity.Order_CW24 ?? 0;
			Order_CW25 = entity.Order_CW25 ?? 0;
			Order_CW26 = entity.Order_CW26 ?? 0;
			Order_CW27 = entity.Order_CW27 ?? 0;
			Order_CW28 = entity.Order_CW28 ?? 0;
			Order_CW29 = entity.Order_CW29 ?? 0;
			Order_CW3 = entity.Order_CW3 ?? 0;
			Order_CW30 = entity.Order_CW30 ?? 0;
			Order_CW31 = entity.Order_CW31 ?? 0;
			Order_CW32 = entity.Order_CW32 ?? 0;
			Order_CW33 = entity.Order_CW33 ?? 0;
			Order_CW34 = entity.Order_CW34 ?? 0;
			Order_CW35 = entity.Order_CW35 ?? 0;
			Order_CW36 = entity.Order_CW36 ?? 0;
			Order_CW37 = entity.Order_CW37 ?? 0;
			Order_CW38 = entity.Order_CW38 ?? 0;
			Order_CW39 = entity.Order_CW39 ?? 0;
			Order_CW4 = entity.Order_CW4 ?? 0;
			Order_CW40 = entity.Order_CW40 ?? 0;
			Order_CW41 = entity.Order_CW41 ?? 0;
			Order_CW42 = entity.Order_CW42 ?? 0;
			Order_CW43 = entity.Order_CW43 ?? 0;
			Order_CW44 = entity.Order_CW44 ?? 0;
			Order_CW45 = entity.Order_CW45 ?? 0;
			Order_CW46 = entity.Order_CW46 ?? 0;
			Order_CW47 = entity.Order_CW47 ?? 0;
			Order_CW48 = entity.Order_CW48 ?? 0;
			Order_CW49 = entity.Order_CW49 ?? 0;
			Order_CW5 = entity.Order_CW5 ?? 0;
			Order_CW50 = entity.Order_CW50 ?? 0;
			Order_CW51 = entity.Order_CW51 ?? 0;
			Order_CW52 = entity.Order_CW52 ?? 0;
			Order_CW53 = entity.Order_CW53 ?? 0;
			Order_CW6 = entity.Order_CW6 ?? 0;
			Order_CW7 = entity.Order_CW7 ?? 0;
			Order_CW8 = entity.Order_CW8 ?? 0;
			Order_CW9 = entity.Order_CW9 ?? 0;
			Order_CYear = entity.Order_CYear;
			PRIO1_Lieferant = entity.PRIO1_Lieferant;
			SummePO = entity.SummePO;
			Verfugbarbestand = entity.Verfugbarbestand ?? 0;
			VPE_Losgroesse = entity.VPE_Losgroesse ?? 0;
		}
		public ArticleROHNeedStockResponseModel(Infrastructure.Data.Entities.Tables.BSD.MaterialRequirementsHeaderEntity entity)
		{
			if(entity is null)
			{
				return;
			}

			// -
			Artikelnummer = entity.Artikelnummer;
			BedarfPO = entity.BedarfPO;
			Bestellung = entity.Bestellung;
			BsQuantityArtikelNr = entity.ArtikelNr;
			EK = entity.EK ?? 0;
			FaNeedsArtikelNr = entity.ArtikelNr;
			GesamtbedarfOffeneFA360 = entity.GesamtbedarfOffeneFA360;
			Lager = entity.Lager ?? 0;
			LieferantArtikelnummer = entity.LieferantArtikelnummer;
			Lieferzeit = entity.Lieferzeit;
			Min_Lagerbestand = entity.Min_Lagerbestand;
			PRIO1_Lieferant = entity.PRIO1_Lieferant;
			SummePO = entity.SummePO;
			Verfugbarbestand = entity.Verfugbarbestand ?? 0;
			VPE_Losgroesse = entity.VPE_Losgroesse ?? 0;
			Mindestbestellmenge = entity.Mindestbestellmenge ?? 0;

			// -
			SyncId = entity.SyncId;
			SyncDate = entity.SyncDate ?? DateTime.MinValue;
			ArticleClassification = entity.artikelklassifizierung;
			Description2 = entity.Description2;
		}
		public List<ArticleROHNeedStockAsList> ToOrdredList()
		{
			var result = new List<ArticleROHNeedStockAsList>
			{
				new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW1).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW1).Key, Lager = Lager_CW1, Order = Order_CW1, Need = Need_CW1, MinToOrder = MinToOrder_CW1 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW2).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW2).Key, Lager = Lager_CW2, Order = Order_CW2, Need = Need_CW2, MinToOrder = MinToOrder_CW2 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW3).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW3).Key, Lager = Lager_CW3, Order = Order_CW3, Need = Need_CW3, MinToOrder = MinToOrder_CW3 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW4).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW4).Key, Lager = Lager_CW4, Order = Order_CW4, Need = Need_CW4, MinToOrder = MinToOrder_CW4 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW5).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW5).Key, Lager = Lager_CW5, Order = Order_CW5, Need = Need_CW5, MinToOrder = MinToOrder_CW5 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW6).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW6).Key, Lager = Lager_CW6, Order = Order_CW6, Need = Need_CW6, MinToOrder = MinToOrder_CW6 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW7).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW7).Key, Lager = Lager_CW7, Order = Order_CW7, Need = Need_CW7, MinToOrder = MinToOrder_CW7 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW8).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW8).Key, Lager = Lager_CW8, Order = Order_CW8, Need = Need_CW8, MinToOrder = MinToOrder_CW8 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW9).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW9).Key, Lager = Lager_CW9, Order = Order_CW9, Need = Need_CW9, MinToOrder = MinToOrder_CW9 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW10).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW10).Key, Lager = Lager_CW10, Order = Order_CW10, Need = Need_CW10, MinToOrder = MinToOrder_CW10 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW11).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW11).Key, Lager = Lager_CW11, Order = Order_CW11, Need = Need_CW11, MinToOrder = MinToOrder_CW11 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW12).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW12).Key, Lager = Lager_CW12, Order = Order_CW12, Need = Need_CW12, MinToOrder = MinToOrder_CW12 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW13).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW13).Key, Lager = Lager_CW13, Order = Order_CW13, Need = Need_CW13, MinToOrder = MinToOrder_CW13 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW14).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW14).Key, Lager = Lager_CW14, Order = Order_CW14, Need = Need_CW14, MinToOrder = MinToOrder_CW14 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW15).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW15).Key, Lager = Lager_CW15, Order = Order_CW15, Need = Need_CW15, MinToOrder = MinToOrder_CW15 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW16).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW16).Key, Lager = Lager_CW16, Order = Order_CW16, Need = Need_CW16, MinToOrder = MinToOrder_CW16 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW17).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW17).Key, Lager = Lager_CW17, Order = Order_CW17, Need = Need_CW17, MinToOrder = MinToOrder_CW17 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW18).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW18).Key, Lager = Lager_CW18, Order = Order_CW18, Need = Need_CW18, MinToOrder = MinToOrder_CW18 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW19).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW19).Key, Lager = Lager_CW19, Order = Order_CW19, Need = Need_CW19, MinToOrder = MinToOrder_CW19 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW20).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW20).Key, Lager = Lager_CW20, Order = Order_CW20, Need = Need_CW20, MinToOrder = MinToOrder_CW20 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW21).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW21).Key, Lager = Lager_CW21, Order = Order_CW21, Need = Need_CW21, MinToOrder = MinToOrder_CW21 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW22).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW22).Key, Lager = Lager_CW22, Order = Order_CW22, Need = Need_CW22, MinToOrder = MinToOrder_CW22 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW23).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW23).Key, Lager = Lager_CW23, Order = Order_CW23, Need = Need_CW23, MinToOrder = MinToOrder_CW23 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW24).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW24).Key, Lager = Lager_CW24, Order = Order_CW24, Need = Need_CW24, MinToOrder = MinToOrder_CW24 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW25).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW25).Key, Lager = Lager_CW25, Order = Order_CW25, Need = Need_CW25, MinToOrder = MinToOrder_CW25 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW26).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW26).Key, Lager = Lager_CW26, Order = Order_CW26, Need = Need_CW26, MinToOrder = MinToOrder_CW26 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW27).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW27).Key, Lager = Lager_CW27, Order = Order_CW27, Need = Need_CW27, MinToOrder = MinToOrder_CW27 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW28).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW28).Key, Lager = Lager_CW28, Order = Order_CW28, Need = Need_CW28, MinToOrder = MinToOrder_CW28 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW29).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW29).Key, Lager = Lager_CW29, Order = Order_CW29, Need = Need_CW29, MinToOrder = MinToOrder_CW29 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW30).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW30).Key, Lager = Lager_CW30, Order = Order_CW30, Need = Need_CW30, MinToOrder = MinToOrder_CW30 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW31).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW31).Key, Lager = Lager_CW31, Order = Order_CW31, Need = Need_CW31, MinToOrder = MinToOrder_CW31 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW32).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW32).Key, Lager = Lager_CW32, Order = Order_CW32, Need = Need_CW32, MinToOrder = MinToOrder_CW32 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW33).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW33).Key, Lager = Lager_CW33, Order = Order_CW33, Need = Need_CW33, MinToOrder = MinToOrder_CW33 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW34).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW34).Key, Lager = Lager_CW34, Order = Order_CW34, Need = Need_CW34, MinToOrder = MinToOrder_CW34 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW35).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW35).Key, Lager = Lager_CW35, Order = Order_CW35, Need = Need_CW35, MinToOrder = MinToOrder_CW35 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW36).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW36).Key, Order = Order_CW36, Need = Need_CW36, MinToOrder = MinToOrder_CW36 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW37).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW37).Key,  Lager = Lager_CW37, Order = Order_CW37, Need = Need_CW37, MinToOrder = MinToOrder_CW37 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW38).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW38).Key,  Lager = Lager_CW38, Order = Order_CW38, Need = Need_CW38, MinToOrder = MinToOrder_CW38 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW39).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW39).Key,  Lager = Lager_CW39, Order = Order_CW39, Need = Need_CW39, MinToOrder = MinToOrder_CW39 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW40).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW40).Key,  Lager = Lager_CW40, Order = Order_CW40, Need = Need_CW40, MinToOrder = MinToOrder_CW40 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW41).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW41).Key,  Lager = Lager_CW41, Order = Order_CW41, Need = Need_CW41, MinToOrder = MinToOrder_CW41 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW42).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW42).Key,  Lager = Lager_CW42, Order = Order_CW42, Need = Need_CW42, MinToOrder = MinToOrder_CW42 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW43).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW43).Key,  Lager = Lager_CW43, Order = Order_CW43, Need = Need_CW43, MinToOrder = MinToOrder_CW43 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW44).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW44).Key,  Lager = Lager_CW44, Order = Order_CW44, Need = Need_CW44, MinToOrder = MinToOrder_CW44 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW45).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW45).Key,  Lager = Lager_CW45, Order = Order_CW45, Need = Need_CW45, MinToOrder = MinToOrder_CW45 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW46).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW46).Key,  Lager = Lager_CW46, Order = Order_CW46, Need = Need_CW46, MinToOrder = MinToOrder_CW46 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW47).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW47).Key,  Lager = Lager_CW47, Order = Order_CW47, Need = Need_CW47, MinToOrder = MinToOrder_CW47 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW48).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW48).Key,  Lager = Lager_CW48, Order = Order_CW48, Need = Need_CW48, MinToOrder = MinToOrder_CW48 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW49).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW49).Key,  Lager = Lager_CW49, Order = Order_CW49, Need = Need_CW49, MinToOrder = MinToOrder_CW49 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW50).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW50).Key,  Lager = Lager_CW50, Order = Order_CW50, Need = Need_CW50, MinToOrder = MinToOrder_CW50 },
			new ArticleROHNeedStockAsList{Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW51).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW51).Key,  Lager = Lager_CW51, Order = Order_CW51, Need = Need_CW51, MinToOrder = MinToOrder_CW51 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW52).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW52).Key,  Lager = Lager_CW52, Order = Order_CW52, Need = Need_CW52, MinToOrder = MinToOrder_CW52 },
			new ArticleROHNeedStockAsList{ Week = SpecialHelper.GetWeekAndYearFromValue(Label_CW53).Value,Year=SpecialHelper.GetWeekAndYearFromValue(Label_CW53).Key,  Lager = Lager_CW53, Order = Order_CW53, Need = Need_CW53, MinToOrder = MinToOrder_CW53 }
			};

			return result;
		}
	}
	public class ArticleROHNeedStock_SupplierStufeResponseModel
	{
		public int CurrentCW { get; set; }
		public int AdressenNr { get; set; }
		public string Name { get; set; }
		public string Stufe { get; set; }
		public ArticleROHNeedStock_SupplierStufeResponseModel(int currrentCW, Infrastructure.Data.Entities.Tables.PRS.AdressenEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			AdressenNr = entity.Nr;
			Name = entity.Name1;
			Stufe = entity.Stufe;
			// -
			CurrentCW = currrentCW;
		}
	}

	public class ArticleROHNeedStockSyncResponseModel: IPaginatedResponseModel<ArticleROHNeedStockResponseModel>
	{

	}
	public class SupplierClassResponseModel
	{
		public int SupplierNr { get; set; }
		public string SupplierClass { get; set; }
		public string SupplierName { get; set; }
		public SupplierClassResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Sl_SupplierClass entity)
		{
			SupplierNr = entity.Nr;
			SupplierClass = entity.Stufe;
			SupplierName = entity.Name1;
		}
	}
	public class ArticleROHNeedStockBySupplierClassRequestModel
	{
		public string SupplierClass { get; set; }
		public int? SupplierNr { get; set; }
		public int? MaxRecord { get; set; } = 100;
	}
	public class ArticleROHNeedStockSyncParamsResponseModel
	{
		public int SyncId { get; set; }
		public DateTime? SyncDate { get; set; }
		public ArticleROHNeedStockSyncParamsResponseModel(Infrastructure.Data.Entities.Tables.BSD.MaterialRequirementsParamsEntity entity)
		{
			if(entity is null)
			{
				return;
			}

			// -*
			SyncId = entity.SyncId;
			SyncDate = entity.SyncDate;
		}
	}

	public class ArticleROHNeedStockAsList
	{
		public int Week { get; set; }
		public int Year { get; set; }
		public decimal Order { get; set; }
		public decimal Lager { get; set; }
		public decimal Need { get; set; }
		public decimal MinToOrder { get; set; }
	}
}
