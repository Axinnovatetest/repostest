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
	public class GetReasonChangeCommitteeStatsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ReasonChangeCommitteeResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ReasonChangeCommitteeRequestModel _data { get; set; }

		public GetReasonChangeCommitteeStatsHandler(Identity.Models.UserModel user, ReasonChangeCommitteeRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<ReasonChangeCommitteeResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				///
				var reasonEntities = Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.GetReasonChangeCommittee(
					_data.From, _data.To, _data.ArticleNumber, _data.LagerId);

				var responseBody = new List<ReasonChangeCommitteeResponseModel> { };
				if(reasonEntities is not null)
				{
					foreach(var item in reasonEntities)
					{
						responseBody.Add(new ReasonChangeCommitteeResponseModel
						{
							Anzahl = item.Anzahl,
							Datum = item.Datum.HasValue ? item.Datum.Value.ToShortDateString() : string.Empty,
							Grund = item.Grund,
							LagerId = item.LagerId,
							Typ = item.Typ,
							ArticleNumber = item.Articlenummer,
							Id = item.Id,
							ArtikelNr = item.ArtikelNr
						});
					}
				}

				return ResponseModel<List<ReasonChangeCommitteeResponseModel>>.SuccessResponse(responseBody);
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

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"GrundnderungAusschuss-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"GrundnderungAusschuss");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 6;

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
					worksheet.Cells[1, 1].Value = $"GrundnderungAusschuss {this._data.ArticleNumber}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					var rowNumber = 1;
					#region Items
					if(data.Success == true)
					{
						//var totalAmount = 0m;
						//var immAmount = 0m;
						//var prodAmount = 0m;

						headerRowNumber = rowNumber + 1;
						// Start adding the header



						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Typ";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Datum";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Anzahl";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "LagerId";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Grund";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "ArticleNumber";

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

								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Typ;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Datum;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Anzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.LagerId;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Grund;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.ArticleNumber;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}

						int decay = 1;

						using(var range = worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0, headerRowNumber + 7, startColumnNumber + numberOfColumns + decay + 1])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
						// Thick countour
						using(var range = worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0, headerRowNumber + 7, startColumnNumber + numberOfColumns + decay + 1])
						{
							range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
						}

						// - Legend
						worksheet.Cells[headerRowNumber + 9, startColumnNumber + numberOfColumns + decay + 0].Value = "+";
						worksheet.Cells[headerRowNumber + 10, startColumnNumber + numberOfColumns + decay + 0].Value = "-";
						//worksheet.Cells[headerRowNumber + 9, startColumnNumber + numberOfColumns + decay + 1].Value = $"FA erstellen ( AB ohne Produktion bis {this._data.DateTill?.ToString("dd-MM-yyyy")})";
						//worksheet.Cells[headerRowNumber + 10, startColumnNumber + numberOfColumns + decay + 1].Value = $"AB erstellen (Prod ohne Bedarf bis {this._data.DateTill?.ToString("dd-MM-yyyy")})";
						using(var range = worksheet.Cells[headerRowNumber + 9, startColumnNumber + numberOfColumns + decay + 0, headerRowNumber + 10, startColumnNumber + numberOfColumns + decay + 1])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}

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
					package.Workbook.Properties.Title = $"GrundnderungAusschuss";
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

		public ResponseModel<List<ReasonChangeCommitteeResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<ReasonChangeCommitteeResponseModel>>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<ReasonChangeCommitteeResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<ReasonChangeCommitteeResponseModel>>.SuccessResponse();
		}
	}
}
