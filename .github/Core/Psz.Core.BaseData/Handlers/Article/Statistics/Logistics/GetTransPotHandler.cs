using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Logistics
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class GetTransPotHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Logistics.TranspotResponseModel>>>
	{
		private UserModel _user { get; set; }
		private bool _data { get; set; }
		public GetTransPotHandler(UserModel user, bool isXLS = false)
		{
			this._user = user;
			this._data = isXLS;
		}
		public ResponseModel<List<Models.Article.Statistics.Logistics.TranspotResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetTransPot();
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Logistics.TranspotResponseModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Logistics.TranspotResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Logistics.TranspotResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Logistics.TranspotResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Logistics.TranspotResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Logistics.TranspotResponseModel>>.SuccessResponse();
		}

		public byte[] GetData()
		{
			var data = Handle();
			if(data.Success == false)
				return null;

			// - 
			return getExcelData(data.Body);
		}
		internal static byte[] getExcelData(List<Models.Article.Statistics.Logistics.TranspotResponseModel> dataEntities)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"TransPot-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Trans Pot.");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 7;

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
					worksheet.Cells[1, 1].Value = $"Trans Pot.";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Kennzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Produktionszeit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Produktivitat";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Stundensatz";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Anzahl von Fertigungsnummer";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Kennzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Produktionszeit;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Produktivitat;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Stundensatz;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.AnzahlvonFertigungsnummer;

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
					package.Workbook.Properties.Title = $"Trans Pot.";
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
