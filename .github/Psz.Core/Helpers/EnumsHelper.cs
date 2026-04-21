using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Helpers
{
	public class EnumsHelper
	{
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
}