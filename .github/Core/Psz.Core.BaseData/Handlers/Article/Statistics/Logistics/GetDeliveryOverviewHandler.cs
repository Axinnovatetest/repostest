using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Logistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;

	public class GetDeliveryOverviewHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		public GetDeliveryOverviewHandler(UserModel user)
		{
			this._user = user;
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

				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile());
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

		public static byte[] SaveToExcelFile()
		{
			var deliveryOverviews = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetDeliveryOverview();
			if(deliveryOverviews != null && deliveryOverviews.Count > 0)
			{
				try
				{
					var tempFolder = System.IO.Path.GetTempPath();
					var filePath = System.IO.Path.Combine(tempFolder, $"deliveryOverview-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

					var file = new FileInfo(filePath);

					// Create the package and make sure you wrap it in a using statement
					ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
					using(var package = new ExcelPackage(file))
					{
						// add a new worksheet to the empty workbook
						ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Lieferanten Lieferübersicht");

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
						worksheet.Cells[1, 1].Value = $"Lieferanten Lieferübersicht - {DateTime.Now.ToString("dd.MM.yyyy")}";
						worksheet.Cells[1, 1].Style.Font.Size = 16;

						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Name";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Lieferantennummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Standardlieferant";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bezeichnung 1";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bezeichnung 2";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bestell-Nr";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Einkaufspreis";
						worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Zolltarif Nr";
						worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Ursprungsland";
						worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Nettogewicht in gr";
						// -
						var longColumns = new List<int> { startColumnNumber + 4, startColumnNumber + 5, startColumnNumber + 6 };

						var rowNumber = headerRowNumber + 1;
						// Loop through 
						foreach(var w in deliveryOverviews)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Name;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.SupplierNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.StandardSupplier;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.ArticleNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Designation1;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Designation2;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.OrderNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.PurchasePrice;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.CustomsTariff;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.OriginCountry;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.NetWeightinGr;

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
						if(deliveryOverviews != null && deliveryOverviews.Count > 0)
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
							if(!longColumns.Contains(i))
								worksheet.Column(i).AutoFit();
						}

						// Set some document properties
						package.Workbook.Properties.Title = "Lieferanten Lieferübersicht";
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

			return null;
		}
	}
}
