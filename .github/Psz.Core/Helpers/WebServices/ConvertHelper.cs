using System;
using System.Globalization;

namespace Psz.Core.Helpers.WebServices
{
	public class ConvertHelper
	{
		public static string ToString(byte[] imageBytes)
		{
			return imageBytes != null ? Convert.ToBase64String(imageBytes) : null;
		}
		public static byte[] ToImageBytes(string imageBase64)
		{
			return !string.IsNullOrEmpty(imageBase64) ? Convert.FromBase64String(imageBase64) : new byte[0];
		}

		public static string ToString(int value)
		{
			return value.ToString();
		}
		public static int ToInt(string value)
		{
			return int.Parse(value);
		}
		public static string ToString(int? value)
		{
			return value.HasValue ? ToString(value.Value) : null;
		}
		public static int? ToNullableInt(string value)
		{
			return !string.IsNullOrEmpty(value) ? int.Parse(value) : (int?)null;
		}

		public static string ToString(decimal value)
		{
			return value.ToString();
		}
		public static decimal ToDecimal(string value)
		{
			return System.Convert.ToDecimal(value.Replace(",", "."), CultureInfo.InvariantCulture);
		}
		public static string ToString(decimal? value)
		{
			return value.HasValue ? ToString(value.Value) : null;
		}
		public static decimal? ToNullableDecimal(string value)
		{
			if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			try
			{
				return System.Convert.ToDecimal(value.Replace(",", "."), CultureInfo.InvariantCulture);
			} catch(Exception)
			{
				return null;
			}
		}

		public static string ToString(bool value)
		{
			return value.ToString().ToLower();
		}
		public static bool ToBoolean(string value)
		{
			return value.ToLower() == "true";
		}

		public static string ToString(DateTime value)
		{
			return value.Ticks.ToString();
		}
		public static DateTime ToDateTime(string value)
		{
			return new DateTime(long.Parse(value));
		}
		public static DateTime? ToNullableDateTime(string value)
		{
			if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			try
			{
				return new DateTime(long.Parse(value));
			} catch(Exception)
			{
				return null;
			}
		}

		public static string ToString(long value)
		{
			return value.ToString();
		}
		public static long ToLong(string value)
		{
			return long.Parse(value);
		}

		public static long ToLong(DateTime value)
		{
			return value.Ticks;
		}
		public static DateTime ToDateTime(long value)
		{
			return new DateTime(value);
		}
	}
}