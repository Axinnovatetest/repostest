namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class SupplierListHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetSupplierResponseModel>>
	{
		private Core.Identity.Models.UserModel _user;
		private SupplierRequestModel _request;

		public SupplierListHandler(Core.Identity.Models.UserModel user, SupplierRequestModel request)
		{
			_user = user;
			_request = request;
		}

		public ResponseModel<GetSupplierResponseModel> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				int totalCount = 0;
				string _artikelNummer = this._request.ArtikelNummer;

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "standardlieferant":
							sortFieldName = "[Standardlieferant]";
							break;
						case "name1":
							sortFieldName = "[Name1]";
							break;
						case "artikelnummer":
							sortFieldName = "[Artikelnummer]";
							break;
						case "Bezeichnung":
							sortFieldName = "[Bezeichnung]";
							break;
						case "grosse":
							sortFieldName = "[Grosse]";
							break;
						case "pruftiefe":
							sortFieldName = "[Pruftiefe]";
							break;

					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
					dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
					{
						FirstRowNumber = this._request.PageSize > 0 ? (this._request.RequestedPage * this._request.PageSize) : 0,
						RequestRows = this._request.FullData ? totalCount : this._request.PageSize
					};
				}
				totalCount = Infrastructure.Data.Access.Joins.Logistics.WeVOHIncomingAccess.CountSupplierRowsBy(_artikelNummer, dataSorting, dataPaging);
				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._request.PageSize > 0 ? (this._request.RequestedPage * this._request.PageSize) : 0,
					RequestRows = this._request.FullData ? totalCount : this._request.PageSize
				};
				var r = this._request.PageSize > 0 ? totalCount / decimal.Parse((this._request.PageSize).ToString()) : 0;

				var data = Infrastructure.Data.Access.Joins.Logistics.WeVOHIncomingAccess.GetSupplierList(_artikelNummer, dataSorting, dataPaging);

				if(data is null || data.Count == 0)
					return ResponseModel<GetSupplierResponseModel>.SuccessResponse(new GetSupplierResponseModel
					{
						Items = new List<SupplierResponseModel>(),
						PageRequested = this._request.RequestedPage,
						PageSize = this._request.PageSize,
						TotalCount = totalCount,
						TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
					});

				var res = data.Select(x => new Psz.Core.Logistics.Models.PlantBookings.SupplierResponseModel(x)).ToList();

				return ResponseModel<GetSupplierResponseModel>.SuccessResponse(new GetSupplierResponseModel
				{
					Items = res,
					PageRequested = this._request.RequestedPage,
					PageSize = this._request.PageSize,
					TotalCount = totalCount,
					TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
				});

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}

		}



		public byte[] GetSupplierDataXLS()
		{
			try
			{
				this._request.FullData = true;
				var response = this.Handle();
				var data = response.Success ? response.Body.Items : null;
				if(data is not null && data.Count > 0)
					data = data.OrderByDescending(x => x.Standardlieferant).ToList();
				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Suppliers-Data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"SuppliersData");

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
					worksheet.Cells[1, 1].Value = $"Suppliers-Data-{DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Standard supplier";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Article Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Designation";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Order Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Size";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Test depth";


					var rowNumber = headerRowNumber + 1;
					{
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{

								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Standardlieferant == 1 ? "YES" : "NO";
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Name1;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bezeichnung;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.BestellNr;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Grosse;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Pruftiefe;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = "0.00%";


								worksheet.Row(rowNumber).Height = 18;

								rowNumber++;
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
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, data.Count + headerRowNumber, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
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
					package.Workbook.Properties.Title = $"Suppliers-Data";
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



		public ResponseModel<GetSupplierResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<GetSupplierResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<GetSupplierResponseModel>.SuccessResponse();
		}
	}

}
