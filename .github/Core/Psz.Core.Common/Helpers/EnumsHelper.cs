using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Psz.Core.Common.Helpers
{
	public class EnumsHelper
	{
		public static List<KeyValuePair<string, int>> GetEnumValuesAndDescriptions<T>()
		{
			Type enumType = typeof(T);

			if(enumType.BaseType != typeof(Enum))
				throw new ArgumentException("T is not System.Enum");

			List<KeyValuePair<string, int>> enumValList = new List<KeyValuePair<string, int>>();

			foreach(var e in Enum.GetValues(typeof(T)))
			{
				var fi = e.GetType().GetField(e.ToString());
				var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

				enumValList.Add(new KeyValuePair<string, int>((attributes.Length > 0) ? attributes[0].Description : e.ToString(), (int)e));
			}

			return enumValList;
		}
	}
}
