using System;
using System.ComponentModel;
using System.Reflection;

namespace Psz.Core.Apps.EDI.Enums
{
	public class DelforEnums
	{
		internal enum FileProcessStatus: int
		{
			[Description("Success")]
			Success = 0,
			[Description("Error")]
			Error = 1,
		}
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
