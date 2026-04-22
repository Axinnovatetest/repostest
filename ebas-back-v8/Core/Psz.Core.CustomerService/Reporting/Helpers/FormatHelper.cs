using System.Globalization;

namespace Psz.Core.CustomerService.Reporting.Helpers
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
		public static string FormatNumber(decimal number, string groupSeparator = " ")
		{
			// Create a custom NumberFormatInfo
			NumberFormatInfo formatInfo = new NumberFormatInfo
			{
				NumberGroupSeparator = groupSeparator,
				NumberDecimalSeparator = ",",
				NumberGroupSizes = new[] { 3 }
			};

			string formattedNumber = number.ToString("#,0.00", formatInfo);

			return formattedNumber;
		}
	}
}