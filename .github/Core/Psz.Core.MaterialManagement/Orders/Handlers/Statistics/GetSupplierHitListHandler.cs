using OfficeOpenXml;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetSupplierHitListHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<SupplierHitListResponseModel>>
	{

		private SupplierHitListRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetSupplierHitListHandler(Identity.Models.UserModel user, SupplierHitListRequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public async Task<ResponseModel<SupplierHitListResponseModel>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var results = await Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetSupplierHitCounts(this._data.ArticleNumber, this._data.DateFrom, this._data.DateTo, this._data.RequestedPage, this._data.PageSize);
				var allCount = await Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetSupplierHitCounts_Count(this._data.ArticleNumber, this._data.DateFrom, this._data.DateTo);

				var responseBody = new SupplierHitListResponseModel();

				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / this._data.PageSize);
				responseBody.PageSize = this._data.PageSize;
				responseBody.PageRequested = this._data.RequestedPage;
				responseBody.Items = results?.Select(x => new SupplierHitListItem(x))?.ToList();

				return await ResponseModel<SupplierHitListResponseModel>.SuccessResponseAsync(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				if(exception.Message.Contains("Timeout expired"))
					return await ResponseModel<SupplierHitListResponseModel>.FailureResponseAsync("Request timeout expired. Please reduce the date range.");
				else
					throw;
			}
		}
		public async Task<ResponseModel<SupplierHitListResponseModel>> ValidateAsync()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return await ResponseModel<SupplierHitListResponseModel>.AccessDeniedResponseAsync();
			}

			return await ResponseModel<SupplierHitListResponseModel>.SuccessResponseAsync();
		}
		public async Task<byte[]> SaveToExcelFile()
		{
			try
			{
				this._data.PageSize = 10000;
				var data = await this.HandleAsync();
				var dataEntities = new List<SupplierHitListItem>();
				if(data.Success)
				{
					dataEntities = data.Body.Items;
				}

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Supplier_Hitlist-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Supplier_Hitlist");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 2;

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
					worksheet.Cells[1, 1].Value = $"Supplier_Hitlist";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Name1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Einkaufsvolumen";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = string.Format("{0:N}", w?.Einkaufsvolumen) + " €";

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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"Highrunner";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

					return await File.ReadAllBytesAsync(filePath);
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}

	}
}
