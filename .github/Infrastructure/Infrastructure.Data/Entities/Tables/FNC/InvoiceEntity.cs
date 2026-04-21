using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class InvoiceEntity
	{
		public string AB_Nr_Lieferant { get; set; }
		public string Abteilung { get; set; }
		public DateTime? Anfrage_Lieferfrist { get; set; }
		public string Anrede { get; set; }
		public string Ansprechpartner { get; set; }
		public DateTime? ApprovalTime { get; set; }
		public int? ApprovalUserId { get; set; }
		public bool? Archived { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public int? Bearbeiter { get; set; }
		public int? Belegkreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Benutzer { get; set; }
		public int? best_id { get; set; }
		public DateTime? Bestellbestätigung_erbeten_bis { get; set; }
		public string Bezug { get; set; }
		public string BillingAddress { get; set; }
		public int? BillingCompanyId { get; set; }
		public string BillingCompanyName { get; set; }
		public string BillingContactName { get; set; }
		public int? BillingDepartmentId { get; set; }
		public string BillingDepartmentName { get; set; }
		public string BillingFax { get; set; }
		public string BillingTelephone { get; set; }
		public int? BookingId { get; set; }
		public string Briefanrede { get; set; }
		public int BudgetYear { get; set; }
		public int? CompanyId { get; set; }
		public string CompanyName { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public bool? datueber { get; set; }
		public DateTime? Datum { get; set; }
		public int? DefaultCurrencyDecimals { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public decimal? DefaultCurrencyRate { get; set; }
		public bool? Deleted { get; set; }
		public DateTime? DeleteTime { get; set; }
		public int? DeleteUserId { get; set; }
		public string DeliveryAddress { get; set; }
		public int? DeliveryCompanyId { get; set; }
		public string DeliveryCompanyName { get; set; }
		public int? DeliveryDepartmentId { get; set; }
		public string DeliveryDepartmentName { get; set; }
		public string DeliveryFax { get; set; }
		public string DeliveryTelephone { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public string Description { get; set; }
		public decimal? Discount { get; set; }
		public decimal? DiscountAmount { get; set; }
		public decimal? DiscountAmountDefaultCurrency { get; set; }
		public string Eingangslieferscheinnr { get; set; }
		public string Eingangsrechnungsnr { get; set; }
		public bool? erledigt { get; set; }
		public decimal? Frachtfreigrenze { get; set; }
		public string Freitext { get; set; }
		public bool? gebucht { get; set; }
		public bool? gedruckt { get; set; }
		public int Id { get; set; }
		public string Ihr_Zeichen { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public string InternalContact { get; set; }
		public int IssuerId { get; set; }
		public string IssuerName { get; set; }
		public bool? Kanban { get; set; }
		public string Konditionen { get; set; }
		public string Kreditorennummer { get; set; }
		public int? Kundenbestellung { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public int? LastRejectionLevel { get; set; }
		public DateTime? LastRejectionTime { get; set; }
		public int? LastRejectionUserId { get; set; }
		public decimal? LeasingMonthAmount { get; set; }
		public int? LeasingNbMonths { get; set; }
		public int? LeasingStartMonth { get; set; }
		public int? LeasingStartYear { get; set; }
		public decimal? LeasingTotalAmount { get; set; }
		public int? Level { get; set; }
		public int? Lieferanten_Nr { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int? LocationId { get; set; }
		public bool? Loschen { get; set; }
		public DateTime? Mahnung { get; set; }
		public string Mandant { get; set; }
		public int? MandantId { get; set; }
		public decimal? Mindestbestellwert { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public bool? Neu { get; set; }
		public int? nr_anf { get; set; }
		public int? nr_bes { get; set; }
		public int? nr_gut { get; set; }
		public int? nr_RB { get; set; }
		public int? nr_sto { get; set; }
		public int? nr_war { get; set; }
		public int Number { get; set; }
		public bool? Offnen { get; set; }
		public int? OrderId { get; set; }
		public string OrderNumber { get; set; }
		public string OrderPlacedEmailMessage { get; set; }
		public string OrderPlacedEmailTitle { get; set; }
		public int? OrderPlacedReportFileId { get; set; }
		public string OrderPlacedSendingEmail { get; set; }
		public string OrderPlacedSupplierEmail { get; set; }
		public DateTime? OrderPlacedTime { get; set; }
		public string OrderPlacedUserEmail { get; set; }
		public int? OrderPlacedUserId { get; set; }
		public string OrderPlacedUserName { get; set; }
		public string OrderPlacementCCEmail { get; set; }
		public string OrderType { get; set; }
		public int? Personal_Nr { get; set; }
		public int? PoPaymentType { get; set; }
		public string PoPaymentTypeName { get; set; }
		public int? ProjectId { get; set; }
		public string ProjectName { get; set; }
		public string Projekt_Nr { get; set; }
		public double? Rabatt { get; set; }
		public bool? Rahmenbestellung { get; set; }
		public string Reference { get; set; }
		public int? Status { get; set; }
		public int? StorageLocationId { get; set; }
		public string StorageLocationName { get; set; }
		public string Straße_Postfach { get; set; }
		public string SupplierEmail { get; set; }
		public string SupplierFax { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
		public string SupplierNumber { get; set; }
		public string SupplierNummer { get; set; }
		public string SupplierPaymentMethod { get; set; }
		public string SupplierPaymentTerm { get; set; }
		public string SupplierTelephone { get; set; }
		public string SupplierTradingTerm { get; set; }
		public string Typ { get; set; }
		public string Unser_Zeichen { get; set; }
		public decimal? USt { get; set; }
		public DateTime? ValidationRequestTime { get; set; }
		public string Versandart { get; set; }
		public string Vorname_NameFirma { get; set; }
		public int? Wahrung { get; set; }
		public string Zahlungsweise { get; set; }
		public string Zahlungsziel { get; set; }
		// - 2024-08-05 
		public bool? IgnoreHandlingFees { get; set; }
		public InvoiceEntity() { }

		public InvoiceEntity(DataRow dataRow)
		{
			AB_Nr_Lieferant = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
			Abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			Anfrage_Lieferfrist = (dataRow["Anfrage_Lieferfrist"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Anfrage_Lieferfrist"]);
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			Ansprechpartner = (dataRow["Ansprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ansprechpartner"]);
			ApprovalTime = (dataRow["ApprovalTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ApprovalTime"]);
			ApprovalUserId = (dataRow["ApprovalUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ApprovalUserId"]);
			Archived = (dataRow["Archived"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Archived"]);
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			Bearbeiter = (dataRow["Bearbeiter"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bearbeiter"]);
			Belegkreis = (dataRow["Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Benutzer"]);
			best_id = (dataRow["best_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["best_id"]);
			Bestellbestätigung_erbeten_bis = (dataRow["Bestellbestätigung erbeten bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestellbestätigung erbeten bis"]);
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			BillingAddress = (dataRow["BillingAddress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingAddress"]);
			BillingCompanyId = (dataRow["BillingCompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BillingCompanyId"]);
			BillingCompanyName = (dataRow["BillingCompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingCompanyName"]);
			BillingContactName = (dataRow["BillingContactName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingContactName"]);
			BillingDepartmentId = (dataRow["BillingDepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BillingDepartmentId"]);
			BillingDepartmentName = (dataRow["BillingDepartmentName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingDepartmentName"]);
			BillingFax = (dataRow["BillingFax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingFax"]);
			BillingTelephone = (dataRow["BillingTelephone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingTelephone"]);
			BookingId = (dataRow["BookingId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BookingId"]);
			Briefanrede = (dataRow["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Briefanrede"]);
			BudgetYear = Convert.ToInt32(dataRow["BudgetYear"]);
			CompanyId = (dataRow["CompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CompanyId"]);
			CompanyName = (dataRow["CompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CompanyName"]);
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			CurrencyId = (dataRow["CurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CurrencyId"]);
			CurrencyName = (dataRow["CurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CurrencyName"]);
			datueber = (dataRow["datueber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["datueber"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			DefaultCurrencyDecimals = (dataRow["DefaultCurrencyDecimals"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyDecimals"]);
			DefaultCurrencyId = (dataRow["DefaultCurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyId"]);
			DefaultCurrencyName = (dataRow["DefaultCurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DefaultCurrencyName"]);
			DefaultCurrencyRate = (dataRow["DefaultCurrencyRate"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DefaultCurrencyRate"]);
			Deleted = (dataRow["Deleted"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Deleted"]);
			DeleteTime = (dataRow["DeleteTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeleteTime"]);
			DeleteUserId = (dataRow["DeleteUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DeleteUserId"]);
			DeliveryAddress = (dataRow["DeliveryAddress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryAddress"]);
			DeliveryCompanyId = (dataRow["DeliveryCompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DeliveryCompanyId"]);
			DeliveryCompanyName = (dataRow["DeliveryCompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryCompanyName"]);
			DeliveryDepartmentId = (dataRow["DeliveryDepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DeliveryDepartmentId"]);
			DeliveryDepartmentName = (dataRow["DeliveryDepartmentName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryDepartmentName"]);
			DeliveryFax = (dataRow["DeliveryFax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryFax"]);
			DeliveryTelephone = (dataRow["DeliveryTelephone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryTelephone"]);
			DepartmentId = (dataRow["DepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DepartmentId"]);
			DepartmentName = (dataRow["DepartmentName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DepartmentName"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Discount = (dataRow["Discount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Discount"]);
			DiscountAmount = (dataRow["DiscountAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountAmount"]);
			DiscountAmountDefaultCurrency = (dataRow["DiscountAmountDefaultCurrency"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountAmountDefaultCurrency"]);
			Eingangslieferscheinnr = (dataRow["Eingangslieferscheinnr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Eingangslieferscheinnr"]);
			Eingangsrechnungsnr = (dataRow["Eingangsrechnungsnr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Eingangsrechnungsnr"]);
			erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt"]);
			Frachtfreigrenze = (dataRow["Frachtfreigrenze"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Frachtfreigrenze"]);
			Freitext = (dataRow["Freitext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freitext"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gedruckt"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Ihr_Zeichen = (dataRow["Ihr Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ihr Zeichen"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			InternalContact = (dataRow["InternalContact"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InternalContact"]);
			IssuerId = Convert.ToInt32(dataRow["IssuerId"]);
			IssuerName = (dataRow["IssuerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IssuerName"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			Konditionen = (dataRow["Konditionen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Konditionen"]);
			Kreditorennummer = (dataRow["Kreditorennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kreditorennummer"]);
			Kundenbestellung = (dataRow["Kundenbestellung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundenbestellung"]);
			Land_PLZ_Ort = (dataRow["Land/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land/PLZ/Ort"]);
			LastRejectionLevel = (dataRow["LastRejectionLevel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastRejectionLevel"]);
			LastRejectionTime = (dataRow["LastRejectionTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastRejectionTime"]);
			LastRejectionUserId = (dataRow["LastRejectionUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastRejectionUserId"]);
			LeasingMonthAmount = (dataRow["LeasingMonthAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["LeasingMonthAmount"]);
			LeasingNbMonths = (dataRow["LeasingNbMonths"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LeasingNbMonths"]);
			LeasingStartMonth = (dataRow["LeasingStartMonth"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LeasingStartMonth"]);
			LeasingStartYear = (dataRow["LeasingStartYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LeasingStartYear"]);
			LeasingTotalAmount = (dataRow["LeasingTotalAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["LeasingTotalAmount"]);
			Level = (dataRow["Level"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Level"]);
			Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferanten-Nr"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			LocationId = (dataRow["LocationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LocationId"]);
			Loschen = (dataRow["Loschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Loschen"]);
			Mahnung = (dataRow["Mahnung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Mahnung"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
			MandantId = (dataRow["MandantId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MandantId"]);
			Mindestbestellwert = (dataRow["Mindestbestellwert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestellwert"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			Neu = (dataRow["Neu"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Neu"]);
			nr_anf = (dataRow["nr_anf"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_anf"]);
			nr_bes = (dataRow["nr_bes"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_bes"]);
			nr_gut = (dataRow["nr_gut"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_gut"]);
			nr_RB = (dataRow["nr_RB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_RB"]);
			nr_sto = (dataRow["nr_sto"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_sto"]);
			nr_war = (dataRow["nr_war"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_war"]);
			Number = Convert.ToInt32(dataRow["Number"]);
			Offnen = (dataRow["Offnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Offnen"]);
			OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderId"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			OrderPlacedEmailMessage = (dataRow["OrderPlacedEmailMessage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedEmailMessage"]);
			OrderPlacedEmailTitle = (dataRow["OrderPlacedEmailTitle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedEmailTitle"]);
			OrderPlacedReportFileId = (dataRow["OrderPlacedReportFileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderPlacedReportFileId"]);
			OrderPlacedSendingEmail = (dataRow["OrderPlacedSendingEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedSendingEmail"]);
			OrderPlacedSupplierEmail = (dataRow["OrderPlacedSupplierEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedSupplierEmail"]);
			OrderPlacedTime = (dataRow["OrderPlacedTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OrderPlacedTime"]);
			OrderPlacedUserEmail = (dataRow["OrderPlacedUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedUserEmail"]);
			OrderPlacedUserId = (dataRow["OrderPlacedUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderPlacedUserId"]);
			OrderPlacedUserName = (dataRow["OrderPlacedUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedUserName"]);
			OrderPlacementCCEmail = (dataRow["OrderPlacementCCEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacementCCEmail"]);
			OrderType = (dataRow["OrderType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderType"]);
			Personal_Nr = (dataRow["Personal-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personal-Nr"]);
			PoPaymentType = (dataRow["PoPaymentType"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PoPaymentType"]);
			PoPaymentTypeName = (dataRow["PoPaymentTypeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PoPaymentTypeName"]);
			ProjectId = (dataRow["ProjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectId"]);
			ProjectName = (dataRow["ProjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectName"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt"]);
			Rahmenbestellung = (dataRow["Rahmenbestellung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmenbestellung"]);
			Reference = Convert.ToString(dataRow["Reference"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Status"]);
			StorageLocationId = (dataRow["StorageLocationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StorageLocationId"]);
			StorageLocationName = (dataRow["StorageLocationName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StorageLocationName"]);
			Straße_Postfach = (dataRow["Straße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße/Postfach"]);
			SupplierEmail = (dataRow["SupplierEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierEmail"]);
			SupplierFax = (dataRow["SupplierFax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierFax"]);
			SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
			SupplierName = (dataRow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierName"]);
			SupplierNumber = (dataRow["SupplierNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierNumber"]);
			SupplierNummer = (dataRow["SupplierNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierNummer"]);
			SupplierPaymentMethod = (dataRow["SupplierPaymentMethod"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierPaymentMethod"]);
			SupplierPaymentTerm = (dataRow["SupplierPaymentTerm"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierPaymentTerm"]);
			SupplierTelephone = (dataRow["SupplierTelephone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierTelephone"]);
			SupplierTradingTerm = (dataRow["SupplierTradingTerm"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierTradingTerm"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Unser_Zeichen = (dataRow["Unser Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Unser Zeichen"]);
			USt = (dataRow["USt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["USt"]);
			ValidationRequestTime = (dataRow["ValidationRequestTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidationRequestTime"]);
			Versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			Wahrung = (dataRow["Wahrung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wahrung"]);
			Zahlungsweise = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
			Zahlungsziel = (dataRow["Zahlungsziel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsziel"]);
			IgnoreHandlingFees = (dataRow["IgnoreHandlingFees"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IgnoreHandlingFees"]);
		}
	}
}

