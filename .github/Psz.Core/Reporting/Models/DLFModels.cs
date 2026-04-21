using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Reporting.Models
{
	public class DLFModels
	{
		#region >>> Header
		public class HeaderModel
		{
			public string BuyerContactName { get; set; }
			public string BuyerContactTelephone { get; set; }
			public string BuyerDUNS { get; set; }
			public string BuyerPartyName { get; set; }
			public string BuyerPurchasingDepartment { get; set; }
			public string ConsigneeCity { get; set; }
			public string ConsigneeContactFax { get; set; }
			public string ConsigneeContactTelephone { get; set; }
			public string ConsigneeCountryName { get; set; }
			public string ConsigneeDUNS { get; set; }
			public string ConsigneePartyIdentification { get; set; }
			public string ConsigneePartyName { get; set; }
			public string ConsigneePostCode { get; set; }
			public string ConsigneeStreet { get; set; }
			public DateTime CreationTime { get; set; }
			public string DocumentNumber { get; set; }
			public long Id { get; set; }
			public string MessageReferenceNumber { get; set; }
			public string MessageType { get; set; }
			public int PreviousReferenceVersionNumber { get; set; }
			public string ReceivingDate { get; set; }
			public string RecipientId { get; set; }
			public string ReferenceNumber { get; set; }
			public int ReferenceVersionNumber { get; set; }
			public string SenderId { get; set; }
			public string SupplierCity { get; set; }
			public string SupplierContactFax { get; set; }
			public string SupplierContactTelephone { get; set; }
			public string SupplierCountryName { get; set; }
			public string SupplierDUNS { get; set; }
			public string SupplierPartyIdentification { get; set; }
			public string SupplierPartyName { get; set; }
			public string SupplierPostCode { get; set; }
			public string SupplierStreet { get; set; }
			public string HorizonStartDate { get; set; }
			public string HorizonEndDate { get; set; }
			public string ConsigneeStorageLocation { get; set; }
			public string ConsigneeUnloadingPoint { get; set; }
			// - Duplicated data
			public string ReceivingDateHeader { get; set; }
			public string DocumentNumberHeader { get; set; }
			// - 
			public string ConsigneeAddress { get; set; }
			public string FooterLine1 { get; set; }
			public string FooterLine2 { get; set; }
			public string FooterLine3 { get; set; }

			public HeaderModel()
			{

			}
			public HeaderModel(Infrastructure.Data.Entities.Tables.CTS.HeaderEntity headerEntity, Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adress, string unloidingPoint)
			{
				if(headerEntity == null)
					return;
				var kunden = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adress?.Nr ?? -1);
				BuyerContactName = headerEntity.BuyerContactName?.Trim();
				BuyerContactTelephone = headerEntity.BuyerContactTelephone?.Trim();
				BuyerDUNS = headerEntity.BuyerDUNS?.Trim();
				BuyerPartyName = headerEntity.BuyerPartyName?.Trim();
				BuyerPurchasingDepartment = headerEntity.BuyerPurchasingDepartment?.Trim();
				ConsigneeCity = headerEntity.ConsigneeCity?.Trim();
				ConsigneeContactFax = headerEntity.ConsigneeContactFax?.Trim();
				ConsigneeContactTelephone = headerEntity.ConsigneeContactTelephone?.Trim();
				ConsigneeCountryName = headerEntity.ConsigneeCountryName?.Trim();
				ConsigneeDUNS = headerEntity.ConsigneeDUNS?.Trim();
				ConsigneePartyIdentification = headerEntity.ManualCreation.HasValue && headerEntity.ManualCreation.Value ? "" : headerEntity.ConsigneePartyIdentification?.Trim();
				ConsigneePartyName = headerEntity.ConsigneePartyName?.Trim();
				ConsigneePostCode = headerEntity.ConsigneePostCode?.Trim();
				ConsigneeStreet = headerEntity.ConsigneeStreet?.Trim();
				CreationTime = headerEntity.CreationTime ?? DateTime.MinValue;
				DocumentNumber = headerEntity.DocumentNumber?.Trim();
				Id = headerEntity.Id;
				MessageReferenceNumber = headerEntity.MessageReferenceNumber?.Trim();
				MessageType = headerEntity.MessageType?.Trim();
				PreviousReferenceVersionNumber = headerEntity.PreviousReferenceVersionNumber ?? 0;
				ReceivingDate = (headerEntity.ReceivingDate.HasValue) ? headerEntity.ReceivingDate.Value.ToString("dd.MM.yyyy") : "";
				RecipientId = headerEntity.RecipientId?.Trim();
				ReferenceNumber = headerEntity.ReferenceNumber?.Trim();
				ReferenceVersionNumber = headerEntity.ReferenceVersionNumber ?? -1;
				SenderId = headerEntity.SenderId?.Trim();
				SupplierCity = headerEntity.SupplierCity?.Trim();
				SupplierContactFax = headerEntity.SupplierContactFax?.Trim();
				SupplierContactTelephone = headerEntity.SupplierContactTelephone?.Trim();
				SupplierCountryName = headerEntity.SupplierCountryName?.Trim();
				SupplierDUNS = headerEntity.SupplierDUNS?.Trim();
				SupplierPartyIdentification = headerEntity.SupplierPartyIdentification?.Trim();
				SupplierPartyName = headerEntity.SupplierPartyName?.Trim();
				SupplierPostCode = $"{headerEntity.SupplierPostCode?.Trim()} {headerEntity.SupplierCity?.Trim()}".Trim();
				SupplierStreet = headerEntity.SupplierStreet?.Trim();
				HorizonStartDate = (headerEntity.ValidFrom.HasValue) ? headerEntity.ValidFrom.Value.ToString("dd.MM.yyyy") : "";
				HorizonEndDate = (headerEntity.ValidTill.HasValue ? headerEntity.ValidTill.Value.ToString("dd.MM.yyyy") : "");
				ConsigneeUnloadingPoint = unloidingPoint;

				// - 
				ReceivingDateHeader = (headerEntity.ReceivingDate ?? DateTime.MinValue).ToString("dd.MM.yyyy");
				DocumentNumberHeader = headerEntity.DocumentNumber?.Trim();
				//-
				ConsigneeAddress = $"{headerEntity.ConsigneePostCode?.Trim()} {headerEntity.ConsigneeCity?.Trim()}".Trim();
				//-//-
				FooterLine1 = $"{BuyerPartyName} | {ConsigneeStreet} | {ConsigneePostCode} {ConsigneeCity} | {ConsigneeCountryName}";
				FooterLine2 = $"E-mail:{adress.EMail} | Web: {adress.WWW}";
				FooterLine3 = $"UID:{kunden?.EG___Identifikationsnummer}";
			}
		}
		#endregion Header <<<<

		#region >>> LineItem
		public class LineItemModel
		{
			public string CallOffDateTime { get; set; }
			public string CallOffNumber { get; set; }
			public string CumulativeReceivedQuantity { get; set; }
			public string CumulativeScheduledQuantity { get; set; }
			public string CustomersItemMaterialNumber { get; set; }
			public string DrawingRevisionNumber { get; set; }
			public string DocumentNumber { get; set; }
			public long HeaderId { get; set; }
			public long Id { get; set; }
			public string LastReceivedQuantity { get; set; }
			public string MaterialAuthorizationDate { get; set; }
			public string MaterialAuthorizationQuantity { get; set; }
			public string PlanningHorizionEnd { get; set; }
			public string PlanningHorizionStart { get; set; }
			public int PositionNumber { get; set; }
			public string PreviousCallOffDate { get; set; }
			public int PreviousCallOffNumber { get; set; }
			public string ProductionAuthorizationDateTime { get; set; }
			public string ProductionAuthorizationQuantity { get; set; }
			public string SuppliersItemMaterialNumber { get; set; }
			public string BuyersInternalProductGroupCode { get; set; }
			public string LastASNDate { get; set; }
			public string LastASNDeliveryDate { get; set; }
			public string LastASNNumber { get; set; }
			// -
			public string Description { get; set; }
			public string ModelYear { get; set; }
			public LineItemModel(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity lineItemEntity)
			{
				if(lineItemEntity == null)
					return;

				CallOffDateTime = lineItemEntity.CallOffDateTime.HasValue ? lineItemEntity.CallOffDateTime.Value.ToString("dd.MM.yyyy") : "";
				CallOffNumber = (lineItemEntity.CallOffNumber ?? -1).ToString("0.##");
				CumulativeReceivedQuantity = (lineItemEntity.CumulativeReceivedQuantity ?? -1).ToString("0.##");
				CumulativeScheduledQuantity = (lineItemEntity.CumulativeScheduledQuantity ?? -1).ToString("0.##");
				CustomersItemMaterialNumber = lineItemEntity.CustomersItemMaterialNumber;
				HeaderId = lineItemEntity.HeaderId;
				Id = lineItemEntity.Id;
				LastReceivedQuantity = (lineItemEntity.LastReceivedQuantity ?? -1).ToString("0.##");
				MaterialAuthorizationDate = lineItemEntity.MaterialAuthorizationDate.HasValue ? lineItemEntity.MaterialAuthorizationDate.Value.ToString("dd.MM.yyyy") : "";
				MaterialAuthorizationQuantity = (lineItemEntity.MaterialAuthorizationQuantity ?? 0).ToString("0.##");
				PlanningHorizionEnd = lineItemEntity.PlanningHorizionEnd.HasValue ? lineItemEntity.PlanningHorizionEnd.Value.ToString("dd.MM.yyyy") : "";
				PlanningHorizionStart = lineItemEntity.PlanningHorizionStart.HasValue ? lineItemEntity.PlanningHorizionStart.Value.ToString("dd.MM.yyyy") : "";
				PositionNumber = lineItemEntity.PositionNumber;
				PreviousCallOffDate = lineItemEntity.PreviousCallOffDate.HasValue ? lineItemEntity.PreviousCallOffDate.Value.ToString("dd.MM.yyyy") : "";
				PreviousCallOffNumber = lineItemEntity.PreviousCallOffNumber ?? -1;
				ProductionAuthorizationDateTime = lineItemEntity.ProductionAuthorizationDateTime.HasValue ? lineItemEntity.ProductionAuthorizationDateTime.Value.ToString("dd.MM.yyyy") : "";
				ProductionAuthorizationQuantity = (lineItemEntity.ProductionAuthorizationQuantity ?? 0).ToString("0.##");
				SuppliersItemMaterialNumber = lineItemEntity.SuppliersItemMaterialNumber;
				DrawingRevisionNumber = lineItemEntity.DrawingRevisionNumber;
				DocumentNumber = lineItemEntity.DocumentNumber;
				BuyersInternalProductGroupCode = lineItemEntity.BuyersInternalProductGroupCode;
				LastASNDate = lineItemEntity.LastASNDate.HasValue ? lineItemEntity.LastASNDate.Value.ToString("dd.MM.yyyy") : "";
				LastASNDeliveryDate = lineItemEntity.LastASNDeliveryDate.HasValue ? lineItemEntity.LastASNDeliveryDate.Value.ToString("dd.MM.yyyy") : "";
				LastASNNumber = lineItemEntity.LastASNNumber;
				// -
				Description = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(SuppliersItemMaterialNumber)?.Bezeichnung2;
				ModelYear = "";
			}
		}
		#endregion LineItem <<<

		#region >>> LineItemPlan

		public class LineItemPlanModel
		{
			public long Id { get; set; }
			public long LineItemId { get; set; }
			public string PlanningQuantityCumulativeQuantity { get; set; }
			public string PlanningQuantityFrequencyIdentifier { get; set; }
			public string PlanningQuantityQuantity { get; set; }
			public string PlanningQuantityRequestedShipmentDate { get; set; }
			public string PlanningQuantityUnitOfMeasure { get; set; }
			public string PlanningQuantityWeeklyPeriodEndDate { get; set; }
			public int PositionNumber { get; set; }
			public string PlanningQuantityDeliveryPlanStatusIdentifier { get; set; }

			public LineItemPlanModel(Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity lineItemPlanEntity)
			{
				if(lineItemPlanEntity == null)
					return;

				Id = lineItemPlanEntity.Id;
				LineItemId = lineItemPlanEntity.LineItemId;
				PlanningQuantityCumulativeQuantity = (lineItemPlanEntity.PlanningQuantityCumulativeQuantity ?? -1).ToString("0.##");
				PlanningQuantityFrequencyIdentifier = lineItemPlanEntity.PlanningQuantityFrequencyIdentifier;
				PlanningQuantityQuantity = (lineItemPlanEntity.PlanningQuantityQuantity ?? -1).ToString("0.##");
				PlanningQuantityRequestedShipmentDate = lineItemPlanEntity.PlanningQuantityRequestedShipmentDate.HasValue ? lineItemPlanEntity.PlanningQuantityRequestedShipmentDate.Value.ToString("dd.MM.yyyy") : "";
				PlanningQuantityUnitOfMeasure = lineItemPlanEntity.PlanningQuantityUnitOfMeasure;
				PlanningQuantityWeeklyPeriodEndDate = lineItemPlanEntity.PlanningQuantityWeeklyPeriodEndDate.HasValue ? lineItemPlanEntity.PlanningQuantityWeeklyPeriodEndDate.Value.ToString("dd.MM.yyyy") : "";
				PositionNumber = lineItemPlanEntity.PositionNumber;
				PlanningQuantityDeliveryPlanStatusIdentifier = getStatus((PlanStatus)(lineItemPlanEntity.PlanningQuantityDeliveryPlanStatusIdentifier ?? -1));
			}

			enum PlanStatus
			{
				FIRM = 1,
				PROD = 2,
				MAT = 3,
				PLAN = 4,
				SBED = 10
			}
			string getStatus(PlanStatus planStatus)
			{
				switch(planStatus)
				{
					case PlanStatus.FIRM:
						return "FIRM";
					case PlanStatus.PROD:
						return "PROD";
					case PlanStatus.MAT:
						return "MAT";
					case PlanStatus.PLAN:
						return "PLAN";
					case PlanStatus.SBED:
						return "SBED";
					default:
						return "";
				}
			}
		}
		#endregion LineItemPlan <<<

		public class I18N
		{
			public enum Language
			{
				DE,
				EN,
				FR,
				SQ,
				CS
			}
			#region >>> Header
			public class HeaderModel
			{
				public string BuyerContactName { get; set; }
				public string BuyerContactTelephone { get; set; }
				public string BuyerDUNS { get; set; }
				public string BuyerPartyName { get; set; }
				public string BuyerPurchasingDepartment { get; set; }
				public string ConsigneeCity { get; set; }
				public string ConsigneeContactFax { get; set; }
				public string ConsigneeContactTelephone { get; set; }
				public string ConsigneeCountryName { get; set; }
				public string ConsigneeDUNS { get; set; }
				public string ConsigneePartyIdentification { get; set; }
				public string ConsigneePartyName { get; set; }
				public string ConsigneePostCode { get; set; }
				public string ConsigneeStreet { get; set; }
				public string CreationTime { get; set; }
				public string DocumentNumber { get; set; }
				public string Id { get; set; }
				public string MessageReferenceNumber { get; set; }
				public string MessageType { get; set; }
				public string PreviousReferenceVersionNumber { get; set; }
				public string ReceivingDate { get; set; }
				public string RecipientId { get; set; }
				public string ReferenceNumber { get; set; }
				public string ReferenceVersionNumber { get; set; }
				public string SenderId { get; set; }
				public string SupplierCity { get; set; }
				public string SupplierContactFax { get; set; }
				public string SupplierContactTelephone { get; set; }
				public string SupplierCountryName { get; set; }
				public string SupplierDUNS { get; set; }
				public string SupplierPartyIdentification { get; set; }
				public string SupplierPartyName { get; set; }
				public string SupplierPostCode { get; set; }
				public string SupplierStreet { get; set; }
				public string HorizonStartDate { get; set; }
				public string HorizonEndDate { get; set; }
				public string ConsigneeStorageLocation { get; set; }
				public string ConsigneeUnloadingPoint { get; set; }

				// - duplicated data
				public string ReceivingDateHeader { get; set; }
				public string DocumentNumberHeader { get; set; }

				// - bullk data
				public string LegendFrequency { get; set; }
				public string LegendStatus { get; set; }

				public HeaderModel()
				{

				}
				public HeaderModel(Language language)
				{
					switch(language)
					{
						case Language.DE:
							{
								BuyerContactName = "";
								BuyerContactTelephone = "";
								BuyerDUNS = "";
								BuyerPartyName = "Besteller";
								BuyerPurchasingDepartment = "";
								ConsigneeCity = "";
								ConsigneeContactFax = "";
								ConsigneeContactTelephone = "";
								ConsigneeCountryName = "";
								ConsigneeDUNS = "";
								ConsigneePartyIdentification = "Warenempfänger";
								ConsigneePartyName = "";
								ConsigneePostCode = "";
								ConsigneeStreet = "";
								ConsigneeStorageLocation = "Lagerort";
								ConsigneeUnloadingPoint = "Abladestelle";
								CreationTime = "";
								DocumentNumber = "DELFOR-Nummer";
								Id = "";
								MessageReferenceNumber = "";
								MessageType = "";
								PreviousReferenceVersionNumber = "";
								ReceivingDate = "Vom";
								RecipientId = "";
								ReferenceNumber = "";
								ReferenceVersionNumber = "Abrufnummer";
								SenderId = "";
								SupplierCity = "";
								SupplierContactFax = "";
								SupplierContactTelephone = "";
								SupplierCountryName = "";
								SupplierDUNS = "";
								SupplierPartyIdentification = "";
								SupplierPartyName = "Lieferant";
								SupplierPostCode = "";
								SupplierStreet = "";
								HorizonStartDate = "Abrufstartdatum";
								HorizonEndDate = "Abrufenddatum";
								//- 
								ReceivingDateHeader = "Abrufdatum";
								DocumentNumberHeader = "Bestellnummer";
								//-
								LegendFrequency = "*Zeitspanne: T: Tageslieferung, W: Wochenlieferung, M: Monatslieferung";
								LegendStatus = "*Freigabestatus: FIRM: Fixes lieferdatum, PROD: Freigabe zu Production, MAT: Freigabe zu Materialbeschaffung, PLAN: Unverbindliche planungsvorschau, RÜCK: Rückstand, SBED: Sofortbedarf";
							}
							break;
						case Language.EN:
							{
								BuyerContactName = "";
								BuyerContactTelephone = "";
								BuyerDUNS = "";
								BuyerPartyName = "Buyer";
								BuyerPurchasingDepartment = "";
								ConsigneeCity = "";
								ConsigneeContactFax = "";
								ConsigneeContactTelephone = "";
								ConsigneeCountryName = "";
								ConsigneeDUNS = "";
								ConsigneePartyIdentification = "";
								ConsigneePartyName = "";
								ConsigneePostCode = "";
								ConsigneeStreet = "";
								ConsigneeStorageLocation = "";
								ConsigneeUnloadingPoint = "";
								CreationTime = "";
								DocumentNumber = "DELFOR-Number";
								Id = "";
								MessageReferenceNumber = "";
								MessageType = "";
								PreviousReferenceVersionNumber = "";
								ReceivingDate = "From";
								RecipientId = "";
								ReferenceNumber = "";
								ReferenceVersionNumber = "";
								SenderId = "";
								SupplierCity = "";
								SupplierContactFax = "";
								SupplierContactTelephone = "";
								SupplierCountryName = "";
								SupplierDUNS = "";
								SupplierPartyIdentification = "";
								SupplierPartyName = "Supplier";
								SupplierPostCode = "";
								SupplierStreet = "";
								HorizonStartDate = "Valid from";
								HorizonEndDate = "Valid till";
								//- 
								ReceivingDateHeader = "Abrufdatum";
								DocumentNumberHeader = "Bestellnummer";
							}
							break;
						case Language.FR:
							{
								BuyerContactName = "";
								BuyerContactTelephone = "";
								BuyerDUNS = "";
								BuyerPartyName = "";
								BuyerPurchasingDepartment = "";
								ConsigneeCity = "";
								ConsigneeContactFax = "";
								ConsigneeContactTelephone = "";
								ConsigneeCountryName = "";
								ConsigneeDUNS = "";
								ConsigneePartyIdentification = "";
								ConsigneePartyName = "";
								ConsigneePostCode = "";
								ConsigneeStreet = "";
								ConsigneeStorageLocation = "";
								ConsigneeUnloadingPoint = "";
								CreationTime = "";
								DocumentNumber = "";
								Id = "";
								MessageReferenceNumber = "";
								MessageType = "";
								PreviousReferenceVersionNumber = "";
								ReceivingDate = "";
								RecipientId = "";
								ReferenceNumber = "";
								ReferenceVersionNumber = "";
								SenderId = "";
								SupplierCity = "";
								SupplierContactFax = "";
								SupplierContactTelephone = "";
								SupplierCountryName = "";
								SupplierDUNS = "";
								SupplierPartyIdentification = "";
								SupplierPartyName = "";
								SupplierPostCode = "";
								SupplierStreet = "";
								HorizonStartDate = "";
								HorizonEndDate = "";
								//- 
								ReceivingDateHeader = "Abrufdatum";
								DocumentNumberHeader = "Bestellnummer";
							}
							break;
						case Language.SQ:
							{
								BuyerContactName = "";
								BuyerContactTelephone = "";
								BuyerDUNS = "";
								BuyerPartyName = "";
								BuyerPurchasingDepartment = "";
								ConsigneeCity = "";
								ConsigneeContactFax = "";
								ConsigneeContactTelephone = "";
								ConsigneeCountryName = "";
								ConsigneeDUNS = "";
								ConsigneePartyIdentification = "";
								ConsigneePartyName = "";
								ConsigneePostCode = "";
								ConsigneeStreet = "";
								ConsigneeStorageLocation = "";
								ConsigneeUnloadingPoint = "";
								CreationTime = "";
								DocumentNumber = "";
								Id = "";
								MessageReferenceNumber = "";
								MessageType = "";
								PreviousReferenceVersionNumber = "";
								ReceivingDate = "";
								RecipientId = "";
								ReferenceNumber = "";
								ReferenceVersionNumber = "";
								SenderId = "";
								SupplierCity = "";
								SupplierContactFax = "";
								SupplierContactTelephone = "";
								SupplierCountryName = "";
								SupplierDUNS = "";
								SupplierPartyIdentification = "";
								SupplierPartyName = "";
								SupplierPostCode = "";
								SupplierStreet = "";
								HorizonStartDate = "";
								HorizonEndDate = "";
								//- 
								ReceivingDateHeader = "Abrufdatum";
								DocumentNumberHeader = "Bestellnummer";
							}
							break;
						case Language.CS:
							{
								BuyerContactName = "";
								BuyerContactTelephone = "";
								BuyerDUNS = "";
								BuyerPartyName = "";
								BuyerPurchasingDepartment = "";
								ConsigneeCity = "";
								ConsigneeContactFax = "";
								ConsigneeContactTelephone = "";
								ConsigneeCountryName = "";
								ConsigneeDUNS = "";
								ConsigneePartyIdentification = "";
								ConsigneePartyName = "";
								ConsigneePostCode = "";
								ConsigneeStreet = "";
								ConsigneeStorageLocation = "";
								ConsigneeUnloadingPoint = "";
								CreationTime = "";
								DocumentNumber = "";
								Id = "";
								MessageReferenceNumber = "";
								MessageType = "";
								PreviousReferenceVersionNumber = "";
								ReceivingDate = "";
								RecipientId = "";
								ReferenceNumber = "";
								ReferenceVersionNumber = "";
								SenderId = "";
								SupplierCity = "";
								SupplierContactFax = "";
								SupplierContactTelephone = "";
								SupplierCountryName = "";
								SupplierDUNS = "";
								SupplierPartyIdentification = "";
								SupplierPartyName = "";
								SupplierPostCode = "";
								SupplierStreet = "";
								HorizonStartDate = "";
								HorizonEndDate = "";
								//- 
								ReceivingDateHeader = "Abrufdatum";
								DocumentNumberHeader = "Bestellnummer";
							}
							break;
						default:
							{
								BuyerContactName = "";
								BuyerContactTelephone = "";
								BuyerDUNS = "";
								BuyerPartyName = "";
								BuyerPurchasingDepartment = "";
								ConsigneeCity = "";
								ConsigneeContactFax = "";
								ConsigneeContactTelephone = "";
								ConsigneeCountryName = "";
								ConsigneeDUNS = "";
								ConsigneePartyIdentification = "";
								ConsigneePartyName = "";
								ConsigneePostCode = "";
								ConsigneeStreet = "";
								ConsigneeStorageLocation = "";
								ConsigneeUnloadingPoint = "";
								CreationTime = "";
								DocumentNumber = "";
								Id = "";
								MessageReferenceNumber = "";
								MessageType = "";
								PreviousReferenceVersionNumber = "";
								ReceivingDate = "";
								RecipientId = "";
								ReferenceNumber = "";
								ReferenceVersionNumber = "";
								SenderId = "";
								SupplierCity = "";
								SupplierContactFax = "";
								SupplierContactTelephone = "";
								SupplierCountryName = "";
								SupplierDUNS = "";
								SupplierPartyIdentification = "";
								SupplierPartyName = "";
								SupplierPostCode = "";
								SupplierStreet = "";
								HorizonStartDate = "";
								HorizonEndDate = "";
								//- 
								ReceivingDateHeader = "";
								DocumentNumberHeader = "";
							}
							break;
					}

				}
			}
			#endregion Header <<<<

			#region >>> LineItem
			public class LineItemModel
			{
				public string CallOffDateTime { get; set; }
				public string CallOffNumber { get; set; }
				public string CumulativeReceivedQuantity { get; set; }
				public string CumulativeScheduledQuantity { get; set; }
				public string CustomersItemMaterialNumber { get; set; }
				public string DrawingRevisionNumber { get; set; }
				public string DocumentNumber { get; set; }
				public string HeaderId { get; set; }
				public string Id { get; set; }
				public string LastReceivedQuantity { get; set; }
				public string MaterialAuthorizationDate { get; set; }
				public string MaterialAuthorizationQuantity { get; set; }
				public string PlanningHorizionEnd { get; set; }
				public string PlanningHorizionStart { get; set; }
				public string PositionNumber { get; set; }
				public string PreviousCallOffDate { get; set; }
				public string PreviousCallOffNumber { get; set; }
				public string ProductionAuthorizationDateTime { get; set; }
				public string ProductionAuthorizationQuantity { get; set; }
				public string SuppliersItemMaterialNumber { get; set; }
				public string BuyersInternalProductGroupCode { get; set; }
				public string LastASNDate { get; set; }
				public string LastASNDeliveryDate { get; set; }
				public string LastASNNumber { get; set; }
				// -
				public string Description { get; set; }
				public string ModelYear { get; set; }
				public LineItemModel(Language language)
				{
					switch(language)
					{
						case Language.DE:
							{
								CallOffDateTime = "";
								CallOffNumber = "";
								CumulativeReceivedQuantity = "Kum. empfangene Menge";
								CumulativeScheduledQuantity = "Kum. versandte Menge";
								CustomersItemMaterialNumber = "Kundenmaterialnummer";
								DrawingRevisionNumber = "";
								DocumentNumber = "";
								HeaderId = "";
								Id = "";
								LastReceivedQuantity = "Letzte gelieferte Menge";
								MaterialAuthorizationDate = "Materialfreigabe/Enddatum";
								MaterialAuthorizationQuantity = "Materialfreigabe/Menge";
								PlanningHorizionEnd = "";
								PlanningHorizionStart = "";
								PositionNumber = "Bestellposition";
								PreviousCallOffDate = "";
								PreviousCallOffNumber = "";
								ProductionAuthorizationDateTime = "Produktfreigabe/Enddatum";
								ProductionAuthorizationQuantity = "Produktfreigabe/Menge";
								SuppliersItemMaterialNumber = "Lieferantenmaterialnummer";
								BuyersInternalProductGroupCode = "";
								LastASNDate = "Letztes ASN-Datum";
								LastASNDeliveryDate = "";
								LastASNNumber = "Letzte ASN-Nummer";
								// - 
								Description = "Beschreibung";
								ModelYear = "Modelljahr";
							}
							break;
						case Language.EN:
							{
								CallOffDateTime = "";
								CallOffNumber = "";
								CumulativeReceivedQuantity = "";
								CumulativeScheduledQuantity = "";
								CustomersItemMaterialNumber = "";
								DrawingRevisionNumber = "";
								DocumentNumber = "";
								HeaderId = "";
								Id = "";
								LastReceivedQuantity = "";
								MaterialAuthorizationDate = "";
								MaterialAuthorizationQuantity = "";
								PlanningHorizionEnd = "";
								PlanningHorizionStart = "";
								PositionNumber = "";
								PreviousCallOffDate = "";
								PreviousCallOffNumber = "";
								ProductionAuthorizationDateTime = "";
								ProductionAuthorizationQuantity = "";
								SuppliersItemMaterialNumber = "";
								BuyersInternalProductGroupCode = "";
								LastASNDate = "";
								LastASNDeliveryDate = "";
								LastASNNumber = "";
							}
							break;
						case Language.FR:
							{
								CallOffDateTime = "";
								CallOffNumber = "";
								CumulativeReceivedQuantity = "";
								CumulativeScheduledQuantity = "";
								CustomersItemMaterialNumber = "";
								DrawingRevisionNumber = "";
								DocumentNumber = "";
								HeaderId = "";
								Id = "";
								LastReceivedQuantity = "";
								MaterialAuthorizationDate = "";
								MaterialAuthorizationQuantity = "";
								PlanningHorizionEnd = "";
								PlanningHorizionStart = "";
								PositionNumber = "";
								PreviousCallOffDate = "";
								PreviousCallOffNumber = "";
								ProductionAuthorizationDateTime = "";
								ProductionAuthorizationQuantity = "";
								SuppliersItemMaterialNumber = "";
								BuyersInternalProductGroupCode = "";
								LastASNDate = "";
								LastASNDeliveryDate = "";
								LastASNNumber = "";
							}
							break;
						case Language.SQ:
							{
								CallOffDateTime = "";
								CallOffNumber = "";
								CumulativeReceivedQuantity = "";
								CumulativeScheduledQuantity = "";
								CustomersItemMaterialNumber = "";
								DrawingRevisionNumber = "";
								DocumentNumber = "";
								HeaderId = "";
								Id = "";
								LastReceivedQuantity = "";
								MaterialAuthorizationDate = "";
								MaterialAuthorizationQuantity = "";
								PlanningHorizionEnd = "";
								PlanningHorizionStart = "";
								PositionNumber = "";
								PreviousCallOffDate = "";
								PreviousCallOffNumber = "";
								ProductionAuthorizationDateTime = "";
								ProductionAuthorizationQuantity = "";
								SuppliersItemMaterialNumber = "";
								BuyersInternalProductGroupCode = "";
								LastASNDate = "";
								LastASNDeliveryDate = "";
								LastASNNumber = "";
							}
							break;
						case Language.CS:
							{
								CallOffDateTime = "";
								CallOffNumber = "";
								CumulativeReceivedQuantity = "";
								CumulativeScheduledQuantity = "";
								CustomersItemMaterialNumber = "";
								DrawingRevisionNumber = "";
								DocumentNumber = "";
								HeaderId = "";
								Id = "";
								LastReceivedQuantity = "";
								MaterialAuthorizationDate = "";
								MaterialAuthorizationQuantity = "";
								PlanningHorizionEnd = "";
								PlanningHorizionStart = "";
								PositionNumber = "";
								PreviousCallOffDate = "";
								PreviousCallOffNumber = "";
								ProductionAuthorizationDateTime = "";
								ProductionAuthorizationQuantity = "";
								SuppliersItemMaterialNumber = "";
								BuyersInternalProductGroupCode = "";
								LastASNDate = "";
								LastASNDeliveryDate = "";
								LastASNNumber = "";
							}
							break;
						default:
							{
								CallOffDateTime = "";
								CallOffNumber = "";
								CumulativeReceivedQuantity = "";
								CumulativeScheduledQuantity = "";
								CustomersItemMaterialNumber = "";
								DrawingRevisionNumber = "";
								DocumentNumber = "";
								HeaderId = "";
								Id = "";
								LastReceivedQuantity = "";
								MaterialAuthorizationDate = "";
								MaterialAuthorizationQuantity = "";
								PlanningHorizionEnd = "";
								PlanningHorizionStart = "";
								PositionNumber = "";
								PreviousCallOffDate = "";
								PreviousCallOffNumber = "";
								ProductionAuthorizationDateTime = "";
								ProductionAuthorizationQuantity = "";
								SuppliersItemMaterialNumber = "";
								BuyersInternalProductGroupCode = "";
								LastASNDate = "";
								LastASNDeliveryDate = "";
								LastASNNumber = "";
							}
							break;
					}
				}
			}
			#endregion LineItem <<<

			#region >>> LineItemPlan

			public class LineItemPlanModel
			{
				public string Id { get; set; }
				public string LineItemId { get; set; }
				public string PlanningQuantityCumulativeQuantity { get; set; }
				public string PlanningQuantityFrequencyIdentifier { get; set; }
				public string PlanningQuantityQuantity { get; set; }
				public string PlanningQuantityRequestedShipmentDate { get; set; }
				public string PlanningQuantityUnitOfMeasure { get; set; }
				public string PlanningQuantityWeeklyPeriodEndDate { get; set; }
				public string PositionNumber { get; set; }
				public string PlanningQuantityDeliveryPlanStatusIdentifier { get; set; }

				public LineItemPlanModel(Language language)
				{
					switch(language)
					{
						case Language.DE:
							{
								Id = "";
								LineItemId = "";
								PlanningQuantityCumulativeQuantity = "Kumulierte Menge";
								PlanningQuantityFrequencyIdentifier = "Zeitspanne";
								PlanningQuantityQuantity = "Eingeteilte Menge";
								PlanningQuantityRequestedShipmentDate = "Geplantes Lieferdatum";
								PlanningQuantityUnitOfMeasure = "Einheit";
								PlanningQuantityWeeklyPeriodEndDate = "Wochenenddatum";
								PositionNumber = "Pos";
								PlanningQuantityDeliveryPlanStatusIdentifier = "Freigabestatus";
							}
							break;
						case Language.EN:
							{
								Id = "";
								LineItemId = "";
								PlanningQuantityCumulativeQuantity = "Cum. Quantity";
								PlanningQuantityFrequencyIdentifier = "Frequency";
								PlanningQuantityQuantity = "Quantity";
								PlanningQuantityRequestedShipmentDate = "RSD";
								PlanningQuantityUnitOfMeasure = "UoM";
								PlanningQuantityWeeklyPeriodEndDate = "Weekly End Date";
								PositionNumber = "Pos";
								PlanningQuantityDeliveryPlanStatusIdentifier = "Plan Status";
							}
							break;
						case Language.FR:
							{
								Id = "";
								LineItemId = "";
								PlanningQuantityCumulativeQuantity = "";
								PlanningQuantityFrequencyIdentifier = "";
								PlanningQuantityQuantity = "";
								PlanningQuantityRequestedShipmentDate = "";
								PlanningQuantityUnitOfMeasure = "";
								PlanningQuantityWeeklyPeriodEndDate = "";
								PositionNumber = "";
								PlanningQuantityDeliveryPlanStatusIdentifier = "";
							}
							break;
						case Language.SQ:
							{
								Id = "";
								LineItemId = "";
								PlanningQuantityCumulativeQuantity = "";
								PlanningQuantityFrequencyIdentifier = "";
								PlanningQuantityQuantity = "";
								PlanningQuantityRequestedShipmentDate = "";
								PlanningQuantityUnitOfMeasure = "";
								PlanningQuantityWeeklyPeriodEndDate = "";
								PositionNumber = "";
								PlanningQuantityDeliveryPlanStatusIdentifier = "";
							}
							break;
						case Language.CS:
							{
								Id = "";
								LineItemId = "";
								PlanningQuantityCumulativeQuantity = "";
								PlanningQuantityFrequencyIdentifier = "";
								PlanningQuantityQuantity = "";
								PlanningQuantityRequestedShipmentDate = "";
								PlanningQuantityUnitOfMeasure = "";
								PlanningQuantityWeeklyPeriodEndDate = "";
								PositionNumber = "";
								PlanningQuantityDeliveryPlanStatusIdentifier = "";
							}
							break;
						default:
							{
								Id = "";
								LineItemId = "";
								PlanningQuantityCumulativeQuantity = "";
								PlanningQuantityFrequencyIdentifier = "";
								PlanningQuantityQuantity = "";
								PlanningQuantityRequestedShipmentDate = "";
								PlanningQuantityUnitOfMeasure = "";
								PlanningQuantityWeeklyPeriodEndDate = "";
								PositionNumber = "";
								PlanningQuantityDeliveryPlanStatusIdentifier = "";
							}
							break;
					}
				}
			}
			#endregion LineItemPlan <<<
		}
	}
}
