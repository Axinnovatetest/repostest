using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Psz.Core.FinanceControl
{
	public class Module
	{
		public static string Version { get; set; } = "1.1.0-rc";

		public static Infrastructure.Services.Reporting.FastReport ReportingService { get; set; }
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }
		public static Infrastructure.Services.ActiveDirectory.ActiveDirectoryManager ActiveDirectoryManager { get; set; }
		public static List<int> BookingLagerIds { get; set; }
		public static List<string> LeasingAllowedCompanies { get; set; }
		public static Common.Models.AppSettingsModel.FNCAppSettings ModuleSettings { get; set; }

		public static string EmailAppDomaineName { get; set; } = "https://finance-control.";
		public static void Initiate(string projectTemplatePath, string reportTemplatePath, Infrastructure.Services.Email.EmailParamtersModel emailParamters, string adPath, string adUsername, string adPassword, Common.Models.AppSettingsModel.FNCAppSettings appSettings)
		{
			// Report
			ReportingService = new Infrastructure.Services.Reporting.FastReport(reportTemplatePath);
			Infrastructure.Services.Reporting.ReportGenerator.SetTemplatesPath(reportTemplatePath);
			Infrastructure.Services.Reporting.ReportGenerator.SetProjectTemplatePath(projectTemplatePath);

			// Email
			EmailingService = new Infrastructure.Services.Email.MailKit(); // - new Infrastructure.Services.Email.NetMail();
			EmailingService.InitiateEmailSender(emailParamters);

			// AD
			ActiveDirectoryManager = new Infrastructure.Services.ActiveDirectory.ActiveDirectoryManager(adPath, adUsername, adPassword);

			// Booking
			BookingLagerIds = appSettings?.BookingLager;

			// 
			LeasingAllowedCompanies = appSettings?.LeasingAllowedCompanies;

			// - 2022-12-12
			ModuleSettings = appSettings;
		}

	}
}
public static class ExtensionsClass
{
	public static bool IsNullOrEmptyOrWhiteSpaces(this string value)
	{
		return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
	}
}