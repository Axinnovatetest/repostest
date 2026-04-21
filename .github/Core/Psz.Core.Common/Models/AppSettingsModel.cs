using System;
using System.Collections.Generic;

namespace Psz.Core.Common.Models
{
	public class AppSettingsModel
	{
		public class AppSettingsEDIModel
		{
			public string DesadvDirectoryName { get; set; }
			public DelforSettings Delfor { get; set; }
			public EdiSettings Edi { get; set; }
			//public class EdiPlatformSettings
			//{
			//	public DateTime TimeSpan { get; set; }
			//}
			public class DelforSettings
			{
				public string NewDirectoryName { get; set; }
				public string ArchiveDirectoryName { get; set; }
				public string ErrorDirectoryName { get; set; }
				public string ProcessedDirectoryName { get; set; }
			}
			public class EdiSettings
			{
				public List<string> TrimSatrtZeroSenderIds { get; set; }
			}
		}
		public class BSD
		{
			public List<string> EngineeringMailingListAddresses { get; set; }
			public List<string> QualityManagementMailingListAddresses { get; set; }
			public string BomImportFileTemplatePath { get; set; }
			public int SpecialCustomerNumberStart { get; set; }
			public int SpecialSupplierNumberStart { get; set; }
			public string ArticlesStatisticsVKUpdate { get; set; }
			public string ArticlesStatisticsSuperbillArticle { get; set; }
			public string ArticlesStatisticsSuperbillFertigung { get; set; }
			public string WMSArticleDATPath { get; set; }
			public List<int> ProductionLagerIds { get; set; }
			public List<int> TechnicArticleIds { get; set; }
			public List<int> ALVirtualBestandArticleIds { get; set; }
			public string EKForecastUpdateFrequency { get; set; }
		}
		public class CTS
		{
			public List<string> ReparaturArticles { get; set; }
			public List<int> Hauplager { get; set; }
			public List<OpenCustomer> OpenCsCustomers { get; set; }
			public List<int> LagersWithoutPPS { get; set; }
			public string DeliveryNoteFilesPath { get; set; }
			// 2024-01-25 - Khelil change H1 to 41 days - public int ProductionFrozenZoneKWCount { get; set; } = 5;
			//rahmen
			public int raMaxCurrentValue { get; set; }
			public int raMinNewValue { get; set; }
			public int raMaxNewValue { get; set; }
			//gutshrift
			public int gsMaxCurrentValue { get; set; }
			public int gsMinNewValue { get; set; }
			public int gsMaxNewValue { get; set; }
			//AB
			public int abMaxCurrentValue { get; set; }
			public int abMinNewValue { get; set; }
			public int abMaxNewValue { get; set; }
			//LS
			public int lsMaxCurrentValue { get; set; }
			public int lsMinNewValue { get; set; }
			public int lsMaxNewValue { get; set; }
			//Rechnung
			public int reMaxCurrentValue { get; set; }
			public int reMinNewValue { get; set; }
			public int reMaxNewValue { get; set; }
			//Forcast
			public int bvMaxCurrentValue { get; set; }
			public int bvMinNewValue { get; set; }
			public int bvMaxNewValue { get; set; }
			public int Delta { get; set; }

			public FooterFactoring FooterFactoring { get; set; }
			public FooterFactoring FooterNotFactoring { get; set; }
			public FAHorizons FAHorizons { get; set; }
			public string Logo { get; set; }
			public string Top100Description { get; set; }
			public string Top100Logo { get; set; }
			public string Top100_2026Logo { get; set; }
			public List<string> InvoiceAdminEmails { get; set; }
			public string InvoiceSenderEmail { get; set; }
			public int ConfirmationToProductionDistanceInDays { get; set; }
			public List<string> IgnoreFaCreateNotificationEmails { get; set; }
		}
		public class OpenCustomer
		{
			public int Number { get; set; }
			public string Name { get; set; }
		}
		public class FooterFactoring
		{
			public string Bank { get; set; }
			public string Konto { get; set; }
			public string BLZ { get; set; }
			public string IBAN { get; set; }
			public string SWOFT_BIC { get; set; }
		}
		public class FAHorizons
		{
			public int H1LengthInDays { get; set; } = 41; // 2024-01-25 - Khelil change H1 to 41 days - // public int H1KWLength { get; set; }
			public int H2KWLength { get; set; }
		}

		public class FNCAppSettings
		{
			public List<int> BookingLager { get; set; }
			public List<string> LeasingAllowedCompanies { get; set; }
			public DateTime LastDayOfOrder { get; set; } = new DateTime(DateTime.Today.Year, 12, 17);
		}
		public class MTM
		{
			public List<int> Lagerort { get; set; }
			public string ReportTemplateFolder { get; set; }
			public int RahmenConsumptionNotificationThreshold { get; set; }
		}
		public class LGT
		{
			public List<int> CC_LagerOrt { get; set; }
			public List<int> tbl_Planung_gestartet_Lagerort_ID { get; set; }
			public List<LGTList> LGTList { get; set; }
			public int ArticleCustomsMaxRenewalDays { get; set; }
			public List<string> ArticleCustomsMaxRenewalDaysEmails { get; set; }
			public string FormatSoftwareIncomingXmlFolder { get; set; }
			public string FormatSoftwareIncomingXmlFolderExport { get; set; }
			public string FormatSoftwareIncomingXmlFolderImport { get; set; }
		}
		public class LGTList
		{
			public string Lager { get; set; }
			public int Lager_Id { get; set; }
			public int Lager_P_Id { get; set; }
			public List<int> Lagerorte_Lagerort_id { get; set; }
			public List<int> bestellte_Artikel_Lagerort_id { get; set; }
		}
		public class ServerPath
		{
			public string BasePath { get; set; }
			public string SuppliersFilesBasePath { get; set; }
		}
		public class Impersonate
		{
			public string ImpersonateUsername { get; set; }
			public string ImpersonatePassword { get; set; }
			public string ImpersonateDomain { get; set; }
		}

		public class HangfireSettings
		{
			public string Path { get; set; }
			public string User { get; set; }
			public string Password { get; set; }
		}
	}
}
