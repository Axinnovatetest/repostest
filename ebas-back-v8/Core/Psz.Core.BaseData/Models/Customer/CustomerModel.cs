using Psz.Core.BaseData.Models.Supplier;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Customer
{
	public class CustomerModel
	{
		public int ImageId { get; set; }
		public int Id { get; set; }
		public int Number { get; set; }
		public bool? Isarchived { get; set; }

		// Kunden table >>> NOT IN GUI <<<
		//public string Bemerkungen { get; set; }
		//public double? Eilzuschlag { get; set; }
		//public string Grund { get; set; }
		//public bool? Lieferscheinadresse { get; set; }
		//public double? Mindermengenzuschlag { get; set; }
		//public int Nr { get; set; }
		public int? Nummer { get; set; }
		//public bool? Regelmäßig_anschreiben { get; set; }
		//public double? Zielaufschlag { get; set; }


		#region >>>> Data <<<<
		// >>> Delivery
		public string DeliveryAddressName { get; set; }
		public int? LSADR { get; set; } // __ Address Nr
		public bool? LSADRANG { get; set; } // __ Angebote
		public bool? LSADRAUF { get; set; } // __ Auftragsbestatigungen
		public bool? LSADRGUT { get; set; } // __ Gutschritten
		public bool? LSADRPROF { get; set; } // __ Proformarechnungen
		public bool? LSADRRG { get; set; } // __ Rechnungen
		public bool? LSADRSTO { get; set; } // __ Stornierungen
		public bool? LSRG { get; set; } // __ RechnungensAdresse
		public string StandardShipping { get; set; } // Standardversand

		// >>> Deviating Address
		public string RG_Department { get; set; } // RG_Abteilung
		public string RG_Country_ZIPLocation { get; set; } // RG_Land_PLZ_ORT
		public string RG_Street_postbox { get; set; } // RG_Strasse_Postfach

		// >>>
		public int? PriceGroup { get; set; } // Preisgruppe
		public int? PriceGroup2 { get; set; } // Preisgruppe2
		public int? DiscountGroupId { get; set; } // Rabattgruppe

		// >>>
		public string Industry { get; set; } // Branche
		public string CustomerGroup { get; set; } // Kundengruppe

		// >>>
		public string SupplierNumber_customers { get; set; } // Lieferantenummer_Kunden
		public string PaymentMethod { get; set; } // Zahlungsweise
		public string ShippingMethod { get; set; } // Versandart
		public int? ConditionAssignmentId { get; set; } // Konditionszuordnungs_Nr
		public int? TermsOfPaymentId { get; set; } // Zahlungskondition

		// >>>
		public bool? Factoring { get; set; }
		public string DebtorsNr { get; set; } // Debitoren_Nr
		public int? FibuFrame { get; set; } // Fibu_rahmen
		public bool? CalculateSalesTax { get; set; } // Umsatzsteuer_berechnen
		public string EC_IdentificationNumber { get; set; } // EG_Identifikationsnummer <<<<<  Ust - Identifikationsnummer ???
		public bool? GrossInvoicing { get; set; } // Bruttofakturierung
		public int? LanguageId { get; set; } // Sprache
		public int? CurrencyId { get; set; } // Währung
		public int? SlipCircleId { get; set; } // Belegkreis
		public bool? CustomerFee_Nr { get; set; } // Zolltarif_Nr

		// >>>
		public double? DunningFee_1 { get; set; } // Mahngebühr_1
		public double? DunningFee_2 { get; set; } // Mahngebühr_2
		public double? DunningFee_3 { get; set; } // Mahngebühr_3

		public int? PaymentExpectedAfter { get; set; } // Zahlung_erwartet_nach
		public double? LatePaymentInterest { get; set; } // Verzugszinsen
		public int? DefaultInteresFromMonitoringLevel { get; set; } // Verzugszinsen_ab_Mahnstufe
		public string ReasonForLock { get; set; } // Grund_für_Sperre

		// >>>
		public int? PaymentPractices { get; set; } // Zahlungsmoral
		public double? CreditLimit { get; set; } // Kreditlimit 
		public bool? OPOS { get; set; }
		public bool? DunningBlock { get; set; } // Mahnsperre 
		public int? WaitingDays { get; set; } // Karenztage 
		public bool? BlockedForFurtherDeliveries { get; set; } // gesperrt_für_weitere_Lieferungen 
		#endregion >>>> Data <<<<


		#region >>>> Address <<<<
		public int AddressId { get; set; }
		public bool? AddressEDIActive { get; set; }
		public int AddressType { get; set; }
		public string AdressPreName { get; set; }
		// > 
		public string AdressDUNS { get; set; }
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
		public bool AdressLock { get; set; }
		public string AdressLevel { get; set; }
		public DateTime? AdressRecordTime { get; set; }
		public string AdressFrom { get; set; }
		public string AdressSortTerm { get; set; }
		// >
		public string AdressFirstName { get; set; }
		public string AdressSalutation { get; set; }
		public string AdressDepartment { get; set; }
		public string AdressFunction { get; set; }
		#endregion >>>> Address <<<<


		#region >>>> Communication <<<<
		// > Communication 
		#endregion >>>> Communication <<<<


		#region >>>> Shipping <<<<
		public decimal? ShippingCosts { get; set; }
		public bool ShippingMondayIsDeliveryDay { get; set; }
		public bool ShippingTuesdayIsDeliveryDay { get; set; }
		public bool ShippingWednesdayIsDeliveryDay { get; set; }
		public bool ShippingThursdayIsDeliveryDay { get; set; }
		public bool ShippingFridayIsDeliveryDay { get; set; }
		// >
		public string ShippingWeekDay { get; set; } // Wochentag_Anlieferung
													//public string ShippingMethod { get; set; } // Versandart    >>>>>>> >>> Viewed in DATA region <<<<
		public decimal? ShippingFreightAllowance { get; set; } // Frachtfreigrenze
		public decimal? ShippingExpressSurcharge { get; set; } // Eilzuschlag
		#endregion >>>> Shipping <<<<

		// > Contact person
		public List<ContactPersonModel> ContactPersons { get; set; } = new List<ContactPersonModel>();

		public CustomerModel(Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity,
			List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> contactPersonsEntities,
			Infrastructure.Data.Entities.Tables.BSD.KundenExtensionEntity kundenExtensionEntity)
		{
			// > Main Ids
			this.Id = kundenEntity.Nr;
			this.Number = addressEntity?.Kundennummer ?? -1; //  kundenEntity.Nummer ?? -1;
			Isarchived = (Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(Id) != null) ? Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(Id).IsArchived : false;
			if(kundenExtensionEntity != null)
			{
				ImageId = kundenExtensionEntity.ImageId;
			}


			// Kunden table >>> NOT IN GUI <<<
			//this.Bemerkungen = kundenEntity.Nr;
			//this.Eilzuschlag = kundenEntity.Nr;
			//this.Grund = kundenEntity.Nr;
			//this.Lieferscheinadresse = kundenEntity.Nr;
			//this.Mindermengenzuschlag = kundenEntity.Nr;
			//this.Nr = kundenEntity.Nr;
			this.Nummer = kundenEntity.Nummer;
			//this.Regelmäßig_anschreiben = kundenEntity.Nr;
			//this.Zielaufschlag = kundenEntity.Nr;


			#region >>>> Data <<<<
			// >>> Delivery
			//this.DeliveryAddressName = kundenEntity.DeliveryAddressName;
			this.LSADR = kundenEntity.LSADR; // __ Address Nr
			this.LSADRANG = kundenEntity.LSADRANG; // __ Angebote
			this.LSADRAUF = kundenEntity.LSADRAUF; // __ Auftragsbestatigungen
			this.LSADRGUT = kundenEntity.LSADRGUT; // __ Gutschritten
			this.LSADRPROF = kundenEntity.LSADRPROF; // __ Proformarechnungen
			this.LSADRRG = kundenEntity.LSADRRG; // __ Rechnungen
			this.LSADRSTO = kundenEntity.LSADRSTO; // __ Stornierungen
			this.LSRG = kundenEntity.LSRG; // __ RechnungensAdresse
			this.StandardShipping = kundenEntity.Standardversand; // Standardversand

			// >>> Deviating Address
			this.RG_Department = kundenEntity.RG_Abteilung; // RG_Abteilung
			this.RG_Country_ZIPLocation = kundenEntity.RG_Land_PLZ_ORT; // RG_Land_PLZ_ORT
			this.RG_Street_postbox = kundenEntity.RG_Strasse_Postfach; // RG_Strasse_Postfach

			// >>>
			this.PriceGroup = kundenEntity.Preisgruppe; // Preisgruppe
			this.PriceGroup2 = kundenEntity.Preisgruppe2; // Preisgruppe2
			this.DiscountGroupId = kundenEntity.Rabattgruppe; // Rabattgruppe

			// >>>
			this.Industry = kundenEntity.Branche; // Branche
			this.CustomerGroup = kundenEntity.Kundengruppe; // Kundengruppe

			// >>>
			this.SupplierNumber_customers = kundenEntity.Lieferantenummer__Kunden_; // Lieferantenummer_Kunden
			this.PaymentMethod = kundenEntity.Zahlungsweise; // Zahlungsweise
			this.ShippingMethod = kundenEntity.Versandart; // Versandart
			this.ConditionAssignmentId = kundenEntity.Konditionszuordnungs_Nr; // Konditionszuordnungs_Nr
			this.TermsOfPaymentId = kundenEntity.Zahlungskondition; // Zahlungskondition

			// >>>
			this.Factoring = kundenEntity.Factoring;
			this.DebtorsNr = kundenEntity.Debitoren_Nr; // Debitoren_Nr
			this.FibuFrame = kundenEntity.fibu_rahmen; // Fibu_rahmen
			this.CalculateSalesTax = kundenEntity.Umsatzsteuer_berechnen; // Umsatzsteuer_berechnen
			this.EC_IdentificationNumber = kundenEntity.EG___Identifikationsnummer; // EG_Identifikationsnummer <<<<<  Ust - Identifikationsnummer ???
			this.GrossInvoicing = kundenEntity.Bruttofakturierung; // Bruttofakturierung
			this.LanguageId = kundenEntity.Sprache; // Sprache
			this.CurrencyId = kundenEntity.Währung; // Währung
			this.SlipCircleId = kundenEntity.Belegkreis; // Belegkreis
			this.CustomerFee_Nr = kundenEntity.zolltarif_nr; // Zolltarif_Nr

			// >>>
			this.DunningFee_1 = kundenEntity.Nr; // Mahngebühr_1
			this.DunningFee_2 = kundenEntity.Nr; // Mahngebühr_2
			this.DunningFee_3 = kundenEntity.Nr; // Mahngebühr_3

			this.PaymentExpectedAfter = kundenEntity.Zahlung_erwartet_nach; // Zahlung_erwartet_nach
			this.LatePaymentInterest = kundenEntity.Verzugszinsen; // Verzugszinsen
			this.DefaultInteresFromMonitoringLevel = kundenEntity.Verzugszinsen_ab_Mahnstufe; // Verzugszinsen_ab_Mahnstufe
			this.ReasonForLock = kundenEntity.Grund_für_Sperre; // Grund_für_Sperre

			// >>>
			this.PaymentPractices = kundenEntity.Zahlungsmoral; // Zahlungsmoral
			this.CreditLimit = kundenEntity.Kreditlimit; // Kreditlimit 
			this.OPOS = kundenEntity.OPOS;
			this.DunningBlock = kundenEntity.Mahnsperre; // Mahnsperre 
			this.WaitingDays = kundenEntity.Karenztage; // Karenztage 
			this.BlockedForFurtherDeliveries = kundenEntity.gesperrt_für_weitere_Lieferungen; // gesperrt_für_weitere_Lieferungen 
			#endregion >>>> Data <<<<


			#region >>>> Address <<<<
			var address = addressEntity != null
				? new Address.AddressItemModel(addressEntity)
				: null;
			if(address != null)
			{
				this.AddressId = address.AdressId;
				this.AddressType = address.AddressType;
				this.AdressPreName = address.PreName;
				// > 
				this.AddressEDIActive = address.AddressEDIActive;
				this.AdressDUNS = address.DUNS;
				// >
				this.AdressTitle = address.Title;
				this.AdressName1 = address.Name1;
				this.AdressName2 = address.Name2;
				this.AdressName3 = address.Name3;
				// >
				this.AdressCountry = address.Country;
				this.AdressCity = address.City;
				this.AdressStreet = address.Street;
				this.AdressStreetZipCode = address.StreetZipCode;
				this.AdressMailbox = address.Mailbox;
				this.AdressMailboxZipCode = address.MailboxZipCode;
				this.AdressMailboxIsPreferred = address.MailboxIsPreferred;
				// >
				this.AdressPhoneNumber = address.PhoneNumber;
				this.AdressFaxNumber = address.FaxNumber;
				this.AdressEmailAdress = address.EmailAdress;
				this.AdressWebsite = address.Website;
				// >
				this.AdressNote = address.Note;
				this.AdressNotes = address.Notes;
				// >
				this.AdressSelection = address.Selection;
				//this.AdressLock = adress.loc;
				this.AdressLevel = address.Level;
				this.AdressRecordTime = address.RecordTime;
				this.AdressFrom = address.From;
				this.AdressSortTerm = address.SortTerm;
				// >
				this.AdressFirstName = address.FirstName;
				this.AdressSalutation = address.Salutation;
				this.AdressDepartment = address.Department;
				this.AdressFunction = address.Function;
			}
			#endregion >>>> Address <<<<


			#region >>>> Communication <<<<
			// > Communication 
			#endregion >>>> Communication <<<<


			#region >>>> Shipping <<<<
			var shipping = new Models.Supplier.ShippingModel(kundenEntity, addressEntity);
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
			#endregion >>>> Shipping <<<<

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
