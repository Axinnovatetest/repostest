using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	using Infrastructure.Data.Access;
	using Infrastructure.Data.Access.Joins.CTS;
	using iText.Layout.Font;
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.CustomerService.CsStatistics.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.Drawing;
	using iText.Layout.Font;

	public class GetFaProductionStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<FaProductionStatusSearchResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private FaProductionStatusSearchRequestModel _data { get; set; }
		public GetFaProductionStatusHandler(FaProductionStatusSearchRequestModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<FaProductionStatusSearchResponseModel> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}


			// -
			DateTime? dateFrom = null;
			DateTime? dateTo = null;
			switch(_data.Horizon)
			{
				case 1:
					{
						dateFrom = null;
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays); // 2024-01-25 - Khelil change H1 to 41 days
						break;
					}
				case 2:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + 1); // 2024-01-25 - Khelil change H1 to 41 days
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7);
						break;
					}
				case 3:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7 + 1);
						dateTo = null;
						break;
					}
				default:
					{
						dateFrom = null;
						dateTo = null;
						break;
					}
			}

			var allCount = 0;
			Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;

			#region > Data sorting & paging

			if(!this._data.FullData)
			{
				dataPaging = new Settings.PaginModel()
				{
					FirstRowNumber = _data.PageSize > 0 ? (_data.RequestedPage * _data.PageSize) : 0,
					RequestRows = _data.PageSize
				};
			}

			Settings.SortingModel dataSorting = null;
			if(!string.IsNullOrWhiteSpace(_data.SortField))
			{
				var sortFieldName = "";
				switch(_data.SortField.ToLower())
				{
					default:
					case "fertigungnummer":
						sortFieldName = "[Fertigungsnummer]";
						break;
					case "artikelnummmer":
						sortFieldName = "[Artikelnummer]";
						break;
					case "bermerkung":
						sortFieldName = "[Bermerkung]";
						break;
					case "anzahl":
						sortFieldName = "[Anzahl]";
						break;
					case "lagerort_id":
						sortFieldName = "[Lagerort_id]";
						break;
					case "werkstermin":
						sortFieldName = "[Werktermin]";
						break;
					case "status":
						sortFieldName = "Status";
						break;
				}

				dataSorting = new Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = _data.SortDesc,
				};
			}
			#endregion

			var FaProductionStatusFromDB = CSStatisticsAccess.GetFaProductionStatus(_data.SearchValue, _data.ProductionStatus, dateFrom, dateTo, dataSorting, dataPaging);

			allCount = FaProductionStatusFromDB.FirstOrDefault() == null ? 0 : FaProductionStatusFromDB.FirstOrDefault().TotalCount;


			return ResponseModel<FaProductionStatusSearchResponseModel>.SuccessResponse(new FaProductionStatusSearchResponseModel
			{
				Items = FaProductionStatusFromDB.Select(x => new FaProductionStatusResponseModel(x)).ToList(),
				PageRequested = _data.RequestedPage,
				PageSize = _data.PageSize,
				TotalCount = FaProductionStatusFromDB != null && FaProductionStatusFromDB.Count > 0 ? allCount : 0,
				TotalPageCount = FaProductionStatusFromDB != null && FaProductionStatusFromDB.Count > 0 ?
				_data.PageSize > 0 ? (int)Math.Ceiling(((decimal)allCount / _data.PageSize)) : 0 : 0,
			});

		}

		public ResponseModel<FaProductionStatusSearchResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.access.____*/)
			{
				return ResponseModel<FaProductionStatusSearchResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<FaProductionStatusSearchResponseModel>.SuccessResponse();
		}

		public byte[] GetExcelData()
		{
			var data = this.Handle();
			if(data == null || !data.Success || data.Body == null || data.Body.Items.Count <= 0)
			{
				return null;
			}
			try
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Fa-Prod-{DateTime.Now.ToString("yyyyMMddTHHmmss")}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 7;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					worksheet.Column(1).Width = 30;
					worksheet.Column(2).Width = 30;
					worksheet.Row(1).Height = 15;
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "FA Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Lager";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Produktionstermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Werktermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Status";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(data.Body != null && data.Body.Items.Count > 0)
					{
						foreach(var l in data.Body.Items/*.OrderBy(a => a.Artikelnummer)*/)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = l.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Lagerort_id;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.Produktionstermin;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Werktermin;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

							switch(l.Status)
							{
								case "Nicht Gestartet":
									worksheet.Cells[rowNumber, startColumnNumber + 6].Value = $"Nicht Gestartet";
									worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Fill.BackgroundColor.SetColor(Color.White);
									break;
								case "Werktermin im Rückstand":
									worksheet.Cells[rowNumber, startColumnNumber + 6].Value = $"Werktermin im Rückstand";
									worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Fill.BackgroundColor.SetColor(Color.Red);
									break;
								case "Werktermin Ok":
									worksheet.Cells[rowNumber, startColumnNumber + 6].Value = $"Werktermin Ok";
									worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Fill.BackgroundColor.SetColor(Color.Green);
									break;
								case "Werktermin zu spät":
									worksheet.Cells[rowNumber, startColumnNumber + 6].Value = $"Werktermin zu spät";
									worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Fill.BackgroundColor.SetColor(Color.Orange);
									break;
								default:
									break;
							}
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}
					// Doc content
					if(data.Body != null && data.Body.Items.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber, 1, rowNumber - 1, numberOfColumns - 1])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}

					//// Pre + Header
					using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}
					// Set some document properties
					package.Workbook.Properties.Title = $"Kapa-{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					// save our new workbook and we are done!

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
