using System;

namespace Psz.Core.CustomerService
{
	using OfficeOpenXml;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.Reflection;

	public class Module
	{
		public static string Version { get; set; } = "0.9.2.0";
		public static Infrastructure.Services.Reporting.FastReport ReportingService { get; set; }
		public static Reporting.FastReport CS_ReportingService { get; set; }
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }
		public static List<int> LagersWithVersionning { get; set; }
		public static Common.Models.AppSettingsModel.CTS CTS { get; set; }
		public static Common.Models.AppSettingsModel.BSD BSD { get; set; }
		public static Common.Models.AppSettingsModel.MTM MTM { get; set; }
		public static Infrastructure.Services.Files.FilesManager FilesManager { get; set; }
		public static string XLS_FORMAT_NUMBER = "0.0#####";
		public static string XLS_FORMAT_DATE = "dd/MM/yyyy";
		public static List<int> Hauplager { get; set; }
		public static string EmailAppDomaineName { get; set; } = "https://customer-service.";
		public static Common.Models.AppSettingsModel.AppSettingsEDIModel EDISettings { get; set; }
		public static string PurchaseEmailAppDomaineName { get; set; } = "https://purchase.";

		public static void Initiate(string reportTemplatePath, Infrastructure.Services.Email.EmailParamtersModel emailParamters, List<int> lagers, List<int> hauplager, Common.Models.AppSettingsModel.CTS cts, Common.Models.AppSettingsModel.BSD bsd, string filesPhysicalPath,
			string tempFilesPhysicalPath, Infrastructure.Services.FileServer.MinioParamtersModel miniomodel, Common.Models.AppSettingsModel.AppSettingsEDIModel ediSettings, Common.Models.AppSettingsModel.MTM mtm)
		{
			// Report
			ReportingService = new Infrastructure.Services.Reporting.FastReport(reportTemplatePath);
			CS_ReportingService = new Psz.Core.CustomerService.Reporting.FastReport(reportTemplatePath);
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
			EmailAppDomaineName = $"{EmailingService.EmailParamtersModel.AppDomaineProtocol}://customer-service.";
			EDISettings = ediSettings;
			MTM = mtm;
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
		public static string cleanArticleSuffix(this string articlenumber)
		{
			// - 2022-04-26 - Khelil remove only Site Suffixes (TN, AL, DE)
			articlenumber = articlenumber.Trim();
			if(string.IsNullOrWhiteSpace(articlenumber) || articlenumber.Length < 2)
			{
				return articlenumber;
			}
			// -
			if(articlenumber.ToLower().EndsWith("al") || articlenumber.ToLower().EndsWith("tn") || articlenumber.ToLower().EndsWith("de"))
			{
				return articlenumber.Substring(0, articlenumber.Length - 2);
			}
			// -
			return articlenumber;
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
		public static string TrimTrailingZeros(this string value)
		{
			return value.TrimEnd('0').TrimEnd('.');
		}
		public static string FormatDecimal(this decimal number, int decimalPlaces)
		{
			string format = $"N{decimalPlaces}";
			CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
			string formattedNumber = number.ToString(format, culture);

			return formattedNumber;
		}
		public static string FormatDecimal(this string number, int decimalPlaces)
		{
			if(decimal.TryParse(number, out var _d))
			{
				string format = $"N{decimalPlaces}";
				CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
				string formattedNumber = _d.ToString(format, culture);

				return formattedNumber;
			}

			return "";
		}
		public static bool dateIsInPast(this DateTime date, bool compareByDay = true)
		{
			if(date < (compareByDay ? DateTime.Today : DateTime.Now))
				return true;
			return false;
		}
	}
}
