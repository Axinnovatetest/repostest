using Infrastructure.Services.Reporting.Models.MTM;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Helpers;
using System.Drawing;
using System.IO;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService
	{
		public ResponseModel<byte[]> UpdateSerieFaProdTimeCosts(Identity.Models.UserModel user)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var validationResponse = this.ValidateUpdateSerieFaProdTimeCosts(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//  - get all open serie fa with production time or costs equal to NULL or Zero
				var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetOpenSerieWithNullOrZeroTimeCosts();

				//  - for each serie fa get the production time and costs from the article
				if(faEntities == null || faEntities.Count == 0)
				{
					return ResponseModel<byte[]>.SuccessResponse();
				}

				var salesPriceType = Common.Enums.ArticleEnums.SalesItemType.Serie;
				var articleIds = faEntities.Select(x => x.Artikel_Nr ?? 0).Distinct().ToList();
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(articleIds, botransaction.connection, botransaction.transaction);
				var itemCalculatoryCostsEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetArbeitskostenByArtikelNr(articleIds, botransaction.connection, botransaction.transaction);
				var priceGroupEntities = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(articleIds, botransaction.connection, botransaction.transaction);
				var salesPriceExtEntities = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticlesAndType(articleIds, (int)salesPriceType, botransaction.connection, botransaction.transaction);
				var staffPriceEntities = Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.GetByArticles(articleIds, botransaction.connection, botransaction.transaction);

				// -
				var logs = new List<Tuple<int, decimal?, decimal?, decimal?, decimal?>>();
				foreach(var faItem in faEntities)
				{
					var articleId = faItem.Artikel_Nr ?? 0;
					var articleEntiy = articleEntities.FirstOrDefault(x => x.ArtikelNr == articleId);
					var priceGroupEntity = priceGroupEntities.FirstOrDefault(x => x.Artikel_Nr == articleId);
					var itemCalculatoryCostsEntity = itemCalculatoryCostsEntities.FirstOrDefault(x => x.Artikel_nr == articleId);
					var salesPriceExtention = salesPriceExtEntities.FirstOrDefault(x => x.ArticleNr == articleId);
					var priceType = CRPHelper.getPriceType(priceGroupEntity, faItem.Anzahl ?? 0);
					var staffPriceEntity = staffPriceEntities.FirstOrDefault(x => x.Artikel_Nr == articleId && x.Staffelpreis_Typ == priceType);

					// - 
					var price = (priceType == "S0" || staffPriceEntity == null) ? (itemCalculatoryCostsEntity?.Betrag ?? 0m) : (staffPriceEntity?.Betrag ?? 0m);
					var time = (priceType == "S0" || staffPriceEntity == null) ? ((articleEntiy?.Produktionszeit ?? salesPriceExtention?.Profuktionszeit) ?? 0m) : (staffPriceEntity?.ProduKtionzeit ?? 0m);

					if(faItem.Zeit != time || faItem.Preis != price)
					{
						// - 
						logs.Add(new Tuple<int, decimal?, decimal?, decimal?, decimal?>(faItem.Fertigungsnummer ?? 0, faItem.Zeit, faItem.Preis, time, price));

						// -
						faItem.Zeit = time;
						faItem.Preis = price;
					}
				}

				// - Update fa Time & Cost on fa
				if(logs.Count>0)
				{
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateProdTimeCosts(faEntities, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(
						logs.Select(x => new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
						{
							AngebotNr = null,
							DateTime = DateTime.Now,
							Id = 0,
							LogObject = "Fertigung",
							LogText = $"[Fertigung] Updated FA ID {x.Item1}: Time from [{x.Item2}] to [{x.Item4}], Cost from [{x.Item3}] to [{x.Item5}]",
							LogType = Helpers.LogHelper.LogType.MODIFICATIONFA.GetDescription(),
							Nr = x.Item1,
							Origin = "CRP",
							PositionNr = null,
							ProjektNr = null,
							UserId = user?.Id,
							Username = user?.Username
						}).ToList(), botransaction.connection, botransaction.transaction);
				}
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<byte[]>.SuccessResponse(getAsExcel(logs));
				}
				else
				{
					return ResponseModel<byte[]>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		ResponseModel<byte[]> ValidateUpdateSerieFaProdTimeCosts(Identity.Models.UserModel user)
		{
			if(user == null || (!user.SuperAdministrator && !user.IsGlobalDirector))
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			return ResponseModel<byte[]>.SuccessResponse();
		}
		private byte[] getAsExcel(List<Tuple<int, decimal?, decimal?, decimal?, decimal?>> data)
		{
			try
			{
				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"data");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 5;

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
					worksheet.Cells[1, 1].Value = $"Updated Serie FA Production Time and Costs - Generated {DateTime.Now:dd.MM.yyyy}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Fertigungs nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Old time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "New time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Old price";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "New price";

					int rowNumber = headerRowNumber + 1;
					// Now add the data
					if(data != null && data.Count > 0)
					{
						foreach(var item in data)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = item.Item1;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = item.Item2;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = item.Item4;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = item.Item3;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = item.Item5;
							rowNumber++;
						}
					}

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6EFCE"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4ec000"));


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
					package.Workbook.Properties.Title = "Report 1";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

					// save our new workbook and we are done!
					package.Save();

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
