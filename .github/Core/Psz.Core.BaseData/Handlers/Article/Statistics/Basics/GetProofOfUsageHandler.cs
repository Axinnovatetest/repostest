using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.Linq;

	public class GetProofOfUsageHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Basics.ProofOfUsageResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.ProofOfUsageRequestModel _data { get; set; }
		public GetProofOfUsageHandler(UserModel user, Models.Article.Statistics.Basics.ProofOfUsageRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Basics.ProofOfUsageResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProofOfUsage(this._data.ArticleId, this._data.CustomerNumber)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProofOfUsage>();

				return ResponseModel<Models.Article.Statistics.Basics.ProofOfUsageResponseModel>.SuccessResponse(
					new Models.Article.Statistics.Basics.ProofOfUsageResponseModel
					{
						ArticleNumber = articleEntity.ArtikelNummer,
						Designation1 = articleEntity.Bezeichnung1,
						Designation2 = articleEntity.Bezeichnung2,
						GoodsGroup = articleEntity.Warengruppe,
						Items = statisticsEntities.Select(x => new Models.Article.Statistics.Basics.ProofOfUsageResponseModel.Item(x))?.ToList()
					});
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Basics.ProofOfUsageResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Basics.ProofOfUsageResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<Models.Article.Statistics.Basics.ProofOfUsageResponseModel>.FailureResponse("Article not found");

			return ResponseModel<Models.Article.Statistics.Basics.ProofOfUsageResponseModel>.SuccessResponse();
		}
		public byte[] GetData()
		{
			var data = Handle();
			if(data.Success == false)
				return null;

			// - 
			return getExcelData(data.Body);
		}
		internal byte[] getExcelData(Models.Article.Statistics.Basics.ProofOfUsageResponseModel data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"{data.ArticleNumber}-VN-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{data.ArticleNumber} Verwendungsnachweis");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 11;
					var preHeaderRowNumber = 5;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 40;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"[Artikel / Kundencode: {data.ArticleNumber}] Verwendungsnachweis";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// Details
					worksheet.Cells[headerRowNumber + 0, startColumnNumber + 0].Value = "Datum";
					worksheet.Cells[headerRowNumber + 0, startColumnNumber + 1].Value = $"{data.CurrentDate}";
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 1].Value = $"{data.ArticleNumber}";
					worksheet.Cells[headerRowNumber + 2, startColumnNumber + 0].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber + 2, startColumnNumber + 1].Value = $"{data.Designation1}";
					worksheet.Cells[headerRowNumber + 3, startColumnNumber + 0].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber + 3, startColumnNumber + 1].Value = $"{data.Designation2}";
					worksheet.Cells[headerRowNumber + 4, startColumnNumber + 0].Value = "Warengruppe";
					worksheet.Cells[headerRowNumber + 4, startColumnNumber + 1].Value = $"{data.GoodsGroup}";
					headerRowNumber = headerRowNumber + preHeaderRowNumber + 1;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Position";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Freigabe";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Sysmonummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Rahmennummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Rahmen";


					var rowNumber = headerRowNumber + 1;
					var dataEntities = data.Items;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = $"{w?.Artikelnummer}";
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = $"{w?.Bezeichnung_1}";
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = $"{w?.Bezeichnung_2}";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = $"{w?.Position}";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Freigabestatus;

							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = $"{w?.Sysmonummer}";
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = $"{w?.Rahmen_Nr}";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Rahmenmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Rahmenauslauf.HasValue == true ? w?.Rahmenauslauf?.ToString("dd/MM/yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = $"{(w?.Rahmen == true ? "Yes" : "No")}";

							worksheet.Row(rowNumber).Height = 20;
							rowNumber += 1;
						}
					}

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
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
					package.Workbook.Properties.Title = $"{data} VK - Preisliste";
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
