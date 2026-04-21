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
	public class GetCTSDashboardByCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CTSDashboardItemModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ManagementOverview.CTS.Models.DashboardRequestModel _data { get; set; }

		public GetCTSDashboardByCustomerHandler(Identity.Models.UserModel user, ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<List<CTSDashboardItemModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<List<CTSDashboardItemModel>>.SuccessResponse(
					Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSDashboardAccess(this._data.DateTill ?? DateTime.Today.AddDays(7 * 5), this._data.CustomerName)
					?.Select(x => new CTSDashboardItemModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<CTSDashboardItemModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<CTSDashboardItemModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<CTSDashboardItemModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<CTSDashboardItemModel>>.SuccessResponse();
		}

		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"CTSDashboard-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"CTS-Dashboard");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 7;

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
					worksheet.Cells[1, 1].Value = $"CTS Dashboard {this._data.CustomerName}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					var rowNumber = 1;
					#region Items
					if(data.Success == true)
					{
						var totalAmount = 0m;
						var immAmount = 0m;
						var prodAmount = 0m;

						headerRowNumber = rowNumber + 1;
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "AB Bedarf";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "AB Gesamt";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bestand";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Open FAs";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Sofort Umsatz";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "FA Umsatz";

						rowNumber = headerRowNumber + 1;
						if(data.Body != null && data.Body.Count > 0)
						{
							//Loop through
							foreach(var w in data.Body)
							{

								totalAmount += w?.ABGesamt ?? 0;
								immAmount += w?.ImmediatAmount ?? 0;
								prodAmount += w?.ProductionAmount ?? 0;

								worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.ABBedarf;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.ABGesamt;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bestand;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.OpenFa;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.ImmediatAmount;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.ProductionAmount;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}

						// - side Summary
						int decay = 1;
						//worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0, headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 1].Merge = true;
						//worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						//worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
						//worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						//worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0].Value = $"{this._data.CustomerName}";
						//worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0].Style.Font.Size = 16;

						worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0].Value = "Umsatz Total";
						worksheet.Cells[headerRowNumber + 3, startColumnNumber + numberOfColumns + decay + 0].Value = "Sofort Umsatz (Lager)";
						worksheet.Cells[headerRowNumber + 5, startColumnNumber + numberOfColumns + decay + 0].Value = "FA Umsatz (nach AB)";
						worksheet.Cells[headerRowNumber + 7, startColumnNumber + numberOfColumns + decay + 0].Value = "Ergebnis(+/-)";

						worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 0].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[headerRowNumber + 3, startColumnNumber + numberOfColumns + decay + 0].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[headerRowNumber + 5, startColumnNumber + numberOfColumns + decay + 0].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[headerRowNumber + 7, startColumnNumber + numberOfColumns + decay + 0].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

						worksheet.Cells[headerRowNumber + 1, startColumnNumber + numberOfColumns + decay + 1].Value = totalAmount;
						worksheet.Cells[headerRowNumber + 3, startColumnNumber + numberOfColumns + decay + 1].Value = immAmount;
						worksheet.Cells[headerRowNumber + 5, startColumnNumber + numberOfColumns + decay + 1].Value = prodAmount;
						worksheet.Cells[headerRowNumber + 7, startColumnNumber + numberOfColumns + decay + 1].Value = immAmount + prodAmount;
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
						worksheet.Cells[headerRowNumber + 9, startColumnNumber + numberOfColumns + decay + 1].Value = $"FA erstellen ( AB ohne Produktion bis {this._data.DateTill?.ToString("dd-MM-yyyy")})";
						worksheet.Cells[headerRowNumber + 10, startColumnNumber + numberOfColumns + decay + 1].Value = $"AB erstellen (Prod ohne Bedarf bis {this._data.DateTill?.ToString("dd-MM-yyyy")})";
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
					package.Workbook.Properties.Title = $"CTSDashboard";
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
