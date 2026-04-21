using OfficeOpenXml;
using System.ComponentModel;
using System.Reflection;

namespace Psz.Core.CRP
{
	public class Module
	{
		public static string Version { get; set; } = "0.9.2.0";
		public static Core.CRP.Reporting.FastReport ReportingService { get; set; }
		public static Reporting.FastReport CRP_ReportingService { get; set; }
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }
		public static List<int> LagersWithVersionning { get; set; }
		public static Common.Models.AppSettingsModel.CTS CTS { get; set; }
		public static Common.Models.AppSettingsModel.BSD BSD { get; set; }
		public static Common.Models.AppSettingsModel.MTM MTM { get; set; }
		public static Infrastructure.Services.Files.FilesManager FilesManager { get; set; }
		public static string XLS_FORMAT_NUMBER = "0.0#####";
		public static string XLS_FORMAT_DATE = "dd/MM/yyyy";
		public static List<int> Hauplager { get; set; }
		public static string EmailAppDomaineName { get; set; } = "https://capacity-requirement-planning.";

		public static Common.Models.AppSettingsModel.AppSettingsEDIModel EDISettings { get; set; }
		public static int ID_ESD_logo { get; set; }
		public static int CRPAgentsRefreshThreshold { get; set; }

		public static void Initiate(string reportTemplatePath, Infrastructure.Services.Email.EmailParamtersModel emailParamters, List<int> lagers, List<int> hauplager, Common.Models.AppSettingsModel.CTS cts, Common.Models.AppSettingsModel.BSD bsd, string filesPhysicalPath,
			string tempFilesPhysicalPath, Infrastructure.Services.FileServer.MinioParamtersModel miniomodel, Common.Models.AppSettingsModel.AppSettingsEDIModel ediSettings, Common.Models.AppSettingsModel.MTM mtm, int id_esd_logo, int cRPAgentsRefreshThreshold)
		{
			// Report
			ReportingService = new Core.CRP.Reporting.FastReport(reportTemplatePath);
			CRP_ReportingService = new Psz.Core.CRP.Reporting.FastReport(reportTemplatePath);
			Infrastructure.Services.Reporting.ReportGenerator.SetTemplatesPath(reportTemplatePath);
			// Email
			EmailingService = new Infrastructure.Services.Email.MailKit(); // - new Infrastructure.Services.Email.NetMail();
			EmailingService.InitiateEmailSender(emailParamters);
			LagersWithVersionning = lagers;
			CTS = cts;
			CTS.InvoiceSenderEmail = string.IsNullOrWhiteSpace(CTS.InvoiceSenderEmail) ? "no-reply@psz-electronic.com" : CTS.InvoiceSenderEmail;
			BSD = bsd;
			initFilesManager(filesPhysicalPath, tempFilesPhysicalPath);
			Hauplager = hauplager;
			EmailAppDomaineName = $"{EmailingService.EmailParamtersModel.AppDomaineProtocolSecured}://capacity-requirement-planning.";
			EDISettings = ediSettings;
			MTM = mtm;
			ID_ESD_logo = id_esd_logo;
			CRPAgentsRefreshThreshold = cRPAgentsRefreshThreshold;
		}
		private static void initFilesManager(string filesPhysicalPath, string tempfilesPhysicalPath)
		{
			FilesManager = new Infrastructure.Services.Files.FilesManager(filesPhysicalPath, tempfilesPhysicalPath);
		}
		// Use This when switching CTS to Minio
		private static void initFilesManagerWithMinio(string filesPhysicalPath, string tempfilesPhysicalPath, Infrastructure.Services.FileServer.MinioParamtersModel miniomodel)
		{
			FilesManager = new Infrastructure.Services.Files.FilesManager(filesPhysicalPath, tempfilesPhysicalPath, miniomodel.Minioaccesskey, miniomodel.Miniosecretkey, miniomodel.Minioendpoint, miniomodel.Miniobucket);
		}
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
		public static bool dateIsInPast(this DateTime date, bool compareByDay = true)
		{
			if(date < (compareByDay ? DateTime.Today : DateTime.Now))
				return true;
			return false;
		}
		internal static string getCellValue(this ExcelRange cell)
		{
			var val = cell.Value;
			if(val == null)
			{
				return "";
			}
			var dateLong = long.TryParse(val.ToString(), out var l) ? l : 0;
			if(cell.Style.Numberformat.Format == "dd\\.mm\\.yyyy;@" && dateLong != 0)
				return DateTime.FromOADate(long.Parse(val.ToString())).ToString("dd.MM.yyyy");
			else
				return val.ToString();
		}
		public static bool StringIsNullOrEmptyOrWhiteSpaces(this string value)
		{
			if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
				return true;
			else
				return false;
		}
	}
}