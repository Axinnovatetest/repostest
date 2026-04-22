using Geocoding;
using Psz.Core.Logistics.Enums;
using Psz.Core.Logistics.Interfaces;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetPlantBookingsByLagerStrategyHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetPlantResponseModel>>
	{
		private readonly Dictionary<int, IPlantBookingStrategy> _strategies;
		private GetPlantBookingsRequestModel _request;
		private Core.Identity.Models.UserModel _user;
		public GetPlantBookingsByLagerStrategyHandler(GetPlantBookingsRequestModel request, Core.Identity.Models.UserModel user)
		{
			_request = request;
			_user = user;

			// populate strategy : 
			_strategies = new Dictionary<int, IPlantBookingStrategy>
			{
				{ (int)LagerEnum.BETN, new TNPlantBookingStrategy() },
				{ (int)LagerEnum.GZTN, new TNPlantBookingStrategy() },
				{ (int)LagerEnum.TN, new TNPlantBookingStrategy() },
				{ (int)LagerEnum.WS, new TNPlantBookingStrategy() },
				{ (int)LagerEnum.Eigenfertigung, new CZPlantBookingStrategy() },
				{ (int)LagerEnum.Albanien, new ALPlantBookingStrategy() }
			};
		}
		public ResponseModel<GetPlantResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				//var bookinsListByLager = new List<GetPlantBookingsResponseModel>();
				int _lagerId = this._request.LagerId;
				string searchValue = this._request.SearchValue;
				int totalCount = 0;

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "Date":
							sortFieldName = "[Datum]";
							break;
						case "packagingnr":
							sortFieldName = "[Verpackungsnr]";
							break;
						case "artikelnummer":
							sortFieldName = "[Artikelnummer]";
							break;
						case "quantity":
							sortFieldName = "[Menge]";
							break;
						case "totalquantity":
							sortFieldName = "[Gesamtmenge]";
							break;
						case "restquantity":
							sortFieldName = "[Restmenge_Rolle_PPS]";
							break;
						case "status":
							sortFieldName = "[Status_Rolle]";
							break;
						case "Inspector":
							sortFieldName = "[Inspektor]";
							break;
						case "Remarque":
							sortFieldName = "[Resultat]";
							break;

					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
				}
				//int totalCount = bookinsListByLager.FirstOrDefault() == null ? 0 : (int)bookinsListByLager.FirstOrDefault().TotalCount;
				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._request.PageSize > 0 ? (this._request.RequestedPage * this._request.PageSize) : 0,
					RequestRows = this._request.FullData ? totalCount : this._request.PageSize
				};

				// start strategy 
				var bookinsListByLager = ExecuteStrategy(this._request.LagerId, searchValue, dataSorting, _request, out totalCount);
				// end strategy 

				var r = this._request.PageSize > 0 ? totalCount / decimal.Parse((this._request.PageSize).ToString()) : 0;

				if(bookinsListByLager is null || bookinsListByLager.Count == 0)
					return ResponseModel<GetPlantResponseModel>.SuccessResponse(new GetPlantResponseModel
					{
						Items = new List<GetPlantBookingsResponseModel>(),
						PageRequested = this._request.RequestedPage,
						PageSize = this._request.PageSize,
						TotalCount = totalCount,
						TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
					});

				return ResponseModel<GetPlantResponseModel>.SuccessResponse(new GetPlantResponseModel
				{
					Items = bookinsListByLager,
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
				var filePath = System.IO.Path.Combine(tempFolder, $"PlantBookings-Data-{DateTime.Now:yyyyMMddTHHmmssfff}-{Guid.NewGuid().ToString()}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PlantBookingsData");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 9;

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
					worksheet.Cells[1, 1].Value = $"PSZ-Plant-Booking-Data-{DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Packaging Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Article Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Original Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Total Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Remaining Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Status";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Inspector";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Remarque";


					var rowNumber = headerRowNumber + 1;
					{
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{
								if(w?.Aktiv == 1) // Check if Aktiv is 1
								{
									if(w?.Datum != null)
									{
										worksheet.Cells[rowNumber, startColumnNumber].Value = w.Datum;  // Assign the DateTime value
										worksheet.Cells[rowNumber, startColumnNumber].Style.Numberformat.Format = "dd/MM/yyyy";  // Apply the date format
									}
									else
									{
										worksheet.Cells[rowNumber, startColumnNumber].Value = string.Empty;  // Leave the cell empty if the date is null
									}
									worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.PackagingNr;
									worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.ArtikelNummer;
									worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Quantity;
									worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.TotalQuantity;
									worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.RestQuantity;
									worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Status;
									worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Inspector;
									worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Remarque;

									worksheet.Row(rowNumber).Height = 18;
									rowNumber += 1;
								}
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
					package.Workbook.Properties.Title = $"PlantBookings-Data";
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
		public ResponseModel<GetPlantResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<GetPlantResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<GetPlantResponseModel>.SuccessResponse();
		}
		#region strategy  implementation

		public List<GetPlantBookingsResponseModel> ExecuteStrategy(int lagerId, string searchValue, Infrastructure.Data.Access.Settings.SortingModel dataSorting, GetPlantBookingsRequestModel request, out int totalCount)
		{
			if(_strategies.TryGetValue(lagerId, out var strategy))
			{
				return strategy.GetPlantBookings(lagerId, searchValue, dataSorting, request, out totalCount);
			}

			totalCount = 0;
			return new List<GetPlantBookingsResponseModel>();
		}
		public class TNPlantBookingStrategy: IPlantBookingStrategy
		{
			public List<GetPlantBookingsResponseModel> GetPlantBookings(int lagerId, string searchValue, Infrastructure.Data.Access.Settings.SortingModel dataSorting, GetPlantBookingsRequestModel request, out int totalCount)
			{
				totalCount = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_TNAccess.CountPlantBookingsRowsByLagerId(lagerId, dataSorting, null);

				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel
				{
					FirstRowNumber = request.PageSize > 0 ? (request.RequestedPage * request.PageSize) : 0,
					RequestRows = request.FullData ? totalCount : request.PageSize
				};

				var data = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_TNAccess.GetByLagerId(lagerId, searchValue, dataSorting, dataPaging);
				return data?.Select(x => new GetPlantBookingsResponseModel(x)).ToList() ?? new List<GetPlantBookingsResponseModel>();
			}
		}
		public class CZPlantBookingStrategy: IPlantBookingStrategy
		{
			public List<GetPlantBookingsResponseModel> GetPlantBookings(int lagerId, string searchValue, Infrastructure.Data.Access.Settings.SortingModel dataSorting, GetPlantBookingsRequestModel request, out int totalCount)
			{
				totalCount = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.CountPlantBookingsLagerCZ(lagerId, searchValue, dataSorting, null);

				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel
				{
					FirstRowNumber = request.PageSize > 0 ? (request.RequestedPage * request.PageSize) : 0,
					RequestRows = request.FullData ? totalCount : request.PageSize
				};

				var data = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetByLagerIdCZ(lagerId, searchValue, dataSorting, dataPaging);
				return data?.Select(x => new GetPlantBookingsResponseModel(x)).ToList() ?? new List<GetPlantBookingsResponseModel>();
			}
		}
		public class ALPlantBookingStrategy: IPlantBookingStrategy
		{
			public List<GetPlantBookingsResponseModel> GetPlantBookings(int lagerId, string searchValue, Infrastructure.Data.Access.Settings.SortingModel dataSorting, GetPlantBookingsRequestModel request, out int totalCount)
			{
				totalCount = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.CountPlantBookingsLagerAL(lagerId, searchValue, dataSorting, null);

				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel
				{
					FirstRowNumber = request.PageSize > 0 ? (request.RequestedPage * request.PageSize) : 0,
					RequestRows = request.FullData ? totalCount : request.PageSize
				};

				var data = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetByLagerIdAl(lagerId, searchValue, dataSorting, dataPaging);
				return data?.Select(x => new GetPlantBookingsResponseModel(x)).ToList() ?? new List<GetPlantBookingsResponseModel>();
			}
		}

		#endregion
	}
}
