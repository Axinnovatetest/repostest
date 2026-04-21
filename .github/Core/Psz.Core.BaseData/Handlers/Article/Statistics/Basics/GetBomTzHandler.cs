using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;

	public class GetBomTzHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Basics.BomTzResponseModel>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public GetBomTzHandler(UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Basics.BomTzResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				if(string.IsNullOrWhiteSpace(this._data) || this._data.Length < 2)
					return ResponseModel<Models.Article.Statistics.Basics.BomTzResponseModel>.SuccessResponse(null);


				// -
				var suffix = this._data.Substring(this._data.Length - 2, 2).ToLower();
				var bestandSuffix = "CZ";
				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz> results = null;
				switch(suffix)
				{
					case "tn":
						bestandSuffix = "TN";
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBomTz_TN(this._data);
						break;
					case "al":
						bestandSuffix = "AL";
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBomTz_AL(this._data);
						break;
					case "de":
						bestandSuffix = "DE";
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBomTz_DE(this._data);
						break;
					default:
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBomTz_CZ(this._data);
						break;
				}

				//- 
				return ResponseModel<Models.Article.Statistics.Basics.BomTzResponseModel>.SuccessResponse(new Models.Article.Statistics.Basics.BomTzResponseModel(bestandSuffix, results));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Basics.BomTzResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Basics.BomTzResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.Statistics.Basics.BomTzResponseModel>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"data-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"BOM TZ");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 11;

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
					worksheet.Cells[1, 1].Value = $"BOM TZ - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung des Bauteils";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bestell-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Kupferzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Mindestbestellmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Wiederbeschaffungszeitraum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bestand AL";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Gesamtbestand";


					var rowNumber = headerRowNumber + 1;
					if(data.Success == true && data.Body.Items != null && data.Body.Items.Count > 0)
					{
						// Loop through 
						foreach(var w in data.Body.Items)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung_des_Bauteils;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Bestell_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Kupferzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Mindestbestellmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Wiederbeschaffungszeitraum;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Gesamtbestand;

							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

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

					if(data.Success == true)
					{
						// Doc content
						if(data.Body != null && data.Body.Items != null && data.Body.Items.Count > 0)
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
					worksheet.Column(2).Width = 50; // force width for 2nd column 
													// Set some document properties
					package.Workbook.Properties.Title = $"BOM TZ";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

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
