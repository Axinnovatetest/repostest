using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using OfficeOpenXml;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class ExportStdSupplierViolationXLS: IHandle<OffenematbstRequestModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		public ExportStdSupplierViolationXLS(UserModel user)
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

			if(_user == null)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			return ResponseModel<byte[]>.SuccessResponse();
		}

		public byte[] GetData()
		{
			var fetchedArticles = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.Offene_mat_bst_access.GetStdSupplierViolation(DateTime.Now.AddMonths(-3));
			return SaveToExcelFile(fetchedArticles);
		}

		internal byte[] SaveToExcelFile(IEnumerable<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.StdSupplierViolationEntity> articlesEntities)
		{

			string XLS_FORMAT_NUMBER = "0.0#####";
			string XLS_FORMAT_DATE = "dd/MM/yyyy";
			try
			{
				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"STD Lieferant");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 12; // updated
					var numberOfColumnstomerge = 2; // updated

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumnstomerge].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
					worksheet.Cells[1, 1].Value = $"Verstoß des Lieferanten";
					worksheet.Cells[1, 1].Style.Font.Size = 20;

					headerRowNumber += 1;
					// - Header End

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Bestellung Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Position";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Start Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Lieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Standardlieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "EK Price Second Source";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Preis von Standardlieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Total Second Source";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Total von Standardlieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Diff";

					var rowNumber = headerRowNumber + 1;
					if(articlesEntities is not null && articlesEntities.Count() > 0)
					{
						// Loop through 
						foreach(var w in articlesEntities)
						{
							// -
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Bestellung_Nr;//
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Datum;//
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Position;//
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Artikelnummer;//
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Start_Anzahl;//
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Lieferant;//
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Standardlieferant;//
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.EK_Price_Second_Source;//
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Preis_von_Standardlieferant;//
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Total_Second_Source;//
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Total_von_Standardlieferant;//
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Diff;//


							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = XLS_FORMAT_NUMBER;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					#region Makeup
					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber - 1, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
						range.Style.Font.Color.SetColor(Color.Black);

						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));

					using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
					}

					// Doc content
					if(articlesEntities != null && articlesEntities.Count() > 0)
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
					#endregion Makeup

					// Set some document properties
					package.Workbook.Properties.Title = $"STD Liefrant";
					package.Workbook.Properties.Author = "PSZ ERP MTM";
					package.Workbook.Properties.Company = "PSZ ERP MTM";

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
