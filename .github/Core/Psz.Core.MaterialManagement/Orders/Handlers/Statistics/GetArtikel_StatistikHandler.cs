using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using OfficeOpenXml;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.IO;


namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetArtikel_StatistikHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetArtikel_StatistikHandler(UserModel user, int data)
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
			List<GetArtikelStatisticsEntity> fetcheddata = new();

			if(_data <= 0)
				return null;

			fetcheddata = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.Offene_mat_bst_access.GetArtikel_Statistik(_data, null, 1);
			return SaveToExcelFile(fetcheddata);
		}

		internal byte[] SaveToExcelFile(
			List<GetArtikelStatisticsEntity> articlesEntities
			)
		{
			string XLS_FORMAT_NUMBER = "0.#######";
			string XLS_FORMAT_NUMBER_2 = "0.#############";
			//string XLS_FORMAT_DATE = "dd/MM/yyyy";
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
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Artikel Statistiks");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 10; // updated


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
					worksheet.Cells[1, 1].Value = $"Artikel Statistiks";
					worksheet.Cells[1, 1].Style.Font.Size = 20;



					// - Header Start
					// - First Column
					//worksheet.Cells[headerRowNumber + 0, startColumnNumber].Value = "Production :";
					//worksheet.Cells[headerRowNumber + 0, startColumnNumber + 1].Value = _data;
					//worksheet.Cells[headerRowNumber + 0, startColumnNumber + 1].Style.Numberformat.Format = XLS_FORMAT_DATE;

					// - Second Column
					//var shiftCols = 2;
					//	worksheet.Cells[headerRowNumber + 2, startColumnNumber  + 1].Value = articlesEntities.FirstOrDefault().Lagerort_id.ToString();


					headerRowNumber += 2;
					// - Header End

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "EK";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bestand";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Sicherheitsbestand";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Lagerort";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bedarf 1Mo";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Gesamt bedarf max 1 Jahr 1";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "offBest";//
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Entnahme der letzen 12 monate";//

					var rowNumber = headerRowNumber + 1;
					if(articlesEntities is not null && articlesEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in articlesEntities)
						{
							// -
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;//
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Bezeichnung_1;//
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.EK == -1 ? "" : w.EK;//
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bestand == -1 ? "" : w.Bestand;//
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Sicherheitsbestand == -1 ? "" : w.Sicherheitsbestand;//
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Lagerort == -1 ? "" : w.Lagerort;//
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Bedarf_1Mo == -1 ? "" : w.Bedarf_1Mo;//
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Gesamtbedarfmax1Jahr == -1 ? "" : w.Gesamtbedarfmax1Jahr;//
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.offBest == -1 ? "" : w.offBest;//
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Entnahme_der_letzen_12_monate == -1 ? "" : w.Entnahme_der_letzen_12_monate;//



							if(0 < w.EK - Math.Truncate(w.EK))
							{
								worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							}

							if(0 < w.Bestand - Math.Truncate(w.Bestand))
							{
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = XLS_FORMAT_NUMBER_2;
							}

							if(0 < w.Bedarf_1Mo - Math.Truncate(w.Bedarf_1Mo))
							{
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = XLS_FORMAT_NUMBER_2;
							}
							if(0 < w.Gesamtbedarfmax1Jahr - Math.Truncate(w.Gesamtbedarfmax1Jahr))
							{
								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = XLS_FORMAT_NUMBER_2;
							}

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
					package.Workbook.Properties.Title = $"Artikel Statistiks";
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
