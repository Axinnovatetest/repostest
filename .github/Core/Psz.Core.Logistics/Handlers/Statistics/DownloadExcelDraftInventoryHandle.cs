using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class DownloadExcelDraftInventoryHandle: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DraftInventory _data { get; set; }
		public DownloadExcelDraftInventoryHandle(DraftInventory _data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = _data;
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

				var response = new List<DraftInventoryListModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetDraftInventory(
				this._data.lagerOrt,
				null,
				null,
				  null


					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new DraftInventoryListModel(k)).ToList();
				int allCount = response.Select(x => x.totalRows).First();
				return ResponseModel<byte[]>.SuccessResponse(
				getExcel(response, _data, "DraftInventory")
					);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

		static byte[] getExcel(List<DraftInventoryListModel> MainRequestModel, DraftInventory _data, string title)
		{

			string filePath = Path.Combine(Path.GetTempPath(), $"DRAFT_INVENTUR - {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"DRAFT_INVENTUR - {DateTime.Now.ToString("yyyy/MM/dd")}");


					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 4;
					var startColumnNumber = 0;
					var numberOfColumns = Enum.GetNames(typeof(ExcelColumnNumber)).Length;
					using(var range = worksheet.Cells[1, 1])
					{
						range.Value = title + " Search :";
						range.Style.Font.Bold = true;
						range.Style.Font.UnderLine = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Font.Color.SetColor(Color.Green);
					}
					using(var range = worksheet.Cells[1, 2])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 1])
					{
						range.Value = "LagerOrt = ";

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 2])
					{
						range.Value = _data.lagerOrt;

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
					}
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ArtikelNr].Value = "Artikel-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.StorageID].Value = "Storage-ID";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.QuantityP3000].Value = "Quantity P3000";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.InventurQuantity].Value = "Inventur Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Difference].Value = "Difference";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.letzteBewegung].Value = "letzte Bewegung";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.CCID_Datum].Value = "CCID_Datum";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var item in MainRequestModel)
					{


						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ArtikelNr].Value = item.ArtikelNr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = item.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = item.Bezeichnung1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.StorageID].Value = item.StorageID;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.QuantityP3000].Value = item.QuantityP3000;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.InventurQuantity].Value = item.InventurQuantity;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Difference].Value = item.Difference;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.letzteBewegung].Value = (item.letzteBewegung == null) ? "" : item.letzteBewegung.Value;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.letzteBewegung].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.CCID_Datum].Value = (item.CCID_Datum == null) ? "" : item.CCID_Datum.Value;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.CCID_Datum].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					// Doc content
					if(MainRequestModel != null && MainRequestModel.Count > 0)
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

					//// Pre + Header
					using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

						//range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						//range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						//range.Style.Font.Color.SetColor(Color.Black);
						//range.Style.ShrinkToFit = true;
					}

					//// Totals



					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Faulty FAs";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();
				}
				// -
				return File.ReadAllBytes(filePath);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public enum ExcelColumnNumber
		{
			ArtikelNr = 1,
			Artikelnummer = 2,
			Bezeichnung1 = 3,
			StorageID = 4,
			QuantityP3000 = 5,
			InventurQuantity = 6,
			Difference = 7,
			letzteBewegung = 8,
			CCID_Datum = 9
		}
	}
}
