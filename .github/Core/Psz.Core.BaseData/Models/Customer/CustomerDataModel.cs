namespace Psz.Core.BaseData.Models.Customer
{
	public class CustomerDataModel
	{
		public string DeliveryAdress { get; set; }
		public int Id { get; set; }//Nr
		public int? Number { get; set; }//Nummer
		public int? AdressId { get; set; }
		public bool? Isarchived { get; set; }
		public string DeliveryAddressName { get; set; }
		//public int? LSADR { get; set; } // __ Address Nr
		//public bool? LSADRANG { get; set; } // __ Angebote
		//public bool? LSADRAUF { get; set; } // __ Auftragsbestatigungen
		//public bool? LSADRGUT { get; set; } // __ Gutschritten
		//public bool? LSADRPROF { get; set; } // __ Proformarechnungen
		//public bool? LSADRRG { get; set; } // __ Rechnungen
		//public bool? LSADRSTO { get; set; } // __ Stornierungen
		//public bool? LSRG { get; set; } // __ RechnungensAdresse
		//public string StandardShipping { get; set; } // Standardversand

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
		public bool? Factoring { get; set; }//Factoring
		public string DebtorsNr { get; set; } // Debitoren_Nr
		public int? FibuFrame { get; set; } // Fibu_rahmen
		public bool? CalculateSalesTax { get; set; } // Umsatzsteuer_berechnen
		public string EG_IdentificationNumber { get; set; } // EG_Identifikationsnummer <<<<<  Ust - Identifikationsnummer ???
		public bool? GrossInvoicing { get; set; } // Bruttofakturierung
												  //public int? LanguageId { get; set; } // Sprache
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
		public bool? OPOS { get; set; }//OPOS
		public bool? DunningBlock { get; set; } // Mahnsperre 
		public int? WaitingDays { get; set; } // Karenztage 
		public bool? BlockedForFurtherDeliveries { get; set; } // gesperrt_für_weitere_Lieferungen 
		public bool? DELFixiert { get; set; }
		public int? DEL { get; set; }

		// 18-03/2025 ticket #43321
		public int? CodeTypeInLSId { get; set; }
		public CustomerDataModel()
		{

		}

		public CustomerDataModel(Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity,
		Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity)
		{
			DeliveryAdress = $"{addressEntity.Name1.Trim()} {addressEntity.Name2.Trim()} {addressEntity.Name3.Trim()} {addressEntity.StraBe.Trim()} {addressEntity.Ort.Trim()}";
			Id = kundenEntity.Nr;//Nr
			Isarchived = (Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(Id) != null) ? Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(Id).IsArchived : false;
			Number = int.TryParse(addressEntity.Kundennummer.ToString(), out int nummer) ? nummer : -1;
			AdressId = int.TryParse(kundenEntity.Nummer.ToString(), out int num) ? num : -1;
			//
			//LSADR = kundenEntity.LSADR; // __ Address Nr
			//LSADRANG = kundenEntity.LSADRANG; // __ Angebote
			//LSADRAUF = kundenEntity.LSADRAUF; // __ Auftragsbestatigungen
			//LSADRGUT = kundenEntity.LSADRGUT; // __ Gutschritten
			//LSADRPROF = kundenEntity.LSADRPROF; // __ Proformarechnungen
			//LSADRRG = kundenEntity.LSADRRG; // __ Rechnungen
			//LSADRSTO = kundenEntity.LSADRSTO; // __ Stornierungen
			//LSRG = kundenEntity.LSRG; // __ RechnungensAdresse
			//StandardShipping = kundenEntity.Standardversand; // Standardversand

			// >>> Deviating Address
			RG_Department = kundenEntity.RG_Abteilung; // RG_Abteilung
			RG_Country_ZIPLocation = kundenEntity.RG_Land_PLZ_ORT; // RG_Land_PLZ_ORT
			RG_Street_postbox = kundenEntity.RG_Strasse_Postfach; // RG_Strasse_Postfach

			// >>>
			PriceGroup = kundenEntity.Preisgruppe; // Preisgruppe
			PriceGroup2 = kundenEntity.Preisgruppe2; // Preisgruppe2
			DiscountGroupId = kundenEntity.Rabattgruppe; // Rabattgruppe

			// >>>
			Industry = kundenEntity.Branche; // Branche
			CustomerGroup = kundenEntity.Kundengruppe; // Kundengruppe

			// >>>
			SupplierNumber_customers = kundenEntity.Lieferantenummer__Kunden_; // Lieferantenummer_Kunden
			PaymentMethod = kundenEntity.Zahlungsweise; // Zahlungsweise
			ShippingMethod = kundenEntity.Versandart; // Versandart
			ConditionAssignmentId = kundenEntity.Konditionszuordnungs_Nr; // Konditionszuordnungs_Nr
			TermsOfPaymentId = kundenEntity.Zahlungskondition; // Zahlungskondition

			// >>>
			Factoring = kundenEntity.Factoring;
			DebtorsNr = kundenEntity.Debitoren_Nr; // Debitoren_Nr
			FibuFrame = kundenEntity.fibu_rahmen; // Fibu_rahmen
			CalculateSalesTax = kundenEntity.Umsatzsteuer_berechnen; // Umsatzsteuer_berechnen
			EG_IdentificationNumber = kundenEntity.EG___Identifikationsnummer; // EG_Identifikationsnummer <<<<<  Ust - Identifikationsnummer ???
			GrossInvoicing = kundenEntity.Bruttofakturierung; // Bruttofakturierung
															  //LanguageId = kundenEntity.Sprache; // Sprache
			CurrencyId = kundenEntity.Währung; // Währung
			SlipCircleId = kundenEntity.Belegkreis; // Belegkreis
			CustomerFee_Nr = kundenEntity.zolltarif_nr; // Zolltarif_Nr

			// >>>
			DunningFee_1 = kundenEntity.Mahngebühr_1; // Mahngebühr_1
			DunningFee_2 = kundenEntity.Mahngebühr_2; // Mahngebühr_2
			DunningFee_3 = kundenEntity.Mahngebühr_3; // Mahngebühr_3

			PaymentExpectedAfter = kundenEntity.Zahlung_erwartet_nach; // Zahlung_erwartet_nach
			LatePaymentInterest = kundenEntity.Verzugszinsen; // Verzugszinsen
			DefaultInteresFromMonitoringLevel = kundenEntity.Verzugszinsen_ab_Mahnstufe; // Verzugszinsen_ab_Mahnstufe
			ReasonForLock = kundenEntity.Grund_für_Sperre; // Grund_für_Sperre

			// >>>
			PaymentPractices = kundenEntity.Zahlungsmoral; // Zahlungsmoral
			CreditLimit = kundenEntity.Kreditlimit; // Kreditlimit 
			OPOS = kundenEntity.OPOS;
			DunningBlock = kundenEntity.Mahnsperre; // Mahnsperre 
			WaitingDays = kundenEntity.Karenztage; // Karenztage 
			BlockedForFurtherDeliveries = kundenEntity.gesperrt_für_weitere_Lieferungen; // gesperrt_für_weitere_Lieferungen 
																						 //>>>>
			DELFixiert = kundenEntity.DELFixiert;
			DEL = kundenEntity.DEL;
			CodeTypeInLSId = kundenEntity.CodeTypeInLSId;
		}

		public Infrastructure.Data.Entities.Tables.PRS.KundenEntity ToKundenEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity()
			{
				Nr = Id,
				Nummer = AdressId,
				Belegkreis = SlipCircleId,
				//Bemerkungen = Bemerkungen,
				Branche = Industry,
				Bruttofakturierung = GrossInvoicing,
				Debitoren_Nr = DebtorsNr,
				EG___Identifikationsnummer = EG_IdentificationNumber,
				//Eilzuschlag = Eilzuschlag,
				Factoring = Factoring,
				fibu_rahmen = FibuFrame,
				gesperrt_für_weitere_Lieferungen = BlockedForFurtherDeliveries,
				//Grund = Grund,
				Grund_für_Sperre = ReasonForLock,
				Karenztage = WaitingDays,
				Konditionszuordnungs_Nr = ConditionAssignmentId,
				Kreditlimit = CreditLimit,
				Kundengruppe = CustomerGroup,
				Lieferantenummer__Kunden_ = SupplierNumber_customers,
				//Lieferscheinadresse = Lieferscheinadresse,
				//LSADR = LSADR,
				//LSADRANG = LSADRANG,
				//LSADRAUF = LSADRAUF,
				//LSADRGUT = LSADRGUT,
				//LSADRPROF = LSADRPROF,
				//LSADRRG = LSADRRG,
				//LSADRSTO = LSADRSTO,
				//LSRG = LSRG,
				//Standardversand = StandardShipping,
				Mahngebühr_1 = DunningFee_1,
				Mahngebühr_2 = DunningFee_2,
				Mahngebühr_3 = DunningFee_3,
				Mahnsperre = DunningBlock,
				// Mindermengenzuschlag = Mindermengenzuschlag,
				OPOS = OPOS,
				Preisgruppe = PriceGroup,
				Preisgruppe2 = PriceGroup2,
				Rabattgruppe = DiscountGroupId,
				//Regelmäßig_anschreiben__ = Regelmäßig_anschreiben,
				RG_Abteilung = RG_Department,
				RG_Land_PLZ_ORT = RG_Country_ZIPLocation,
				RG_Strasse_Postfach = RG_Street_postbox,
				//Sprache = LanguageId,
				Umsatzsteuer_berechnen = CalculateSalesTax,
				Versandart = ShippingMethod,
				Verzugszinsen = LatePaymentInterest,
				Verzugszinsen_ab_Mahnstufe = DefaultInteresFromMonitoringLevel,
				Währung = CurrencyId,
				Zahlung_erwartet_nach = PaymentExpectedAfter,
				Zahlungskondition = TermsOfPaymentId,
				Zahlungsmoral = PaymentPractices,
				Zahlungsweise = PaymentMethod,
				//Zielaufschlag = Zielaufschlag,
				zolltarif_nr = CustomerFee_Nr,
				DELFixiert = DELFixiert,
				DEL = DEL,
				// 18-03/2025 ticket #43321
				CodeTypeInLSId = CodeTypeInLSId,
				CodeTypeInLS = CodeTypeInLSId.HasValue ? ((Enums.AddressEnums.LSCodeTypes)CodeTypeInLSId).GetDescription() : null,
			};
		}
	}
}
