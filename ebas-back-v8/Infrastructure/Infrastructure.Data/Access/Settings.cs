using System;

namespace Infrastructure.Data.Access
{
	public class Settings
	{
		public const int MAX_BATCH_SIZE = 500;

		public static void SetConnectionString(string connectionString)
		{
			_connectionString = connectionString;
		}
		public static void SetConnectionStringNlog(string connectionStringNlog)
		{
			_connectionStringNlog = connectionStringNlog;
		}
		public static void SetConnectionStringBudget(string connectionString)
		{
			_connectionStringBudegt = connectionString;
		}

		private static string _connectionString { get; set; }
		private static string _connectionStringNlog { get; set; }
		public static string ConnectionString
		{
			get
			{
				return _connectionString;
			}
		}
		public static string ConnectionStringNlog
		{
			get
			{
				return _connectionStringNlog;
			}
		}
		private static string _connectionStringBudegt { get; set; }
		public static string ConnectionString_FNC
		{
			get
			{
				return _connectionStringBudegt;
			}
		}

		#region >>>>>> MTM <<<<<<<<<<
		public static void SetConnectionStringMTM(string connectionString)
		{
			_connectionStringMTM = connectionString;
		}
		private static string _connectionStringMTM { get; set; }
		public static string ConnectionStringMTM
		{
			get
			{
				return _connectionStringMTM;
			}
		}
		#endregion MTM

		#region >>>>>> KS <<<<<<<<<<
		private static string _connectionStringKWS { get; set; }
		private static string _connectionStringKCZ { get; set; }
		private static string _connectionStringKGZ { get; set; }
		private static string _connectionStringKAL { get; set; }
		public static string ConnectionStringKWS
		{
			get
			{
				return _connectionStringKWS;
			}
		}
		public static string ConnectionStringKCZ
		{
			get
			{
				return _connectionStringKCZ;
			}
		}
		public static string ConnectionStringKGZ
		{
			get
			{
				return _connectionStringKGZ;
			}
		}
		public static string ConnectionStringKAL
		{
			get
			{
				return _connectionStringKAL;
			}
		}
		public static void SetConnectionStringK(string connectionStringWS, string connectionStringCZ, string connectionStringGZ, string connectionStringAL)
		{
			_connectionStringKWS = connectionStringWS;
			_connectionStringKCZ = connectionStringCZ;
			_connectionStringKGZ = connectionStringGZ;
			_connectionStringKAL = connectionStringAL;
		}
		#endregion KS

		#region >>>>>> EDI Platform <<<<<<<<<<
		public static void SetEdiPlatformCnxEBAS(string connectionString)
		{
			_connectionStringEdiPlatformCnxEBAS = connectionString;
		}
		private static string _connectionStringEdiPlatformCnxEBAS { get; set; }
		public static string ConnectionStringEdiPlatformCnxEBAS
		{
			get
			{
				return _connectionStringEdiPlatformCnxEBAS;
			}
		}
		#endregion

		public class PaginModel
		{
			public int FirstRowNumber { get; set; }
			public int RequestRows { get; set; }
		}

		public class SortingModel
		{
			public string SortFieldName { get; set; }
			public bool SortDesc { get; set; }
		}
		public class FilterModel
		{
			public string FilterFieldName { get; set; }
			public string FirstFilterValue { get; set; }
			public FilterTypes FilterType { get; set; }
			public string SecondFilterValue { get; set; }
			public string ConnectorType { get; set; } = " AND";
			public int QueryLevel { get; set; } = 0;
		}
		public enum FilterTypes
		{
			Number = 0,
			String = 1,
			Date = 2,
			Boolean = 3,
			Query = 4
		}

	}

	public static class ExtensionsClass
	{
		public static string GetDescription(this Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if(name != null)
			{
				System.Reflection.FieldInfo field = type.GetField(name);
				if(field != null)
				{
					System.ComponentModel.DescriptionAttribute attr =
							Attribute.GetCustomAttribute(field,
								typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
					if(attr != null)
					{
						return attr.Description;
					}
				}
			}
			return null;
		}
		public static string SqlEscape(this string value, bool allowPercent = false)
		{
			var result = (value ?? "").Replace("'", "''").Replace("[", "[[]").Replace("_", "[_]");
			if(!allowPercent)
				result = result.Replace("%", "[%]");

			// -
			return result;
		}
		public static string TruncateLeft(this string input, int length)
		{
			if(string.IsNullOrEmpty(input) || length >= input.Length)
				return input;

			return input.Substring(input.Length - length);
		}
	}
}
