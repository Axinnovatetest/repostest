using MoreLinq;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.ManagementOverview.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetProductionReschedulingHandler: IHandle<Identity.Models.UserModel, ResponseModel<ProductionReschedulingResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ProductionReschedulingRequestModel _data { get; set; }

		public GetProductionReschedulingHandler(Identity.Models.UserModel user, ProductionReschedulingRequestModel data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<ProductionReschedulingResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				#region > Data sorting & paging
				Infrastructure.Data.Access.Settings.PaginModel dataPagingList = null;
				if(_data.pagination is not null)
				{
					dataPagingList = new Infrastructure.Data.Access.Settings.PaginModel()
					{
						FirstRowNumber = _data.pagination.PageSize > 0 ? (this._data.pagination.RequestedPage * this._data.pagination.PageSize) : 0,
						RequestRows = _data.pagination is not null ? _data.pagination.PageSize : 10
					};
				}

				#endregion

				/// 
				var kwCount = this._data.fzKwCount ?? 5;
				List<ProductionFrozenZoneResponseModel> lsResult = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSProductionFrozenZoneAccess(kwCount, _data.productType == Enums.CTSDashboard.InFrozen, _data.from, _data.to, dataPagingList)
												?.Select(x => new ProductionFrozenZoneResponseModel(x))?.ToList();

				return ResponseModel<ProductionReschedulingResponseModel>.SuccessResponse(new ProductionReschedulingResponseModel
				{
					Items =
					new PaginatorSyncResponseModel<ProductionFrozenZoneResponseModel>
					{
						Items = lsResult,
						PageRequested = this._data.pagination is not null ? this._data.pagination.RequestedPage : 0,
						PageSize = _data.pagination is not null ? _data.pagination.PageSize : 10,
						TotalCount = lsResult is not null && lsResult.Count > 0 ? lsResult[0].TotalCount : 0,
						TotalPageCount = _data.pagination is not null && _data.pagination.PageSize > 0 ? (int)Math.Ceiling(((decimal)(lsResult is not null && lsResult.Count > 0 && lsResult[0].TotalCount > 0 ? lsResult[0].TotalCount : 0) / this._data.pagination.PageSize)) : 0,
						syncDate = lsResult is not null && lsResult.Count > 0 ? lsResult.FirstOrDefault().SyncDate : null
					},
					KwCount = kwCount
				});


				//return ResponseModel<ProductionReschedulingResponseModel>.SuccessResponse(
				//	new ProductionReschedulingResponseModel
				//	{
				//		ItemsBroughtInFrozenZone = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSProductionFrozenZoneAccess(kwCount, true, dataPagingList1)
				//							?.Select(x => new ProductionFrozenZoneResponseModel(x))?.ToList(),
				//	,
				//	//	ItemsSentOutFrozenZone = new IPaginatedResponseModel<List<ProductionFrozenZoneResponseModel>>()
				//	//	{
				//	//		Items = Infrastructure Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSProductionFrozenZoneAccess(kwCount, false, dataPagingList2)
				//	//?.Select(x => new ProductionFrozenZoneResponseModel(x))?.ToList()
				//	//	},
				//		KwCount = kwCount
				//	});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<ProductionReschedulingResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ProductionReschedulingResponseModel>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<ProductionReschedulingResponseModel>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<ProductionReschedulingResponseModel>.SuccessResponse();
		}

		public byte[] GetDataXLS()
		{
			try
			{
				this._data.pagination = null;
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"ProductionRescheduling-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					if(data != null && data.Success == true)
					{
						addWorkSheet(data.Body.Items.Items, "Into FrozenZone", data.Body.KwCount, package);
						addWorkSheet(data.Body.Items.Items, "Out of FrozenZone", data.Body.KwCount, package);
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"ProductionRescheduling";
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

		private void addWorkSheet(List<ProductionFrozenZoneResponseModel> data, string titie, int kwCount, ExcelPackage package)
		{
			// add a new worksheet to the empty workbook
			ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{titie}");

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
			worksheet.Cells[1, 1].Value = $"Production Rescheduling - {titie} : [{kwCount} KW - {DateTime.Today.AddDays(kwCount * 7).ToString("dd-MM-yyyy")}]";
			worksheet.Cells[1, 1].Style.Font.Size = 16;

			var rowNumber = 1;
			#region Items
			//if(data.Success == true)
			{
				headerRowNumber = rowNumber + 1;
				// Start adding the header
				worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Fertigungsnummer";
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Termin Ursprunglich";
				worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Termin Bestatigt";
				worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Änderungsdatum";
				worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Lagerort";
				worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "offen Anzahl";

				rowNumber = headerRowNumber + 1;
				if(data != null && data.Count > 0)
				{
					//Loop through
					foreach(var w in data)
					{
						worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Fertigungsnummer;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Termin_Ursprunglich?.ToString("dd-MM-yyyy");
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Termin_Bestatigt1?.ToString("dd-MM-yyyy");
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.LastUpdateDate?.ToString("dd-MM-yyyy");
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Lagerort_id;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Anzahl;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}
				}
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
		}
	}
}
