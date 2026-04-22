using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Psz.Core.Helpers
{
	public class StringHelpers
	{
		public static System.Globalization.CultureInfo NUMERIC_CULTURE_INFO = new System.Globalization.CultureInfo("en-US");
		private static RNGCryptoServiceProvider _rngCsp = new RNGCryptoServiceProvider();

		public static string FirstLetterToUpper(string str)
		{
			if(string.IsNullOrEmpty(str))
			{
				return null;
			}

			if(str.Length > 1)
			{
				return char.ToUpper(str[0]) + str.Substring(1);
			}

			return str.ToUpper();
		}

		public static string GenerateRandomKey(int lenght,
			bool allowNumbers = true,
			bool allowLetters = true,
			bool uppers = true,
			bool lowers = true,
			string customChars = null)
		{
			if(!allowNumbers && !allowLetters)
			{
				allowNumbers = true;
				allowLetters = true;
			}

			if(allowLetters && !allowNumbers
				&& !uppers && !lowers)
			{
				uppers = true;
			}

			var chars = "";
			if(allowNumbers)
			{
				chars += "0123456789";
			}
			if(allowLetters)
			{
				if(!string.IsNullOrEmpty(customChars))
				{
					chars += customChars;
				}
				else
				{
					if(uppers)
					{
						chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
					}
					if(lowers)
					{
						chars += "abcdefghijklmnopqrstuvwxyz";
					}
				}
			}

			var data = new byte[lenght];
			using(var crypto = new RNGCryptoServiceProvider())
			{
				crypto.GetBytes(data);
			}
			var result = new StringBuilder(lenght);
			foreach(byte b in data)
			{
				result.Append(chars[b % (chars.Length)]);
			}
			return result.ToString();
		}
		//  - 2025-06-11 detect and correct wrongly displayed chars
		public static string FixMojibake(string broken)
		{
			if(string.IsNullOrEmpty(broken))
				return broken;

			// Attempts to decode the string as if it had been incorrectly read as Latin-1
			byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(broken);
			return Encoding.UTF8.GetString(bytes);
		}

		public static bool ContainsMojibake(string input)
		{
			if(string.IsNullOrEmpty(input))
				return false;

			// Common mojibake patterns (Ã, Â, broken UTF-8)
			var pattern = @"(Ã.|Â.|�)";
			return Regex.IsMatch(input, pattern);
		}
	}
}

namespace Psz.Core
{
	public static class StringExtensions
	{
		public static string TrimStart(this string target, string trimString)
		{
			if(string.IsNullOrEmpty(trimString))
				return target;

			string result = target;
			while(result.StartsWith(trimString))
			{
				result = result.Substring(trimString.Length);
			}

			return result;
		}
		public static string TrimEnd(this string target, string trimString)
		{
			if(string.IsNullOrEmpty(trimString))
				return target;

			string result = target;
			while(result.EndsWith(trimString))
			{
				result = result.Substring(0, result.Length - trimString.Length);
			}

			return result;
		}
		public static bool IsSameAs(this string source, string target, bool isCaseSensitive = true, bool trimPaddingSpaces = false)
		{
			if(source == null && target == null)
				return true;

			if(source == null || target == null)
				return false;

			if(trimPaddingSpaces)
			{
				source = source.Trim();
				target = target.Trim();
			}

			return isCaseSensitive
				? string.Equals(source, target, StringComparison.Ordinal)
				: string.Equals(source, target, StringComparison.OrdinalIgnoreCase);
		}
	}
}