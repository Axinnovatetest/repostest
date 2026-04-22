using System;

namespace Psz.Core.CustomerService.Models.DeliveryNote
{
	public class OrderElementModel
	{
		public int? ItemTypeId { get; set; } // Typ
		public int ChangeType { get; set; }
		public string ItemCustomerDescription { get; set; }
		public decimal CurrentItemPriceCalculationNet { get; set; }
		public string CustomerItemNumber { get; set; }
		// >>>>>>>>>>>>>>
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
		public decimal? UnitPriceBasis { get; set; } // Preiseinheit // Preiseinheit
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

		// >>>>>> CHANGES >>>>>>>>
		public bool HasPendingChange { get; set; }
		public bool HasPendingCancel { get; set; }
		public int? originalPosition { get; set; }

		// >>>>>> History
		public DateTime? CreateDate { get; set; }
		public Decimal? CalculatedValue { get; set; }

		// > Consignee
		//public ConsigneeModel Consignee { get; set; } = new ConsigneeModel();

		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }

		public OrderElementModel()
		{

		}

		public OrderElementModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity messageEntity,
			Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity ordersLineItemEntity,
			Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity ordersScheduleLineEntity,
			Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity deliveryLineItemEntity)
		{
			ItemTypeId = null;
			Id = artikelEntity.Nr;
			OrderNumber = artikelEntity.AngebotNr?.ToString();
			OrderId = artikelEntity.AngebotNr ?? -1;
			Done = (artikelEntity.erledigt_pos ?? false);
			// >>>>>>>>>>>>>>>>
			ItemId = artikelEntity.ArtikelNr ?? -1;
			ItemNumber = ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber;
			RP = false; // artikelEntity.RP ?? false;
			Position = Convert.ToInt32(ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber);
			OpenQuantity_Quantity = decimal.TryParse(ordersScheduleLineEntity?.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity, out var v) ? v : 0;
			DesiredDate = Convert.ToDateTime(ordersScheduleLineEntity?.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime);
			DeliveryDate = null; // artikelEntity.Liefertermin;
			StorageLocationId = -1; // artikelEntity.Lagerort_id.HasValue ? artikelEntity.Lagerort_id.Value : -1;
			StorageLocationName = ""; // >>>>>>>
			FixedUnitPrice = artikelEntity.EKPreise_Fix ?? false; // <<<<<< !
			UnitPrice = decimal.TryParse(ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet, out var v1) ? v1 : 0;
			FixedTotalPrice = artikelEntity.VKFestpreis ?? false;
			TotalPrice = decimal.TryParse(ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount, out var v2) ? v2 : 0;
			UnitPriceBasis = decimal.TryParse(ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis, out var v3) ? v3 : 0;
			Discount = Convert.ToDecimal(artikelEntity.Rabatt ?? 0);
			VAT = Convert.ToDecimal(artikelEntity.USt ?? 0);
			// >>>>>>>>>>>>>>
			Designation1 = ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber;
			Designation2 = ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription;
			Designation3 = "";
			MeasureUnitQualifier = ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier;
			DrawingIndex = ""; //artikelEntity?.Zeichnungsnummer;
			CopperBase = 0; //artikelEntity.Kupferbasis ?? 0;
			DelFixed = false; //artikelEntity.DELFixiert ?? false;
			DelNote = 0; //artikelEntity.DEL ?? 0;
			CopperWeight = 0; //decimal.TryParse(artikelEntity.EinzelCuGewicht, out var v) ? v : 0;
			CopperSurcharge = 0; //decimal.TryParse(artikelEntity.Einzelkupferzuschlag, out var v) ? v : 0;
			ProductionNumber = 0; //(artikelEntity.Fertigungsnummer ?? 0);
								  // >>>>>>>>  >>>>>
			UnloadingPoint = artikelEntity.Abladestelle;
			OpenQuantity_UnitPrice = 0; //decimal.TryParse(artikelEntity.Einzelpreis, out var v) ? v : 0;
			OpenQuantity_TotalPrice = 0; //decimal.TryParse(artikelEntity.Gesamtpreis, out var v) ? v : 0;
			OpenQuantity_CopperWeight = 0; //decimal.TryParse(artikelEntity.GesamtCuGewicht , out var v) ? v : 0;
			OpenQuantity_CopperSurcharge = 0; //decimal.TryParse(artikelEntity.Gesamtkupferzuschlag , out var v) ? v : 0;
			OriginalOrderQuantity = decimal.TryParse(ordersScheduleLineEntity?.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity, out var v4) ? v4 : 0;
			OriginalOrderAmount = decimal.TryParse(ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount, out var v5) ? v5 : 0; // >>>>>>> all schedule items of position
			DeliveredQuantity = 0; // decimal.TryParse(artikelEntity.Geliefert , out var v) ? v : 0;
								   // >>>>>>>>>>>>>>>>
			FreeText = ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text;
			Note1 = ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text;
			Note2 = ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text;
			//  >>> CHANGES >>>>
			HasPendingChange = false;
			HasPendingCancel = false;
			originalPosition = int.TryParse(ordersLineItemEntity?.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber, out var p) ? p : (int?)null;
			CreateDate = messageEntity?.EditTime ?? ordersLineItemEntity?.EditTime ?? ordersScheduleLineEntity?.CreateTime;
			Index_Kunde = artikelEntity?.Index_Kunde;
			Index_Kunde_Datum = artikelEntity?.Index_Kunde_Datum;
		}
	}
}
