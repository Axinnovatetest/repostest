namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetLogsDataHandler : IHandle<Identity.Models.UserModel, ResponseModel<LogsResponseModel>>
	{
		private LogsDataRequestModel _request;
	    private Core.Identity.Models.UserModel _user;
	public GetLogsDataHandler(LogsDataRequestModel request, Core.Identity.Models.UserModel user)
	{
		_request = request;
		_user = user;
	}

		public ResponseModel<LogsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				
				var DataLogsList = new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity>();
				string searchValue = this._request.SearchValue;

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "id":
							sortFieldName = "[Id]";
							break;
						case "lastupdatetime":
							sortFieldName = "[LastUpdateTime]";
							break;
						case "lastupdateuserfullName":
							sortFieldName = "[LastUpdateUserFullName]";
							break;
						case "lastupdateuserid":
							sortFieldName = "[LastUpdateUserId]";
							break;
						case "lastupdateusername":
							sortFieldName = "[LastUpdateUsername]";
							break;
						case "logdescription":
							sortFieldName = "[LogDescription]";
							break;
						case "logobject":
							sortFieldName = "[LogObject]";
							break;
						case "logobjectId":
							sortFieldName = "[LogObjectId]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
				}
				int totalCount = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsLogsAccess.CountPlantBookingsDataLogs(dataSorting, dataPaging);

				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._request.PageSize > 0 ? (this._request.RequestedPage * this._request.PageSize) : 0,
					 RequestRows = this._request.FullData ? totalCount : this._request.PageSize

				 };


				DataLogsList = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsLogsAccess.GetDataLogs(searchValue, dataSorting, dataPaging);
				var r = this._request.PageSize > 0 ? totalCount / decimal.Parse((this._request.PageSize).ToString()) : 0;
				if(DataLogsList is null || DataLogsList.Count == 0)
						return ResponseModel<LogsResponseModel>.SuccessResponse(new LogsResponseModel
						{
							Items = new List<LogsDataResponseModel>(),
							PageRequested = this._request.RequestedPage,
							PageSize = this._request.PageSize,
							TotalCount = totalCount,
							TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
						});

				var result = DataLogsList.Select(x => new LogsDataResponseModel(x)).ToList();


					return ResponseModel<LogsResponseModel>.SuccessResponse(new LogsResponseModel
					{
						Items = result,
						PageRequested = this._request.RequestedPage,
						PageSize = this._request.PageSize,
						TotalCount = totalCount,
						TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
					});
				


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
				this._request.FullData = true;
				var response = this.Handle();
				var data = response.Success ? response.Body.Items : null;
				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"PlantBookings-Logs-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PlantBookingsLogs");

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
					worksheet.Cells[1, 1].Value = $"PSZ-Plant-Booking-LOGS-{DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Id";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Last Updated time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "UserName";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Description";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Log Object";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Log Object Id";


					var rowNumber = headerRowNumber + 1;
					{
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{

								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Id;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.LastUpdateTime;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.LastUpdateUserFullName;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.LogDescription;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.LogObject;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.LogObjectId;

								worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

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
					package.Workbook.Properties.Title = $"PlantBookings-Logs";
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


		public ResponseModel<LogsResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<LogsResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<LogsResponseModel>.SuccessResponse();
		}
	}
}

