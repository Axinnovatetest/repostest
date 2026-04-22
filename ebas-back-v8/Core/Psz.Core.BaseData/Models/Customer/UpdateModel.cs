using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Customer
{
	using Psz.Core.BaseData.Models.Supplier;
	public class UpdateModel
	{
		public int Id { get; set; }
		public int Number { get; set; }


		// Kunden table >>> NOT IN GUI <<<
		public string Bemerkungen { get; set; }
		public double? Eilzuschlag { get; set; }
		public string Grund { get; set; }
		public bool? Lieferscheinadresse { get; set; }
		public double? Mindermengenzuschlag { get; set; }
		public int Nr { get; set; }
		public int? Nummer { get; set; }
		public bool? Regelmäßig_anschreiben { get; set; }
		public double? Zielaufschlag { get; set; }


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
		public string Branch { get; set; } // Branche
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
		public int AddressNr { get; set; }
		public int AddressType { get; set; }
		public string AdressPreName { get; set; }
		// > 
		public bool? AddressEDIActive { get; set; }
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

		public Infrastructure.Data.Entities.Tables.PRS.AdressenEntity ToAddressenEntity(int Nr, int? kundenNr)
		{
			return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity()
			{
				Nr = Nr,
				Kundennummer = kundenNr,
				Adresstyp = this.AddressType, // 

				Anrede = this.AdressPreName,
				Auswahl = this.AdressSelection,
				Bemerkung = this.AdressNote,
				Bemerkungen = this.AdressNotes,
				Briefanrede = this.AdressSalutation,
				Dienstag_Anliefertag = this.ShippingTuesdayIsDeliveryDay,
				Donnerstag_Anliefertag = this.ShippingThursdayIsDeliveryDay,
				Duns = this.AdressDUNS,
				EMail = this.AdressEmailAdress,
				Erfasst = this.AdressRecordTime,
				Fax = this.AdressFaxNumber,
				Freitag_Anliefertag = this.ShippingFridayIsDeliveryDay,
				Funktion = this.AdressFunction,
				Land = this.AdressCountry,
				Mittwoch_Anliefertag = this.ShippingWednesdayIsDeliveryDay,
				Montag_Anliefertag = this.ShippingMondayIsDeliveryDay,
				Name1 = this.AdressName1,
				Name2 = this.AdressName2,
				Name3 = this.AdressName3,
				Ort = this.AdressCity,
				PLZ_Postfach = this.AdressMailboxZipCode,
				PLZ_StraBe = this.AdressStreetZipCode,
				Postfach = this.AdressMailbox,
				Postfach_bevorzugt = this.AdressMailboxIsPreferred,
				Sortierbegriff = this.AdressSortTerm,
				StraBe = this.AdressStreet,
				Stufe = this.AdressLevel,
				Telefon = this.AdressPhoneNumber,
				Titel = this.AdressTitle,
				Von = this.AdressFrom,
				Vorname = this.AdressFirstName,
				WWW = this.AdressWebsite,
				EDI_Aktiv = this.AddressEDIActive,


				Sperren = false,
				//Abteilung = string.Empty,
				//Lieferantennummer = null,
				//Personalnummer = null,
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.KundenEntity ToKundenEntity(int Nr, int? KundenNr)
		{
			return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity()
			{
				Nr = Nr,
				Nummer = KundenNr,

				Bemerkungen = this.Bemerkungen,
				Branche = this.Branch,
				Bruttofakturierung = this.GrossInvoicing,
				Debitoren_Nr = this.DebtorsNr,
				EG___Identifikationsnummer = this.EC_IdentificationNumber,
				Eilzuschlag = this.Eilzuschlag,
				Factoring = this.Factoring,
				fibu_rahmen = this.FibuFrame,
				gesperrt_für_weitere_Lieferungen = this.BlockedForFurtherDeliveries,
				Grund = this.Grund,
				Grund_für_Sperre = this.ReasonForLock,
				Karenztage = this.WaitingDays,
				Konditionszuordnungs_Nr = this.ConditionAssignmentId,
				Kreditlimit = this.CreditLimit,
				Kundengruppe = this.CustomerGroup,
				Lieferantenummer__Kunden_ = this.SupplierNumber_customers,
				Lieferscheinadresse = this.Lieferscheinadresse,
				LSADR = this.LSADR,
				LSADRANG = this.LSADRANG,
				LSADRAUF = this.LSADRANG,
				LSADRGUT = this.LSADRGUT,
				LSADRPROF = this.LSADRPROF,
				LSADRRG = this.LSADRRG,
				LSADRSTO = this.LSADRSTO,
				LSRG = this.LSRG,
				Mahngebühr_1 = this.DunningFee_1,
				Mahngebühr_2 = this.DunningFee_2,
				Mahngebühr_3 = this.DunningFee_3,
				Mahnsperre = this.DunningBlock,
				Mindermengenzuschlag = this.Mindermengenzuschlag,
				//Nr = this.Nr,
				//Nummer = this.Nummer,
				OPOS = this.OPOS,
				Preisgruppe = this.PriceGroup,
				Preisgruppe2 = this.PriceGroup2,
				Rabattgruppe = this.DiscountGroupId,
				Regelmäßig_anschreiben__ = this.Regelmäßig_anschreiben,
				RG_Abteilung = this.RG_Department,
				RG_Land_PLZ_ORT = this.RG_Country_ZIPLocation,
				RG_Strasse_Postfach = this.RG_Street_postbox,
				Sprache = this.LanguageId,
				Standardversand = this.StandardShipping,
				Umsatzsteuer_berechnen = this.CalculateSalesTax,
				Versandart = this.ShippingMethod,
				Verzugszinsen = this.LatePaymentInterest,
				Verzugszinsen_ab_Mahnstufe = this.DefaultInteresFromMonitoringLevel,
				Währung = this.CurrencyId,
				Zahlung_erwartet_nach = this.PaymentExpectedAfter,
				Zahlungskondition = this.TermsOfPaymentId,
				Zahlungsmoral = this.PaymentPractices,
				Zahlungsweise = this.PaymentMethod,
				Zielaufschlag = this.Zielaufschlag,
				zolltarif_nr = this.CustomerFee_Nr,
			};
		}

		public Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity ToAddressenExtensionEntity(int Nr)
		{
			return new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity
			{
				Id = -1,
				AdressenNr = Nr,
				Duns = this.AdressDUNS,
			};
		}
	}
}
