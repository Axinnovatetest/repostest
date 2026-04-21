using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.CustomerService
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class GetAVEvaluationHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.CustomerService.AVEvaluationResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.CustomerService.AVEvaluationRequestModel _data { get; set; }
		public GetAVEvaluationHandler(UserModel user, Models.Article.Statistics.CustomerService.AVEvaluationRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.CustomerService.AVEvaluationResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.CustomerService.GetAVEvaluation(this._data.DateFrom, this._data.DateTill);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.CustomerService.AVEvaluationResponseModel>>.SuccessResponse(
						statisticsEntities.Select(x => new Models.Article.Statistics.CustomerService.AVEvaluationResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.CustomerService.AVEvaluationResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.CustomerService.AVEvaluationResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.CustomerService.AVEvaluationResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.CustomerService.AVEvaluationResponseModel>>.SuccessResponse();
		}
		public byte[] GetData()
		{
			var data = Handle();
			if(data.Success == false)
				return null;

			// - 
			return getExcelData(data.Body);
		}
		internal static byte[] getExcelData(List<Models.Article.Statistics.CustomerService.AVEvaluationResponseModel> dataEntities)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"AV-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"AV");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 24;

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
					worksheet.Cells[1, 1].Value = $"AV Auswertung";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Datum Anderung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Anderungsbeschreibung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Anderung von";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Umsatzsteuer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Preiseinheit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Groesse";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Exportgewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Zolltarif nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Gewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Warengruppe";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Warentyp";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Produktionszeit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Stundensatz";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Verpackungsart";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Verpackungsmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Losgroesse";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "DB I ohne";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Preisgruppen Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Verkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "Standardlieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "Name";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Datum_Anderung.HasValue == true ? $"{w?.Datum_Anderung.Value.ToString("dd/MM/yyyy")}" : "";
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Anderungsbeschreibung;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Anderung_von;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Umsatzsteuer;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Preiseinheit;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Groesse;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Exportgewicht;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Zolltarif_nr;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Gewicht;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Warengruppe;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Warentyp;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Produktionszeit;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Stundensatz;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.Verpackungsart;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.Verpackungsmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = w?.Losgroesse;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = w?.DB_I_ohne;
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = w?.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 20].Value = w?.Preisgruppen_Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 21].Value = w?.Verkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 22].Value = w?.Standardlieferant;
							worksheet.Cells[rowNumber, startColumnNumber + 23].Value = w?.Name1;

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
					package.Workbook.Properties.Title = $"AV";
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
