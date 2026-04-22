using System.Collections.Generic;
using System.Reflection;
using System;

namespace Psz.Core.SharedKernel.Extensions
{
	public static class SharedExtensions
	{
		//public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source , Func<TSource , TKey> keySelector)
		//{
		//	HashSet<TKey> seenKeys = new HashSet<TKey>();
		//	foreach (TSource element in source)
		//	{
		//		if (seenKeys.Add(keySelector(element)))
		//		{
		//			yield return element;
		//		}
		//	}
		//}
		public static List<string> CompareObjects<T>(T obj1, T obj2, string objectName)
		{
			if(obj1 == null || obj2 == null)
				throw new ArgumentNullException("Objects to compare cannot be null");

			List<string> differences = new List<string>();
			PropertyInfo[] properties = typeof(T).GetProperties();

			foreach(var prop in properties)
			{
				object value1 = prop.GetValue(obj1);
				object value2 = prop.GetValue(obj2);

				if(!object.Equals(value1, value2)) // Check if values are different
				{
					differences.Add($"Update [{prop.Name}] from {{{value1}}} to {{{value2}}} on [{objectName}]");
				}
			}

			return differences;
		}
	}
}
