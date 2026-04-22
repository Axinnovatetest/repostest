using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Reporting.Helpers
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
		public static string FormatDecimal(string number, int decimalPlaces)
		{
			if(decimal.TryParse(number, out var _d))
			{
				string format = $"N{decimalPlaces}";
				CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
				string formattedNumber = _d.ToString(format, culture);

				return formattedNumber;
			}

			return "";
		}
	}
}
