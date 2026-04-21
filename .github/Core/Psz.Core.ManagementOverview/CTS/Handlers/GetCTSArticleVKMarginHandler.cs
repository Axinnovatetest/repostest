using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetCTSArticleVKMarginHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleVKMarginResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetCTSArticleVKMarginHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<List<ArticleVKMarginResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<List<ArticleVKMarginResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSArticleVKMarginAccess(this._data)
					?.Select(x => new ArticleVKMarginResponseModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<ArticleVKMarginResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<ArticleVKMarginResponseModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<ArticleVKMarginResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<ArticleVKMarginResponseModel>>.SuccessResponse();
		}

		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"ArticleVKMargin-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"VK-Margin");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 12;

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
					worksheet.Cells[1, 1].Value = $"Article VK [Margin: {this._data}%]";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					var rowNumber = 1;
					#region Items
					if(data.Success == true)
					{
						headerRowNumber = rowNumber + 1;
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "SUM Material mit CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "SUM Material ohne CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Kalkulatorische kosten";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "EK mit CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "EK ohne CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "VK PSZ";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "DB I mit CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "DB I ohne CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Prozent mit CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Prozent ohne CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Freigabestatus";

						rowNumber = headerRowNumber + 1;
						if(data.Body != null && data.Body.Count > 0)
						{
							//Loop through
							foreach(var w in data.Body)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.SUM_Material_Mit_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.SUM_Material_ohne_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Kalkulatorische_kosten;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.EK_Mit_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.EK_ohne_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.VK_PSZ;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.DB_I_Mit_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.DB_I_Ohne_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Prozent_Mit_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Prozent_Ohne_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Freigabestatus;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}
					#endregion items

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
					package.Workbook.Properties.Title = $"ArticleVK";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

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
