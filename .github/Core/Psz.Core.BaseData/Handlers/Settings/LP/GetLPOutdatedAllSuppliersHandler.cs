using System;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Psz.Core.BaseData.Models.CustomerSupplierLP;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.LP
{
	public class GetLPOutdatedAllSuppliersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.CustomerSupplierLP.LPModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetLPOutdatedAllSuppliersHandler(Identity.Models.UserModel user, int nr)
		{
			this._user = user;
			this._data = nr;
		}
		public ResponseModel<List<Models.CustomerSupplierLP.LPModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var LPEntity = Infrastructure.Data.Access.Joins.Kunden_LieferantenAccess.GetLPOutdatedAllSuppliersWithStandardSupplier(this._data);
				var response = new List<Models.CustomerSupplierLP.LPModel>();
				if(LPEntity != null && LPEntity.Count > 0)
				{
					foreach(var item in LPEntity)
					{
						response.Add(new Models.CustomerSupplierLP.LPModel(item));
					}
				}
				return ResponseModel<List<Models.CustomerSupplierLP.LPModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.CustomerSupplierLP.LPModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.CustomerSupplierLP.LPModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.CustomerSupplierLP.LPModel>>.SuccessResponse();
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
					worksheet.Cells[1, 1].Value = $"Veraltete Artikel für alle Lieferanten {DateTime.Now.ToString("dd.MM.yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestell-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Angebot";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Mindestbestellmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Wiederbeschäffungzeitraum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Standardlieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Standard";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Lieferant";


					var rowNumber = headerRowNumber + 1;
					if(data.Success == true && data.Body.Count > 0)
					{
						// Loop through 
						foreach(var w in data.Body)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Articlenumber;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Order_number;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.purchasing_price;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Offer;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Offer_date.HasValue == true ? w.Offer_date.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Minimum_Order_Quantity;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Replacement_period;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.StandardLiefrentenName;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = string.IsNullOrWhiteSpace(w?.StandardLiefrentenName) && string.IsNullOrWhiteSpace(w?.Name1) ? false : w?.StandardLiefrentenName?.Trim()?.ToLower() == w?.Name1?.Trim()?.ToLower();
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Name1;

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
					//for(int i = 1; i <= numberOfColumns; i++)
					//{
					//	worksheet.Column(i).AutoFit();
					//}
					worksheet.Column(1).AutoFit();
					worksheet.Column(numberOfColumns).AutoFit();

					// Set some document properties
					package.Workbook.Properties.Title = $"Lieferant - LP";
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
