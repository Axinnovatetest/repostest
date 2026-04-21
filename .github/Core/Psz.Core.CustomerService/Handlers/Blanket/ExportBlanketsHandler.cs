using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Drawing;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class ExportBlanketsHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private bool? _data { get; set; }
		public ExportBlanketsHandler(Identity.Models.UserModel user, bool? data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var data = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetExportBlanket(false);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage())
				{
					var worksheet = package.Workbook.Worksheets.Add("Sheet1");
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 13;

					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Rahmen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Doc Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Originalmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Restmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Einzelpreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Preis Restmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Startdatunm";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Enddatum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Status";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Erschöpfungsgrad";

					var rowNumber = headerRowNumber + 1;
					if(data != null && data.Count > 0)
					{
						foreach(var p in data)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = p.BlanketNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.BlanketDocNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.SupplierName;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.ArticleNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.ArticleDesignation1;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.ArticleDesignation2;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.OriginalQuantity;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.RestQuantity;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.UnitPrice;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.RestPrice;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.StartDate.ToString("dd/MM/yyyy");
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.EndDate.ToString("dd/MM/yyyy");
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Status;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.Consumption;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							
							rowNumber += 1;
						}
					}
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

						string color = "#fff";
						for(int i = 0; i < data.Count; i++)
						{
							switch(data[i].Status)
							{
								case nameof(Enums.BlanketEnums.RAStatus.InProgress):
									color = "#FFCC99";
									break;
								case nameof(Enums.BlanketEnums.RAStatus.Validated):
									color = "#47FF47";
									break;
								case nameof(Enums.BlanketEnums.RAStatus.Canceled):
									color = "#FF4747";
									break;
								case nameof(Enums.BlanketEnums.RAStatus.Closed):
									color = "#99A7AD";
									break;
								case nameof(Enums.BlanketEnums.RAStatus.Draft):
								default:
									break;
							}
							worksheet.Cells[i+ headerRowNumber + 1, startColumnNumber + 12].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							worksheet.Cells[i+ headerRowNumber + 1, startColumnNumber + 12].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(color));
						}
					}

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					// Fit the columns according to its content - 25052025-09-12 - remove Hejdukova
					//for(int i = 1; i <= numberOfColumns; i++)
					//{
					//	worksheet.Column(i).AutoFit();
					//}

					// Set some document properties
					package.Workbook.Properties.Title = "Rahmens";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();
					return ResponseModel<byte[]>.SuccessResponse(package.GetAsByteArray());
				}

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw e;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

	}
}
