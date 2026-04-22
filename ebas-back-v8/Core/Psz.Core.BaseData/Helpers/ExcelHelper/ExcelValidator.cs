using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Helpers.ExcelHelper
{
	public class ExcelValidator
	{
		public static int ExpectedNumberOfColumns { get; set; } = 8;
		public static List<string> expectedColumnNames = new List<string>() { "Artikelnummer", "Bestell-Nr", "Einkaufspreis", "Angebot", "Datum", "Mindestbestellmenge", "Wiederbeschaffungszeitraum" };
		public static bool ValidateExcelFileColumns(IFormFile file)
		{
			try
			{
				// License for EPPLUS
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				if(file != null && file.Length > 0)
				{
					using(ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
					{
						var worksheet = excelPackage.Workbook.Worksheets.First();

						if(worksheet.Dimension?.Rows > 2)
						{
							var headerRow = worksheet.Cells[2, 1, 1, worksheet.Dimension.Columns].Select(cell => cell.Text.Trim()).ToList();
							if(expectedColumnNames.All(columnName => headerRow.Contains(columnName)))
							{
								if(headerRow.Count == ExpectedNumberOfColumns)
								{
									return true;
								}
							}
						}
					}
				}

			} catch(Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
			return false;
		}

		private static readonly string[] ColumnNames =
			{ "Artikelnummer", "Bestell-Nr", "Einkaufspreis", "Datum", "Mindestbestellmenge", "Wiederbeschaffungszeitraum" };

		public static IEnumerable<string> ValidateExcelFile(IFormFile excelFile)
		{
			// License for EPPLUS
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			var invalidCells = new List<string>();

			using(var stream = excelFile.OpenReadStream())
			{
				using(var package = new ExcelPackage(stream))
				{
					ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

					for(int row = 3; row <= worksheet.Dimension.End.Row; row++)
					{
						for(int col = 1; col <= 7; col++)
						{
							if(col == 4)
							{
								continue;
							}

							ExcelRange cell = worksheet.Cells[row, col];
							var cellValue = cell.Value;

							if(col == 1 && string.IsNullOrEmpty(cellValue?.ToString()))
							{
								invalidCells.Add(GetCellReference(row, col));
							}
							if(col == 2 && string.IsNullOrEmpty(cellValue?.ToString()))
							{
								invalidCells.Add(GetCellReference(row, col));
							}

							if(col == 3 && !(IsValidDecimal(cellValue)))
							{
								invalidCells.Add(GetCellReference(row, col));
							}

							if(col == 5 && !IsValidDate(cell))
							{
								invalidCells.Add(GetCellReference(row, col));
							}

							if(col == 6 && !IsValidInteger(cellValue))
							{
								invalidCells.Add(GetCellReference(row, col));
							}
							if(col == 7 && !IsValidInteger(cellValue))
							{
								invalidCells.Add(GetCellReference(row, col));
							}
						}
					}
				}
			}

			return invalidCells;
		}

		private static bool IsValidDecimal(object value)
		{
			if(value == null || value.ToString().Trim() == "")
			{
				return false;
			}

			decimal parsedValue;
			return decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out parsedValue);
		}

		private static bool IsValidDate(ExcelRange cell)
		{
			if(cell == null || cell.Value == null || cell.Value?.ToString().Trim() == "")
			{
				return false;
			}
			var date = cell.GetValue<DateTime?>();

			return date is DateTime;
		}

		private static bool IsValidInteger(object value)
		{
			if(value == null || value.ToString().Trim() == "")
			{
				return false;
			}

			int parsedInt;
			return int.TryParse(value.ToString(), out parsedInt);
		}

		private static string GetCellReference(int row, int column)
		{
			return ExcelCellBase.GetAddress(row, column);
		}

		public static byte[] ColorizeExcelCells(IFormFile excelFile, IEnumerable<string> invalidCells)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using(var stream = excelFile.OpenReadStream())
			{
				using(var package = new ExcelPackage(stream))
				{
					ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

					foreach(var cellRef in invalidCells)
					{
						int row, col;

						if(!ParseCellReference(cellRef, out row, out col))
						{
							continue;
						}

						if(row <= 0 || col <= 0)
						{
							continue;
						}

						worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
					}

					using(var memoryStream = new MemoryStream())
					{
						package.SaveAs(memoryStream);
						return memoryStream.ToArray();
					}
				}
			}
		}

		private static bool ParseCellReference(string cellRef, out int row, out int col)
		{
			row = col = 0;

			if(string.IsNullOrEmpty(cellRef))
			{
				return false;
			}

			int i = 0;
			bool isRow = true;

			while(i < cellRef.Length)
			{
				char c = cellRef[i];

				if(c >= 'A' && c <= 'Z')
				{
					if(!isRow)
					{
						return false;
					}
					row *= 26;
					row += c - 'A' + 1;
				}
				else if(c >= '0' && c <= '9')
				{
					if(isRow)
					{
						return false;
					}
					col *= 10;
					col += c - '0';
				}
				else if(c == ' ')
				{
					isRow = false;
				}
				else
				{
					return false;
				}

				i++;
			}

			return true;
		}

	}
}
