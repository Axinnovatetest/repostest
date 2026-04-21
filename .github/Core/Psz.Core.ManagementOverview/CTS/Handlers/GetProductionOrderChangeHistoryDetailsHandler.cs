using Infrastructure.Data.Entities.Joins.MGO;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetProductionOrderChangeHistoryDetailsHandler: IHandle<UserModel, ResponseModel<List<GetProductionOrderChangeHistoryDetailResponseModel>>>
	{
		private UserModel _user { get; set; }
		private GetProductionOrderChangeHistoryDetailRequestModel _data { get; set; }
		public GetProductionOrderChangeHistoryDetailsHandler(UserModel user,GetProductionOrderChangeHistoryDetailRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<GetProductionOrderChangeHistoryDetailResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				//if(!this._user.IsGlobalDirector && !this._user.IsAdministrator && this._user.Access.ManagementOverview.CtsPlanning == false)
				//{
				//	return ResponseModel<List<FaChangesWeekYearHoursLeftResponseModel>>.SuccessResponse(null);
				//}

				var responseBody = new List<GetProductionOrderChangeHistoryDetailResponseModel>();
				// -  
				int horizon = Module.AppSettingsCTS.FAHorizons.H1LengthInDays;
				var entities = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetProductionOrderHistory(this._data.DateFrom, this._data.DateTo,
					this._data.ProductionOrderWarehouseId, this._data.InFrozenZone, this._data.OutFrozenZone,this._data.Status);
				if(entities != null && entities.Count > 0)
				{
					responseBody.AddRange(entities.Select(x => new GetProductionOrderChangeHistoryDetailResponseModel(x)));
				}

				// -
				return ResponseModel<List<GetProductionOrderChangeHistoryDetailResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<GetProductionOrderChangeHistoryDetailResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<GetProductionOrderChangeHistoryDetailResponseModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<GetProductionOrderChangeHistoryDetailResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<GetProductionOrderChangeHistoryDetailResponseModel>>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();
				if(data == null || !data.Success || data.Body == null || data.Body.Count <= 0)
				{
					return null;
				}
				var tempFolder = Path.GetTempPath();
				var filePath = Path.Combine(tempFolder, $"Production-Order-Changes-History-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage(file))
				{
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Production Order Changes History");

					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 5;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Production number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Change Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Confirmed Deadline";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Previous Deadline";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Production Order Time";

					var rowNumber = headerRowNumber + 1;
					var elements = data.Body;
					if(elements.Count > 0)
					{
						foreach(var p in elements)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.ProductionOrderNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.ChangeDate;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.ConfirmedDeadline;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.PreviousDeadline;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.ProductionOrderTime;

							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber += 1;
						}
					}
					if(elements != null && elements.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber, 1, rowNumber - 1, numberOfColumns - 3])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}

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
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).Width = 25;
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"Production-Order-Changes-History-{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
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
