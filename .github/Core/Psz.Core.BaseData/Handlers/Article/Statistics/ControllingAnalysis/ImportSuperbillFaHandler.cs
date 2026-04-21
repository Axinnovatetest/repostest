using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class ImportSuperbillFaHandler: IHandle<UserModel, ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>>
	{
		private UserModel _user { get; set; }
		private ImportFileRequestBusinessModel _data { get; set; }
		public ImportSuperbillFaHandler(UserModel user, ImportFileRequestBusinessModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var errors = new List<string>();
				var fertigungsnummers = ReadFromExcel(this._data.FileByteArray, out errors);
				if(errors != null && errors.Count > 0)
					return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.FailureResponse(errors);


				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				// -
				var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(fertigungsnummers);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					fertigungsnummers.Select(x =>
					{
						var fa = faEntities.FirstOrDefault(y => y.Fertigungsnummer == x);
						return
						ObjectLogHelper.getLog(this._user, fa?.ID ?? 0, "SuperbillROH", "",
						$"fertigungsnummer: {x}",
						Enums.ObjectLogEnums.Objects.ArticleFertigung.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add);
					}
					)?.ToList(), botransaction.connection, botransaction.transaction);

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetSuperbillROH_MultiQuery(
						fertigungsnummers?.Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHInput
						{
							Fertigungsnummer = x
						})?.ToList(),
						botransaction.connection, botransaction.transaction, isCreate: false);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.SuccessResponse(results);
				}
				else
				{
					return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.AccessDeniedResponse();
			}

			return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.SuccessResponse();
		}

		public ResponseModel<string> Check()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return ResponseModel<string>.FailureResponse(validationResponse.Errors.Select(x => x.Value).ToList());
				}

				var errors = new List<string>();
				var fertigungsnummers = ReadFromExcel(this._data.FileByteArray, out errors);
				if(errors != null && errors.Count > 0)
					return ResponseModel<string>.FailureResponse(errors);

				// - 
				return ResponseModel<string>.SuccessResponse("");
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<int> ReadFromExcel(byte[] fileBytes, out List<string> errors)
		{
			errors = new List<string> { };
			try
			{

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				var package = new ExcelPackage(new MemoryStream(fileBytes));

				var worksheet = package.Workbook.Worksheets[0];
				var rowStart = worksheet.Dimension.Start.Row;
				var rowEnd = worksheet.Dimension.End.Row;

				// get number of rows and columns in the sheet
				var rows = worksheet.Dimension.Rows;
				var columns = worksheet.Dimension.Columns;
				var startRowNumber = 2;
				var startColNumber = 1;

				if(rows > 1 && columns > 0)
				{
					var faNumbers = new List<int> { };

					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							string number = Core.Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber])?.Trim();
							if(int.TryParse(Core.Common.Helpers.Formatters.XLS.EscapeDecimalSeparator(number), out var _fanumber))
							{
								faNumbers.Add(_fanumber);
							}
							else
							{
								errors.Add($"Row {i}: invalid FA-Number [{number}].");
							}
						} catch(System.Exception exceptionInternal)
						{
							Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
							errors.Add($"Row {i}: unknown error.");
						}
					}

					return faNumbers;
				}
				else
				{
					errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
					return null;
				}
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}
		public ResponseModel<byte[]> GetDetailExcelData()
		{
			try
			{
				var _handle = this.Handle();
				if(_handle is null)
					return null;
				if(_handle.Success != true)
					return ResponseModel<byte[]>.FailureResponse(_handle.Errors.Select(x => x.Value)?.ToList());


				var dataEntities = _handle.Body?.SuperbillROHDetails ?? null;

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"ControllingAnalysis_SuperbillROHDetail-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Superbill ROH Detail");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 32;

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
					worksheet.Cells[1, 1].Value = $"Superbill ROH Detail";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikel FG";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Menge FG";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 1 FG";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung 2 FG";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Artikel ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Menge ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bezeichnung 1 ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bezeichnung 2 ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Standardlieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bestell-Nr ROH";
					// -
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Einkaufspreis ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Kupferzahl ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Mindestbestellmenge ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Wiederbeschaffungszeitraum ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "UL zertifiziert ROH";

					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Bestand ROH CZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Bestand ROH TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Bestand ROH AL";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Bestand ROH WS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Bestand ROH GZ";

					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Bestand ROH Obsolete";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Rahmen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "Rahmen Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "Rahmenmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 24].Value = "Rahmenauslauf";

					worksheet.Cells[headerRowNumber, startColumnNumber + 25].Value = "Mindestbestand ROH CZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 26].Value = "Mindestbestand ROH TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 27].Value = "Mindestbestand ROH AL";
					worksheet.Cells[headerRowNumber, startColumnNumber + 28].Value = "Mindestbestand ROH KHTN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 29].Value = "Mindestbestand ROH GZ";

					worksheet.Cells[headerRowNumber, startColumnNumber + 30].Value = "Mindestbestand ROH Obsolete";
					worksheet.Cells[headerRowNumber, startColumnNumber + 31].Value = "ROH Angebotsdatum";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Artikelnummer_FG;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Menge_FG;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bez1_FG;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bez2_FG;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Artikelnummer_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Menge_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Bez1_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Bez2_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Standardlieferant;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Bestell_Nr_ROH;
							// -
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Einkaufspreis_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Kupferzahl_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Mindestbestellmenge_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Wiederbeschaffungszeitraum_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.UL_zertifiziert_ROH;

							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.Bestand_ROH_CZ;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.Bestand_ROH_TN;
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = w?.Bestand_ROH_AL;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = w?.Bestand_ROH_WS;
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = w?.Bestand_ROH_GZTN;

							worksheet.Cells[rowNumber, startColumnNumber + 20].Value = w?.Bestand_ROH_Obsolete;
							worksheet.Cells[rowNumber, startColumnNumber + 21].Value = w?.Rahmen;
							worksheet.Cells[rowNumber, startColumnNumber + 22].Value = w?.Rahmen_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 23].Value = w?.Rahmenmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 24].Value = w?.Rahmenauslauf;

							worksheet.Cells[rowNumber, startColumnNumber + 25].Value = w?.Mindestbestand_ROH_CZ;
							worksheet.Cells[rowNumber, startColumnNumber + 26].Value = w?.Mindestbestand_ROH_TN;
							worksheet.Cells[rowNumber, startColumnNumber + 27].Value = w?.Mindestbestand_ROH_AL;
							worksheet.Cells[rowNumber, startColumnNumber + 28].Value = w?.Mindestbestand_ROH_KHTN;
							worksheet.Cells[rowNumber, startColumnNumber + 29].Value = w?.Mindestbestand_ROH_GZTN;

							worksheet.Cells[rowNumber, startColumnNumber + 30].Value = w?.Mindestbestand_ROH_Obsolete;
							worksheet.Cells[rowNumber, startColumnNumber + 31].Value = w?.ROH_Angebotsdatum;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
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
					if(dataEntities != null && dataEntities.Count > 0)
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
					package.Workbook.Properties.Title = $"Superbill ROH Detail";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

					return ResponseModel<byte[]>.SuccessResponse(File.ReadAllBytes(filePath));
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public ResponseModel<byte[]> GetSumExcelData()
		{
			try
			{
				var _handle = this.Handle();
				if(_handle is null)
					return null;
				if(_handle.Success != true)
					return ResponseModel<byte[]>.FailureResponse(_handle.Errors.Select(x => x.Value)?.ToList());

				var dataEntities = _handle.Body?.SuperbillROHSums ?? null;
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"SuperbillROHSum-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Superbill ROH Sum");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 28;

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
					worksheet.Cells[1, 1].Value = $"Superbill ROH Sum";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Menge ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bez1 ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bez2 ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Standardlieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bestell Nr ROH";
					// -
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Einkaufspreis ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Kupferzahl ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Mindestbestellmenge ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Wiederbeschaffungszeitraum ROH";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "UL zertifiziert ROH";

					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Bestand ROH CZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Bestand ROH TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Bestand ROH AL";

					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Rahmen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Rahmen Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Rahmenmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Rahmenauslauf";

					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Mindestbestand ROH TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Mindestbestand ROH AL";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Mindestbestand ROH CZ";

					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Bestand ROH WS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "Mindestbestand ROH WS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "Bestand ROH GZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 24].Value = "Mindestbestand ROH GZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 25].Value = "Bestand ROH Obsolete";
					worksheet.Cells[headerRowNumber, startColumnNumber + 26].Value = "Mindestbestand ROH Obsolete";

					worksheet.Cells[headerRowNumber, startColumnNumber + 27].Value = "ROH Angebotsdatum";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Menge_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bez1_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bez2_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Standardlieferant;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Bestell_Nr_ROH;
							// -
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Einkaufspreis_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Kupferzahl_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Mindestbestellmenge_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Wiederbeschaffungszeitraum_ROH;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.UL_zertifiziert_ROH;

							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Bestand_ROH_CZ;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Bestand_ROH_TN;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Bestand_ROH_AL;

							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Rahmen;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.Rahmen_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.Rahmenmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = w?.Rahmenauslauf;

							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = w?.Mindestbestand_ROH_TN;
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = w?.Mindestbestand_ROH_AL;
							worksheet.Cells[rowNumber, startColumnNumber + 20].Value = w?.Mindestbestand_ROH_CZ;

							worksheet.Cells[rowNumber, startColumnNumber + 21].Value = w?.Bestand_ROH_WS;
							worksheet.Cells[rowNumber, startColumnNumber + 22].Value = w?.Mindestbestand_ROH_WS;
							worksheet.Cells[rowNumber, startColumnNumber + 23].Value = w?.Bestand_ROH_GZTN;
							worksheet.Cells[rowNumber, startColumnNumber + 24].Value = w?.Mindestbestand_ROH_GZTN;
							worksheet.Cells[rowNumber, startColumnNumber + 25].Value = w?.Bestand_ROH_Obsolete;
							worksheet.Cells[rowNumber, startColumnNumber + 26].Value = w?.Mindestbestand_ROH_Obsolete;

							worksheet.Cells[rowNumber, startColumnNumber + 27].Value = w?.ROH_Angebotsdatum;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
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
					if(dataEntities != null && dataEntities.Count > 0)
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
					package.Workbook.Properties.Title = $"Superbill ROH Sum";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

					return ResponseModel<byte[]>.SuccessResponse(File.ReadAllBytes(filePath));
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
