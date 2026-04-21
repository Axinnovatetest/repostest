using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Controlling
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class GetDBHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Controlling.DBResponseModel>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetDBHandler(UserModel user, int number)
		{
			this._user = user;
			this._data = number;
		}
		public ResponseModel<List<Models.Article.Statistics.Controlling.DBResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Controlling.GetDB(this._data);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Controlling.DBResponseModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Controlling.DBResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Controlling.DBResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Controlling.DBResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Controlling.DBResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.Statistics.Controlling.DBResponseModel>>.FailureResponse("Article not found.");

			return ResponseModel<List<Models.Article.Statistics.Controlling.DBResponseModel>>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = Path.GetTempPath();
				var filePath = Path.Combine(tempFolder, $"DB-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"DB");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 16;

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
					worksheet.Cells[1, 1].Value = $"Datum - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					int rowNumber = 0;

					if(data.Success)
					{
						var _data = data.Body;
						if(_data != null && _data.Count > 0)
						{
							// - doc. Header
							worksheet.Cells[headerRowNumber + 0, 1].Value = "Kalkulation für:";
							worksheet.Cells[headerRowNumber + 1, 1].Value = _data[0].ArtikelnummerOriginal;
							worksheet.Cells[headerRowNumber + 2, 1].Value = _data[0].Bezeichnung_1;
							headerRowNumber += 3;

							// - table - Start adding the header
							worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Position";
							worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Material";
							worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung";
							worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Standardlieferant";
							worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Anzahl";
							worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "EK";
							worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "MGK";
							worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Kupfer";
							worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Summe mit CU";
							worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Summe Basis 150";

							rowNumber = headerRowNumber + 1;
							// Loop through 
							foreach(var w in _data)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Position;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung_des_Bauteils;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Name1;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Anzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Einkaufspreis.HasValue == true ? w.Einkaufspreis.Value : "";
								worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Gewicht.HasValue == true ? w.Gewicht.Value : "";
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Kupferzuschlag.HasValue == true ? w.Kupferzuschlag.Value : "";
								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.SummeEK.HasValue == true ? w.SummeEK.Value : "";
								worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.SummeEKohneCU.HasValue == true ? w.SummeEKohneCU.Value : "";
								worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}

							#region // - table - Footer - //
							// - Summe Material
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = "Summe Material";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = getSumMaterial(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = getSumMaterial_wo(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber++;

							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = "Kalkulatorische Kosten";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = getKalkulatorischeKosten(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = getKalkulatorischeKosten_wo(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber++;

							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = "EK PSZ";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = getEK(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = getEK_wo(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber++;

							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = "VK PSZ";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = getVK(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = getVK_wo(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber++;

							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = "DB";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = getDB(_data); // vk1 - ek1
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = getDB_wo(_data); // vk2 - ek2
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber++;

							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = "Prozent";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = getPercent(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = getPercent_wo(_data);
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber++;
							#endregion // - table - Footer - //
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
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9E1F2"));

					if(data.Success == true)
					{
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
					package.Workbook.Properties.Title = $"DB";
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
		decimal getSumMaterial(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return data == null || data.Count <= 0 ? 0 : data.Sum(x => x.SummeEK ?? 0);
		}
		decimal getSumMaterial_wo(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return data == null || data.Count <= 0 ? 0 : data.Sum(x => x.SummeEKohneCU ?? 0);
		}
		decimal getKalkulatorischeKosten(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return data == null || data.Count <= 0 ? 0 : data[0].SummevonBetrag ?? 0;
		}
		decimal getKalkulatorischeKosten_wo(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return data == null || data.Count <= 0 ? 0 : data[0].SummevonBetrag ?? 0;
		}
		decimal getEK(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return getKalkulatorischeKosten(data) + getSumMaterial(data);
		}
		decimal getEK_wo(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return getKalkulatorischeKosten_wo(data) + getSumMaterial_wo(data);
		}
		decimal getVK(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return data == null || data.Count <= 0 ? 0 : data[0].VK_PSZ_ink_Kupfer ?? 0;
		}
		decimal getVK_wo(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return data == null || data.Count <= 0 ? 0 : data[0].Verkaufspreis_1 ?? 0;
		}
		decimal getDB(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return getVK(data) - getEK(data);
		}
		decimal getDB_wo(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			return getVK_wo(data) - getEK_wo(data);
		}
		decimal getPercent(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			var ek = getEK(data);
			return 100 * getDB(data) / (ek == 0 ? 1 : ek);
		}
		decimal getPercent_wo(List<Models.Article.Statistics.Controlling.DBResponseModel> data)
		{
			var ek = getEK_wo(data);
			return 100 * getDB_wo(data) / (ek == 0 ? 1 : ek);
		}
	}
}
