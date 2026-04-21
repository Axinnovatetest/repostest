using Infrastructure.Data.Access.Joins.BSD;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using static Psz.Core.BaseData.Models.Article.Statistics.Sales.ArticleMinStockDetailedAnalysisModels;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Sales
{
	public class GetMinStockDetailedAnalysisHandler: IHandle<UserModel, ResponseModel<ArticleMinStockDetailedAnalysisResponseModel>>
	{
		private UserModel _user { get; set; }
		private ArticleMinStockDetailedAnalysisRequestModel _data { get; set; }

		public GetMinStockDetailedAnalysisHandler()
		{

		}

		public GetMinStockDetailedAnalysisHandler(UserModel user, ArticleMinStockDetailedAnalysisRequestModel data)
		{
			this._user = user;
			this._data = data;

		}
		public ResponseModel<ArticleMinStockDetailedAnalysisResponseModel> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;

				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var sortFieldName = "";
				switch(this._data.SortField.ToLower())
				{
					default:
					case "articlenumber":
						sortFieldName = "[Artikelnummer]";
						break;
					case "articlenr":
						sortFieldName = "[ArticleNr]";
						break;
					case "mindesbestand":
						sortFieldName = "[Mindestbestand]";
						break;
					case "bestand":
						sortFieldName = "[Bestand]";
						break;
					case "vkmitcu150einzelpreis":
						sortFieldName = "[Vkmitcu150Einzelpreis]";
						break;
					case "vkmitcu150mindestbestandgesamtpreis":
						sortFieldName = "[Vkmitcu150MindestbestandGesamtpreis]";
						break;
					case "vkmitcueinzelpreis":
						sortFieldName = "[VkmitcuEinzelpreis]";
						break;
					case "vkmitcumindestbestandgesamtpreis":
						sortFieldName = "[VkmitcuMindestbestandGesamtpreis]";
						break;
					case "vkcu150herstellkostenenizel":
						sortFieldName = "[Vkcu150Herstellkosten]";
						break;
					case "vkcudelHerstellkostenEnizel":
						sortFieldName = "[VkCuDelHerstellkosten]";
						break;
					case "vkcu150herstellkostengesamtpreis":
						sortFieldName = "[GesamtpreisHerstellkosten150]";
						break;
					case "vkcudelherstellkostengesamtpreis":
						sortFieldName = "[GesamtpreisHerstellkostenDel]";
						break;
				}

				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this._data.SortDesc,
				};

				if(!this._data.FullData)
				{
					dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
					{
						FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
						RequestRows = this._data.PageSize
					};
				};

				var results = ArticleMinStockHistoryAccess.getMinStockDetailedAnalysis(
					this._data.ArticleNumber,
						dataSorting,
						dataPaging);

				int totalCount = ArticleMinStockHistoryAccess.getMinStockDetailedAnalysis_Count(this._data.ArticleNumber);



				return ResponseModel<ArticleMinStockDetailedAnalysisResponseModel>.SuccessResponse(
				   new ArticleMinStockDetailedAnalysisResponseModel()
				   {
					   Items = results,
					   PageSize = this._data.PageSize,
					   PageRequested = this._data.RequestedPage,
					   TotalCount = results != null && results.Count > 0 ? totalCount : 0,
					   TotalPageCount = results != null && results.Count > 0 ?
						this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)totalCount / this._data.PageSize)) : 0 : 0
				   });

			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}


		public ResponseModel<byte[]> GetDataXLS(UserModel user, ArticleMinStockDetailedAnalysisRequestModel data)
		{
			try
			{
				data.FullData = true;
				var results = new GetMinStockDetailedAnalysisHandler(user, data).Handle();

				if(!results.Success)
				{
					return ResponseModel<byte[]>.FailureResponse(results.Errors?.Select(x => x.Value)?.ToList());
				}

				if(results.Body.Items.Count <= 0)
				{
					return ResponseModel<byte[]>.FailureResponse("Empty data");
				}

				var _data = results.Body;
				// - 
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Min Stock Detailed Analysis");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 11;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 23;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Article Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Mindestbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Einzelpreis (VK CU 150)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Gesamtpreis Mindestbestand (VK CU 150)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Einzelpreis (VK CU DEL)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Gesamtpreis Mindestbestand (VK CU DEL)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Herstellkosten Einzel (VK CU 150)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Herstellkosten Einzel (VK CU DEL)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Gesamtpreis Herstellkosten (VK CU 150)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Gesamtpreis Herstellkosten (VK CU DEL)";



					var rowNumber = headerRowNumber;
					//Loop through
					foreach(var l in _data.Items)
					{
						rowNumber++;
						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = l.ArticleNumber;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Mindesbestand;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Bestand;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.Einzelpreis_VK_CU150;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Mindestbestandgesamtpreis_VK_CU150;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.Einzelpreis_VK_PSZ_ink_Kupfer;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.Mindestbestandgesamtpreis_VK_PSZ_ink_Kupfer;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.Einzelpreis_VK_CU150_Herstellkosten;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.GesamtPreis_Herstellerkosten_CU_150;
						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.Enizelpreis_VK_CU_DEL_Herstellkosten;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.GesamtPreis_Herstellerkosten_CU_Del;



						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;


						worksheet.Row(rowNumber).Height = 20;
					}




					using(var range = worksheet.Cells[headerRowNumber, 1, rowNumber, numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}
					// Format headers
					worksheet.Row(1).Height = 15;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9E1F2"));
					// Set some document properties
					package.Workbook.Properties.Title = $"MinStock-DetailedAnalysis-{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					// save our new workbook and we are done!


					return ResponseModel<byte[]>.SuccessResponse(package.GetAsByteArray());
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public ResponseModel<ArticleMinStockDetailedAnalysisResponseModel> Validate()
		{
			if(this._user == null || (!this._user.SuperAdministrator && this._user.Access.MasterData.ArticleStatistics != true))
			{
				return ResponseModel<ArticleMinStockDetailedAnalysisResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<ArticleMinStockDetailedAnalysisResponseModel>.SuccessResponse();
		}

	}
}
