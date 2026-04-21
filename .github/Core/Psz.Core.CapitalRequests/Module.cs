using System.ComponentModel;
using System.Reflection;

namespace Psz.Core.CapitalRequests
{
	public class Module
	{
		public static Infrastructure.Services.Email.MailKit EmailingService { get; set; }
		public static void Initiate(Infrastructure.Services.Email.EmailParamtersModel emailParamters)
		{
			EmailingService = new Infrastructure.Services.Email.MailKit();
			EmailingService.InitiateEmailSender(emailParamters);
		}
	}
	public static class ExtensionClass
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
		public static bool StringIsNullOrEmptyOrWhiteSpaces(this string value)
		{
			if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
				return true;
			else
				return false;
		}
	}
}
