using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
	public class Module
	{
	}
	public static class ExtensionsClass
	{
		public static bool StringIsNullOrEmptyOrWhiteSpaces(this string value)
		{
			if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
				return true;
			else
				return false;
		}
		/// <summary>
		/// clean the sql query input from dangerous characters
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string SanitizeForSql(this string input)
		{
			if(string.IsNullOrWhiteSpace(input))
				return string.Empty;

			return new string(input
				.Where(c => c != '\'' && c != '\"' && c != ';')
				.ToArray())
				.Trim();
		}
	}
}
