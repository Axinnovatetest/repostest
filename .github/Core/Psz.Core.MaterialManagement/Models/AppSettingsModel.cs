namespace Psz.Core.MaterialManagement.Models
{
	public class AppSettingsModel
	{
		public class MTM
		{
			public List<int> Lagerort { get; set; }
			public string ReportTemplateFolder { get; set; }
			public List<int> SpecialUserNummers { get; set; }
			public string ABDestinationEmailAddress { get; set; }
			public int RahmenConsumptionNotificationThreshold { get; set; }
		}
		public class BSD
		{
			public List<string> EngineeringMailingListAddresses { get; set; }
			public List<string> QualityManagementMailingListAddresses { get; set; }
			public string BomImportFileTemplatePath { get; set; }
			public string ArticlesStatisticsVKUpdate { get; set; }
			public string ArticlesStatisticsSuperbillArticle { get; set; }
			public string ArticlesStatisticsSuperbillFertigung { get; set; }
			public int SpecialCustomerNumberStart { get; set; }
			public int SpecialSupplierNumberStart { get; set; }
			public List<int> ProductionLagerIds { get; set; }
			public string DeliveryNoteFilesPath { get; set; }
			public string WMSArticleDATPath { get; set; }
			public List<int> ALVirtualBestandArticleIds { get; set; }
		}
	}
}
