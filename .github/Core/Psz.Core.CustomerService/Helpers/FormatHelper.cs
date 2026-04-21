using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Helpers
{
	public class FormatHelper
	{
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
