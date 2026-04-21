using System;
using System.Collections.Generic;
namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	public class GetMaterialbestandSpezifischLautNummernkreisHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public GetMaterialbestandSpezifischLautNummernkreisHandler(UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<byte[]>.SuccessResponse(GetData(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		public static byte[] GetData(string data)
		{
			var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetMaterialbestandSpezifischLautNummernkreisEngineering(data);

			return SaveToExcelFileEngineering(data, statisticsEntities);
		}
		public static byte[] GetDataPurchase(List<string> data)
		{
			if(data == null || data.Count == 0)
				return null;
			var orExtesnion = GetQueryOrExtension(data);
			var andExtesnion = GetQueryAndExtension(data);
			var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetMaterialbestandSpezifischLautNummernkreisPurchase(orExtesnion, andExtesnion);


			return SaveToExcelFilePurchase($"{string.Join(" | ", data)}", statisticsEntities);
		}
		public static byte[] GetDataEngineering(string data)
		{
			var dataEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetMaterialbestandSpezifischLautNummernkreisEngineering(data);

			try
			{
				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{data} - Materialbestand");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 12;

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
					worksheet.Cells[1, 1].Value = $"[Kundenname: {data}] Materialbestand";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Nummernkreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Lagerort id";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Lagerort";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Mindestbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bestell Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Name1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Kupferzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Bestandskosten";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Nummernkreis;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Lagerort_id;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Lagerort;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Mindestbestand;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Bestell_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Kupferzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Bestandskosten;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = "#0.000 €";


							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
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
					if(dataEntities != null && dataEntities.Count > 0)
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
					package.Workbook.Properties.Title = $"{data} Materialbestand";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		internal static byte[] SaveToExcelFileEngineering(string data,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_MaterialbestandSpezifischLautNummernkreis> dataEntities)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"{data}-Materialbestand-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{data} - Materialbestand");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 12;

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
					worksheet.Cells[1, 1].Value = $"[Kundenname: {data}] Materialbestand";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Nummernkreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Lagerort id";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Lagerort";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Mindestbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bestell Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Name1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Kupferzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Bestandskosten";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Nummernkreis;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Lagerort_id;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Lagerort;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Mindestbestand;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Bestell_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Kupferzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Bestandskosten;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = "#0.000 €";


							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
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
					if(dataEntities != null && dataEntities.Count > 0)
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
					package.Workbook.Properties.Title = $"{data} Materialbestand";
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
		internal static byte[] SaveToExcelFilePurchase(string data,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_MaterialbestandSpezifischLautNummernkreisPurchase> dataEntities)
		{
			try
			{
				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{data} - Materialbestand");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 10;

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
					worksheet.Cells[1, 1].Value = $"Artikel: [{data}] Materialbestand";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Mindestbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bestell-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Lieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Kupferzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Bestandskosten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Lieferzeit";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Mindestbestand;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Bestell_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Kupferzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Bestandskosten;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = "#0.000 €";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Lieferzeit;


							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
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
					if(dataEntities != null && dataEntities.Count > 0)
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
					package.Workbook.Properties.Title = $"{data} Materialbestand";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		internal static string GetQueryOrExtension(List<string> data)
		{
			if(data == null || data.Count == 0)
				return null;
			var extension = string.Empty;
			foreach(var item in data)
			{
				extension += $"Artikel.Artikelnummer LIKE '{item}%'";
				if(data.IndexOf(item) != data.Count - 1)
					extension += " OR ";
			}
			return extension;
		}
		internal static string GetQueryAndExtension(List<string> data)
		{
			if(data == null || data.Count == 0)
				return null;
			var extension = string.Empty;
			foreach(var item in data)
			{
				extension += $"Artikel.Artikelnummer NOT LIKE '{item}%'";
				if(data.IndexOf(item) != data.Count - 1)
					extension += " AND ";
			}
			return extension;
		}
	}
}