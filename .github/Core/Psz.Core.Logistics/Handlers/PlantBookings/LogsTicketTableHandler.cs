namespace Psz.Core.Logistics.Handlers.PlantBookings

{
	public class LogsTicketTableHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetLogsTicketResponseModel>>
	{
		private Core.Identity.Models.UserModel _user;
		private LogsTicketRequestModel _data;

		public LogsTicketTableHandler(Core.Identity.Models.UserModel user, LogsTicketRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<GetLogsTicketResponseModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - 
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;

				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					var sortFieldName = "";
					switch(this._data.SortField.ToLower())
					{
						default:
						case "id":
							sortFieldName = "[Id]";
							break;
						case "userid":
							sortFieldName = "[UserId]";
							break;
						case "username":
							sortFieldName = "[Username]";
							break;
						case "userfullname":
							sortFieldName = "[Userfullname]";
							break;
						case "creationdate":
							sortFieldName = "[CreationDate]";
							break;
						case "lagerid":
							sortFieldName = "[LagerId]";
							break;
						case "artikelnummer":
							sortFieldName = "[artikelnummer]";
							break;
						case "ticketscount":
							sortFieldName = "[ticketscount]";
							break;
						case "verpackungnr":
							sortFieldName = "[verpackungnr]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}
				int totalCount = Infrastructure.Data.Access.Joins.Logistics.PlantBookingsTicketLogsAccess.CountLogsTicket(dataSorting, dataPaging);
				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.FullData ? totalCount : this._data.PageSize

				};

				var results = Infrastructure.Data.Access.Joins.Logistics.PlantBookingsTicketLogsAccess.GetLogsTicket(_data.SearchValue, dataSorting, dataPaging);
				var r = this._data.PageSize > 0 ? totalCount / decimal.Parse((this._data.PageSize).ToString()) : 0;
				if(results is null || results.Count == 0)
					return ResponseModel<GetLogsTicketResponseModel>.SuccessResponse(
						new GetLogsTicketResponseModel()
						{
							Items = new List<LogsTicketResponseModel>(),
							PageRequested = this._data.RequestedPage,
							PageSize = this._data.PageSize,
							TotalCount = totalCount,
							TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
						}
						);

				return ResponseModel<GetLogsTicketResponseModel>.SuccessResponse(new GetLogsTicketResponseModel
				{
					Items = results.Select(x => new LogsTicketResponseModel(x)).ToList(),
					PageRequested = this._data.RequestedPage,
					PageSize = this._data.PageSize,
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
				this._data.FullData = true;
				var response = this.Handle();
				var data = response.Success ? response.Body.Items : null;
				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Logs-Ticket-Data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Logs-Ticket-Data");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 8;

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
					worksheet.Cells[1, 1].Value = $"Logs-Ticket-Data-{DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "User Id";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "User Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "User Full Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Creation Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "LagerId";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "artikel Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "tickets Count";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "verpackung Nr";

					var rowNumber = headerRowNumber + 1;
					{
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{

								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.UserId;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.UserName;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Userfullname;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.CreationDate;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.LagerId;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.ticketscount;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.verpackungnr;

								worksheet.Cells[rowNumber, startColumnNumber +3 ].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;

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
					package.Workbook.Properties.Title = $"Logs-Ticket-Data";
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

		public ResponseModel<GetLogsTicketResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<GetLogsTicketResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<GetLogsTicketResponseModel>.SuccessResponse();
		}
	}
}
