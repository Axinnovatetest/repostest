using System;
using System.Globalization;

namespace Infrastructure.Services.Helpers
{
	public static class FormatHelper
	{
		public static string FormatDecimal(decimal number, int decimalPlaces)
		{
			string format = $"N{decimalPlaces}";
			CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
			string formattedNumber = number.ToString(format, culture);

			return formattedNumber;
		}
		public static string FormatDecimal2Places(decimal number)
		{
			return number.ToString("F2");
		}
	}
}
