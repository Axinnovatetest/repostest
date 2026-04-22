using Infrastructure.Services.Utils;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.HistoryFG;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.HistoryFG
{
	public class ImportHistoryFGByExcelHandler: IHandle<UserModel, ResponseModel<List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>>>
	{
		private readonly UserModel _user;
		private readonly ImportFromExcelRequestModel _data;

		public ImportHistoryFGByExcelHandler(UserModel user, ImportFromExcelRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var errors = new List<string>();
				var transaction = new TransactionsManager();
				var response = ReadFromExcel(_data.AttachmentFilePath, _user, _data.Date, transaction, out errors);
				if(errors != null && errors.Count > 0)
					return ResponseModel<List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>>.FailureResponse(errors);

				return ResponseModel<List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>> Validate()
		{
			if(this._data.Date > DateTime.Now)
			{
				return ResponseModel<List<HistoryDataFGDetailsResponseModel>>.FailureResponse($"Import date must be less than current Date");
			}
			if(_user == null)
				return ResponseModel<List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>>.AccessDeniedResponse();
			return ResponseModel<List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>>.SuccessResponse();
		}

		internal static List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel> ReadFromExcel(string filePath, UserModel user, DateTime importDate, Infrastructure.Services.Utils.TransactionsManager botransaction, out List<string> errors)
		{
			errors = new List<string> { };
			try
			{
				botransaction.beginTransaction();
				var fileInfo = new System.IO.FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				var package = new ExcelPackage(fileInfo);
				var worksheet = package.Workbook.Worksheets[0];
				var rowStart = worksheet.Dimension.Start.Row;
				var rowEnd = worksheet.Dimension.End.Row;

				rowEnd -= 0;

				var rows = worksheet.Dimension.Rows;
				var columns = worksheet.Dimension.Columns;
				var startRowNumber = 2;
				var startColNumber = 1;
				var guid = Guid.NewGuid().ToString();
				//ValidateExcelColumns(worksheet, startColNumber, errors);
				var ExpectedColumns = new Dictionary<int, string>
				{
					{ 0, "Artikelnummer" },
					{ 1, "Kunde" },
					{ 2, "Bezeichnung 1" },
					{ 3, "Bezeichnung 2" },
					{ 4, "Freigabestatus" },
					{ 5, "CS Kontakt" },
					{ 6, "Lagerort" },
					{ 7, "Bestand" },
					{ 8, "VK Gesamt." },
					{ 9, "Kosten gesamt (mit CU)" },
					{ 10, "Kosten gesamt (ohne CU)" },
					{ 11, "VKE" },
					{ 12, "UBG" },
					{ 13, "Std EDI" }
				};
				if(rows > 1 && columns > 1)
				{

					var positions = new List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>();
					int ColNum = 0;
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						ColNum = 0;
						try
						{

							var artikelnummer = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var Kunde = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var ArticleDesignation1 = Common.Helpers.Formatters.XLS.GetCellValueFgAsString(worksheet.Cells[i, startColNumber + ColNum], true);
							ColNum++;
							var ArticleDesignation2 = Common.Helpers.Formatters.XLS.GetCellValueFgAsString(worksheet.Cells[i, startColNumber + ColNum], true);
							ColNum++;
							var ArticleReleaseStatus = Common.Helpers.Formatters.XLS.GetCellValueFgAsString(worksheet.Cells[i, startColNumber + ColNum], true);
							ColNum++;
							var CsKontakt = Common.Helpers.Formatters.XLS.GetCellValueFgAsString(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var WarehouseName = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var StockQuantity = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var TotalSalesPrice = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var TotalCostsWithCU = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var TotalCostsWithoutCU = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var UnitSalesPrice = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var UBG = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[i, startColNumber + ColNum]);
							ColNum++;
							var EDIStandard = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[i, startColNumber + ColNum]);



							var chechArtikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByArtikelNummer(artikelnummer);
							if(chechArtikel == null)
							{
								errors.Add($"Row {i} Article [{artikelnummer}] not found.");
							}
							else
							{
								if(chechArtikel.Warengruppe != "EF")
									errors.Add($"Row {i} Article [{artikelnummer}] is not finished Goods");
								//if(!chechArtikel.aktiv.Value || !chechArtikel.aktiv.HasValue)
								//	errors.Add($"Row {i} Article [{artikelnummer}] is not active");

								//get customer
								var kries = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByCustomerNumber(chechArtikel.CustomerNumber ?? -1);
								var names = kries?.Select(k => k.Kunde).ToList();
								if(names != null && !names.Contains(Kunde))
									errors.Add($"Row {i} kunde [{Kunde}] does not match Artikelnummer [{artikelnummer}]");
							}
							string articleNr = Infrastructure.Data.Access.Tables.ArtikelAccess.GetArtikelId(artikelnummer);

							positions.Add(new HistoryDataFGDetailsResponseModel
							{
								ArticleNumber = artikelnummer,
								CustomerName = Kunde,
								CustomerNumber = chechArtikel == null ? 0 : chechArtikel.CustomerNumber,
								ArticleDesignation1 = ArticleDesignation1,
								ArticleDesignation2 = ArticleDesignation2,
								ArticleReleaseStatus = ArticleReleaseStatus,
								CsContact = CsKontakt,
								WarehouseName = WarehouseName,
								StockQuantity = Convert.ToDecimal(StockQuantity),
								TotalSalesPrice = Convert.ToDecimal(TotalSalesPrice),
								TotalCostsWithCu = Convert.ToDecimal(TotalCostsWithCU),
								TotalCostsWithoutCu = Convert.ToDecimal(TotalCostsWithoutCU),
								UnitSalesPrice = Convert.ToDecimal(UnitSalesPrice),
								UBG = ConevertToBool(UBG),
								EdiStandard = ConevertToBool(EDIStandard),
								ArticleNr = Convert.ToInt32(articleNr)
							});

						} catch(System.Exception exceptionInternal)
						{
							Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
							if(ColNum >= ExpectedColumns.Count)
								ColNum = (ExpectedColumns.Count - 1);
							errors.Add($"Row {i}: {exceptionInternal.Message} - Column [{ColNum}] : {ExpectedColumns[ColNum]}");
						}
					}
					// Check if positions list is empty or null before inserting header
					if(positions == null || positions.Count == 0)
					{
						errors.Add("No valid data to insert.");
					}


					if(!botransaction.commit())
					{
						botransaction.rollback();
						return null;
					}
					return positions;
				}
				else
				{
					errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
					return new List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>(); // return empty list if invalid
				}
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}

		internal static bool ConevertToBool(string value)
		{
			return value == "Yes" ? true : false;
		}

		public static void ValidateExcelColumns(ExcelWorksheet worksheet, int startColNumber, List<string> errors)
		{
			var expectedColumns = new Dictionary<int, string>
			{
				{ 0, "Artikelnummer" },
				{ 1, "Kunde" },
				{ 2, "Bezeichnung 1" },
				{ 3, "Bezeichnung 2" },
				{ 4, "Freigabestatus" },
				{ 5, "CS Kontakt" },
				{ 6, "Lagerort" },
				{ 7, "Bestand" },
				{ 8, "VK Gesamt." },
				{ 9, "Kosten gesamt (mit CU)" },
				{ 10, "Kosten gesamt (ohne CU)" },
				{ 11, "VKE" },
				{ 12, "UBG" },
				{ 13, "Std EDI" }
			};

			int row = 1; // Start at first row
			foreach(var column in expectedColumns)
			{
				var cellValue = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[row, startColNumber + column.Key]);

				// Checking compatibility (ignoring spaces and case)
				if(cellValue?.Trim().ToLower() != column.Value.ToLower())
				{
					errors.Add($"Incompatible column : {column.Value} [column {startColNumber + column.Key}]");
				}
			}

			// Add a global message if errors are detected
			if(errors.Any())
			{
				errors.Add("Please download the correct FG DRAFT.");
			}
		}
	}
}
