using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Psz.Core.Logistics
{
	public class Module
	{
		public static Reporting.FastReport Logistic_ReportingService { get; set; }
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }
		public static Infrastructure.Services.Files.FilesManager FilesManager { get; set; }
		public static Common.Models.AppSettingsModel.LGT LGT { get; set; }
		public static Common.Models.AppSettingsModel.BSD BSD { get; set; }
		public static Common.Models.AppSettingsModel.ServerPath serverPath { get; set; }
		public static Common.Models.AppSettingsModel.Impersonate NetworkUser { get; set; }

		public static string EmailAppDomaineName { get; set; } = "https://logistics.";
		public static List<int> LagerortIds { get; set; }
		public static string XLS_FORMAT_NUMBER = "0.0#####";
		public static string XLS_FORMAT_DATE = "dd/MM/yyyy";
		public static string XLS_COLOR_GREEN = "#4ec000";
		public static string XLS_COLOR_GREEN_LIGHT = "#C6EFCE";
		public static void Initiate(string reportTemplatePath, Infrastructure.Services.Email.EmailParamtersModel emailParamters, Common.Models.AppSettingsModel.LGT appSettings,
			string filesPhysicalPath, string tempFilesPhysicalPath, Common.Models.AppSettingsModel.Impersonate networkUser,
			Core.Common.Models.AppSettingsModel.BSD bsdAppSettings)
		{
			Logistic_ReportingService = new Psz.Core.Logistics.Reporting.FastReport(reportTemplatePath);
			// Email
			EmailingService = new Infrastructure.Services.Email.MailKit();
			EmailingService.InitiateEmailSender(emailParamters);
			LGT = appSettings;
			NetworkUser = networkUser;
			BSD = bsdAppSettings;
			initFilesManager(filesPhysicalPath, tempFilesPhysicalPath);
		}
		private static void initFilesManager(string filesPhysicalPath, string tempfilesPhysicalPath)
		{
			FilesManager = new Infrastructure.Services.Files.FilesManager(filesPhysicalPath, tempfilesPhysicalPath);
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
	}
}
