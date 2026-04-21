using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.Linq;

	public class GetRohArticlesSuppliersHanlder: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		public GetRohArticlesSuppliersHanlder(UserModel user)
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

				return ResponseModel<byte[]>.SuccessResponse(GetData());
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

		public byte[] GetData()
		{
			var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetRohArticlesSuppliers();

			return SaveToExcelFile(statisticsEntities);
		}

		internal byte[] SaveToExcelFile(IEnumerable<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_RohArticlesSuppliers> dataEntities)
		{
			try
			{
				var chars = new char[] { ' ', '#' };

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Articles");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 22;

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
					worksheet.Cells[1, 1].Value = $"Articles";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Lieferantennummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Lieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Stufe";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "ist_Priolieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Artikelklassifizierung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "ist Systemaktiv";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Status";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Bestell-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Angebot";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Angebot Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Mindestbestellmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Wiederbeschaffungszeitraum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Verpackungseinheit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Last2YearsOrderQuantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "LastYearsBookingQuantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Bedarf PO";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Manufacturer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Manufacturer Number";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count() > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							// -
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.lieferantennummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Lieferant;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Stufe;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.ist_Priolieferant;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.artikelklassifizierung;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.ist_Systemaktiv;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Status;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Artikelbezeichnung;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Artikelbezeichnung2;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Bestell_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Angebot;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Angebot_Datum;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Mindestbestellmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.Wiederbeschaffungszeitraum;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.Verpackungseinheit;
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = w?.Last2YearsOrderQuantity;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = w?.LastYearsBookingQuantity;
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = w?.BedarfPO;
							worksheet.Cells[rowNumber, startColumnNumber + 20].Value = w?.Manufacturer;
							worksheet.Cells[rowNumber, startColumnNumber + 21].Value = w?.ManufacturerNumber;
							// -

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
					if(dataEntities != null && dataEntities.Count() > 0)
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
					worksheet.Column(7).Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"Rahmenliste";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
