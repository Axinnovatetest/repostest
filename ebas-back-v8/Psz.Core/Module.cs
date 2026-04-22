using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core
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
	}
}