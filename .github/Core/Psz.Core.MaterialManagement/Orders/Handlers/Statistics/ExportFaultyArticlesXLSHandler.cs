using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using OfficeOpenXml;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;


namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class ExportFaultyArticlesXLSHandler: IHandle<int, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public ExportFaultyArticlesXLSHandler(UserModel user, int data)
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
			List<Dispows120Entity> fetchedArticles = new();

			fetchedArticles = _data switch
			{
				1 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws90(null, null, "", 1),
				2 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getws40New(null, null, "", 1),
				3 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetTN90(null, null, "", 1),
				4 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetTN40(null, null, "", 1),
				5 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getbetn90(null, null, "", 1),
				6 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getbetn40(null, null, "", 1),
				7 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ90(null, null, "", 1),
				8 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCZ30(null, null, "", 1),
				9 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getal90(null, null, "", 1),
				10 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.Getal30(null, null, "", 1),
				11 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ90(null, null, "", 1),
				12 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetGZ40(null, null, "", 1),
				13 => Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetDE90(null, null, "", 1),
				_ => null

			};

			return SaveToExcelFile(fetchedArticles);
		}

		internal byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.Dispows120Entity> articlesEntities
			)
		{

			string XLS_FORMAT_NUMBER = "0.0#####";
			string XLS_FORMAT_DATE = "dd/MM/yyyy";
			try
			{
				var chars = new char[] { ' ', '#' };
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Dispo-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Faulty Articles");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 14; // updated

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
					worksheet.Cells[1, 1].Value = $"Dispo ";
					worksheet.Cells[1, 1].Style.Font.Size = 20;

					// - Header Start
					// - First Column

					// - Second Column
					//var shiftCols = 3;
					worksheet.Cells[headerRowNumber + 1, startColumnNumber].Value = "Lager :";
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 1].Value = articlesEntities.FirstOrDefault().Lagerort_id.ToString();

					//	worksheet.Cells[headerRowNumber + 2, startColumnNumber  + 1].Value = articlesEntities.FirstOrDefault().Lagerort_id.ToString();


					headerRowNumber += 3;
					// - Header End

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Name1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Stucklisten_Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "SummevonBruttobedarf";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "MaxvonTermin_Materialbedarf";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Differenz";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Mindestbestellmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Lagerort ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Lagerort_id";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Rahmen_Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Rahmenmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "obsolet";

					var rowNumber = headerRowNumber + 1;
					if(articlesEntities is not null && articlesEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in articlesEntities)
						{
							// -
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Stücklisten_Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.SummevonBruttobedarf;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.MaxvonTermin_Materialbedarf;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Differenz;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Mindestbestellmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Lagerort;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Lagerort_id;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Rahmen_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Rahmenmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w.obsolet ? "YES" : "NO";

							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = XLS_FORMAT_NUMBER;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					#region Makeup
					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber - 2, numberOfColumns])
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
					if(articlesEntities != null && articlesEntities.Count > 0)
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
					package.Workbook.Properties.Title = $"Dispo Faulty Articles";
					package.Workbook.Properties.Author = "PSZ ERP MTM";
					package.Workbook.Properties.Company = "PSZ ERP MTM";

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
