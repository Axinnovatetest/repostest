using System.ComponentModel;
using System.Reflection;
using System;

namespace Psz.Core.ManagementOverview
{
	public class Module
	{
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }
		public static string EmailAppDomaineName { get; set; } = "https://management-overview.";

		public static string XLS_FORMAT_NUMBER = "0.0#####";
		public static string XLS_FORMAT_DATE = "dd/MM/yyyy";
		public static Common.Models.AppSettingsModel.LGT AppSettingsLGT { get; set; }
		public static Common.Models.AppSettingsModel.BSD AppSettingsBSD { get; set; }
		public static Common.Models.AppSettingsModel.CTS AppSettingsCTS{ get; set; }

		public static void Initiate(Infrastructure.Services.Email.EmailParamtersModel emailParamters, Common.Models.AppSettingsModel.BSD appSettings, Common.Models.AppSettingsModel.LGT appSettingsLGT, Common.Models.AppSettingsModel.CTS appSettingsCTS)
		{
			// Email
			EmailingService = new Infrastructure.Services.Email.MailKit();
			EmailingService.InitiateEmailSender(emailParamters);
			AppSettingsBSD = appSettings;
			AppSettingsLGT = appSettingsLGT;
			AppSettingsCTS = appSettingsCTS;
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
