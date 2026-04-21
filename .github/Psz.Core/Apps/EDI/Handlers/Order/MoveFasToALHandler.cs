using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Apps.EDI.Handlers
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class MoveFasToALHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private byte[] _data { get; set; }

		public MoveFasToALHandler(Identity.Models.UserModel user, byte[] data)
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


				#region // -- transaction-based logic -- //

				List<string> errors = new List<string>();
				var excelData = ReadFromExcel(out errors);
				if(errors.Count > 0)
				{ return ResponseModel<byte[]>.FailureResponse(errors); }

				if(excelData == null || excelData.Count() <= 0)
				{
					return ResponseModel<byte[]>.SuccessResponse(null);
				}

				// - 
				IEnumerable<MoveFaModel> oldArticlesNotFound = null;
				IEnumerable<MoveFaModel> fasUpdated = null;
				IEnumerable<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> oldArticlesUbg = null;
				List<Infrastructure.Data.Access.Joins.BSD.Migration.ArticleBomEntity> oldArticlesWUbgInStl = null;

				IEnumerable<MoveFaModel> newArticlesNotFound = null;
				IEnumerable<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> newArticlesUbg = null;
				List<Infrastructure.Data.Access.Joins.BSD.Migration.ArticleBomEntity> newArticlesWUbgInStl = null;

				#region  - // - FA  - // -
				var fas = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(excelData.Select(x => x.FaNumber).ToList());
				var fasNotFound = excelData.Where(x => fas.Exists(y => y.Fertigungsnummer == x.FaNumber) == false);
				var fasNotOpenOrStarted = fas?.Where(x => x.Kennzeichen.ToLower().Trim() != "offen" || (x.FA_Gestartet ?? false) == true);
				var fasOpenNotStarted = fas?.Where(x => x.Kennzeichen.ToLower().Trim() == "offen" && (x.FA_Gestartet ?? false) != true);
				var fasWithOrd = new List<Infrastructure.Data.Access.Joins.BSD.Migration.FertigungWOrder>();
				IEnumerable<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> fasOpenNotStartedWoOrders = null;
				// - all FAs are not Open (Storno or Erledigt) or Started
				if(fasOpenNotStarted == null || fasOpenNotStarted.Count() <= 0)
				{
					goto Done;
				}
				fasWithOrd = Infrastructure.Data.Access.Joins.BSD.Migration.GetFaWOrders(fasOpenNotStarted.Select(x => x.Fertigungsnummer ?? -1));
				if(fasWithOrd != null || fasWithOrd.Count() > 0)
				{
					var _fasWAb = fasWithOrd?.ToList();
					fasOpenNotStartedWoOrders = fasOpenNotStarted; //.Where(x => _fasWAb.Exists(y => y.Fertigungsnummer == x.Fertigungsnummer) == false);
				}

				#endregion fa

				#region  - // - OLD Articles  - // -
				var oldArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(excelData.Select(x => x.OldArticle).ToList());
				oldArticlesNotFound = excelData.Where(x => oldArticles?.Exists(y => y.ArtikelNummer?.Trim()?.ToLower() == x.OldArticle.Trim()?.ToLower()) == false);

				// - 1
				oldArticlesUbg = oldArticles?.Where(x => x.UBG == true);
				var oldArticlesNonUbg = oldArticles?.Where(x => x.UBG != true);
				// - all articles are UBGs
				if(oldArticlesNonUbg == null || oldArticlesNonUbg.Count() <= 0)
				{
					goto Done;
				}

				// - 2
				oldArticlesWUbgInStl = Infrastructure.Data.Access.Joins.BSD.Migration.GetBomsWUbg(oldArticlesNonUbg?.Select(x => x.ArtikelNr));
				var oldArticlesWoUbgInStl = oldArticlesNonUbg?.Where(x => oldArticlesWUbgInStl?.Exists(y => y.ArticleNr == x.ArtikelNr) == false);
				// - all articles have UBGs in STL
				if(oldArticlesWoUbgInStl == null || oldArticlesWoUbgInStl.Count() <= 0)
				{
					goto Done;
				}
				#endregion old articles

				#region - // - NEW Articles  - // -
				var newArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(excelData.Select(x => x.NewArticle).ToList());
				newArticlesNotFound = excelData.Where(x => newArticles?.Exists(y => y.ArtikelNummer?.Trim()?.ToLower() == x.NewArticle.Trim()?.ToLower()) == false)?.ToList();

				// - 1
				newArticlesUbg = newArticles?.Where(x => x.UBG == true);
				var newArticlesNonUbg = newArticles?.Where(x => x.UBG != true);
				// - all articles are UBGs
				if(newArticlesNonUbg == null || newArticlesNonUbg.Count() <= 0)
				{
					goto Done;
				}

				// - 2
				newArticlesWUbgInStl = Infrastructure.Data.Access.Joins.BSD.Migration.GetBomsWUbg(newArticlesNonUbg.Select(x => x.ArtikelNr));
				var newArticlesWoUbgInStl = newArticlesNonUbg.Where(x => newArticlesWUbgInStl.Exists(y => y.ArticleNr == x.ArtikelNr) == false)?.ToList();
				// - all articles have UBGs in STL
				if(newArticlesWoUbgInStl == null || newArticlesWoUbgInStl.Count() <= 0)
				{
					goto Done;
				}
				#endregion new articles


				int NEW_LAGER = 26;
				botransaction.beginTransaction();

				#region - // - Move FA data - // -

				// - update articles
				//Infrastructure.Services.Logging.Logger.LogTrace($"fasOpenNotStartedWoAbs -- BEFORE -- ");
				//Infrastructure.Services.Logging.Logger.LogTrace($"{System.Text.Json.JsonSerializer.Serialize(fasOpenNotStartedWoOrders)}");
				foreach(var item in fasOpenNotStartedWoOrders)
				{
					var x = excelData.FirstOrDefault(y => y.FaNumber == item.Fertigungsnummer);
					var y = newArticlesWoUbgInStl.FirstOrDefault(z => z.ArtikelNummer?.ToLower()?.Trim() == x?.NewArticle?.ToLower()?.Trim());
					item.Artikel_Nr = y?.ArtikelNr;
				}
				//Infrastructure.Services.Logging.Logger.LogTrace($"fasOpenNotStartedWoAbs -- AFTER -- ");
				//Infrastructure.Services.Logging.Logger.LogTrace($"{System.Text.Json.JsonSerializer.Serialize(fasOpenNotStartedWoOrders)}");

				// - 1 - change Lagers & Articles
				Infrastructure.Data.Access.Tables.PRS.FertigungAccess.ChangeLagerNdArticleWithTransaction(fasOpenNotStartedWoOrders.Select(x => new KeyValuePair<int, int>(x.ID, x.Artikel_Nr ?? -1)), NEW_LAGER, botransaction.connection, botransaction.transaction);

				// - 2 - update STL
				Infrastructure.Data.Access.Joins.BSD.Migration.UpdateFaStlWithTransaction(fasOpenNotStartedWoOrders.Select(x => x.ID), NEW_LAGER, botransaction.connection, botransaction.transaction);

				// - 3 update AB/BV positions articles
				foreach(var item in fasWithOrd)
				{
					var _ = excelData.FirstOrDefault(y => y.FaNumber == item.Fertigungsnummer);
					if(_ is not null)
					{
						var res = Order.UpdateElementItem(new Models.Order.UpdateElementItemModel
						{
							Id = item.OrderPositionId,
							ItemNumber = _.NewArticle,
							OrderId = item.OrderId
						}, this._user, botransaction);
					}
				}
				var _fasOpenNotStartedWoAbs = fasOpenNotStartedWoOrders.ToList();
				fasUpdated = excelData.Where(x => _fasOpenNotStartedWoAbs.Exists(y => y.Fertigungsnummer == x.FaNumber));

				#endregion move fa data

				#endregion // -- transaction-based logic -- //

				//- Jump on error breaking
				Done:
				if(botransaction.commit())
				{
					return ResponseModel<byte[]>.SuccessResponse(getExcel(fasUpdated, fasNotFound, fasNotOpenOrStarted, fasWithOrd,
						 oldArticlesNotFound, oldArticlesUbg, oldArticlesWUbgInStl,
						 newArticlesNotFound, newArticlesUbg, newArticlesWUbgInStl));
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
			if(this._user == null || this._user.SuperAdministrator != true)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

		internal IEnumerable<MoveFaModel> ReadFromExcel(out List<string> errors)
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
					var startRowNumber = 2;
					var startColNumber = 1;

					// - min 2 row and 5 columns
					if(rows > 1 && columns > 4)
					{
						var data = new List<MoveFaModel>();
						var err = false;
						// loop through the worksheet rows and columns
						for(int i = startRowNumber; i <= rowEnd; i++)
						{
							err = false;
							try
							{
								var oldArticle = Convert.ToString(getCellValue(worksheet.Cells[i, startColNumber + 0]));
								var faNumber = Convert.ToInt32(getCellValue(worksheet.Cells[i, startColNumber + 1]));
								var openQuantity = Convert.ToInt32(getCellValue(worksheet.Cells[i, startColNumber + 2]));
								var faDate = Convert.ToDateTime(getCellValue(worksheet.Cells[i, startColNumber + 3]));
								var newArticle = Convert.ToString(getCellValue(worksheet.Cells[i, startColNumber + 4]));

								if(string.IsNullOrWhiteSpace(oldArticle))
								{
									errors.Add($"Row {i}: invalid Old Article [{oldArticle}].");
									err = true;
								}
								if(faNumber <= 0)
								{
									errors.Add($"Row {i}: invalid FA-Number [{faNumber}].");
									err = true;
								}
								if(openQuantity <= 0)
								{
									errors.Add($"Row {i}: invalid FA Open Quantity [{openQuantity}].");
									err = true;
								}
								//if(string.IsNullOrWhiteSpace(oldArticle))
								//{
								//	errors.Add($"Row {i}: invalid Old Article [{oldArticle}].");
								//	err = true;
								//}
								if(string.IsNullOrWhiteSpace(newArticle))
								{
									errors.Add($"Row {i}: invalid New Article [{newArticle}].");
									err = true;
								}


								// - 
								if(!err)
								{
									data.Add(new MoveFaModel
									{
										FaDate = faDate,
										FaNumber = faNumber,
										NewArticle = newArticle,
										OldArticle = oldArticle,
										OpenQuantity = openQuantity,
									});
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
		internal class MoveFaModel
		{
			public int FaNumber { get; set; }
			public string OldArticle { get; set; }
			public string NewArticle { get; set; }
			public int OpenQuantity { get; set; }
			public DateTime FaDate { get; set; }
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


		internal byte[] getExcel(
			IEnumerable<MoveFaModel> fasUpdated,
			IEnumerable<MoveFaModel> fasNotFound,
			IEnumerable<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> fasNotOpenOrStarted,
			List<Infrastructure.Data.Access.Joins.BSD.Migration.FertigungWOrder> fasWithAB,
			IEnumerable<MoveFaModel> oldArticlesNotFound,
			IEnumerable<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> oldArticlesUbg,
			List<Infrastructure.Data.Access.Joins.BSD.Migration.ArticleBomEntity> oldArticlesWUbgInStl,
			IEnumerable<MoveFaModel> newArticlesNotFound,
			IEnumerable<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> newArticlesUbg,
			List<Infrastructure.Data.Access.Joins.BSD.Migration.ArticleBomEntity> newArticlesWUbgInStl
			)
		{
			try
			{
				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage())
				{
					// add sheet for FA
					addSheet(fasUpdated, package.Workbook.Worksheets.Add($"FA Updated"));
					addSheet(fasNotFound?.Select(x => x.FaNumber.ToString()), package.Workbook.Worksheets.Add($"FA Not Found"), "FA-Nummer");
					addSheet(fasNotOpenOrStarted?.Select(x => new KeyValuePair<string, string>(x.Fertigungsnummer.ToString(), x.Kennzeichen)), package.Workbook.Worksheets.Add($"FA Storno-Erledigt"), "FA-Nummer", "Kennzeichen");
					addSheet(fasWithAB?.Select(x => new KeyValuePair<string, string>(x.Fertigungsnummer.ToString(), x.OrderNumber.ToString())), package.Workbook.Worksheets.Add($"FA with AB/BV"), "FA-Nummer", "AB-Nummer", true);

					// - add sheets for OLD Articles
					addSheet(oldArticlesNotFound?.Select(x => x.OldArticle.ToString()), package.Workbook.Worksheets.Add($"Old Article Not Found"), "Art-Nummer");
					addSheet(oldArticlesUbg?.Select(x => x.ArtikelNummer.ToString()), package.Workbook.Worksheets.Add($"Old Article UBG"), "Art-Nummer");
					addSheet(oldArticlesWUbgInStl, package.Workbook.Worksheets.Add($"Old Articles STL UBG"));

					// - add sheets for NEW Articles
					addSheet(newArticlesNotFound?.Select(x => x.OldArticle.ToString()), package.Workbook.Worksheets.Add($"New Article Not Found"), "Art-Nummer");
					addSheet(newArticlesUbg?.Select(x => x.ArtikelNummer.ToString()), package.Workbook.Worksheets.Add($"New Article UBG"), "Art-Nummer");
					addSheet(newArticlesWUbgInStl, package.Workbook.Worksheets.Add($"New Articles STL UBG"));


					// Set some document properties
					package.Workbook.Properties.Title = "FA-transfert";
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
		private static void addSheet(IEnumerable<string> data, ExcelWorksheet worksheet, string title)
		{
			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 1;
			var startColumnNumber = 1;
			var numberOfColumns = 1;
			// Start adding the header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = title;
			worksheet.TabColor = Color.Red;

			var rowNumber = headerRowNumber + 1;
			if(data?.Count() > 0)
			{
				// Loop through 
				foreach(var item in data)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = item;

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

			// Fit the columns according to its content
			for(int i = 1; i <= numberOfColumns; i++)
			{
				worksheet.Column(i + startColumnNumber).AutoFit();
			}
		}
		private static void addSheet(IEnumerable<KeyValuePair<string, string>> data, ExcelWorksheet worksheet, string title, string title2, bool warn = false)
		{
			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 1;
			var startColumnNumber = 1;
			var numberOfColumns = 2;
			// Start adding the header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = title;
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = title2;
			worksheet.TabColor = Color.Red;
			if(warn)
			{
				worksheet.TabColor = Color.Orange;
			}

			var rowNumber = headerRowNumber + 1;
			if(data?.Count() > 0)
			{
				// Loop through 
				foreach(var item in data)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = item.Key;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = item.Value;

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

			// Fit the columns according to its content
			for(int i = 1; i <= numberOfColumns; i++)
			{
				worksheet.Column(i + startColumnNumber).AutoFit();
			}
		}
		private static void addSheet(IEnumerable<Infrastructure.Data.Access.Joins.BSD.Migration.ArticleBomEntity> data, ExcelWorksheet worksheet)
		{
			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 1;
			var startColumnNumber = 1;
			var numberOfColumns = 4;
			// Start adding the header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "STL Pos";
			worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "STL Anzahl";
			worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "STL Artikelnummer";
			worksheet.TabColor = Color.Red;

			var rowNumber = headerRowNumber + 1;
			if(data?.Count() > 0)
			{
				// Loop through 
				foreach(var item in data)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = item.ArticleNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = item.BomPosition;
					worksheet.Cells[rowNumber, startColumnNumber + 2].Value = item.BomQuantity;
					worksheet.Cells[rowNumber, startColumnNumber + 3].Value = item.BomArticleNumber;

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

			// Fit the columns according to its content
			for(int i = 1; i <= numberOfColumns; i++)
			{
				worksheet.Column(i + startColumnNumber).AutoFit();
			}
		}
		private static void addSheet(IEnumerable<MoveFaModel> data, ExcelWorksheet worksheet)
		{
			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 1;
			var startColumnNumber = 1;
			var numberOfColumns = 3;
			// Start adding the header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "FA Number";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Old Article";
			worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "New Article";
			worksheet.TabColor = Color.Green;

			var rowNumber = headerRowNumber + 1;
			if(data?.Count() > 0)
			{
				// Loop through 
				foreach(var item in data)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = item.FaNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = item.OldArticle;
					worksheet.Cells[rowNumber, startColumnNumber + 2].Value = item.NewArticle;

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

			// Fit the columns according to its content
			for(int i = 1; i <= numberOfColumns; i++)
			{
				worksheet.Column(i + startColumnNumber).AutoFit();
			}
		}
	}
}
