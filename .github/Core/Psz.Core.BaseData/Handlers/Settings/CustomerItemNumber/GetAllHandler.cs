using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.CustomerItemNumber
{
	using OfficeOpenXml;
	using System.Drawing;
	using System.IO;
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<List<Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get()?.Select(x => new Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel>>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"LP-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Lieferant - LP");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
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
					worksheet.Cells[1, 1].Value = $"Kundennummern - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Nummerschlüssel";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Stufe";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "CS Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Technik Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Technik Kontakt TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Projektbetreuer D";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Kundennummer";


					var rowNumber = headerRowNumber + 1;
					if(data.Success == true && data.Body.Count > 0)
					{
						// Loop through 
						foreach(var w in data.Body)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Nummerschlussel;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Kunde;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Stufe;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.CS_Kontakt;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Technik_Kontakt;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Technik_Kontakt_TN;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Projektbetreuer_D;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Kundennummer;

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

					if(data.Success == true)
					{
						// Doc content
						if(data.Body != null && data.Body.Count > 0)
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
					package.Workbook.Properties.Title = $"Kundennummern";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

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
