using OfficeOpenXml;
using System;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.Linq;
	using System.Threading.Tasks;

	public class GetArticleLogisticsHandler: IHandleAsync<UserModel, ResponseModel<Models.Article.Statistics.Basics.ArticleLogisticsResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.ArticleLogisticsRequestModel _data { get; set; }
		public GetArticleLogisticsHandler(UserModel user, Models.Article.Statistics.Basics.ArticleLogisticsRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<Models.Article.Statistics.Basics.ArticleLogisticsResponseModel>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};

				var sortFieldName = "";
				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					switch(this._data.SortField.ToLower())
					{
						default:
						case "artikelnummer":
							sortFieldName = "[ArtikelNummer]";
							break;
					}
				}
				#endregion

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetArticleLogistics(this._data.ArticleEndings, this._data.ArticleNumber, this._data.hasProductionPlace, this._data.activeOnly ?? true, sortFieldName, this._data.SortDesc, this._data.RequestedPage, this._data.PageSize);
				var allCount = await Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetArticleLogistics_Count(this._data.ArticleEndings, this._data.ArticleNumber, this._data.hasProductionPlace, this._data.activeOnly ?? true);
				var extensionEntities = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(results?.Select(x => x.ArtikelNr)?.ToList());
				var lagerortEntities = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(extensionEntities?.Select(x => x.ProductionPlace1_Id ?? -1)?.ToList());
				//- 
				var responseBody = new Models.Article.Statistics.Basics.ArticleLogisticsResponseModel();

				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (this._data.PageSize <= 0 ? 1 : this._data.PageSize));
				responseBody.PageSize = this._data.PageSize;
				responseBody.PageRequested = this._data.RequestedPage;
				responseBody.Items = results?.Select(x =>
				{
					var ext = extensionEntities.FirstOrDefault(y => y.ArticleId == x.ArtikelNr);
					var lag = lagerortEntities.FirstOrDefault(z => z.Lagerort_id == ext?.ProductionPlace1_Id);
					return new Models.Article.Statistics.Basics.ArticleLogisticsItem(x, lag);
				})?.ToList();

				return ResponseModel<Models.Article.Statistics.Basics.ArticleLogisticsResponseModel>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public Task<ResponseModel<Models.Article.Statistics.Basics.ArticleLogisticsResponseModel>> ValidateAsync()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Basics.ArticleLogisticsResponseModel>.AccessDeniedResponseAsync();
			}

			return ResponseModel<Models.Article.Statistics.Basics.ArticleLogisticsResponseModel>.SuccessResponseAsync();
		}
		public async Task<byte[]> GetDataXLS()
		{
			try
			{
				var data = await this.HandleAsync();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"ArticleLogistics-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Article Logistics");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 3;

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
					worksheet.Cells[1, 1].Value = $"Article Logistics - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Produktionstätte";


					var rowNumber = headerRowNumber + 1;
					if(data.Success == true && data.Body.Items != null && data.Body.Items.Count > 0)
					{
						// Loop through 
						foreach(var w in data.Body.Items)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.ArticleNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Designation1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.ProductionPlaceName;

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
						if(data.Body != null && data.Body.Items != null && data.Body.Items.Count > 0)
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
					worksheet.Column(2).Width = 50; // force width for 2nd column 
													// Set some document properties
					package.Workbook.Properties.Title = $"Logistics - Liste";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

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
