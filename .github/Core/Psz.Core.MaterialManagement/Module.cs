using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Psz.Core.MaterialManagement
{
	public class Module
	{
		public static Infrastructure.Services.Reporting.FastReport ReportingService { get; set; }
		public static Infrastructure.Services.Reporting.IText iTextReportingService { get; set; }
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }
		public static Infrastructure.Services.Files.FilesManager FilesManager { get; set; }
		public static string EmailAppDomaineName { get; set; } = "https://material-management.";
		public static string PurchaseEmailAppDomaineName { get; set; } = "https://purchase.";
		public static List<int> LagerortIds { get; set; }
		public static string XLS_FORMAT_NUMBER = "0.0#####";
		public static string XLS_FORMAT_DATE = "dd/MM/yyyy";
		public static string BestellungTemplateFolder { get; set; }
		public static string ReportsTemplatePath { get; set; }
		public static List<int> ALVirtualBestandArticleIds { get; set; }
		public static Models.AppSettingsModel.MTM ModuleSettings { get; set; }
		public static void Initiate(Infrastructure.Services.Email.EmailParamtersModel emailParamters, Models.AppSettingsModel.MTM appSettings,
			string reportsTemplatePath, Models.AppSettingsModel.BSD appSettingsBSD, string filesPhysicalPath, string tempFilesPhysicalPath,
			Infrastructure.Services.FileServer.MinioParamtersModel miniomodel
			)
		{
			// Report
			ReportingService = new Infrastructure.Services.Reporting.FastReport(reportsTemplatePath);
			Infrastructure.Services.Reporting.ReportGenerator.SetTemplatesPath(reportsTemplatePath);

			// Email
			EmailingService = new Infrastructure.Services.Email.MailKit();
			EmailingService.InitiateEmailSender(emailParamters);
			LagerortIds = appSettings.Lagerort;
			BestellungTemplateFolder = reportsTemplatePath;
			ReportsTemplatePath = reportsTemplatePath;
			ALVirtualBestandArticleIds = appSettingsBSD.ALVirtualBestandArticleIds;

			// -
			FilesManager = new Infrastructure.Services.Files.FilesManager(filesPhysicalPath, tempFilesPhysicalPath);
			// uncomment when switching MTM to Minio
			//FilesManager = new Infrastructure.Services.Files.FilesManager(filesPhysicalPath, tempFilesPhysicalPath, miniomodel.Minioaccesskey, miniomodel.Miniosecretkey, miniomodel.Minioendpoint, miniomodel.Miniobucket, miniomodel.MinioSubbucket);

			ModuleSettings = appSettings;

		}

		public static List<ArticleAndFaultyWeek> faultyArticles { get; set; }
		public static DateTime FetchedDataDate { get; set; }
	}

	public static class ExtensionsClass
	{
		public static string GetDescription(this Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if(name != null)
			{
				FieldInfo field = type.GetField(name);
				if(field != null)
				{
					DescriptionAttribute attr =
						   Attribute.GetCustomAttribute(field,
							 typeof(DescriptionAttribute)) as DescriptionAttribute;
					if(attr != null)
					{
						return attr.Description;
					}
				}
			}
			return null;
		}
		public static string FormatDecimal(this decimal number, int decimalPlaces)
		{
			string format = $"N{decimalPlaces}";
			CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
			string formattedNumber = number.ToString(format, culture);

			return formattedNumber;
		}
	}
}
