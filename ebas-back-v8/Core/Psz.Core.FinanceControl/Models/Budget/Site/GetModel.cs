using Infrastructure.Data.Entities.Tables.FNC;
using Infrastructure.Data.Entities.Tables.STG;
using System;

namespace Psz.Core.FinanceControl.Models.Budget.Site
{
	public class GetModel
	{
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public DateTime CreateTime { get; set; }
		public int CreateUserId { get; set; }
		public string Description { get; set; }
		public string DirectorName { get; set; }
		public string DirectorEmail { get; set; }
		public int? DirectorId { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public int CompanyId { get; set; }
		public bool? IsActive { get; set; }
		public string LagalName { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public byte[] Logo { get; set; }
		public string LogoPath { get; set; }
		public string Name { get; set; }
		public string PostalCode { get; set; }
		public string Telephone { get; set; }
		public string Telephone2 { get; set; }
		public string Type { get; set; }

		// - FNC Extension
		public int Id { get; set; }
		public string PurchaseGroupEmail { get; set; }
		public string PurchaseGroupName { get; set; }
		public int? PurchaseId { get; set; }
		public string PurchaseName { get; set; }
		public int? PurchaseProfileId { get; set; }
		public string OrderPrefix { get; set; }

		// - 
		public string Account1 { get; set; }
		public string Account2 { get; set; }
		public string Account3 { get; set; }
		public string Account4 { get; set; }
		public string BankDetails1 { get; set; }
		public string BankDetails2 { get; set; }
		public string BankDetails3 { get; set; }
		public string BankDetails4 { get; set; }
		public string BLZ1 { get; set; }
		public string BLZ2 { get; set; }
		public string BLZ3 { get; set; }
		public string BLZ4 { get; set; }
		public string CompanyName { get; set; }
		public string CustomsNumber { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public string DeliveryAddress { get; set; }
		public string HRB { get; set; }
		public string IBAN1 { get; set; }
		public string IBAN2 { get; set; }
		public string IBAN3 { get; set; }
		public string IBAN4 { get; set; }
		public string Manager1 { get; set; }
		public string Manager2 { get; set; }
		public string Manager3 { get; set; }
		public string Phone { get; set; }
		public string SWIFT_BIC1 { get; set; }
		public string SWIFT_BIC2 { get; set; }
		public string SWIFT_BIC3 { get; set; }
		public string SWIFT_BIC4 { get; set; }
		public string TaxNumberID { get; set; }
		public string VATNumberID { get; set; }

		// -
		public string ReportDefaultLanguage { get; set; }
		public int? ReportDefaultLanguageId { get; set; }

		// -
		public string FinanceProfileEmail { get; set; }
		public string FinanceProfileName { get; set; }
		public int? FinanceProfileId { get; set; }
		// -
		public int? SuperValidatorOneId { get; set; }
		public int? SuperValidatorTowId { get; set; }

		public int? FinanceValidatorOneId { get; set; }
		public int? FinanceValidatorTowId { get; set; }
		public GetModel(CompanyEntity companyEntity, CompanyExtensionEntity companyExtensionEntity)
		{
			Address = companyEntity?.Address;
			Address2 = companyEntity?.Address2;
			City = companyEntity?.City;
			Country = companyEntity?.Country;
			CreateTime = companyEntity?.CreateTime ?? DateTime.MinValue;
			CreateUserId = companyEntity?.CreateUserId ?? -1;
			Description = companyEntity?.Description;
			DirectorName = companyEntity?.DirectorName;
			DirectorEmail = companyEntity?.DirectorEmail;
			DirectorId = companyEntity?.DirectorId;
			Email = companyEntity?.Email;
			Fax = companyEntity?.Fax;
			CompanyId = companyEntity?.Id ?? -1;
			IsActive = companyEntity?.IsActive;
			LagalName = companyEntity?.LagalName;
			LastUpdateTime = companyEntity?.LastUpdateTime;
			LastUpdateUserId = companyEntity?.LastUpdateUserId;
			Logo = companyEntity?.Logo;
			LogoPath = companyEntity?.LogoExtension;
			Name = companyEntity?.Name;
			PostalCode = companyEntity?.PostalCode;
			Telephone = companyEntity?.Telephone;
			Telephone2 = companyEntity?.Telephone2;
			Type = companyEntity?.Type;

			// - FNC Extension
			Id = companyExtensionEntity?.Id ?? -1;
			PurchaseGroupEmail = companyExtensionEntity?.PurchaseGroupEmail;
			PurchaseGroupName = companyExtensionEntity?.PurchaseGroupName;
			PurchaseId = companyExtensionEntity?.PurchaseProfileId;
			PurchaseName = companyExtensionEntity?.PurchaseName;
			PurchaseProfileId = companyExtensionEntity?.PurchaseProfileId;
			OrderPrefix = companyExtensionEntity?.OrderPrefix;

			// - 
			Account1 = companyExtensionEntity?.Account1;
			Account2 = companyExtensionEntity?.Account2;
			Account3 = companyExtensionEntity?.Account3;
			Account4 = companyExtensionEntity?.Account4;
			BankDetails1 = companyExtensionEntity?.BankDetails1;
			BankDetails2 = companyExtensionEntity?.BankDetails2;
			BankDetails3 = companyExtensionEntity?.BankDetails3;
			BankDetails4 = companyExtensionEntity?.BankDetails4;
			BLZ1 = companyExtensionEntity?.BLZ1;
			BLZ2 = companyExtensionEntity?.BLZ2;
			BLZ3 = companyExtensionEntity?.BLZ3;
			BLZ4 = companyExtensionEntity?.BLZ4;
			CompanyName = companyExtensionEntity?.CompanyName;
			CustomsNumber = companyExtensionEntity?.CustomsNumber;
			DefaultCurrencyName = companyExtensionEntity?.DefaultCurrencyName;
			DeliveryAddress = companyExtensionEntity?.DeliveryAddress;
			HRB = companyExtensionEntity?.HRB;
			IBAN1 = companyExtensionEntity?.IBAN1;
			IBAN2 = companyExtensionEntity?.IBAN2;
			IBAN3 = companyExtensionEntity?.IBAN3;
			IBAN4 = companyExtensionEntity?.IBAN4;
			Manager1 = companyExtensionEntity?.Manager1;
			Manager2 = companyExtensionEntity?.Manager2;
			Manager3 = companyExtensionEntity?.Manager3;
			Phone = companyExtensionEntity?.Phone;
			PurchaseName = companyExtensionEntity?.PurchaseName;
			SWIFT_BIC1 = companyExtensionEntity?.SWIFT_BIC1;
			SWIFT_BIC2 = companyExtensionEntity?.SWIFT_BIC2;
			SWIFT_BIC3 = companyExtensionEntity?.SWIFT_BIC3;
			SWIFT_BIC4 = companyExtensionEntity?.SWIFT_BIC4;
			TaxNumberID = companyExtensionEntity?.TaxNumberID;
			VATNumberID = companyExtensionEntity?.VATNumberID;
			//-
			ReportDefaultLanguage = companyExtensionEntity?.ReportDefaultLanguage;
			ReportDefaultLanguageId = companyExtensionEntity?.ReportDefaultLanguageId;
			// -
			FinanceProfileEmail = companyExtensionEntity?.FinanceProfileEmail;
			FinanceProfileName = companyExtensionEntity?.FinanceProfileName;
			FinanceProfileId = companyExtensionEntity?.FinanceProfileId;
			// - 
			SuperValidatorOneId = companyExtensionEntity?.SuperValidatorOneId;
			SuperValidatorTowId = companyExtensionEntity?.SuperValidatorTowId;
			// -
			FinanceValidatorOneId = companyExtensionEntity?.FinanceValidatorOneId;
			FinanceValidatorTowId = companyExtensionEntity?.FinanceValidatorTowId;
		}
	}
}
