using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class WorkArea
	{
		public static Core.Models.ResponseModel<byte[]> ExportToExcel()
		{
			try
			{
				var wAreaEntities = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get()
					?.Where(x => x.IsArchived == false)?.ToList();
				if(wAreaEntities == null || wAreaEntities.Count <= 0)
					return null;

				return Core.Models.ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(wAreaEntities));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		internal static byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> workAreas)
		{
			try
			{
				workAreas = workAreas.OrderBy(x => x.CountryId)?.ThenBy(x => x.HallId)?.ThenBy(x => x.DepartmentId)?.ThenBy(x => x.Name)?.ToList();
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"workAreas-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();
				var halls = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get();
				var departments = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get();

				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Work Areas");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 4;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"Work Areas {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Country";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Hall";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Department";


					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var w in workAreas)
					{
						var country = countries.Find(x => x.Id == w.CountryId);
						var hall = halls.Find(x => x.Id == w.HallId);
						var dept = departments.Find(x => x.Id == w.DepartmentId);

						worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = country?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = hall?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = dept?.Name;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Doc content
					if(workAreas != null && workAreas.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Work Areas";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

					return File.ReadAllBytes(filePath);
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
