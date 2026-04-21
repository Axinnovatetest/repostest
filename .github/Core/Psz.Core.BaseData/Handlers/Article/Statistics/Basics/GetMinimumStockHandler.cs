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
	using System.Linq;

	public class GetMinimumStockHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockResponseModel>>>
	{
		private UserModel _user { get; set; }
		public GetMinimumStockHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetMinimumStock();
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockResponseModel>>.SuccessResponse(
						statisticsEntities.Select(x => new Models.Article.Statistics.Basics.MinimumStockResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockResponseModel>>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"MinimumStock-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"MinimumStock");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 4;
					var startColumnNumber = 1;
					var numberOfColumns = 8;

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
					worksheet.Cells[1, 1].Value = $"Minimum Stock list";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					var rowNumber = 0;
					#region Lieferant
					if(data.Success == true)
					{
						headerRowNumber = rowNumber + 1;
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "PSZ Nummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "EK";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Lagerort";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Aktueller Bestand";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bestandskosten";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Mindestbestand";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Mindestbestandskosten";

						rowNumber = headerRowNumber + 1;
						if(data.Body != null && data.Body.Count > 0)
						{
							//Loop through
							foreach(var w in data.Body)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.PSZ_Nummer;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Bezeichnung;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.EK;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Lagerort;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Aktueller_Bestand;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Bestandskosten;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Mindestbestand;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Mindestbestandskosten;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}
					#endregion Lieferant

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"MinimumStock";
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
