using System;

namespace Psz.Core.Common.Helpers
{
	public static class Formatters
	{
		public class XLS
		{
			public static string GetCellValue(OfficeOpenXml.ExcelRange cell)
			{
				var val = cell.Value;
				if(val == null)
				{
					return "";
				}

				return val.ToString();
			}
			public static string EscapeDecimalSeparator(string input)
			{
				if(!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
				{
					input = input.Replace(",", ".");
				}

				return input;
			}

			// - Ints
			public static int GetInt(OfficeOpenXml.ExcelRange cell)
			{
				return int.TryParse(EscapeDecimalSeparator(GetCellValue(cell)), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var _val) ? _val : 0;
			}
			public static int GetInt(string input)
			{
				return int.TryParse(EscapeDecimalSeparator(input), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var _val) ? _val : 0;
			}

			// - Decimals
			public static decimal? GetDecimalNullable(OfficeOpenXml.ExcelRange cell)
			{
				return decimal.TryParse(EscapeDecimalSeparator(GetCellValue(cell)), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var _val) ? _val : null;
			}
			public static decimal GetDecimal(OfficeOpenXml.ExcelRange cell)
			{
				return decimal.TryParse(EscapeDecimalSeparator(GetCellValue(cell)), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var _val) ? _val : 0;
			}
			public static decimal GetDecimal(string input)
			{
				return decimal.TryParse(EscapeDecimalSeparator(input), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var _val) ? _val : 0;
			}

			// - Floats
			public static float GetFloat(string input)
			{
				return float.TryParse(EscapeDecimalSeparator(input), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var _val) ? _val : 0;
			}
			public static float GetFloat(OfficeOpenXml.ExcelRange cell)
			{
				return float.TryParse(EscapeDecimalSeparator(GetCellValue(cell)), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var _val) ? _val : 0;
			}

			// - Dates
			//public static int GetDateYear(int year)
			//{
			//	// - 2023-02-02 - stupid conditions on Years in XLS formulas
			//	return year >= 0 && year < 10000 ? (year >= 1900 ? year : year - 1900) : year;
			//}
			public static string GetDateFormula(DateTime date)
			{
				// - 2023-02-02 - stupid conditions on Years in XLS formulas
				// - https://support.microsoft.com/en-us/office/date-function-e36c0c8c-4104-49da-ab83-82328b832349#:~:text=The%20DATE%20function%20returns%20the,that%20represents%20a%20particular%20date.&text=The%20DATE%20function%20syntax%20has,include%20one%20to%20four%20digits.
				return date.Year >= 1900 && date.Year < 10000 ? $"=DATE({date.Year}, {date.Month}, {date.Day})" : $"=DATE(0, {date.Month}, {date.Day})";
			}
			public static string GetCellValueFg(OfficeOpenXml.ExcelRange cell)
			{
				var val = cell.Value;
				if(val == null || val.ToString() == "")
				{
					throw new InvalidOperationException("Cell value cannot be null");
				}

				return val.ToString();
			}
			public static string GetCellValueFgAsNumerique(OfficeOpenXml.ExcelRange cell)
			{
				var val = cell.Value;
				if(val == null || !decimal.TryParse(val.ToString(), out decimal ValNum))
				{
					throw new InvalidOperationException("Cell value cannot be null");
				}

				return val.ToString();
			}
			public static string GetCellValueFgAsString(OfficeOpenXml.ExcelRange cell, bool rejectNull = false)
			{
				var val = cell.Value;
				if(rejectNull && (val == null || val.ToString() == ""))
				{
					throw new InvalidOperationException("Cell value cannot be null");
				}

				return val?.ToString() ?? "";
			}
		}
	}
}
