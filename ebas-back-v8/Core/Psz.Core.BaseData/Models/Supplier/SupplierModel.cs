using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class SupplierModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int AdressId { get; set; } = -1;
		public int ImageId { get; set; }

		// > External Ids 
		public string VenderId { get; set; }

		// > Data
		// .p1
		/**/
		public int? DiscountGroupId { get; set; } // RabbatGruppe
		public string DiscountGroupName { get; set; }
		/**/
		public int? ConditionAssignmentNumber { get; set; } // Konditionszuordnungs_Nr
		public string ConditionAssignmentNumberName { get; set; }
		/**/
		public string Industry { get; set; } // Branch
		/**/
		public string SuppliersGroup { get; set; } // Lieferantengruppe
												   // public string ImdsFirmenID { get; set; }  // Missing: IMDS Firmen-ID
		public bool CalculateSalesTax { get; set; } // Umsatzsteuer_berechnen
		public string EgIdentificationNumber { get; set; } // EG_Identifikationsnummer
		/**/
		public int? LanguageId { get; set; } // Sprache
		public string LanguageName { get; set; }
		/**/
		public int? CurrencyId { get; set; } // Wahrung
		public string CurrencyName { get; set; } // Wahrung
		/**/
		public int? SlipCircleId { get; set; } // Belegkreis
		public string SlipCircleName { get; set; }
		public bool Dunning { get; set; } // Mahnsperre
		public bool BlockedForFurtherOrders { get; set; } // Gesperrt_für_weitere_Bestellungen
		public string ReasonForBlocking { get; set; } // Grund_fur_Sperre
													  // .p2
		public string PaymentMethod { get; set; } // Zahlungsweise
		public decimal? OrderLimit { get; set; } // Bestellimit
		public decimal? TargetImpact { get; set; } // Zielaufschlag
		public decimal? MinimumValue { get; set; } // Mindestbestellwert
		public decimal? SurchargeMinimumOrderValue { get; set; } // Zuschlag_Mindestbestellwert

		// > Payment
		public bool DunningBlockSupplier { get; set; }

		#region > Adress
		public int AddressType { get; set; }
		public string PreName { get; set; }
		// > 
		public bool? AddressEDIActive { get; set; }
		public string AdressDUNS { get; set; }
		public Enums.AddressEnums.Categories AdressCategory { get; set; }
		public int? AdressSupplierNumber { get; set; }
		public int? AdressCustomerNumber { get; set; }
		public int? AdressPersonalNumber { get; set; }
		// >
		public string AdressTitle { get; set; }
		public string AdressName1 { get; set; }
		public string AdressName2 { get; set; }
		public string AdressName3 { get; set; }
		// >
		public string AdressCountry { get; set; }
		public string AdressCity { get; set; }
		public string AdressStreet { get; set; }
		public string AdressStreetZipCode { get; set; }
		public string AdressMailbox { get; set; }
		public string AdressMailboxZipCode { get; set; }
		public bool AdressMailboxIsPreferred { get; set; }
		// >
		public string AdressPhoneNumber { get; set; }
		public string AdressFaxNumber { get; set; }
		public string AdressEmailAdress { get; set; }
		public string AdressWebsite { get; set; }
		// >
		public string AdressNote { get; set; }
		public string AdressNotes { get; set; }
		// >
		public bool AdressSelection { get; set; }
		public string AdressLevel { get; set; }
		public DateTime? AdressRecordTime { get; set; }
		public string AdressFrom { get; set; }
		public string AdressSortTerm { get; set; }
		// >
		public string AdressFirstName { get; set; }
		public string AdressSalutation { get; set; }
		public string AdressDepartment { get; set; }
		public string AdressFunction { get; set; }
		#endregion

		// > Shipping
		public decimal? ShippingCosts { get; set; }
		public bool ShippingMondayIsDeliveryDay { get; set; }
		public bool ShippingTuesdayIsDeliveryDay { get; set; }
		public bool ShippingWednesdayIsDeliveryDay { get; set; }
		public bool ShippingThursdayIsDeliveryDay { get; set; }
		public bool ShippingFridayIsDeliveryDay { get; set; }
		// >
		public string ShippingWeekDay { get; set; } // Wochentag_Anlieferung
		public string ShippingMethod { get; set; } // Versandart
		public decimal? ShippingFreightAllowance { get; set; } // Frachtfreigrenze
		public decimal? ShippingExpressSurcharge { get; set; } // Eilzuschlag

		// > Common
		public string Name { get; set; }
		public string AdressText { get; set; }

		// > Order
		public bool RemindOrderConfirmation { get; set; }
		public string WaitingPeriod { get; set; }

		// > CustomerNumberSupplier
		public string CustomerNumberSupplier { get; set; }
		public string CustomerNumberSupplierSC { get; set; }
		public string CustomerNumberSupplierScCz { get; set; }
		public string CustomerNumberSupplierPszCz { get; set; }
		public string CustomerNumberSupplierPszTn { get; set; }
		public string CustomerNumberSupplierPszAl { get; set; }

		// > ?
		public bool Lh { get; set; }
		public DateTime? LhDate { get; set; }

		public List<ContactPersonModel> ContactPersons { get; set; } = new List<ContactPersonModel>();

		public SupplierModel()
		{ }
		public SupplierModel(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity supplierEntity,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressEntity,
			List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> contactPersonsEntities,
			Infrastructure.Data.Entities.Tables.BSD.RabatthauptGruppenEntity discountGroupEntity,
			Infrastructure.Data.Entities.Tables.PRS.KonditionsZuordnungsTabelleEntity assignementConditionsEntity,
			Infrastructure.Data.Entities.Tables.BSD.IndustryEntity industryEntity,
			Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity currencyEntity,
			Infrastructure.Data.Entities.Tables.BSD.BelegkreiseVorgabenEntity slipCicleEntity,
			Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity suppliersGroupEntity,
			Infrastructure.Data.Entities.Tables.BSD.LieferantenExtensionEntity lieferantenExtensionEntity)
		{
			// > Main Ids
			this.Id = supplierEntity.Nr;
			this.Number = supplierEntity.Nummer ?? -1;
			this.AdressId = -1;

			if(lieferantenExtensionEntity != null)
			{
				ImageId = lieferantenExtensionEntity.ImageId;
			}
			// > Data
			this.SlipCircleId = supplierEntity.Belegkreis;
			this.SlipCircleName = supplierEntity.Belegkreis.HasValue
				? slipCicleEntity?.Bezeichnung
				: string.Empty;
			this.LanguageId = supplierEntity.Sprache;
			this.LanguageName = supplierEntity.Sprache.HasValue
				? "HAS VALUE"
				: string.Empty;
			this.DiscountGroupId = supplierEntity.Rabattgruppe;
			this.DiscountGroupName = discountGroupEntity?.Rabatthauptgruppe.ToString();
			this.ConditionAssignmentNumber = supplierEntity.Konditionszuordnungs_Nr;
			this.ConditionAssignmentNumberName = assignementConditionsEntity?.Text;
			this.CurrencyId = supplierEntity.Wahrung;
			this.CurrencyName = supplierEntity.Wahrung.HasValue
				? currencyEntity?.Wahrung
				: string.Empty;
			this.EgIdentificationNumber = supplierEntity.EG_Identifikationsnummer;
			this.SuppliersGroup = supplierEntity.Lieferantengruppe;
			this.Industry = supplierEntity.Branche;

			// > Adress
			var adress = adressEntity != null
				? new Address.AddressItemModel(adressEntity)
				: null;
			if(adress != null)
			{
				this.AddressEDIActive = adress.AddressEDIActive;
				this.AddressType = adress.AddressType;
				this.PreName = adress.PreName;
				this.AdressCategory = adress.Category;
				this.AdressCity = adress.City;
				this.AdressCountry = adress.Country;
				this.AdressCustomerNumber = adress.CustomerNumber;
				this.AdressDUNS = adress.DUNS;
				this.AdressEmailAdress = adress.EmailAdress;
				this.AdressFaxNumber = adress.FaxNumber;
				this.AdressFrom = adress.From;
				this.AdressId = adress.Id;
				this.AdressLevel = adress.Level;
				this.AdressMailbox = adress.Mailbox;
				this.AdressMailboxIsPreferred = adress.MailboxIsPreferred;
				this.AdressMailboxZipCode = adress.MailboxZipCode;
				this.AdressName1 = adress.Name1;
				this.AdressName2 = adress.Name2;
				this.AdressName3 = adress.Name3;
				this.AdressNote = adress.Note;
				this.AdressNotes = adress.Notes;
				this.AdressPersonalNumber = adress.PersonalNumber;
				this.AdressPhoneNumber = adress.PhoneNumber;
				this.AdressRecordTime = adress.RecordTime;
				this.AdressSelection = adress.Selection;
				this.AdressSortTerm = adress.SortTerm;
				this.AdressStreet = adress.Street;
				this.AdressStreetZipCode = adress.StreetZipCode;
				this.AdressSupplierNumber = adress.SupplierNumber;
				this.AdressTitle = adress.Title;
				this.AdressWebsite = adress.Website;
				this.AdressSalutation = adress.Salutation;
				this.AdressFirstName = adress.FirstName;
				this.AdressDepartment = adress.Department;
				this.AdressFunction = adress.Function;
			}

			// > Shipping
			var shipping = new Models.Supplier.ShippingModel(supplierEntity, adressEntity);
			if(shipping != null)
			{
				this.ShippingExpressSurcharge = shipping.ExpressSurcharge;
				this.ShippingFreightAllowance = shipping.FreightAllowance;
				this.ShippingFridayIsDeliveryDay = shipping.FridayIsDeliveryDay;
				this.ShippingMondayIsDeliveryDay = shipping.MondayIsDeliveryDay;
				this.ShippingCosts = shipping.ShippingCosts;
				this.ShippingMethod = shipping.ShippingMethod;
				this.ShippingWeekDay = shipping.ShippingWeekDay;
				this.ShippingThursdayIsDeliveryDay = shipping.ThursdayIsDeliveryDay;
				this.ShippingTuesdayIsDeliveryDay = shipping.TuesdayIsDeliveryDay;
				this.ShippingWednesdayIsDeliveryDay = shipping.WednesdayIsDeliveryDay;
			}


			// > Common
			this.Name = string.Empty;
			this.AdressText = string.Empty;
			if(adress != null)
			{
				this.AdressId = adress.Id;
				this.Name = !string.IsNullOrEmpty(adress.Name1)
					? adress.Name1
					: !string.IsNullOrEmpty(adress.Name2)
						? adress.Name2
						: adress.Name3;
				this.AdressText = $"{adress.StreetZipCode} {adress.Street}, {adress.City}, {adress.Country}";
			}

			// > External Ids
			this.VenderId = supplierEntity.Kreditoren_Nr;

			// > Payment
			this.PaymentMethod = supplierEntity.Zahlungsweise;
			this.Dunning = supplierEntity.Mahnsperre ?? false;
			this.DunningBlockSupplier = supplierEntity.Mahnsperre_Lieferant ?? false;
			this.CalculateSalesTax = supplierEntity.Umsatzsteuer_berechnen ?? false;

			// > Order
			this.RemindOrderConfirmation = supplierEntity.Bestellbestatigung_anmahnen ?? false;
			this.SurchargeMinimumOrderValue = Convert.ToDecimal(supplierEntity.Zuschlag_Mindestbestellwert ?? 0);
			this.MinimumValue = Convert.ToDecimal(supplierEntity.Mindestbestellwert ?? 0);
			this.BlockedForFurtherOrders = supplierEntity.Gesperrt_fur_weitere_Bestellungen ?? false;
			this.ReasonForBlocking = supplierEntity.Grund_fur_Sperre;
			this.WaitingPeriod = supplierEntity.Karenztage;
			this.OrderLimit = Convert.ToDecimal(supplierEntity.Bestellimit ?? 0);

			// > CustomerNumberSupplier
			this.CustomerNumberSupplier = supplierEntity.Kundennummer_Lieferanten;
			this.CustomerNumberSupplierSC = supplierEntity.Kundennummer_SC_Lieferanten;
			this.CustomerNumberSupplierScCz = supplierEntity.Kundennummer_SC_CZ_Lieferanten;
			this.CustomerNumberSupplierPszCz = supplierEntity.Kundennummer_PSZ_CZ_Lieferanten;
			this.CustomerNumberSupplierPszTn = supplierEntity.Kundennummer_PSZ_TN_Lieferanten;
			this.CustomerNumberSupplierPszAl = supplierEntity.Kundennummer_PSZ_AL_Lieferanten;

			// > Unknowing!
			this.TargetImpact = Convert.ToDecimal(supplierEntity.Zielaufschlag ?? 0);
			this.Lh = supplierEntity.LH ?? false;
			this.LhDate = supplierEntity.LH_Datum;

			// > Contact Persons
			this.ContactPersons = new List<ContactPersonModel>();
			if(contactPersonsEntities != null)
			{
				foreach(var ansprechpartnerEntity in contactPersonsEntities)
				{
					this.ContactPersons.Add(new ContactPersonModel(ansprechpartnerEntity));
				}
			}
		}
	}
}
