using System.Drawing;
using OfficeOpenXml;
using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Handlers.HistoryFG
{
	public class FGBestandHeaderAnalysisHandler
	{
		private Identity.Models.UserModel _user { get; set; }
		private Psz.Core.CRP.Models.HistoryFG.AnalysisEntryModel _data { get; set; }
		public FGBestandHeaderAnalysisHandler(Identity.Models.UserModel user, Models.HistoryFG.AnalysisEntryModel data)
		{
			this._user = user;
			_data = data;
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
			var HistoryDetailsFGBestand = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryDetailsFGBestandAccess.Get();

			/*if(articleProductionPlace == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.TN)
			{
				storageLocation = Enums.OrderEnums.GetArticleHauplager(Psz.Core.Apps.EDI.Enums.OrderEnums.ArticleProductionPlace__.WS);
			}*/
			//HistoryDetailsFGBestandEntity.Select(e => e.Auslieferlager) = storageLocation.Key;

			return SaveToExcelFile(HistoryDetailsFGBestand);
		}

		internal byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> HistoryDetailsFGBestandEntity)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"FG_Details_Besatnd-{DateTime.Now:yyyyMMddTHHmmss}.xlsx");
				var file = new FileInfo(filePath);

				// EPPlus License Context
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage(file))
				{
					var worksheet = package.Workbook.Worksheets.Add("FG_Details_Besatnd-Data");

					// Header settings
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var headers = new[] {
				"Article Designation 1", "Article Designation 2", "Article Number", "Article Release Status", "CS Contact",
				"EDI Standard", "Header ID", "Stock Quantity", "Total Costs With CU",
				"Total Costs Without CU", "Total Sales Price", "UBG", "Unit Sales Price", "Warehouse ID", "Warehouse Name"
			};

					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Title (Merged Header)
					worksheet.Cells[1, 1, 1, headers.Length].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"FG_Details_Besatnd_Data-{DateTime.Now:dd/MM/yyyy}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9E1F2"));

					// Header Row
					for(int i = 0; i < headers.Length; i++)
					{
						worksheet.Cells[headerRowNumber, startColumnNumber + i].Value = headers[i];
					}

					// Data Rows
					var rowNumber = headerRowNumber + 1;
					if(HistoryDetailsFGBestandEntity != null)
					{
						foreach(var item in HistoryDetailsFGBestandEntity)
						{
							worksheet.Cells[rowNumber, 1].Value = item?.ArticleDesignation1;
							worksheet.Cells[rowNumber, 2].Value = item?.ArticleDesignation2;
							worksheet.Cells[rowNumber, 3].Value = item?.ArticleNumber;
							worksheet.Cells[rowNumber, 4].Value = item?.ArticleReleaseStatus;
							worksheet.Cells[rowNumber, 5].Value = item?.CsContact;
							worksheet.Cells[rowNumber, 6].Value = item?.EdiStandard;
							worksheet.Cells[rowNumber, 7].Value = item?.HeaderId;
							worksheet.Cells[rowNumber, 8].Value = item?.StockQuantity;
							worksheet.Cells[rowNumber, 9].Value = item?.TotalCostsWithCu;
							worksheet.Cells[rowNumber, 10].Value = item?.TotalCostsWithoutCu;
							worksheet.Cells[rowNumber, 11].Value = item?.TotalSalesPrice;
							worksheet.Cells[rowNumber, 12].Value = item?.UBG;
							worksheet.Cells[rowNumber, 13].Value = item?.UnitSalesPrice;
							worksheet.Cells[rowNumber, 14].Value = item?.WarehouseId;
							worksheet.Cells[rowNumber, 15].Value = item?.WarehouseName;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber++;
						}
					}

					// Styling
					using(var range = worksheet.Cells[1, 1, headerRowNumber, headers.Length])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(Color.Black);
					}

					using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, headers.Length])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					for(int i = 1; i <= headers.Length; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Document Properties
					package.Workbook.Properties.Title = "FG_Details_Besatnd-Data";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					package.Save();

					return File.ReadAllBytes(filePath);
				}
			} catch(Exception ex)
			{
				throw new Exception("Error generating Excel file.", ex);
			}
		}


	}

}



