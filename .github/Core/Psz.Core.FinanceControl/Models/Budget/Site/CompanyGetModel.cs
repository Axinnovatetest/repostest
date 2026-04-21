using System;

namespace Psz.Core.FinanceControl.Models.Budget.Site
{
	public class CompanyGetModel
	{
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public DateTime CreateTime { get; set; }
		public int CreateUserId { get; set; }
		public string Description { get; set; }
		public string DirectorEmail { get; set; }
		public int? DirectorId { get; set; }
		public string DirectorName { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public int Id { get; set; }
		public bool? IsActive { get; set; }
		public string LagalName { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public byte[] Logo { get; set; }
		public string LogoExtension { get; set; }
		public string Name { get; set; }
		public string PostalCode { get; set; }
		public string Telephone { get; set; }
		public string Telephone2 { get; set; }
		public string Type { get; set; }

		// - FNC Extension
		public int ExtensionId { get; set; }
		public string PurchaseGroupEmail { get; set; }
		public string PurchaseGroupName { get; set; }
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
		public int CompanyId { get; set; }
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
		public int? PurchaseId { get; set; }
		public string PurchaseName { get; set; }
		public string SWIFT_BIC1 { get; set; }
		public string SWIFT_BIC2 { get; set; }
		public string SWIFT_BIC3 { get; set; }
		public string SWIFT_BIC4 { get; set; }
		public string TaxNumberID { get; set; }
		public string VATNumberID { get; set; }

		public CompanyGetModel(Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity extensionEntity)
		{
			Address = companyEntity?.Address;
			Address2 = companyEntity?.Address2;
			City = companyEntity?.City;
			Country = companyEntity?.Country;
			CreateTime = companyEntity?.CreateTime ?? DateTime.MinValue;
			CreateUserId = companyEntity?.CreateUserId ?? -1;
			Description = companyEntity?.Description;
			DirectorEmail = companyEntity?.DirectorEmail;
			DirectorId = companyEntity?.DirectorId;
			DirectorName = companyEntity?.DirectorName;
			Email = companyEntity?.Email;
			Fax = companyEntity?.Fax;
			Id = companyEntity?.Id ?? -1;
			IsActive = companyEntity?.IsActive;
			LagalName = companyEntity?.LagalName;
			LastUpdateTime = companyEntity?.LastUpdateTime;
			LastUpdateUserId = companyEntity?.LastUpdateUserId;
			Logo = companyEntity?.Logo;
			LogoExtension = companyEntity?.LogoExtension;
			Name = companyEntity?.Name;
			PostalCode = companyEntity?.PostalCode;
			Telephone = companyEntity?.Telephone;
			Telephone2 = companyEntity?.Telephone2;
			Type = companyEntity?.Type;


			ExtensionId = extensionEntity?.Id ?? -1;
			PurchaseGroupEmail = extensionEntity?.PurchaseGroupEmail;
			PurchaseGroupName = extensionEntity?.PurchaseGroupName;
			PurchaseProfileId = extensionEntity?.PurchaseProfileId;
			OrderPrefix = extensionEntity?.OrderPrefix;


			// - 
			Account1 = extensionEntity?.Account1;
			Account2 = extensionEntity?.Account2;
			Account3 = extensionEntity?.Account3;
			Account4 = extensionEntity?.Account4;
			BankDetails1 = extensionEntity?.BankDetails1;
			BankDetails2 = extensionEntity?.BankDetails2;
			BankDetails3 = extensionEntity?.BankDetails3;
			BankDetails4 = extensionEntity?.BankDetails4;
			BLZ1 = extensionEntity?.BLZ1;
			BLZ2 = extensionEntity?.BLZ2;
			BLZ3 = extensionEntity?.BLZ3;
			BLZ4 = extensionEntity?.BLZ4;
			CompanyName = extensionEntity?.CompanyName;
			CustomsNumber = extensionEntity?.CustomsNumber;
			DefaultCurrencyName = extensionEntity?.DefaultCurrencyName;
			DeliveryAddress = extensionEntity?.DeliveryAddress;
			HRB = extensionEntity?.HRB;
			IBAN1 = extensionEntity?.IBAN1;
			IBAN2 = extensionEntity?.IBAN2;
			IBAN3 = extensionEntity?.IBAN3;
			IBAN4 = extensionEntity?.IBAN4;
			Manager1 = extensionEntity?.Manager1;
			Manager2 = extensionEntity?.Manager2;
			Manager3 = extensionEntity?.Manager3;
			Phone = extensionEntity?.Phone;
			PurchaseName = extensionEntity?.PurchaseName;
			SWIFT_BIC1 = extensionEntity?.SWIFT_BIC1;
			SWIFT_BIC2 = extensionEntity?.SWIFT_BIC2;
			SWIFT_BIC3 = extensionEntity?.SWIFT_BIC3;
			SWIFT_BIC4 = extensionEntity?.SWIFT_BIC4;
			TaxNumberID = extensionEntity?.TaxNumberID;
			VATNumberID = extensionEntity?.VATNumberID;
		}
		public Infrastructure.Data.Entities.Tables.STG.CompanyEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity
			{
				Address = Address,
				Address2 = Address2,
				City = City,
				Country = Country,
				CreateTime = CreateTime,
				CreateUserId = CreateUserId,
				Description = Description,
				DirectorEmail = DirectorEmail,
				DirectorId = DirectorId,
				DirectorName = DirectorName,
				Email = Email,
				Fax = Fax,
				Id = Id,
				IsActive = IsActive,
				LagalName = LagalName,
				LastUpdateTime = LastUpdateTime,
				LastUpdateUserId = LastUpdateUserId,
				Logo = Logo,
				LogoExtension = LogoExtension,
				Name = Name,
				PostalCode = PostalCode,
				Telephone = Telephone,
				Telephone2 = Telephone2,
				Type = Type,
			};
		}
		public Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity ToCompanyExtension()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity
			{
				Id = ExtensionId,
				CompanyId = Id,
				CompanyName = Name,
				OrderPrefix = OrderPrefix,
				PurchaseGroupEmail = PurchaseGroupEmail,
				PurchaseGroupName = PurchaseGroupName,
				PurchaseProfileId = PurchaseProfileId,

				// - 
				Account1 = Account1,
				Account2 = Account2,
				Account3 = Account3,
				Account4 = Account4,
				BankDetails1 = BankDetails1,
				BankDetails2 = BankDetails2,
				BankDetails3 = BankDetails3,
				BankDetails4 = BankDetails4,
				BLZ1 = BLZ1,
				BLZ2 = BLZ2,
				BLZ3 = BLZ3,
				BLZ4 = BLZ4,
				CustomsNumber = CustomsNumber,
				DefaultCurrencyName = DefaultCurrencyName,
				DeliveryAddress = DeliveryAddress,
				HRB = HRB,
				IBAN1 = IBAN1,
				IBAN2 = IBAN2,
				IBAN3 = IBAN3,
				IBAN4 = IBAN4,
				Manager1 = Manager1,
				Manager2 = Manager2,
				Manager3 = Manager3,
				Phone = Phone,
				PurchaseName = PurchaseName,
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
