using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Supplier
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
		public bool? AddressEDIActive { get; set; }
		public int AddressType { get; set; }
		public string AdressPreName { get; set; }
		// > 
		public int? AdressDUNS { get; set; }
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

		public Infrastructure.Data.Entities.Tables.FNC.KonditionsZuordnungsTabelleEntity ToKonditionsEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.KonditionsZuordnungsTabelleEntity
			{
				Bemerkung = "",
				Nettotage = null,
				Nr = 0,
				Skonto = null,
				Skontotage = null,
				Text = ""
			};
		}
	}
}
