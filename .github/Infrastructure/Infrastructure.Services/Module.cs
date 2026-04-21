using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Infrastructure.Services
{
	public class Module
	{
		public static string PurchaseEmailAppDomaineName { get; set; } = "https://purchase.";
		public static Email.MailKit EmailingService { get; set; }
		public static string EmailAppDomaineName { get; set; } = "https://capacity-requirement-planning.";
		public static string INSEmailAppDomaineName { get; set; } = "https://customer-service.";
		public static string MTMEmailAppDomaineName { get; set; } = "https://material-management.";
		public static int H1LengthInDays { get; set; }
		public static Infrastructure.Services.Files.FilesManager FilesManager { get; set; }
		private static void initFilesManager(string filesPhysicalPath, string tempfilesPhysicalPath, string ImpersonateUserName = null, string ImpresonatePassword = null, string ImpersonateDomain = null)
		{
			FilesManager = new Infrastructure.Services.Files.FilesManager(filesPhysicalPath, tempfilesPhysicalPath, ImpersonateUserName, ImpresonatePassword, ImpersonateDomain);
		}
		public static void Initiate(string filesPhysicalPath, string tempFilesPhysicalPath, Email.EmailParamtersModel emailParamters, int h1LengthInDays, string ImpersonateUserName = null, string ImpresonatePassword = null, string ImpersonateDomain = null)
		{
			initFilesManager(filesPhysicalPath, tempFilesPhysicalPath, ImpersonateUserName, ImpresonatePassword, ImpersonateDomain);
			EmailingService = new Infrastructure.Services.Email.MailKit();
			EmailingService.InitiateEmailSender(emailParamters);
			H1LengthInDays = h1LengthInDays;
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
		public static bool ContainsAny(this string haystack, params string[] needles)
		{
			foreach(string needle in needles)
			{
				if(haystack.Contains(needle))
					return true;
			}

			return false;
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
	}
}
