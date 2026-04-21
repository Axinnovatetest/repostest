using System;
using System.Globalization;

namespace Psz.Core.Common.Helpers
{
	public class MathHelper
	{
		public static string ToFixedFormat(decimal number, uint decimals)
		{
			string numberStr = number.ToString("0.000000", CultureInfo.InvariantCulture);

			var parts = numberStr.Split('.');
			if(parts.Length == 0)
			{
				return numberStr;
			}

			string part2 = parts[1];
			if(part2.Length >= 3)
			{
				part2 = part2.Substring(0, 3);
			}

			return parts[0] + "." + part2;
		}
		public static decimal ToFixed(decimal number, uint decimals)
		{
			return decimal.Round(Convert.ToDecimal(ToFixedFormat(number, decimals)),
				(int)decimals,
				MidpointRounding.AwayFromZero);
		}

		public static decimal RoundDecimal(decimal value, int decimalPart = 6)
		{
			return decimal.Round(value, decimalPart, MidpointRounding.AwayFromZero);
		}
		public static double RoundDouble(double value)
		{
			return System.Math.Round(value, 3);
		}

		public static decimal GetDecimal(double value)
		{
			return RoundDecimal((decimal)value);
		}
		public static float GetFloat(double value)
		{
			return (float)System.Math.Round(value, 3);
		}
		public static float GetFloat(decimal value)
		{
			return (float)System.Math.Round(value, 3);
		}

		public static decimal GetPrice(decimal unitPrice, decimal quantity, decimal discount)
		{
			decimal price = unitPrice * quantity;

			decimal discount_amount = price * (discount / 100);

			return RoundDecimal(price - discount_amount);
		}
		public static decimal GetDiscountAmount(decimal unitPrice, decimal quantity, decimal discount)
		{
			decimal price = unitPrice * quantity;

			return RoundDecimal(price * (discount / 100));
		}
	}
}