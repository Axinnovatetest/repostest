using MoreLinq.Extensions;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Statistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.ManagementOverview.Statistics.Handlers
{
	public class PSZFGTNStatsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<PSZFGTNResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private PSZFGTNRequestModel _data { get; set; }

		public PSZFGTNStatsHandler(Identity.Models.UserModel user, PSZFGTNRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<PSZFGTNResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				///
				var prefixView = "";
				switch(_data.CountryCode)
				{
					case CountryCode.AL:
					case CountryCode.TN:
					case CountryCode.BETN:
					case CountryCode.GZTN:
						prefixView = "_TN";
						break;
					case CountryCode.CZ:
						prefixView = "_CZ";
						break;
					case CountryCode.KHTN:
						prefixView = "_WS";
						break;
					default:
						break;
				}
				var reasonEntities = Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.GetPSZFGTN(
					_data.CountryCode.GetDescription(), _data.From, _data.To, prefixView);

				var responseBody = new List<PSZFGTNResponseModel> { };
				if(reasonEntities is not null)
				{
					foreach(var item in reasonEntities)
					{
						responseBody.Add(new PSZFGTNResponseModel(item));
					}
				}

				return ResponseModel<List<PSZFGTNResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();
				var title = _data.CountryCode == CountryCode.KHTN ? "WS" : _data.CountryCode.GetDescription();
				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"PSZFG-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PSZFG-{title}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = _data.CountryCode == CountryCode.AL ? 7 : 9;

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
					worksheet.Cells[1, 1].Value = $"PSZFG-{title} {this._data.From.ToString("dd.MM.yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					var rowNumber = 1;
					#region Items
					if(data.Success == true)
					{
						headerRowNumber = rowNumber + 1;
						// Start adding the header


						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = $"Umsatz_PSZ_{title}";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = $"Arbeitskosten_PSZ_{title}";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Stundensatz";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Produktionszeit";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Marge ohne CU";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Marge mit CU";
						if(_data.CountryCode != CountryCode.AL)
						{
							worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Produktivitat (FA Zeit)";
							worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Produktivitat (Artikel Zeit)";
						}

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
								if(_data.CountryCode != CountryCode.AL)
								{
									worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
									worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								}

								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Umsatz_PSZ_TN;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Arbeitskosten_PSZ_TN;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Stundensatz;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Produktionszeit;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Marge_ohne_CU;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Marge_mit_CU;
								if(_data.CountryCode != CountryCode.AL)
								{
									worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Produktivitat__FA_Zeit_;
									worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Produktivitat_Artikelzeit_;
								}

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}

						int decay = 1;
						//using(var range = worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0, headerRowNumber + 7, startColumnNumber + numberOfColumns + decay + 1])
						//{
						//	range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						//	range.Style.Fill.BackgroundColor.SetColor(Color.White);
						//	range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						//	range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						//	range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						//	range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						//}
						//// Thick countour
						//using(var range = worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0, headerRowNumber + 7, startColumnNumber + numberOfColumns + decay + 1])
						//{
						//	range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
						//}

						// - Legend
						//worksheet.Cells[headerRowNumber + 9, startColumnNumber + numberOfColumns + decay + 0].Value = "+";
						//worksheet.Cells[headerRowNumber + 10, startColumnNumber + numberOfColumns + decay + 0].Value = "-";
						//worksheet.Cells[headerRowNumber + 9, startColumnNumber + numberOfColumns + decay + 1].Value = $"FA erstellen ( AB ohne Produktion bis {this._data.DateTill?.ToString("dd-MM-yyyy")})";
						//worksheet.Cells[headerRowNumber + 10, startColumnNumber + numberOfColumns + decay + 1].Value = $"AB erstellen (Prod ohne Bedarf bis {this._data.DateTill?.ToString("dd-MM-yyyy")})";
						//using(var range = worksheet.Cells[headerRowNumber + 9, startColumnNumber + numberOfColumns + decay + 0, headerRowNumber + 10, startColumnNumber + numberOfColumns + decay + 1])
						//{
						//	range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						//	range.Style.Fill.BackgroundColor.SetColor(Color.White);
						//	range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						//	range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						//	range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						//	range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						//}

						worksheet.Column(startColumnNumber + numberOfColumns + decay + 0).AutoFit();
						worksheet.Column(startColumnNumber + numberOfColumns + decay + 1).AutoFit();
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
					package.Workbook.Properties.Title = $"PSZFGTN";
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

		public ResponseModel<List<PSZFGTNResponseModel>> Validate()
		{
			if(this._data.CountryCode == null)
			{
				return ResponseModel<List<PSZFGTNResponseModel>>.FailureResponse(key: "2", value: "Invalid Country Code");
			}
			//if(this._user == null)
			//{
			//	return ResponseModel<List<PSZFGTNResponseModel>>.AccessDeniedResponse();
			//}
			//var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			//if(userEntity == null)
			//	return ResponseModel<List<PSZFGTNResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<PSZFGTNResponseModel>>.SuccessResponse();
		}
	}
}
