using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using Psz.Core.Common.Models;


namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class Fehler_AuswertungABAccessHandler
	{
		private Identity.Models.UserModel _user { get; set; }
		public Fehler_AuswertungABAccessHandler(Identity.Models.UserModel user)
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
			var Fehler_AuswertungEntity = Infrastructure.Data.Access.Joins.CTS.Fehler_AuswertungABAccess.GetFehlerAuswertungAB();

			return SaveToExcelFile(Fehler_AuswertungEntity);
		}

		internal byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Joins.CTS.Fehler_AuswertungABEntity> Fehler_AuswertungEntity
			)
		{
			try
			{
				var chars = new char[] { ' ', '#' };

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Fehler Auswertung AB");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 9;

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
					worksheet.Cells[1, 1].Value = $"Fehler Auswertung AB Data [{DateTime.Now.ToString("dd/MM/yyyy")}]";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Dokument-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Position";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Vorfall-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Artikel-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Menge Offen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Liefertermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Auslieferlager";

					var rowNumber = headerRowNumber + 1;
					{
						if(Fehler_AuswertungEntity != null)
						{
							// Loop through 
							foreach(var w in Fehler_AuswertungEntity)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Kunde;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.DokumentNr;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Position;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.VorfallNr;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Bezeichnung1;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Mengeoffen;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.liefertermin.HasValue == true ? w?.liefertermin : "N/V";
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Auslieferlager is not null ? w?.Auslieferlager : "N/V";
								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;

							}
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
					if(Fehler_AuswertungEntity != null)
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
					package.Workbook.Properties.Title = $"Fehler-Auswertung-AB-Data";
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



