using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using OfficeOpenXml;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetBestandProWerkohneBedarfHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetBestandProWerkohneBedarfHandler(UserModel user, int data)
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
			List<BestandProWerkohneBedarfEntity> fetchedArticles = new();

			var lagers = Psz.Core.MaterialManagement.Helpers.SpecialHelper.GetLagersPerMainLager(_data);

			if(lagers is null)
				throw new InvalidDataException("No Lager Found");

			fetchedArticles = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.Offene_mat_bst_access.GetBestandProWerkohneBedarf(lagers.ElementAt(0), lagers.ElementAt(1), lagers.ElementAt(2), lagers.ElementAt(3), lagers.ElementAt(4), lagers.ElementAt(5), lagers.ElementAt(6), lagers.ElementAt(7));

			return SaveToExcelFile(fetchedArticles, this._data);
		}

		internal byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.BestandProWerkohneBedarfEntity> articlesEntities
			, int lager)
		{

			string XLS_FORMAT_NUMBER = "0.0#####";
			//string XLS_FORMAT_DATE = "dd/MM/yyyy";
			try
			{
				var chars = new char[] { ' ', '#' };
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Dispo-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Bestand Pro Werk ohne Bedarf");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 8; // updated
					var numberOfColumnstomerge = 2; // updated

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
					worksheet.Cells[1, 1].Value = $"Bestand Pro Werk ohne Bedarf";
					worksheet.Cells[1, 1].Style.Font.Size = 20;




					// - Second Column
					//var shiftCols = 2;




					headerRowNumber += 2;
					// - Header End
					if(lager == 6)
					{
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestand";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "BedarfTN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "BedarfBETN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "BedarfWS";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "BedarfAL";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "BedarfDE";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "BedarfGZTN";//
					}
					if(lager == 42)
					{
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestand";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "BedarfTN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "BedarfBETN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "BedarfCZ";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "BedarfAL";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "BedarfDE";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "BedarfGZTN";//
					}
					if(lager == 26)
					{
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestand";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "BedarfTN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "BedarfBETN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "BedarfWS";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "BedarfCZ";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "BedarfDE";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "BedarfGZTN";//
					}
					if(lager == 15)
					{
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestand";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "BedarfTN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "BedarfBETN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "BedarfWS";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "BedarfAL";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "BedarfCZ";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "BedarfGZTN";//
					}
					if(lager == 7)
					{
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestand";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "BedarfCZ";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "BedarfBETN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "BedarfWS";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "BedarfAL";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "BedarfDE";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "BedarfGZTN";//
					}
					if(lager == 60)
					{
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestand";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "BedarfTN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "BedarfCZ";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "BedarfWS";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "BedarfAL";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "BedarfDE";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "BedarfGZTN";//
					}
					if(lager == 102)
					{
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestand";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "BedarfTN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "BedarfBETN";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "BedarfWS";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "BedarfAL";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "BedarfDE";//
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "BedarfCZ";//
					}


					var rowNumber = headerRowNumber + 1;
					if(articlesEntities is not null && articlesEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in articlesEntities)
						{
							// -
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;//
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Bestand;//
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.BedarfTN;//
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.BedarfBETN;//
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.BedarfWS;//
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.BedarfAL;//
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.BedarfDE;//
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.BedarfGZTN;//

							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = XLS_FORMAT_NUMBER;

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
					package.Workbook.Properties.Title = $"Bestand Pro Werk ohne Bedarf";
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