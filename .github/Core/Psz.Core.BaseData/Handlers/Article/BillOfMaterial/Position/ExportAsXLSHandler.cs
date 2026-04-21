using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class ExportAsXLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.ExportAsXLSModel _data { get; set; }


		public ExportAsXLSHandler(Identity.Models.UserModel user, Models.Article.BillOfMaterial.ExportAsXLSModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
				// - get last bom
				if(this._data.BomVerion == null)
				{
					var articleExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArticleId);
					var stucklistenPositionsEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data.ArticleId);
					// --
					return ResponseModel<byte[]>.SuccessResponse(getXLSData(articleEntity, articleExtensionEntity, stucklistenPositionsEntities, null, this._data.PositionsOnly));
				}
				else
				{
					var articleExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticleAndBomVersion(this._data.ArticleId, this._data.BomVerion ?? -1);
					if(articleExtensionEntity == null)
					{
						Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.Insert(
							new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
							{
								ArticleId = this._data.ArticleId,
								ArticleDesignation = articleEntity?.Bezeichnung1,
								ArticleNumber = articleEntity?.ArtikelNummer,
								BomStatus = Enums.ArticleEnums.BomStatus.Approved.GetDescription(), // BOM should be validated, as per Input data
								BomStatusId = (int)Enums.ArticleEnums.BomStatus.Approved,
								BomValidFrom = null,
								BomVersion = this._data.BomVerion,
								Id = -1,
								LastUpdateTime = DateTime.Now,
								LastUpdateUserId = -2
							});
						// -
						articleExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticleAndBomVersion(this._data.ArticleId, this._data.BomVerion ?? -1);
					}
					var snapshotsEntities = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndVersion(this._data.ArticleId, this._data.BomVerion);
					// --
					return ResponseModel<byte[]>.SuccessResponse(getXLSData(articleEntity, articleExtensionEntity, null, snapshotsEntities, this._data.PositionsOnly));
				}
			} catch(Exception e)
			{
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
				return ResponseModel<byte[]>.FailureResponse(key: "1", value: "Article not found");

			var snapshotsEntities = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndVersion(this._data.ArticleId, this._data.BomVerion);
			if(this._data.BomVerion != null && (snapshotsEntities == null || snapshotsEntities.Count <= 0))
				return ResponseModel<byte[]>.FailureResponse("BOM version not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal static byte[] getXLSData(
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity oarticleEntity,
			Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity articleExtensionEntity,
			List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> stucklistenPositionsEntities,
			List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> snapshotEntities,
			bool positionsOnly = false)
		{
			try
			{
				var positionArticleIds = stucklistenPositionsEntities?.Select(x => x.Artikel_Nr_des_Bauteils ?? -1)?.ToList() ?? new List<int>();
				positionArticleIds.AddRange(snapshotEntities?.Select(x => x.Artikel_Nr_des_Bauteils ?? -1)?.ToList() ?? new List<int>());
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(positionArticleIds);
				var bestellnummernEntities = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByStandardSupplier(positionArticleIds);
				var addressEntites = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(bestellnummernEntities?.Select(x => x.Lieferanten_Nr ?? -1)?.ToList());
				var chars = new char[] { ' ', '#' };

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"data-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"BOM");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
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
					if(!positionsOnly)
					{
						var user = "Admin IT";
						if(articleExtensionEntity.BomStatusId == (int)Enums.ArticleEnums.BomStatus.Approved)
						{
							user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(snapshotEntities?[0]?.SnapshotUserId ?? -1)?.Username;
						}
						worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
						worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						worksheet.Cells[1, 1].Value = $"[{oarticleEntity.ArtikelNummer}] {oarticleEntity.Bezeichnung1} - {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
						worksheet.Cells[1, 1].Style.Font.Size = 16;
						worksheet.Cells[2, 1].Value = $"Version:";
						worksheet.Cells[2, 2].Value = $"{articleExtensionEntity.BomVersion}";
						worksheet.Cells[3, 1].Value = $"Status:";
						worksheet.Cells[3, 2].Value = $"{articleExtensionEntity.BomStatus}{(articleExtensionEntity.BomStatusId == (int)Enums.ArticleEnums.BomStatus.Approved ? $" by {user} on {(snapshotEntities?[0]?.SnapshotTime)?.ToString("dd/MM/yyyy")}" : "")}";
						worksheet.Cells[4, 1].Value = $"Valid from:";
						worksheet.Cells[4, 2].Value = $"{articleExtensionEntity.BomValidFrom}";
						headerRowNumber = 6;
					}



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Pos";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Artikelbezeichnung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelbezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Standardlieferant / Bestellnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "EK-Preis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Dokument";


					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(stucklistenPositionsEntities != null && stucklistenPositionsEntities.Count > 0)
					{
						stucklistenPositionsEntities = stucklistenPositionsEntities.OrderBy(x => x.Artikelnummer).ToList();
						foreach(var w in stucklistenPositionsEntities)
						{
							var articleEntity = articleEntities?.FirstOrDefault(x => x.ArtikelNr == w?.Artikel_Nr_des_Bauteils);
							var bestellnummern = bestellnummernEntities.FirstOrDefault(x => x.ArtikelNr == w?.Artikel_Nr_des_Bauteils);
							var address = addressEntites.FirstOrDefault(x => x.Nr == (bestellnummern?.Lieferanten_Nr ?? -1));

							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Position;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung_des_Bauteils;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = articleEntity?.Bezeichnung2;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = $"{address?.Name1}# {bestellnummern?.Bestell_Nr}".Trim(chars);
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = bestellnummern?.Einkaufspreis.HasValue == true ? bestellnummern?.Einkaufspreis.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Anzahl.HasValue == true ? w.Anzahl.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.DocumentId > 0 ? "Yes" : "No";

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}
					else
					{
						if(snapshotEntities != null && snapshotEntities.Count > 0)
						{
							foreach(var w in snapshotEntities)
							{
								//worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Position;
								//worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Artikelnummer;
								//worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung_des_Bauteils;
								//worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Anzahl;
								//worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.DocumentId > 0 ? "Yes" : "No";


								var articleEntity = articleEntities?.FirstOrDefault(x => x.ArtikelNr == w?.Artikel_Nr_des_Bauteils);
								var bestellnummern = bestellnummernEntities.FirstOrDefault(x => x.ArtikelNr == w?.Artikel_Nr_des_Bauteils);
								var address = addressEntites.FirstOrDefault(x => x.Nr == (bestellnummern?.Lieferanten_Nr ?? -1));

								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Position;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung_des_Bauteils;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = articleEntity?.Bezeichnung2;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = $"{address?.Name1}# {bestellnummern?.Bestell_Nr}".Trim(chars);
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = bestellnummern?.Einkaufspreis.HasValue == true ? bestellnummern?.Einkaufspreis.Value : "";
								worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Anzahl.HasValue == true ? w.Anzahl.Value : "";
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.DocumentId > 0 ? "Yes" : "No";

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
						range.Style.ShrinkToFit = true; // - expand cols
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Doc content
					if((stucklistenPositionsEntities != null && stucklistenPositionsEntities.Count > 0)
						|| (snapshotEntities != null && snapshotEntities.Count > 0))
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
						//worksheet.Column(i).AutoFit();
						worksheet.Column(i).Width = 25;
					}

					// -
					worksheet.Column(1).Width = 10;
					worksheet.Column(numberOfColumns).Width = 10;
					worksheet.Column(numberOfColumns - 1).Width = 10;
					worksheet.Column(numberOfColumns - 2).Width = 10;

					// -
					worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
					worksheet.PrinterSettings.FitToPage = true;

					// Set some document properties
					package.Workbook.Properties.Title = $"BOM";
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
	}
}
