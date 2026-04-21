using OfficeOpenXml;
using System;
using System.Drawing;
using System.IO;


namespace Psz.Core.BaseData.Handlers.Article.Statistics.Logistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetOutHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Logistics.OutResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Logistics.OutRequestModel _data { get; set; }
		public GetOutHandler(UserModel user, Models.Article.Statistics.Logistics.OutRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Logistics.OutResponseModel> Handle()
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

				string sortFieldName = "";
				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					switch(this._data.SortField.ToLower())
					{
						case "psz_number":
							sortFieldName = "[PSZ_Nummer]";
							break;
						case "customernr":
							sortFieldName = "[Kundennummer]";
							break;
						case "type":
							sortFieldName = "[Typ]";
							break;
						case "number":
							sortFieldName = "[Nummer]";
							break;
						case "ordered":
							sortFieldName = "[Bestellt]";
							break;
						case "opencurrent":
							sortFieldName = "[OffenAktuell]";
							break;
						case "reference":
							sortFieldName = "[Bezug]";
							break;
						case "booked":
							sortFieldName = "[gebucht]";
							break;
						case "distributionwarehouse":
							sortFieldName = "[Auslieferlager]";
							break;
						case "Completed":
							sortFieldName = "[Erledigt]";
							break;
						default:
						case "date":
							sortFieldName = "[Termin]";
							break;
					}
				}

				#endregion

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetOut(this._data.ArticleNr, this._data.SearchTerm, this._data.Booked, this._data.Completed, sortFieldName, this._data.SortDesc, this._data.RequestedPage, this._data.PageSize);
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetOut_Count(this._data.ArticleNr, this._data.SearchTerm, this._data.Booked, this._data.Completed);

				//- 
				var responseBody = new Models.Article.Statistics.Logistics.OutResponseModel();

				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (this._data.PageSize <= 0 ? 1 : this._data.PageSize));
				responseBody.PageSize = this._data.PageSize;
				responseBody.PageRequested = this._data.RequestedPage;
				responseBody.Items = results?.Select(x => new Models.Article.Statistics.Logistics.OutResponseItem(x))?.ToList();

				return ResponseModel<Models.Article.Statistics.Logistics.OutResponseModel>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Logistics.OutResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Logistics.OutResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleNr) == null)
				return ResponseModel<Models.Article.Statistics.Logistics.OutResponseModel>.FailureResponse("Article not found.");

			return ResponseModel<Models.Article.Statistics.Logistics.OutResponseModel>.SuccessResponse();
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
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Logistics OUT");

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
					worksheet.Cells[1, 1].Value = $"Logistics - OUT - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PSZ nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Kundennummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Typ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bestellt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Öffen/Aktuell";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bezug";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Gebucht";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Auslieferlager";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Erledigt";


					var rowNumber = headerRowNumber + 1;
					if(data.Success == true)
					{
						// Loop through 
						foreach(var w in data.Body.Items)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.PSZ_Number;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.CustomerNr;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Type;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Number;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = !string.IsNullOrWhiteSpace(w?.Ordered) ? decimal.TryParse(w?.Ordered, out var d) ? d : 0m : 0m;
							// - worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = "#";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = !string.IsNullOrWhiteSpace(w?.OpenCurrent) ? decimal.TryParse(w?.OpenCurrent, out var e) ? e : 0m : 0m;
							// - worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = "#";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Date.HasValue == true ? w.Date.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Reference;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Booked?.Trim()?.ToLower() == "true" ? "Yes" : "No";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.DistributionWarehouse;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Completed?.Trim()?.ToLower() == "true" ? "Yes" : "No";

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
					package.Workbook.Properties.Title = $"Logistics - OUT";
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
