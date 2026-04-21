using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.Linq;
	public class ArticleCustomsCheckAddHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private byte[] _data { get; set; }

		public ArticleCustomsCheckAddHandler(Identity.Models.UserModel user, byte[] data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<byte[]> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				List<string> errors = new List<string>();
				var excelData = ReadFromExcel(out errors);
				if(errors.Count > 0)
				{ return ResponseModel<byte[]>.FailureResponse(errors); }

				var tempId = botransaction.connection.ClientConnectionId.GetHashCode();
				var total = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArticlesCount(botransaction.connection, botransaction.transaction);
				var totalWoZn = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArticlesWoZollNrCount(botransaction.connection, botransaction.transaction);
				var totalWWrongZn = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArticlesWithZollNrNotInXLS(excelData, $"{this._user.Id}_{tempId}", botransaction.connection, botransaction.transaction);

				// -
				var insertedId = Infrastructure.Data.Access.Tables.LGT.ArticleCustomsNumberCheckAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity
				{
					ArticlesTotalCount = total,
					ArticlesWithoutNumberCount = totalWoZn,
					ArticlesWithWrongNumberCount = totalWWrongZn?.Count ?? 0,
					CheckDate = DateTime.Now,
					CheckUser = this._user.Id,
					CheckUserName = this._user.Username,
					Id = 0
				}, botransaction.connection, botransaction.transaction);

				var newItem = Infrastructure.Data.Access.Tables.LGT.ArticleCustomsNumberCheckAccess.GetWithTransaction(insertedId, botransaction.connection, botransaction.transaction);
				// -

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<byte[]>.SuccessResponse(getExcel(totalWWrongZn));
				}
				else
				{
					return ResponseModel<byte[]>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

		internal IEnumerable<string> ReadFromExcel(out List<string> errors)
		{
			errors = new List<string> { };
			try
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(MemoryStream memStream = new MemoryStream(this._data))
				{
					ExcelPackage package = new ExcelPackage(memStream);
					var worksheet = package.Workbook.Worksheets[0];
					var rowStart = worksheet.Dimension.Start.Row;
					var rowEnd = worksheet.Dimension.End.Row;

					// footer rows
					rowEnd -= 0;

					// get number of rows and columns in the sheet
					var rows = worksheet.Dimension.Rows;
					var columns = worksheet.Dimension.Columns;
					var startRowNumber = 1;
					var startColNumber = 1;

					if(rows > 1 && columns > 1)
					{
						var col1 = Convert.ToString(getCellValue(worksheet.Cells[1, 1]));
						var col2 = Convert.ToString(getCellValue(worksheet.Cells[1, 2]));
						//if(col1 != "Zolltarifnummer" || col2 != "Bezeichnung")
						//{
						//	errors.Add($"Excel Columns Incompatible please download the right DRAFT");
						//	return null;
						//}
						var data = new List<string>();

						// loop through the worksheet rows and columns
						for(int i = startRowNumber; i <= rowEnd; i++)
						{
							try
							{
								var zolltarifNr = getCellValue(worksheet.Cells[i, startColNumber]);
								var bezeichnung = getCellValue(worksheet.Cells[i, startColNumber + 1]);
								if(!string.IsNullOrWhiteSpace(zolltarifNr))
								{
									data.Add(zolltarifNr.Trim());
								}
								else
								{
									//errors.Add($"Row {i}: invalid FA number [{FA}].");
								}
							} catch(System.Exception exceptionInternal)
							{
								Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
								errors.Add($"Row {i}: unknown error. " + exceptionInternal.Message);
							}
						}

						// - 
						return data;
					}
					else
					{
						errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
						return null;
					}
				}
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}
		internal static string getCellValue(ExcelRange cell)
		{
			var val = cell.Value;
			if(val == null)
			{
				return "";
			}

			return val.ToString().Trim();
		}


		internal byte[] getExcel(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> articleWwZollNr)
		{
			try
			{
				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage())
				{
					// add sheet for Articles wo ZollNr
					var articleWoZollNr = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetArticlesWoZollNr();
					addSheet(articleWoZollNr, package.Workbook.Worksheets.Add($"ohne Zolltarifnummer"));

					// add sheet for Articles w wrong ZollNr
					addSheet(articleWwZollNr, package.Workbook.Worksheets.Add($"falsche Zolltarifnummer"), true);

					// Set some document properties
					package.Workbook.Properties.Title = "Zolltarifnummern";
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
		private static void addSheet(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> data, ExcelWorksheet worksheet, bool lastColumnError = false)
		{
			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 1;
			var startColumnNumber = 1;
			var numberOfColumns = 4;
			// Start adding the header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
			worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Freigabestatus";
			worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Zolltarif-Nr";


			var rowNumber = headerRowNumber + 1;
			if(data?.Count > 0)
			{

				// Loop through 
				foreach(var item in data)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = item.ArtikelNummer;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = item.Bezeichnung1;
					worksheet.Cells[rowNumber, startColumnNumber + 2].Value = item.Freigabestatus;
					worksheet.Cells[rowNumber, startColumnNumber + 3].Value = item.Zolltarif_nr;
					if(lastColumnError)
					{
						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Font.Color.SetColor(Color.Red);
					}

					// -
					worksheet.Row(rowNumber).Height = 18;
					rowNumber += 1;
				}

				// Doc content
				using(var range = worksheet.Cells[2, 1, rowNumber - 1, numberOfColumns])
				{
					range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(Color.White);
					range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				}

				//// Pre + Header
				using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
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
			}

			// Fit the columns according to its content
			for(int i = 1; i <= numberOfColumns; i++)
			{
				worksheet.Column(i + startColumnNumber).AutoFit();
			}
		}
	}
}
