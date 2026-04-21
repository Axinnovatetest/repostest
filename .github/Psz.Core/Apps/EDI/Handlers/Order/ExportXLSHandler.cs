using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public partial class Order
	{
		public class ExportXLSHandler: IHandle<UserModel, ResponseModel<byte[]>>
		{
			private UserModel _user { get; set; }
			private int _data { get; set; }
			public ExportXLSHandler(UserModel user, int id)
			{
				this._user = user;
				this._data = id;
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

					return ResponseModel<byte[]>.SuccessResponse(GetData());
				} catch(Exception exception)
				{
					Infrastructure.Services.Logging.Logger.Log(exception);
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

				if(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data) == null)
					return ResponseModel<byte[]>.FailureResponse("Order not found.");

				return ResponseModel<byte[]>.SuccessResponse();
			}

			public byte[] GetData()
			{
				var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
				var orderItemEntites = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data, false);
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntites.Select(x => x.ArtikelNr ?? -1)?.ToList());

				return SaveToExcelFile(orderEntity, orderItemEntites, articleEntities);
			}

			internal byte[] SaveToExcelFile(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity orderEntity,
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> orderItemEntities,
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> orderArticleEntities
				)
			{
				try
				{
					var chars = new char[] { ' ', '#' };
					var tempFolder = System.IO.Path.GetTempPath();
					var filePath = System.IO.Path.Combine(tempFolder, $"Order-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

					var file = new FileInfo(filePath);

					// FIXME: Replace EPPlus by NPOI, or some other alt
					ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
					// Create the package and make sure you wrap it in a using statement
					using(var package = new ExcelPackage(file))
					{
						// add a new worksheet to the empty workbook
						ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"EDI Orders");

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
						worksheet.Cells[1, 1].Value = $"EDI Order";
						worksheet.Cells[1, 1].Style.Font.Size = 16;

						// - Header Start
						// - First Column
						worksheet.Cells[headerRowNumber + 0, startColumnNumber].Value = "Kunde";
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 1].Value = orderEntity?.Unser_Zeichen;
						worksheet.Cells[headerRowNumber + 1, startColumnNumber].Value = "Name";
						worksheet.Cells[headerRowNumber + 1, startColumnNumber + 1].Value = orderEntity?.Vorname_NameFirma;
						worksheet.Cells[headerRowNumber + 2, startColumnNumber].Value = "Name 2";
						worksheet.Cells[headerRowNumber + 2, startColumnNumber + 1].Value = orderEntity?.Name2;
						worksheet.Cells[headerRowNumber + 3, startColumnNumber].Value = "Name 3";
						worksheet.Cells[headerRowNumber + 3, startColumnNumber + 1].Value = orderEntity?.Name3;
						worksheet.Cells[headerRowNumber + 4, startColumnNumber].Value = "Ansprechpartner";
						worksheet.Cells[headerRowNumber + 4, startColumnNumber + 1].Value = orderEntity?.Ansprechpartner;
						worksheet.Cells[headerRowNumber + 5, startColumnNumber].Value = "Abteilung";
						worksheet.Cells[headerRowNumber + 5, startColumnNumber + 1].Value = orderEntity?.Abteilung;
						worksheet.Cells[headerRowNumber + 6, startColumnNumber].Value = "Straße/Postfach";
						worksheet.Cells[headerRowNumber + 6, startColumnNumber + 1].Value = orderEntity?.Straße_Postfach;
						worksheet.Cells[headerRowNumber + 7, startColumnNumber].Value = "Adress 2";
						worksheet.Cells[headerRowNumber + 7, startColumnNumber + 1].Value = orderEntity?.Land_PLZ_Ort;
						worksheet.Cells[headerRowNumber + 8, startColumnNumber].Value = "Briefanrede";
						worksheet.Cells[headerRowNumber + 8, startColumnNumber + 1].Value = orderEntity?.Briefanrede;

						// - Second Column
						var shiftCols = 3;
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + shiftCols].Value = "Dokument Nr";
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + shiftCols + 1].Value = orderEntity?.Bezug;
						worksheet.Cells[headerRowNumber + 1, startColumnNumber + shiftCols].Value = "Ihr Zeichen";
						worksheet.Cells[headerRowNumber + 1, startColumnNumber + shiftCols + 1].Value = orderEntity?.Ihr_Zeichen;
						worksheet.Cells[headerRowNumber + 2, startColumnNumber + shiftCols].Value = "Versand";
						worksheet.Cells[headerRowNumber + 2, startColumnNumber + shiftCols + 1].Value = orderEntity?.Versandart;
						worksheet.Cells[headerRowNumber + 3, startColumnNumber + shiftCols].Value = "Zahlungsziel";
						worksheet.Cells[headerRowNumber + 3, startColumnNumber + shiftCols + 1].Value = orderEntity?.Falligkeit.HasValue == true ? orderEntity?.Falligkeit : "";
						worksheet.Cells[headerRowNumber + 3, startColumnNumber + shiftCols + 1].Style.Numberformat.Format = Program.XLS_FORMAT_DATE;
						worksheet.Cells[headerRowNumber + 3, startColumnNumber + shiftCols + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
						worksheet.Cells[headerRowNumber + 4, startColumnNumber + shiftCols].Value = "Konditionen";
						worksheet.Cells[headerRowNumber + 4, startColumnNumber + shiftCols + 1].Value = orderEntity?.Konditionen;
						worksheet.Cells[headerRowNumber + 5, startColumnNumber + shiftCols].Value = "Notiz";
						worksheet.Cells[headerRowNumber + 5, startColumnNumber + shiftCols + 1].Value = orderEntity?.Freie_Text;
						worksheet.Cells[headerRowNumber + 6, startColumnNumber + shiftCols].Value = "Projekt Nr";
						worksheet.Cells[headerRowNumber + 6, startColumnNumber + shiftCols + 1].Value = orderEntity?.Projekt_Nr;
						worksheet.Cells[headerRowNumber + 7, startColumnNumber + shiftCols].Value = "Vorfall Nr";
						worksheet.Cells[headerRowNumber + 7, startColumnNumber + shiftCols + 1].Value = orderEntity?.Angebot_Nr;
						worksheet.Cells[headerRowNumber + 7, startColumnNumber + shiftCols + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

						headerRowNumber += 10;
						// - Header End

						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Pos";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Dokument Nr";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung 1";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Original Anzahl";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Wunschtermin";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Einzel VK Festpreis";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Einheitspreis Basis";

						var rowNumber = headerRowNumber + 1;
						if(orderItemEntities != null && orderItemEntities.Count > 0)
						{
							// Loop through 
							foreach(var w in orderItemEntities)
							{
								var article = orderArticleEntities?.FirstOrDefault(x => x.ArtikelNr == w?.ArtikelNr);
								// -
								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Position;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = orderEntity?.Bezug;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = article?.ArtikelNummer;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bezeichnung1;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.OriginalAnzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Wunschtermin.HasValue == true ? w?.Wunschtermin : "";
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.VKEinzelpreis;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Preiseinheit;

								worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Program.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Program.XLS_FORMAT_DATE;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Program.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Program.XLS_FORMAT_NUMBER;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}

						#region Makeup
						//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
						using(var range = worksheet.Cells[1, 1, headerRowNumber - 2, numberOfColumns])
						{
							range.Style.Font.Bold = true;
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
							range.Style.Font.Color.SetColor(Color.Black);
							range.Style.ShrinkToFit = false;
						}
						// Darker Blue in Top cell
						worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

						using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
						{
							range.Style.Font.Bold = true;
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
							range.Style.Font.Color.SetColor(Color.Black);
							range.Style.ShrinkToFit = false;
						}

						// Doc content
						if(orderItemEntities != null && orderItemEntities.Count > 0)
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
						#endregion Makeup

						// Set some document properties
						package.Workbook.Properties.Title = $"EDI Orders";
						package.Workbook.Properties.Author = "PSZ ERP";
						package.Workbook.Properties.Company = "PSZ ERP";

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
}
