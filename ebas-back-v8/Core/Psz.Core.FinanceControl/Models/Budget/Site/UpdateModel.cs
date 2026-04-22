namespace Psz.Core.FinanceControl.Models.Budget.Site
{
	public class UpdateModel
	{
		public int CompanyId { get; set; }
		public string CompanyName { get; set; }
		public int? DirectorId { get; set; }
		public string DirectorEmail { get; set; }
		public string DirectorName { get; set; }

		// - FNC Extension
		public int Id { get; set; }
		public string PurchaseGroupEmail { get; set; }
		public string PurchaseGroupName { get; set; }
		public int? PurchaseProfileId { get; set; }
		public string OrderPrefix { get; set; }
		public string ReportDefaultLanguage { get; set; }
		public int? ReportDefaultLanguageId { get; set; }

		// -
		public string FinanceProfileEmail { get; set; }
		public string FinanceProfileName { get; set; }
		public int? FinanceProfileId { get; set; }
		//- souilmi 11/06/2024
		public int? SuperValidatorOneId { get; set; }
		public string SuperValidatorOneName { get; set; }
		public string SuperValidatorOneEmail { get; set; }
		//
		public int? SuperValidatorTowId { get; set; }
		public string SuperValidatorTowName { get; set; }
		public string SuperValidatorTowEmail { get; set; }


		// -
		public string FinanceValidatorOneEmail { get; set; }
		public string FinanceValidatorOneName { get; set; }
		public int? FinanceValidatorOneId { get; set; }
		public string FinanceValidatorTowEmail { get; set; }
		public string FinanceValidatorTowName { get; set; }
		public int? FinanceValidatorTowId { get; set; }

		public Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity ToCompanyExtension()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity
			{
				Id = Id,
				CompanyId = CompanyId,
				CompanyName = CompanyName,
				PurchaseGroupEmail = PurchaseGroupEmail,
				PurchaseGroupName = PurchaseGroupName,
				PurchaseProfileId = PurchaseProfileId,
				OrderPrefix = OrderPrefix,
				ReportDefaultLanguage = ReportDefaultLanguage,
				ReportDefaultLanguageId = ReportDefaultLanguageId,
				FinanceProfileEmail = FinanceProfileEmail,
				FinanceProfileId = FinanceProfileId,
				FinanceProfileName = FinanceProfileName,
				//
				SuperValidatorOneId = SuperValidatorOneId,
				SuperValidatorOneName = SuperValidatorOneName,
				SuperValidatorOneEmail = SuperValidatorOneEmail,
				//
				SuperValidatorTowId = SuperValidatorTowId,
				SuperValidatorTowName = SuperValidatorTowName,
				SuperValidatorTowEmail = SuperValidatorTowEmail,
				FinanceValidatorOneId = FinanceValidatorOneId,
				FinanceValidatorOneName = FinanceValidatorOneName,
				FinanceValidatorOneEmail = FinanceValidatorOneEmail,
				FinanceValidatorTowId = FinanceValidatorTowId,
				FinanceValidatorTowName = FinanceValidatorTowName,
				FinanceValidatorTowEmail = FinanceValidatorTowEmail,
			};
		}
		public Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity ToCompanyExtension(
			Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity companyExtensionEntity)
		{
			if(companyExtensionEntity == null)
				companyExtensionEntity = new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity();

			companyExtensionEntity.CompanyId = CompanyId;
			companyExtensionEntity.CompanyName = CompanyName;
			companyExtensionEntity.PurchaseGroupEmail = PurchaseGroupEmail;
			companyExtensionEntity.PurchaseGroupName = PurchaseGroupName;
			companyExtensionEntity.PurchaseProfileId = PurchaseProfileId;
			companyExtensionEntity.OrderPrefix = OrderPrefix;
			companyExtensionEntity.ReportDefaultLanguage = ReportDefaultLanguage;
			companyExtensionEntity.ReportDefaultLanguageId = ReportDefaultLanguageId;
			companyExtensionEntity.FinanceProfileEmail = FinanceProfileEmail;
			companyExtensionEntity.FinanceProfileId = FinanceProfileId;
			companyExtensionEntity.FinanceProfileName = FinanceProfileName;
			//
			companyExtensionEntity.SuperValidatorOneId = SuperValidatorOneId;
			companyExtensionEntity.SuperValidatorOneName = SuperValidatorOneName;
			companyExtensionEntity.SuperValidatorOneEmail = SuperValidatorOneEmail;
			//
			companyExtensionEntity.SuperValidatorTowId = SuperValidatorTowId;
			companyExtensionEntity.SuperValidatorTowName = SuperValidatorTowName;
			companyExtensionEntity.SuperValidatorTowEmail = SuperValidatorTowEmail;

			companyExtensionEntity.FinanceValidatorOneId = FinanceValidatorOneId;
			companyExtensionEntity.FinanceValidatorOneName = FinanceValidatorOneName;
			companyExtensionEntity.FinanceValidatorOneEmail = FinanceValidatorOneEmail;
			companyExtensionEntity.FinanceValidatorTowId = FinanceValidatorTowId;
			companyExtensionEntity.FinanceValidatorTowName = FinanceValidatorTowName;
			companyExtensionEntity.FinanceValidatorTowEmail = FinanceValidatorTowEmail;
			return companyExtensionEntity;
		}
	}
}
