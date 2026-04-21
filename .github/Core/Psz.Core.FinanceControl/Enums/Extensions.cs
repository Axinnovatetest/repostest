using System;

namespace Psz.Core.FinanceControl
{

	public static class ExtensionsClass
	{
		public static string GetDescription(this Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if(name != null)
			{
				System.Reflection.FieldInfo field = type.GetField(name);
				if(field != null)
				{
					System.ComponentModel.DescriptionAttribute attr =
							Attribute.GetCustomAttribute(field,
								typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
					if(attr != null)
					{
						return attr.Description;
					}
				}
			}
			return null;
		}
		public static bool IsnullOrEmptyOrWhiteSpaces(this string value)
		{
			return string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value);
		}
	}
}
