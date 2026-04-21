using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class VKUpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Core.Common.Models.ImportFileModel _data { get; set; }


		public VKUpdateHandler(Identity.Models.UserModel user, Core.Common.Models.ImportFileModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var errors = new List<string>();
				var articles = new List<KeyValuePair<int, string>>();

				// -
				var data = ReadFromExcel(this._data.FilePath, errors, articles);
				if(errors != null && errors.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.FailureResponse(errors);
				}

				if(articles == null || articles.Count <= 0)
				{
					return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.SuccessResponse(null);
				}

				var articleIds = articles?.Select(x => x.Key)?.ToList();
				// - before shot
				var oldPriceEntities = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(articleIds);
				var results = new List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>();
				Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.UpdateStatisticsSalesPriceTrans(data, this._user.Name);

				// - after shot
				var newPriceEntities = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(articleIds);
				foreach(var oldPriceItem in oldPriceEntities)
				{
					var newPriceItem = newPriceEntities.Find(x => x.Artikel_Nr == oldPriceItem.Artikel_Nr);
					if(newPriceItem != null)
					{
						var articleIdx = articles.FindIndex(x => x.Key == oldPriceItem.Artikel_Nr);
						if(articleIdx >= 0)
						{
							results.Add(new Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel
							{
								//FIXME: TBD
								ArticleNumber = articles[articleIdx].Value,
								OldVKPrice = oldPriceItem.Verkaufspreis ?? 0,
								NewVKPrice = newPriceItem.Verkaufspreis ?? 0
							});
						}
					}
				}

				// -- Article level Logging
				if(results != null && results.Count > 0)
				{
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(results?.Select(x => x.ArticleNumber)?.ToList())
						?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();

					var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					foreach(var resultItem in results)
					{
						if(resultItem.OldVKPrice != resultItem.NewVKPrice)
						{
							var articleEntity = articleEntities.Find(x => x.ArtikelNummer.Trim().ToLower() == resultItem.ArticleNumber.Trim().ToLower());
							logs.Add(ObjectLogHelper.getLog(this._user, articleEntity?.ArtikelNr ?? 0,
							$"UpdateVKOnly",
							$"{resultItem.OldVKPrice}",
							$"{resultItem.NewVKPrice}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit));
						}
					}
					if(logs.Count > 0)
						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);

					//Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					//    results.Select(x=>
					//    ObjectLogHelper.getLog(this._user, 0,
					//    $"Article [UpdateVKOnly]",
					//    $"{x.ArticleNumber} | {x.OldVKPrice} >> {x.NewVKPrice}",
					//    $"",
					//    Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					//    Enums.ObjectLogEnums.LogType.Edit)
					//    )?.ToList());
				}

				// --
				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.SuccessResponse();
		}

		internal static List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenStatisticsEntity> ReadFromExcel(string filePath, List<string> errors, List<KeyValuePair<int, string>> articleIds)
		{
			try
			{
				var fileInfo = new System.IO.FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				var package = new ExcelPackage(fileInfo);
				var worksheet = package.Workbook.Worksheets[0];
				var rowStart = worksheet.Dimension.Start.Row;
				var rowEnd = worksheet.Dimension.End.Row;

				// footer rows
				rowEnd -= 0;

				// get number of rows and columns in the sheet
				var rows = worksheet.Dimension.Rows;
				var columns = worksheet.Dimension.Columns;
				var startRowNumber = 2;
				var startColNumber = 1;
				var MIN_ROWS_COUNT = 1;
				var MIN_COLUMNS_COUNT = 8;


				if(rows >= MIN_ROWS_COUNT && columns >= MIN_COLUMNS_COUNT)
				{
					var data = new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenStatisticsEntity> { };
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get()
						?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();

					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							var _articleNumber = Core.Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber]);
							var _price1 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 1]);
							var _bis1 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 2]);
							var _price2 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 3]);
							var _bis2 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 4]);
							var _price3 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 5]);
							var _bis3 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 6]);
							var _price4 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 7]);

							if(string.IsNullOrWhiteSpace(_articleNumber))
							{
								//errors.Add($"Row {i}: invalid Article number [{_articleNumber}].");
								continue;
							}

							var _articleIdx = articleEntities.FindIndex(x => x.ArtikelNummer.Trim().ToLower() == _articleNumber.Trim().ToLower());
							if(_articleIdx < 0)
							{
								errors.Add($"Row {i}: Article number [{_articleNumber}] not found.");
								continue;
							}
							var idy = articleIds.FindIndex(x => x.Key == articleEntities[_articleIdx].ArtikelNr);
							if(idy >= 0)
							{
								errors.Add($"Row {i}: duplicate article number [{_articleNumber}].");
								continue;
							}

							if(_price1 <= 0)
							{
								errors.Add($"Row {i}: invalid value: Price 1 [{_price1}].");
								continue;
							}
							// - 2022-03-02 Frischholz
							//if (_price2 <= 0)
							//{
							//    errors.Add($"Row {i}: invalid value: Price 2 [{_price2}].");
							//    continue;
							//}
							//if (_price3 <= 0)
							//{
							//    errors.Add($"Row {i}: Invalid value. Price 3 [{_price3}].");
							//    continue;
							//}

							if(_price1 < _price2)
							{
								errors.Add($"Row {i}: Invalid value. Price 1 [{_price1}] < Price 2 [{_price2}].");
								continue;
							}
							if(_price2 > 0 && _price2 < _price3)

							{
								errors.Add($"Row {i}: Invalid value. Price 2 [{_price2}] < Price 3 [{_price3}].");
								continue;
							}
							if(_price2 > 0 && _price3 > 0 && _price3 < _price4)

							{
								errors.Add($"Row {i}: Invalid value. Price 3 [{_price3}] < Price 4 [{_price4}].");
								continue;
							}

							// - everything is good
							data.Add(
								new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenStatisticsEntity
								{
									Artikelnummer = _articleNumber,
									Artikel_Nr = -1,
									Bis1 = _bis1,
									Bis2 = _bis2,
									Bis3 = _bis3,
									Nr = -1,
									Preis1 = _price1,
									Preis2 = _price2,
									Preis3 = _price3,
									Preis4 = _price4
								});
							// -
							articleIds.Add(new KeyValuePair<int, string>(articleEntities[_articleIdx].ArtikelNr, articleEntities[_articleIdx].ArtikelNummer));
						} catch(System.Exception exceptionInternal)
						{
							Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
							errors.Add($"Row {i}: unknown error.");
						}
					}


					return data;
				}
				else
				{
					errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
					return null;
				}
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}

		public static byte[] GetDataXLS(List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel> data)
		{
			try
			{
				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"VKUpdate-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"VKUpdate");

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
					worksheet.Cells[1, 1].Value = $"VK-Update - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Alter Preis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Neuer Preis";


					var rowNumber = headerRowNumber + 1;
					{
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.ArticleNumber;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.OldVKPrice;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.NewVKPrice;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
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
					if(data != null && data.Count > 0)
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
					package.Workbook.Properties.Title = $"VKUpdate - OUT";
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
