using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Psz.Core.BaseData
{
	public class Module
	{
		public static string Version { get; set; } = "1.1.0-rc";

		public static Infrastructure.Services.Reporting.FastReport ReportingService { get; set; }
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }
		public static string EmailAppDomaineName { get; set; } = "https://master-data.";
		public static Infrastructure.Services.ActiveDirectory.ActiveDirectoryManager ActiveDirectoryManager { get; set; }
		public static Common.Models.AppSettingsModel.CTS CTS { get; set; }
		public static Common.Models.AppSettingsModel.BSD AppSettings { get; set; }
		public static Common.Models.AppSettingsModel.ServerPath serverPath { get; set; }
		public static Common.Models.AppSettingsModel.Impersonate impersonate { get; set; }
		public static List<int> LagersWithVersionning { get; set; }
		public static string XLS_FORMAT_NUMBER = "0.0#####";
		public static string XLS_FORMAT_DATE = "dd/MM/yyyy";
		public static List<int> HauplagerCTS { get; set; }


		public static void Initiate(string reportTemplatePath, Infrastructure.Services.Email.EmailParamtersModel emailParamters, string adPath, string adUsername, string adPassword, Common.Models.AppSettingsModel.BSD appSettings, Common.Models.AppSettingsModel.CTS cts, List<int> hauptlager, List<int> lagers
			, Common.Models.AppSettingsModel.ServerPath _serverPath, Common.Models.AppSettingsModel.Impersonate _impersonate)
		{
			// Report
			ReportingService = new Infrastructure.Services.Reporting.FastReport(reportTemplatePath);
			Infrastructure.Services.Reporting.ReportGenerator.SetTemplatesPath(reportTemplatePath);
			//Infrastructure.Services.Reporting.ReportGenerator.SetProjectTemplatePath(projectTemplatePath);

			// Email
			EmailingService = new Infrastructure.Services.Email.MailKit(); // - new Infrastructure.Services.Email.NetMail();
			EmailingService.InitiateEmailSender(emailParamters);
			AppSettings = appSettings;
			CTS = cts;

			// AD
			ActiveDirectoryManager = new Infrastructure.Services.ActiveDirectory.ActiveDirectoryManager(adPath, adUsername, adPassword);
			// Booking
			//BookingLagerIds = string.IsNullOrWhiteSpace(bookingLagerId) ? null : new List<string>(bookingLagerId.Split(';'));
			HauplagerCTS = hauptlager;
			LagersWithVersionning = lagers;
			serverPath = _serverPath;
			impersonate = _impersonate;
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
		public static bool IsNullOrEmptyOrWitheSpaces(this string value)
		{
			return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
		}
	}
}
