using System.Globalization;

namespace Psz.Core.Tools
{
	public class Formatter
	{
		public static string FormatDecimal(decimal value,
		   uint precesion,
		   string unitShortName = null,
		   bool unitAfterValue = true,
		   bool emptySpaceSeparator = true)
		{
			string currentNoComaFormatSpecifier = string.Concat("0.", new string('0', (int)precesion));
			string currentComaFormatSpecifier = string.Concat("0." /*  "##,#."  */, new string('0', (int)precesion));

			var response = precesion == 0
				? value.ToString(currentNoComaFormatSpecifier, CultureInfo.InvariantCulture)
				: value.ToString(currentComaFormatSpecifier, CultureInfo.InvariantCulture);

			if(!string.IsNullOrEmpty(unitShortName))
			{
				response = unitAfterValue
				? string.Concat(response, (emptySpaceSeparator ? " " : ""), unitShortName)
				: string.Concat(unitShortName, (emptySpaceSeparator ? " " : ""), response);
			}

			return response;
		}

		public static string Percentage(decimal value)
		{
			return value.ToString("G29") + "%";
		}
	}
}
