using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Models.Order.Element
{
	public class OrderItemModel
	{
		public string ItemCustomerDescription { get; set; }
		public decimal CurrentItemPriceCalculationNet { get; set; }
		public string CustomerItemNumber { get; set; }
		// >>>>>>>>>>>>>>
		public int? ItemTypeId { get; set; }
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public int OrderId { get; set; }
		public bool Done { get; set; }
		// >>>>>>>>>>>>>>
		public int ItemId { get; set; }
		public string ItemNumber { get; set; }
		public bool RP { get; set; } // RP
		public int Position { get; set; }
		public decimal OpenQuantity_Quantity { get; set; } // Aktuelle Anzahl
		public DateTime? DesiredDate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public int StorageLocationId { get; set; }
		public string StorageLocationName { get; set; }
		public bool FixedUnitPrice { get; set; }
		public decimal UnitPrice { get; set; } // EinzelVkFestpreis // VKEinzelpreis
		public bool FixedTotalPrice { get; set; } // VkFestpreis // (VK-Festpreis  / VKGesamtpreis)
		public decimal TotalPrice { get; set; } // VkFestpreis // (VK-Festpreis  / VKGesamtpreis)
		public decimal UnitPriceBasis { get; set; } // Preiseinheit // Preiseinheit
		public decimal Discount { get; set; } // Rabatt
		public decimal VAT { get; set; } // Umsatzsteuer // USt
										 // >>>>>>>>>>>>>>
		public string Designation1 { get; set; } // Bezeichnung1
		public string Designation2 { get; set; } // Bezeichnung2
		public string Designation3 { get; set; } // Bezeichnung3
		public string MeasureUnitQualifier { get; set; } // Einheit
		public string DrawingIndex { get; set; } // Zeichnungsnummer
		public int CopperBase { get; set; } // Kupferbasis // Kupferbasis
		public bool DelFixed { get; set; } // Fix // (DEL fixiert / DEL)
		public int DelNote { get; set; } // Notiz // DEL
		public decimal CopperWeight { get; set; } // Kupfergewicht // EinzelCu-Gewicht
		public decimal CopperSurcharge { get; set; } // Kupferzuschlag // Einzelkupferzuschlag
		public int ProductionNumber { get; set; } // Fertigungsauftrag
												  // >>>>>>>>>>>>>>
		public string UnloadingPoint { get; set; } // Abladestelle
		public decimal OpenQuantity_UnitPrice { get; set; } // 8 // Einzelpreis: Einzelpreis
		public decimal OpenQuantity_TotalPrice { get; set; } // 9 // Gesamtpreis : Gesamtpreis
		public decimal OpenQuantity_CopperWeight { get; set; } // 10 // Kupfergewicht: GesamtCu-Gewicht
		public decimal OpenQuantity_CopperSurcharge { get; set; } // 11 // Kupferzuschlag: Gesamtkupferzuschlag
		public decimal OriginalOrderQuantity { get; set; }
		public decimal OriginalOrderAmount { get; set; }
		public decimal DeliveredQuantity { get; set; }
		// >>>>>>>>>>>>>>
		public string FreeText { get; set; }
		public string Note1 { get; set; }
		public string Note2 { get; set; }

		// > Consignee
		public ConsigneeModel Consignee { get; set; } = new ConsigneeModel();

		public int Version { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }
		public int? LS_ZU_AB { get; set; }
		public string CSInterneBemerkung { get; set; }
		public string Postext { get; set; }
		// - 2022-06-20
		public int ProductionId { get; set; } // FertigungsId


		public int? RahmenPosId { get; set; }
		public int? RahmenId { get; set; }
		public int? RahmenVorfallNr { get; set; }
		public string OrderType { get; set; }
		public bool? OrderIsManualCreation { get; set; }

		public OrderItemModel() { }
		public OrderItemModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity orderItemDb,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity itemDb)
		{
			var storageLocationDb = orderItemDb.Lagerort_id.HasValue ? Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get((int)orderItemDb.Lagerort_id) : null;

			var orderItemsExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderItemsIds(new List<int> { orderItemDb.Nr });
			var orderItemsExtensionDb = orderItemsExtensionsDb == null || orderItemsExtensionsDb.Count <= 0 ? null : orderItemsExtensionsDb[0];
			var fertigungEntity = orderItemDb?.Fertigungsnummer.HasValue == true && orderItemDb?.Fertigungsnummer.Value > 0
				? Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(new List<int> { orderItemDb?.Fertigungsnummer ?? -1 })?.Where(x => x.Kennzeichen?.ToLower()?.Trim() != "storno")?.FirstOrDefault()
				: null;

			Id = orderItemDb.Nr;
			OrderNumber = orderItemDb.AngebotNr?.ToString();
			OrderId = orderItemDb.AngebotNr ?? -1;
			Done = (orderItemDb.erledigt_pos ?? false);
			// >>>>>>>>>>>>>>>>
			ItemId = orderItemDb.ArtikelNr ?? -1;
			ItemNumber = itemDb?.ArtikelNummer;
			RP = orderItemDb.RP ?? false;
			Position = orderItemDb.Position ?? 0;
			OpenQuantity_Quantity = Convert.ToDecimal(orderItemDb.Anzahl ?? 0);
			DesiredDate = orderItemDb.Wunschtermin;
			DeliveryDate = orderItemDb.Liefertermin;
			StorageLocationId = storageLocationDb != null ? storageLocationDb.LagerortId : -1;
			StorageLocationName = storageLocationDb?.Lagerort;
			FixedUnitPrice = orderItemDb.VKFestpreis ?? false; // <<<<<< !
			UnitPrice = Convert.ToDecimal(orderItemDb.VKEinzelpreis ?? 0);
			FixedTotalPrice = orderItemDb.VKFestpreis ?? false;
			TotalPrice = Convert.ToDecimal(orderItemDb.VKGesamtpreis ?? 0);
			UnitPriceBasis = Convert.ToDecimal(orderItemDb.Preiseinheit ?? 0);
			Discount = Convert.ToDecimal(orderItemDb.Rabatt ?? 0);
			VAT = Convert.ToDecimal(orderItemDb.USt ?? 0);
			// >>>>>>>>>>>>>>
			Designation1 = itemDb?.Bezeichnung1;
			Designation2 = itemDb?.Bezeichnung2;
			Designation3 = itemDb?.Bezeichnung3;
			MeasureUnitQualifier = itemDb.Einheit;
			DrawingIndex = itemDb.Zeichnungsnummer;
			CopperBase = orderItemDb.Kupferbasis ?? 0;
			DelFixed = orderItemDb.DELFixiert ?? false;
			DelNote = orderItemDb.DEL ?? 0;
			CopperWeight = Convert.ToDecimal(orderItemDb.EinzelCuGewicht ?? 0);
			CopperSurcharge = Convert.ToDecimal(orderItemDb.Einzelkupferzuschlag ?? 0);
			ProductionNumber = (orderItemDb.Fertigungsnummer ?? 0);
			// >>>>>>>>>>>>>>>>
			UnloadingPoint = orderItemDb.Abladestelle;
			OpenQuantity_UnitPrice = Convert.ToDecimal(orderItemDb.Einzelpreis ?? 0);
			OpenQuantity_TotalPrice = Convert.ToDecimal(orderItemDb.Gesamtpreis ?? 0);
			OpenQuantity_CopperWeight = Convert.ToDecimal(orderItemDb.GesamtCuGewicht ?? 0);
			OpenQuantity_CopperSurcharge = Convert.ToDecimal(orderItemDb.Gesamtkupferzuschlag ?? 0);
			OriginalOrderQuantity = Convert.ToDecimal(orderItemDb.OriginalAnzahl ?? 0);
			OriginalOrderAmount = Convert.ToDecimal(orderItemDb.Gesamtpreis ?? 0);
			DeliveredQuantity = Convert.ToDecimal(orderItemDb.Geliefert ?? 0);
			// >>>>>>>>>>>>>>>>
			FreeText = orderItemDb.Freies_Format_EDI;
			Note1 = orderItemDb.Bemerkungsfeld1;
			Note2 = orderItemDb.Bemerkungsfeld2;

			// > ---EDI---EDI---EDI---EDI---EDI---
			// HasPendingChange = pendingOrderItemsChangesDb.Exists(e => e.Type == itemChangeTypeChanged);
			// HasPendingCancel = pendingOrderItemsChangesDb.Exists(e => e.Type == itemChangeTypeCanceled);
			// originalPosition = elementDb.PositionZUEDI;

			Version = (orderItemsExtensionDb?.Version ?? 0);
			Index_Kunde = orderItemDb.Index_Kunde;
			Index_Kunde_Datum = orderItemDb.Index_Kunde_Datum;
			CSInterneBemerkung = orderItemDb.CSInterneBemerkung;
			Postext = orderItemDb.POSTEXT;

			ProductionId = fertigungEntity?.ID ?? 0;
			OrderType = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderItemDb.AngebotNr ?? -1)?.Typ;
		}
	}
}
