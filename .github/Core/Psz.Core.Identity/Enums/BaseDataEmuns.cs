using System;
using System.ComponentModel;
using System.Reflection;

namespace Psz.Core.Identity.Enums
{
	public class BaseDataEmuns
	{
		public enum BaseDataLager
		{
			[Description("Albania")]
			AL = 26,
			[Description("Tunisia TN")]
			TN = 7,
			[Description("Tunisia BETN")]
			BETN = 60,
			[Description("Tunisia WS")]
			KHTN = 42,
			[Description("Germany")]
			DE = 15,
			[Description("Czech")]
			CZ = 6,
			[Description("Hauptlager/AL")]
			MainAL = 24,
			[Description("Hauptlager/TN")]
			MainTN = 4,
			[Description("Hauptlager/BETN")]
			MainBETN = 58,
			[Description("Hauptlager/WS")]
			MainKHTN = 41,
			[Description("Hauptlager/D")]
			MainDE = 8,
			[Description("Hauptlager/CZ")]
			MainCZ = 3,
			[Description("Tunisia GZTN")]
			GZTN = 102,
			[Description("Hauptlager/GZ")]
			MainGZTN = 101,
		}

	}
}
namespace Psz.Core.Identity
{
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
