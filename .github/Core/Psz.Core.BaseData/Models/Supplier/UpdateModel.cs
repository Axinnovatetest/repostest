using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class UpdateModel
	{
		public int Id { get; set; }
		public int Number { get; set; }

		// > Data
		// .p1
		/**/
		public int? DiscountGroupId { get; set; } // RabbatGruppe
		/**/
		public int? ConditionAssignmentNumber { get; set; } // Konditionszuordnungs_Nr
		/**/
		public string Industry { get; set; } // Branch
		/**/
		public string SuppliersGroup { get; set; } // Lieferantengruppe
		public bool CalculateSalesTax { get; set; } // Umsatzsteuer_berechnen
		public string EgIdentificationNumber { get; set; } // EG_Identifikationsnummer
		/**/
		public int? LanguageId { get; set; } // Sprache
		/**/
		public int? CurrencyId { get; set; } // Wahrung
		/**/
		public int? SlipCircleId { get; set; } // Belegkreis
		public bool Dunning { get; set; } // Mahnsperre
		public bool BlockedForFurtherOrders { get; set; } // Gesperrt_für_weitere_Bestellungen
		public string ReasonForBlocking { get; set; } // Grund_fur_Sperre
													  // .p2
		public string PaymentMethod { get; set; } // Zahlungsweise
		public decimal? OrderLimit { get; set; } // Bestellimit
		public decimal? TargetImpact { get; set; } // Zielaufschlag
		public decimal? MinimumValue { get; set; } // Mindestbestellwert
		public decimal? SurchargeMinimumOrderValue { get; set; } // Zuschlag_Mindestbestellwert

		#region > Adress
		public int AddressNr { get; set; }
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
		#endregion

		// > Payment
		public bool DunningBlockSupplier { get; set; }

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

		// > Order
		public bool RemindOrderConfirmation { get; set; }
		public string WaitingPeriod { get; set; }

		public List<ContactPersonModel> ContactPersons { get; set; } = new List<ContactPersonModel>();

		public Infrastructure.Data.Entities.Tables.PRS.AdressenEntity ToAdressenEntity(int? lieferantenNummer)
		{
			return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity()
			{
				Nr = this.AddressNr,
				Lieferantennummer = lieferantenNummer,
				Adresstyp = this.AddressType,

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
				Abteilung = string.Empty,
				// Kundennummer = null,
				// Personalnummer = null,
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity ToAdressenExtensionEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity()
			{
				Id = -1,
				AdressenNr = this.AddressNr,
				Duns = this.AdressDUNS,
			};
		}
	}
}
