using OfficeOpenXml;
using System;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Logistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.Linq;

	public class GetInOutDetailsHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Logistics.InOutDetailsResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Logistics.InOutDetailsRequestModel _data { get; set; }
		public GetInOutDetailsHandler(UserModel user, Models.Article.Statistics.Logistics.InOutDetailsRequestModel nummer)
		{
			this._user = user;
			this._data = nummer;
		}
		public ResponseModel<Models.Article.Statistics.Logistics.InOutDetailsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
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
					switch(this._data.SortField?.ToLower())
					{
						case "articlenumber":
							sortFieldName = "[ArtikelNummer]";
							break;
						case "type":
							sortFieldName = "[Typ]";
							break;
						case "ordernr":
							sortFieldName = "[Bestellung-Nr]";
							break;
						case "number":
							sortFieldName = "[Anzahl]";
							break;
						default:
						case "":
						case "date":
							sortFieldName = "[Datum]";
							break;
						case "name":
							sortFieldName = "[Name1]";
							break;
						case "storagelocationbefore":
							sortFieldName = "[Lagerplatz_von]";
							break;
						case "storagelocationafter":
							sortFieldName = "[Lagerplatz_nach]";
							break;
						case "rollennummer":
							sortFieldName = "[Rollennummer]";
							break;
						case "gebucht_von":
							sortFieldName = "[Gebucht von]";
							break;
						case "bemerkung":
							sortFieldName = "[Bemerkung]";
							break;
					}

				#endregion

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetInOutDetails(this._data.ArticleNumber, this._data.Date.HasValue ? this._data.Date.Value.ToString("yyyyMMdd") : "", this._data.Name, this._data.OrderNr, this._data.Type, this._data.StorageLocationBefore, this._data.StorageLocationAfter, this._data.Rollennummer, sortFieldName, this._data.SortDesc, this._data.RequestedPage, this._data.PageSize);
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetInOutDetails_Count(this._data.ArticleNumber, this._data.Date.HasValue ? this._data.Date.Value.ToString("yyyyMMdd") : "", this._data.Name, this._data.OrderNr, this._data.Type, this._data.StorageLocationBefore, this._data.StorageLocationAfter, this._data.Rollennummer);

				//- 
				var responseBody = new Models.Article.Statistics.Logistics.InOutDetailsResponseModel();

				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (this._data.PageSize <= 0 ? 1 : this._data.PageSize));
				responseBody.PageSize = this._data.PageSize;
				responseBody.PageRequested = this._data.RequestedPage;
				responseBody.Items = results?.Select(x => new Models.Article.Statistics.Logistics.InOutDetailItem(x))?.ToList();

				return ResponseModel<Models.Article.Statistics.Logistics.InOutDetailsResponseModel>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Logistics.InOutDetailsResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Logistics.InOutDetailsResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber) == null)
				return ResponseModel<Models.Article.Statistics.Logistics.InOutDetailsResponseModel>.FailureResponse("Article not found.");

			return ResponseModel<Models.Article.Statistics.Logistics.InOutDetailsResponseModel>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Logistics-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Logistics IN/OUT");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 11;

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
					worksheet.Cells[1, 1].Value = $"Logistics - IN/OUT - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PSZ Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Typ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bestell-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Lagerplatz von";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Lagerplatz nach";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Rollernummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Gebucht von";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Bemerkung";



					var rowNumber = headerRowNumber + 1;
					if(data.Success == true && data.Body.Items != null && data.Body.Items.Count > 0)
					{
						// Loop through 
						foreach(var w in data.Body.Items)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.ArticleNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Type;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.OrderNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = !string.IsNullOrWhiteSpace(w?.Number) ? decimal.TryParse(w?.Number, out var d) ? d : 0m : 0m;
							// - worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = "#";
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Date.HasValue == true ? w.Date.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Name;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.StorageLocationBefore;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.StorageLocationAfter;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Rollennummer;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Gebucht_von;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Bemerkung;

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

					// Set some document properties
					package.Workbook.Properties.Title = $"Logistics - INOUT";
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
