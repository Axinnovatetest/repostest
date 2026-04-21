using System;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class SupplierDataModel
	{
		public int Id { get; set; }//lieferenten Nr
		public int Number { get; set; }//lieferentennummer
		public int AdressId { get; set; }//adressen Nr
		public bool? Isarchived { get; set; }
		public int? DiscountGroupId { get; set; } // RabbatGruppe
		public string DiscountGroupName { get; set; }
		public int? ConditionAssignmentNumber { get; set; } // Konditionszuordnungs_Nr
		public string ConditionAssignmentNumberName { get; set; }
		public string Industry { get; set; } // Branch
		public string SuppliersGroup { get; set; } // Lieferantengruppe
		public bool CalculateSalesTax { get; set; } // Umsatzsteuer_berechnen
		public string EgIdentificationNumber { get; set; } // EG_Identifikationsnummer
														   //public int? LanguageId { get; set; } // Sprache
														   //public string LanguageName { get; set; }
		public int? CurrencyId { get; set; } // Wahrung
		public string CurrencyName { get; set; } // Wahrung
		public int? SlipCircleId { get; set; } // Belegkreis
		public string SlipCircleName { get; set; }
		public bool Dunning { get; set; } // Mahnsperre
		public bool BlockedForFurtherOrders { get; set; } // Gesperrt_für_weitere_Bestellungen
		public string ReasonForBlocking { get; set; } // Grund_fur_Sperre
		public string PaymentMethod { get; set; } // Zahlungsweise
		public decimal? OrderLimit { get; set; } // Bestellimit
		public double? TargetImpact { get; set; } // Zielaufschlag
		public decimal? MinimumValue { get; set; } // Mindestbestellwert
		public decimal? SurchargeMinimumOrderValue { get; set; } // Zuschlag_Mindestbestellwert
																 //public string ShippingMethod { get; set; } // Versandart
																 //public decimal? ShippingExpressSurcharge { get; set; } // Eilzuschlag
																 //public string ShippingWeekDay { get; set; } // Wochentag_Anlieferung
																 //public decimal? ShippingFreightAllowance { get; set; } // Frachtfreigrenze
		public string CustomerPszNumber { get; set; }

		public SupplierDataModel()
		{

		}

		public SupplierDataModel(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity lierferentenEntity)
		{
			Id = lierferentenEntity.Nr;
			Isarchived = (Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(Id) != null) ? Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(Id).IsArchived : false;
			Number = lierferentenEntity.Nummer ?? -1;
			AdressId = (lierferentenEntity.Nummer.HasValue && Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(lierferentenEntity.Nummer.Value) != null) ?
				Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(lierferentenEntity.Nummer.Value).Lieferantennummer ?? -1 : -1;
			DiscountGroupId = lierferentenEntity.Rabattgruppe;
			DiscountGroupName = (lierferentenEntity.Rabattgruppe.HasValue && Infrastructure.Data.Access.Tables.BSD.RabatthauptgruppenAccess.Get(lierferentenEntity.Rabattgruppe.Value) != null) ?
				Infrastructure.Data.Access.Tables.BSD.RabatthauptgruppenAccess.Get(lierferentenEntity.Rabattgruppe.Value).Beschreibung : null;
			ConditionAssignmentNumber = lierferentenEntity.Konditionszuordnungs_Nr;
			ConditionAssignmentNumberName = (lierferentenEntity.Konditionszuordnungs_Nr.HasValue && Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(lierferentenEntity.Konditionszuordnungs_Nr.Value) != null) ?
				Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(lierferentenEntity.Konditionszuordnungs_Nr.Value).Text : null;
			Industry = lierferentenEntity.Branche;
			SuppliersGroup = lierferentenEntity.Lieferantengruppe;
			CalculateSalesTax = lierferentenEntity.Umsatzsteuer_berechnen.HasValue ? lierferentenEntity.Umsatzsteuer_berechnen.Value : false;
			EgIdentificationNumber = lierferentenEntity.EG_Identifikationsnummer;
			//LanguageId = lierferentenEntity.Sprache;
			//LanguageName
			CurrencyId = lierferentenEntity.Wahrung;
			CurrencyName = (lierferentenEntity.Wahrung.HasValue && Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get(lierferentenEntity.Wahrung.Value) != null) ?
				Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get(lierferentenEntity.Wahrung.Value).Wahrung : null;
			SlipCircleId = lierferentenEntity.Belegkreis;
			SlipCircleName = (lierferentenEntity.Belegkreis.HasValue && Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Get(lierferentenEntity.Belegkreis.Value) != null) ?
				Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Get(lierferentenEntity.Belegkreis.Value).Bezeichnung : null;
			Dunning = lierferentenEntity.Mahnsperre.HasValue ? lierferentenEntity.Mahnsperre.Value : false;
			BlockedForFurtherOrders = lierferentenEntity.Gesperrt_fur_weitere_Bestellungen.HasValue ? lierferentenEntity.Gesperrt_fur_weitere_Bestellungen.Value : false;
			ReasonForBlocking = lierferentenEntity.Grund_fur_Sperre;
			PaymentMethod = lierferentenEntity.Zahlungsweise;
			OrderLimit = decimal.TryParse(lierferentenEntity.Bestellimit.ToString(), out var val) ? val : (decimal?)null;
			TargetImpact = double.TryParse(lierferentenEntity.Zielaufschlag.ToString(), out var val2) ? val2 : (double?)null;
			MinimumValue = decimal.TryParse(lierferentenEntity.Mindestbestellwert.ToString(), out var val3) ? val3 : (decimal?)null;
			SurchargeMinimumOrderValue = decimal.TryParse(lierferentenEntity.Zuschlag_Mindestbestellwert.ToString(), out var val4) ? val4 : (decimal?)null;
			//ShippingMethod = lierferentenEntity.Versandart;
			//ShippingExpressSurcharge = decimal.TryParse(lierferentenEntity.Eilzuschlag.ToString(), out var val5) ? val5 : (decimal?)null;
			//ShippingWeekDay = lierferentenEntity.Wochentag_Anlieferung;
			//ShippingFreightAllowance = decimal.TryParse(lierferentenEntity.Frachtfreigrenze.ToString(), out var val6) ? val6 : (decimal?)null;
			CustomerPszNumber = lierferentenEntity.Kundennummer_Lieferanten;
		}

		public Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity
			{
				Belegkreis = SlipCircleId,
				Bestellimit = double.TryParse(OrderLimit.ToString(), out var val4) ? val4 : 0,
				Branche = Industry,
				//Eilzuschlag = float.TryParse(ShippingExpressSurcharge.ToString(), out var val2) ? val2 : 0,
				//Frachtfreigrenze = float.TryParse(ShippingFreightAllowance.ToString(), out var val5) ? val5 : 0,
				Gesperrt_fur_weitere_Bestellungen = BlockedForFurtherOrders,
				Grund_fur_Sperre = ReasonForBlocking,
				Konditionszuordnungs_Nr = ConditionAssignmentNumber,
				Lieferantengruppe = SuppliersGroup,
				Mahnsperre = Dunning,
				Mindestbestellwert = double.TryParse(MinimumValue.ToString(), out var val6) ? val6 : 0,
				Nr = Id,
				Nummer = Number,
				Rabattgruppe = DiscountGroupId,
				//Versandart = ShippingMethod,
				Wahrung = CurrencyId,
				//Wochentag_Anlieferung = ShippingWeekDay,
				Zahlungsweise = PaymentMethod,
				Zielaufschlag = Single.TryParse(TargetImpact.ToString(), out var val1) ? val1 : 0,
				Zuschlag_Mindestbestellwert = double.TryParse(SurchargeMinimumOrderValue.ToString(), out var val3) ? val3 : 0,
				EG_Identifikationsnummer = EgIdentificationNumber,
				Umsatzsteuer_berechnen = CalculateSalesTax,
				Kundennummer_Lieferanten = CustomerPszNumber
			};
		}

	}
}
