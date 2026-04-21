using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class GetArtikellisteRahmenHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		public GetArtikellisteRahmenHandler(UserModel user)
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
			var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetRahmenlist();

			var bestellnummernEntities = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByStandardSupplier(statisticsEntities.Select(x => x.ArtikelNr)?.ToList());
			var addressEntites = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(bestellnummernEntities?.Select(x => x.Lieferanten_Nr ?? -1)?.ToList());

			return SaveToExcelFile(statisticsEntities, bestellnummernEntities, addressEntites);
		}

		internal byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_Rahmen> dataEntities,
			List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> bestellnummernEntities,
			List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity> adressenEntities
			)
		{
			try
			{
				var chars = new char[] { ' ', '#' };
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Rahmenliste-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Rahmenliste");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 10;

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
					worksheet.Cells[1, 1].Value = $"Rahmenliste";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Rahmen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Rahmen-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Rahmenmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Rahmenauslauf";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Restmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Standardlieferant / Bestellnummer";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							var bestellnummern = bestellnummernEntities.FirstOrDefault(x => x.ArtikelNr == w?.ArtikelNr);
							var address = adressenEntities.FirstOrDefault(x => x.Nr == (bestellnummern?.Lieferanten_Nr ?? -1));
							// -
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.ArtikelNummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Bezeichnung1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung2;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Rahmen;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Rahmen_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Rahmenmenge;
							//worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = "#";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Rahmenauslauf;
							//worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Rahmenrest;
							//worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = "#";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = bestellnummern?.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = $"{address?.Name1}# {bestellnummern?.Bestell_Nr}".Trim(chars);
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
					//worksheet.Column(6).Style.Numberformat.Format = "#.######";
					//worksheet.Column(8).Style.Numberformat.Format = "#.######";
					//worksheet.Column(9).Style.Numberformat.Format = "#.######";
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
