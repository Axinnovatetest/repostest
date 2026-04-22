using System;
using System.Data;


namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class CompanyExtensionEntity
	{
		public string Account1 { get; set; }
		public string Account2 { get; set; }
		public string Account3 { get; set; }
		public string Account4 { get; set; }
		public string Address { get; set; }
		public string BankDetails1 { get; set; }
		public string BankDetails2 { get; set; }
		public string BankDetails3 { get; set; }
		public string BankDetails4 { get; set; }
		public string BLZ1 { get; set; }
		public string BLZ2 { get; set; }
		public string BLZ3 { get; set; }
		public string BLZ4 { get; set; }
		public string City { get; set; }
		public int CompanyId { get; set; }
		public string CompanyName { get; set; }
		public string Country { get; set; }
		public string CustomsNumber { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public string DeliveryAddress { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public string FinanceProfileEmail { get; set; }
		public int? FinanceProfileId { get; set; }
		public string FinanceProfileName { get; set; }
		public string FinanceValidatorOneEmail { get; set; }
		public int? FinanceValidatorOneId { get; set; }
		public string FinanceValidatorOneName { get; set; }
		public string FinanceValidatorTowEmail { get; set; }
		public int? FinanceValidatorTowId { get; set; }
		public string FinanceValidatorTowName { get; set; }
		public string HRB { get; set; }
		public string IBAN1 { get; set; }
		public string IBAN2 { get; set; }
		public string IBAN3 { get; set; }
		public string IBAN4 { get; set; }
		public int Id { get; set; }
		public string Manager1 { get; set; }
		public string Manager2 { get; set; }
		public string Manager3 { get; set; }
		public string OrderPrefix { get; set; }
		public string Phone { get; set; }
		public string PostalCode { get; set; }
		public string PurchaseGroupEmail { get; set; }
		public string PurchaseGroupName { get; set; }
		public int? PurchaseId { get; set; }
		public string PurchaseName { get; set; }
		public int? PurchaseProfileId { get; set; }
		public string ReportDefaultLanguage { get; set; }
		public int? ReportDefaultLanguageId { get; set; }
		public string Site { get; set; }
		public string SuperValidatorOneEmail { get; set; }
		public int? SuperValidatorOneId { get; set; }
		public string SuperValidatorOneName { get; set; }
		public string SuperValidatorTowEmail { get; set; }
		public int? SuperValidatorTowId { get; set; }
		public string SuperValidatorTowName { get; set; }
		public string SWIFT_BIC1 { get; set; }
		public string SWIFT_BIC2 { get; set; }
		public string SWIFT_BIC3 { get; set; }
		public string SWIFT_BIC4 { get; set; }
		public string TaxNumberID { get; set; }
		public string VATNumberID { get; set; }


		public CompanyExtensionEntity() { }

		public CompanyExtensionEntity(DataRow dataRow)
		{
			Account1 = (dataRow["Account1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Account1"]);
			Account2 = (dataRow["Account2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Account2"]);
			Account3 = (dataRow["Account3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Account3"]);
			Account4 = (dataRow["Account4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Account4"]);
			Address = (dataRow["Address"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Address"]);
			BankDetails1 = (dataRow["BankDetails1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BankDetails1"]);
			BankDetails2 = (dataRow["BankDetails2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BankDetails2"]);
			BankDetails3 = (dataRow["BankDetails3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BankDetails3"]);
			BankDetails4 = (dataRow["BankDetails4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BankDetails4"]);
			BLZ1 = (dataRow["BLZ1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BLZ1"]);
			BLZ2 = (dataRow["BLZ2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BLZ2"]);
			BLZ3 = (dataRow["BLZ3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BLZ3"]);
			BLZ4 = (dataRow["BLZ4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BLZ4"]);
			City = (dataRow["City"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["City"]);
			CompanyId = Convert.ToInt32(dataRow["CompanyId"]);
			CompanyName = (dataRow["CompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CompanyName"]);
			Country = (dataRow["Country"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Country"]);
			CustomsNumber = (dataRow["CustomsNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomsNumber"]);
			DefaultCurrencyId = (dataRow["DefaultCurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyId"]);
			DefaultCurrencyName = (dataRow["DefaultCurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DefaultCurrencyName"]);
			DeliveryAddress = (dataRow["DeliveryAddress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryAddress"]);
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			FinanceProfileEmail = (dataRow["FinanceProfileEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FinanceProfileEmail"]);
			FinanceProfileId = (dataRow["FinanceProfileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FinanceProfileId"]);
			FinanceProfileName = (dataRow["FinanceProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FinanceProfileName"]);
			FinanceValidatorOneEmail = (dataRow["FinanceValidatorOneEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FinanceValidatorOneEmail"]);
			FinanceValidatorOneId = (dataRow["FinanceValidatorOneId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FinanceValidatorOneId"]);
			FinanceValidatorOneName = (dataRow["FinanceValidatorOneName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FinanceValidatorOneName"]);
			FinanceValidatorTowEmail = (dataRow["FinanceValidatorTowEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FinanceValidatorTowEmail"]);
			FinanceValidatorTowId = (dataRow["FinanceValidatorTowId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FinanceValidatorTowId"]);
			FinanceValidatorTowName = (dataRow["FinanceValidatorTowName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FinanceValidatorTowName"]);
			HRB = (dataRow["HRB"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HRB"]);
			IBAN1 = (dataRow["IBAN1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IBAN1"]);
			IBAN2 = (dataRow["IBAN2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IBAN2"]);
			IBAN3 = (dataRow["IBAN3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IBAN3"]);
			IBAN4 = (dataRow["IBAN4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IBAN4"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Manager1 = (dataRow["Manager1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Manager1"]);
			Manager2 = (dataRow["Manager2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Manager2"]);
			Manager3 = (dataRow["Manager3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Manager3"]);
			OrderPrefix = (dataRow["OrderPrefix"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPrefix"]);
			Phone = (dataRow["Phone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Phone"]);
			PostalCode = (dataRow["PostalCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PostalCode"]);
			PurchaseGroupEmail = (dataRow["PurchaseGroupEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PurchaseGroupEmail"]);
			PurchaseGroupName = (dataRow["PurchaseGroupName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PurchaseGroupName"]);
			PurchaseId = (dataRow["PurchaseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PurchaseId"]);
			PurchaseName = (dataRow["PurchaseName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PurchaseName"]);
			PurchaseProfileId = (dataRow["PurchaseProfileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PurchaseProfileId"]);
			ReportDefaultLanguage = (dataRow["ReportDefaultLanguage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReportDefaultLanguage"]);
			ReportDefaultLanguageId = (dataRow["ReportDefaultLanguageId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ReportDefaultLanguageId"]);
			Site = (dataRow["Site"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Site"]);
			SuperValidatorOneEmail = (dataRow["SuperValidatorOneEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SuperValidatorOneEmail"]);
			SuperValidatorOneId = (dataRow["SuperValidatorOneId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SuperValidatorOneId"]);
			SuperValidatorOneName = (dataRow["SuperValidatorOneName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SuperValidatorOneName"]);
			SuperValidatorTowEmail = (dataRow["SuperValidatorTowEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SuperValidatorTowEmail"]);
			SuperValidatorTowId = (dataRow["SuperValidatorTowId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SuperValidatorTowId"]);
			SuperValidatorTowName = (dataRow["SuperValidatorTowName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SuperValidatorTowName"]);
			SWIFT_BIC1 = (dataRow["SWIFT_BIC1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SWIFT_BIC1"]);
			SWIFT_BIC2 = (dataRow["SWIFT_BIC2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SWIFT_BIC2"]);
			SWIFT_BIC3 = (dataRow["SWIFT_BIC3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SWIFT_BIC3"]);
			SWIFT_BIC4 = (dataRow["SWIFT_BIC4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SWIFT_BIC4"]);
			TaxNumberID = (dataRow["TaxNumberID"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TaxNumberID"]);
			VATNumberID = (dataRow["VATNumberID"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["VATNumberID"]);
		}

		public CompanyExtensionEntity ShallowClone()
		{
			return new CompanyExtensionEntity
			{
				Account1 = Account1,
				Account2 = Account2,
				Account3 = Account3,
				Account4 = Account4,
				Address = Address,
				BankDetails1 = BankDetails1,
				BankDetails2 = BankDetails2,
				BankDetails3 = BankDetails3,
				BankDetails4 = BankDetails4,
				BLZ1 = BLZ1,
				BLZ2 = BLZ2,
				BLZ3 = BLZ3,
				BLZ4 = BLZ4,
				City = City,
				CompanyId = CompanyId,
				CompanyName = CompanyName,
				Country = Country,
				CustomsNumber = CustomsNumber,
				DefaultCurrencyId = DefaultCurrencyId,
				DefaultCurrencyName = DefaultCurrencyName,
				DeliveryAddress = DeliveryAddress,
				Email = Email,
				Fax = Fax,
				FinanceProfileEmail = FinanceProfileEmail,
				FinanceProfileId = FinanceProfileId,
				FinanceProfileName = FinanceProfileName,
				HRB = HRB,
				IBAN1 = IBAN1,
				IBAN2 = IBAN2,
				IBAN3 = IBAN3,
				IBAN4 = IBAN4,
				Id = Id,
				Manager1 = Manager1,
				Manager2 = Manager2,
				Manager3 = Manager3,
				OrderPrefix = OrderPrefix,
				Phone = Phone,
				PostalCode = PostalCode,
				PurchaseGroupEmail = PurchaseGroupEmail,
				PurchaseGroupName = PurchaseGroupName,
				PurchaseId = PurchaseId,
				PurchaseName = PurchaseName,
				PurchaseProfileId = PurchaseProfileId,
				ReportDefaultLanguage = ReportDefaultLanguage,
				ReportDefaultLanguageId = ReportDefaultLanguageId,
				Site = Site,
				SuperValidatorOneEmail = SuperValidatorOneEmail,
				SuperValidatorOneId = SuperValidatorOneId,
				SuperValidatorOneName = SuperValidatorOneName,
				SuperValidatorTowEmail = SuperValidatorTowEmail,
				SuperValidatorTowId = SuperValidatorTowId,
				SuperValidatorTowName = SuperValidatorTowName,
				SWIFT_BIC1 = SWIFT_BIC1,
				SWIFT_BIC2 = SWIFT_BIC2,
				SWIFT_BIC3 = SWIFT_BIC3,
				SWIFT_BIC4 = SWIFT_BIC4,
				TaxNumberID = TaxNumberID,
				VATNumberID = VATNumberID
			};
		}
	}
}

