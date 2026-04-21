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

	public class MainHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int lagerId { get; set; }
		public MainHandler(Identity.Models.UserModel user, int lagerId)
		{
			this._user = user;
			this.lagerId = lagerId;
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
				var title = "Bestandlist Lager";
				var response = new List<MainRequestModel>();

				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.DownloadExcelBestandlistLagerAccess(this.lagerId);
				//if(PackingListEntity != null && PackingListEntity.Count > 0)
				response = PackingListEntity.Select(k => new MainRequestModel(k)).ToList();
				//else
				//	return ResponseModel<byte[]>.FailureResponse("Empty file sent.");

				return ResponseModel<byte[]>.SuccessResponse(getExcel(response, lagerId, title));
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
		static byte[] getExcel(List<MainRequestModel> MainRequestModel, int lagerId, string title)
		{

			string filePath = Path.Combine(Path.GetTempPath(), $"PSZ_Inventurliste komplett {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PSZ_Inventurliste komplett - {DateTime.Now.ToString("yyyy/MM/dd")}");


					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 4;
					var startColumnNumber = 0;
					var numberOfColumns = Enum.GetNames(typeof(ExcelColumnNumber)).Length;
					// Search :

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
						range.Value = " LagerOrt = ";

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 2])
					{
						range.Value = lagerId;

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
					}

					//Search.Style.Font.Color.SetColor(Color.Red);
					//Search.Style.Font.UnderLine = true;
					//var rangeSearch = worksheet.Cells[1, 2, 3, 4];
					//rangeSearch.Merge = true;
					//rangeSearch.Value = "lagerOrt = "+ lagerId;
					//rangeSearch.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//rangeSearch.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//rangeSearch.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//rangeSearch.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					//rangeSearch.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Sklad].Value = "Sklad";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.PosledniTransakce].Value = "Poslední Transakce";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.CisloPSZ].Value = "Cislo PSZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Mnozstvi].Value = "Množství";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.CisloVyrobce].Value = "Cislo Vyrobce";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Kontrolaok].Value = "Kontrola ok?";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.WE_VOH_ID].Value = "WE_VOH_ID";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var item in MainRequestModel)
					{

						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Sklad].Value = item.Sklad;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.PosledniTransakce].Value = (item.letzteBewegung == null) ? "" : item.letzteBewegung.Value;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.PosledniTransakce].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.CisloPSZ].Value = item.CisloPSZ;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Mnozstvi].Value = item.Mnozstvi;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.CisloVyrobce].Value = item.CisloVyrobce;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Kontrolaok].Value = item.KontrolaOk;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.WE_VOH_ID].Value = item.WE_VOH_ID;

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
			Sklad = 1,
			PosledniTransakce = 2,
			CisloPSZ = 3,
			Mnozstvi = 4,
			CisloVyrobce = 5,
			Kontrolaok = 6,
			WE_VOH_ID = 7
		}
	}


}
