using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	using MoreLinq;
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public partial class Order
	{
		public class ExportUnvalidatedXLSHandler: IHandle<UserModel, ResponseModel<byte[]>>
		{
			private UserModel _user { get; set; }
			private List<int> _data { get; set; }
			public ExportUnvalidatedXLSHandler(UserModel user, List<int> ids)
			{
				this._user = user;
				this._data = ids;
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
				var orderEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetUnconfirmedConfirmationByKundenNrEDI(
					Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data)
					?.Select(x => x.Nummer ?? -1)?.ToList());
				var orderItemEntites = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderEntities.Select(x => x.Nr)?.ToList(), false);
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntites?.Select(x => x.ArtikelNr ?? -1)?.ToList());

				return SaveToExcelFile(orderEntities, orderItemEntites, articleEntities);
			}

			internal byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> orderEntity,
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

						saveForMonoCustomer(worksheet, orderEntity, orderItemEntities, orderArticleEntities);

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
			void saveForMonoCustomer(ExcelWorksheet worksheet,
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> orderEntities,
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> orderItemEntities,
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> orderArticleEntities)
			{
				// Keep track of the row that we're on, but start with four to skip the header
				var headerRowNumber = 2;
				var startColumnNumber = 1;
				var numberOfColumns = 21;

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
				worksheet.Cells[1, 1].Value = $"EDI Orders";
				worksheet.Cells[1, 1].Style.Font.Size = 16;
				// - Header End

				// Start adding the header
				worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "BE-Nummer";
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Position";
				worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "AB-Nummer";
				worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Index Kunde";
				worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Kunden-Nr.";
				worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "PSZ";
				worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Offen m. Abzug Läger";
				worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "BE-Menge";
				worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Offenen Menge";
				worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "WT";
				worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "LT";
				worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "VOH";
				worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "LKW";
				worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "LKW";
				worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "LKW";
				worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "LKW";
				worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Lieferschein";
				worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Bemerkung";
				worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "EM / RP";
				worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "FA";
				worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Preis BE";

				var rowNumber = headerRowNumber + 1;
				if(orderEntities != null && orderEntities.Count > 0)
				{
					foreach(var orderEntity in orderEntities.DistinctBy(x => x.Nr)?.OrderBy(x => x.Datum))
					{
						var _orderItemEntities = orderItemEntities.Where(x => x.AngebotNr == orderEntity.Nr)?.OrderBy(x => x.Position ?? 0)?.ToList();
						if(_orderItemEntities != null && _orderItemEntities.Count > 0)
						{
							// Loop through 
							foreach(var w in _orderItemEntities)
							{
								var article = orderArticleEntities?.FirstOrDefault(x => x.ArtikelNr == w?.ArtikelNr);
								// -
								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = orderEntity?.Bezug;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Position;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Index_Kunde;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = article?.Bezeichnung1;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = article?.ArtikelNummer;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.OriginalAnzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.AktuelleAnzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Wunschtermin.HasValue == true ? w?.Wunschtermin : "";
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 12].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 13].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 14].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 15].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 16].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 17].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 18].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 19].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 20].Value = w?.Gesamtpreis;

								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Program.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Program.XLS_FORMAT_DATE;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Program.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 20].Style.Numberformat.Format = Program.XLS_FORMAT_NUMBER;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}
				}

				#region Makeup
				//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
				using(var range = worksheet.Cells[1, 1, headerRowNumber - 1, numberOfColumns])
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
			}
		}
	}
}
