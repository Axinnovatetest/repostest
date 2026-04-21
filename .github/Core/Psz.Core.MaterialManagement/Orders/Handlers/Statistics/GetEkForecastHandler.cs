using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using Infrastructure.Data.Entities.Tables.BSD;
using OfficeOpenXml;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetEkForecastHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		public GetEkForecastHandler(UserModel user)
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

				return ResponseModel<byte[]>.SuccessResponse(getXlsData());
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

		internal byte[] getXlsData()
		{
			try
			{
				var data = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Details();
				if(data is null || data.Count <= 0)
				{
					return null;
				}

				using(var stream = new MemoryStream())
				{
					// FIXME: Replace EPPlus by NPOI, or some other alt
					OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
					// Create the package and make sure you wrap it in a using statement
					using(var package = new OfficeOpenXml.ExcelPackage())
					{
						// Group the data by FaProductionSite to avoid multiple data passes
						var groupedData = data.GroupBy(x => x.FaProductionSite).OrderBy(g => g.Key);

						foreach(var warehouseGroup in groupedData)
						{
							// warehouseGroup.Key is the FaProductionSite (warehouse)
							// warehouseGroup is the collection of items for that warehouse
							addDetailedHistory(package, warehouseGroup, warehouseGroup.Key);
						}
						// Set some document properties
						package.Workbook.Properties.Title = $"Data";
						package.Workbook.Properties.Author = "PSZ ERP";
						package.Workbook.Properties.Company = "PSZ ERP";

						//-	
						return package.GetAsByteArray();
					}
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		internal void addDetailedHistory(OfficeOpenXml.ExcelPackage package, IEnumerable<MaterialRequirementsFaNeedsSnapshotEntity> data, int warehouse)
		{

			OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"L{warehouse}");
			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 2;
			var startColumnNumber = 1;
			var numberOfColumns = 8;

			// Add some formatting to the worksheet
			worksheet.TabColor = System.Drawing.Color.Yellow;
			worksheet.DefaultRowHeight = 11;
			worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
			worksheet.Row(2).Height = 20;
			worksheet.Row(1).Height = 30;
			worksheet.Row(headerRowNumber).Height = 20;

			// Pre Header
			worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
			worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
			worksheet.Cells[1, 1].Value = $"Data L{warehouse}";
			worksheet.Cells[1, 1].Style.Font.Size = 16;

			// Start adding the header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "FA-Number";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "FA-Article";
			worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "FA-Quantity";
			worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "FA-Date";
			worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "MA-Article";
			worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "MA-Quantity";
			worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "MA-Date";
			worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "FA-Lager";
			// - 

			var rowNumber = headerRowNumber + 1;
			if(data != null && data.Count() > 0)
			{
				// Loop through 
				foreach(var w in data)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.FaNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.FaArticleNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.FaOpenQuantity;
					worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.FaDate?.ToString("dd.MM.yyyy");
					worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.FaMaterialArticleNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.FaMaterialOpenQuantity;
					worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.FaMaterialDate?.ToString("dd.MM.yyyy");
					worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.FaProductionSite;

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
				range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
				range.Style.ShrinkToFit = false;
			}
			// Darker Blue in Top cell
			worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White/*System.Drawing.ColorTranslator.FromHtml("#D9E1F2")*/);

			// Doc content
			if(data != null && data.Count() > 0)
			{
				using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
				{
					range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
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
		}
	}
}
