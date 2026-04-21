using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.Linq;

	public class GetBedarfHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Basics.BedarfResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.BedarfRequestModel _data { get; set; }
		public GetBedarfHandler(UserModel user, Models.Article.Statistics.Basics.BedarfRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Basics.BedarfResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				string fertigungNumber, fertigungLager;
				switch(this._data.Land)
				{
					case 1:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.TN}";
						fertigungLager = $"Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.TN.GetDescription()}";
						break;
					case 2:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.WS}";
						fertigungLager = $"{Common.Enums.ArticleEnums.ArticleProductionPlace.WS.GetDescription()}";
						break;
					case 3:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.AL}";
						fertigungLager = $"Albanien - {Common.Enums.ArticleEnums.ArticleProductionPlace.AL.GetDescription()}";
						break;
					case 4:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.CZ}";
						fertigungLager = $"Eigenfertigung - {Common.Enums.ArticleEnums.ArticleProductionPlace.CZ.GetDescription()}";
						break;
					case 5:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.DE}";
						fertigungLager = $"Fertigung - {Common.Enums.ArticleEnums.ArticleProductionPlace.DE.GetDescription()}";
						break;
					//case 6:
					//	fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.BETN}";
					//	fertigungLager = $"Benane Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.BETN.GetDescription()}";
					//	break;
					case 7:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN}";
						fertigungLager = $"Ghezala Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN.GetDescription()}";
						break;
					default:
						fertigungNumber = "";
						fertigungLager = "";
						break;
				}
				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.BruttoBedarf.getQueryParamed(this._user.Id, this._data.ArticleId, this._data.ArticleNumber, this._data.Land, fertigungNumber, fertigungLager, Module.AppSettings?.ALVirtualBestandArticleIds);

				//- 2024-04-24 - AK - Add ProjectPurchase to Bestellungen
				if(results?.Item4.Count > 0)
				{
					var pos = results.Item4.Select(x => x.PO ?? -1).ToList();
					var bestellungenEntities = Infrastructure.Data.Access.Tables.BSD.BestellungenAccess.GetByProjectNr(results.Item4.Select(x => x.PO ?? -1).ToList());
					foreach(var item in results.Item4)
					{
						var be = bestellungenEntities.FirstOrDefault(x => x.Projekt_Nr == $"{item.PO}");
						if(be is not null)
						{
							item.ProjectPurchase = be.ProjectPurchase;
							item.CreateDatePO = be.Datum;
						}
					}
				}
				// - 2025-02-10 - AK - show FA date
				if(results?.Item1.Count > 0)
				{
					var fas = results.Item1.Select(x => int.TryParse(x.Fertigung, out var result) ? result : 0).ToList();
					var fertigungEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(fas);
					foreach(var item in results.Item1)
					{
						var fa = fertigungEntities.FirstOrDefault(x => $"{x.Fertigungsnummer}" == item.Fertigung?.Trim());
						if(fa is not null)
						{
							item.CreateDateFA = fa.Datum;
						}
					}
				}
				if(results?.Item2.Count > 0)
				{
					var fas = results.Item2.Select(x => int.TryParse(x.Fertigung, out var result) ? result : 0).ToList();
					var fertigungEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(fas);
					foreach(var item in results.Item2)
					{
						var fa = fertigungEntities.FirstOrDefault(x => $"{x.Fertigungsnummer}" == item.Fertigung?.Trim());
						if(fa is not null)
						{
							item.CreateDateFA = fa.Datum;
						}
					}
				}

				return ResponseModel<Models.Article.Statistics.Basics.BedarfResponseModel>.SuccessResponse(
					new Models.Article.Statistics.Basics.BedarfResponseModel
					{
						Title = results?.Item5,
						BedarfPositive = results?.Item1.Select(x => new Models.Article.Statistics.Basics.BedarfResponseModel.NeedsModel(x))?.ToList(),
						BedarfNegative = results?.Item2.Select(x => new Models.Article.Statistics.Basics.BedarfResponseModel.NeedsModel(x))?.ToList(),
						Suppliers = results?.Item3.Select(x => new Models.Article.Statistics.Basics.BedarfResponseModel.SupplierModel(x))?.ToList(),
						SubItems = results?.Item4.Select(x => new Models.Article.Statistics.Basics.BedarfResponseModel.BestellungModel(x))?.ToList()
					});
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log($"GetBedarf: ArticleId [{this._data.ArticleId}], ArticleNumber [{this._data.ArticleNumber}], Land [{this._data.Land}]");
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Basics.BedarfResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Basics.BedarfResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<Models.Article.Statistics.Basics.BedarfResponseModel>.FailureResponse("Article not found");

			if(new List<int> { 1, 2, 3, 4, 5, 6, 7 }.Exists(x => x == this._data.Land) != true)
				return ResponseModel<Models.Article.Statistics.Basics.BedarfResponseModel>.FailureResponse("Land not found");

			return ResponseModel<Models.Article.Statistics.Basics.BedarfResponseModel>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Bedarf-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Bedarf");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 4;
					var startColumnNumber = 1;
					var numberOfColumns = 16;

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
					worksheet.Cells[1, 1].Value = $"{data.Body.Title}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;


					var bestand = 0m;
					var reserviert = 0m;
					if(data.Body.BedarfNegative != null && data.Body.BedarfNegative.Count > 0)
					{
						bestand = data.Body.BedarfNegative[0].Verfug_Ini ?? 0;
						reserviert = data.Body.BedarfNegative[0].Reserviert_Menge ?? 0;
					}
					else
					{
						if(data.Body.BedarfPositive != null && data.Body.BedarfPositive.Count > 0)
						{
							bestand = data.Body.BedarfPositive[0].Verfug_Ini ?? 0;
							reserviert = data.Body.BedarfPositive[0].Reserviert_Menge ?? 0;
						}
					}
					worksheet.Cells[2, 1].Value = $"Artikelnummer: {this._data.ArticleNumber}";
					worksheet.Cells[2, 2].Value = $"Bezeichnung: {articleEntity?.Bezeichnung1}";
					worksheet.Cells[2, 3].Value = $"Bestand: {bestand.ToString("0.###")}";
					worksheet.Cells[2, 4].Value = $"Reserviert: {reserviert.ToString("0.###")}";

					var rowNumber = 0;
					#region Lieferant
					if(data.Body.Suppliers != null && data.Body.Suppliers.Count > 0)
					{
						headerRowNumber = rowNumber + 4;
						worksheet.Cells[headerRowNumber - 1, startColumnNumber].Value = "Lieferanten";
						worksheet.Cells[headerRowNumber - 1, startColumnNumber].Style.Font.Size = 16;
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Lieferant";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Std.";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bestell-Nr";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Preis";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "LT";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "MQ";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Telefon";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Fax";

						rowNumber = headerRowNumber + 1;
						if(data.Success == true)
						{
							//Loop through
							foreach(var w in data.Body.Suppliers)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Lieferant;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Standar_Liferent;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bestell_Nr;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Peis;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.LT;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.MQO;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Telefon;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Fax;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}
					#endregion Lieferant

					#region Bestellungen
					if(data.Body.SubItems != null && data.Body.SubItems.Count > 0)
					{
						headerRowNumber = rowNumber + 4;
						worksheet.Cells[headerRowNumber - 1, startColumnNumber].Value = "Bestellungen";
						worksheet.Cells[headerRowNumber - 1, startColumnNumber].Style.Font.Size = 16;
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PO #";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Lieferant";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Anzahl";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Liefertermin";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "AB Termin";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "AB";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bemerkung";

						rowNumber = headerRowNumber + 1;
						if(data.Success == true)
						{
							//Loop through
							foreach(var w in data.Body.SubItems)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.PO;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.VornameFirma;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Anzhal;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Liefertermin?.ToString("dd/MM/yyyy");
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.ABtermin?.ToString("dd/MM/yyyy");
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.AB;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Bemerkung;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}
					#endregion Bestellungen

					#region FA Gestartet
					if(data.Body.BedarfPositive != null && data.Body.BedarfPositive.Count > 0)
					{
						headerRowNumber = rowNumber + 4;
						worksheet.Cells[headerRowNumber - 1, startColumnNumber].Value = "FA Gestartet";
						worksheet.Cells[headerRowNumber - 1, startColumnNumber].Style.Font.Size = 16;
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Termin FA";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "FA-Nummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "FA Offen";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Anzahl/Stk";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bedarf/FA";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Termin MA";
						worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Verfügar";
						worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bedarf summiert";
						worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "S Extern";
						worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "S Intern";
						worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "K";
						worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "T.K.";
						worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Gesch.";

						rowNumber = headerRowNumber + 1;
						if(data.Success == true)
						{
							//Loop through
							foreach(var w in data.Body.BedarfPositive)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Termin_Bestatigen;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Fertigung;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.ArtikelNummer;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bezeichnung;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.FA_Offen;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Anzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Bedarf_FA;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Termin_MA;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Verfugbar;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Bedarf_Summiert;
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.S_Extetrn;
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.S_Intern;
								worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Kommisioniert_komplett;
								worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Kommisioniert_teilweise;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Kabel_geschnitten;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}
					#endregion FA Gestartet

					#region FA nicht Gestartet
					if(data.Body.BedarfNegative != null && data.Body.BedarfNegative.Count > 0)
					{
						headerRowNumber = rowNumber + 4;
						worksheet.Cells[headerRowNumber - 1, startColumnNumber].Value = "FA nicht Gestartet";
						worksheet.Cells[headerRowNumber - 1, startColumnNumber].Style.Font.Size = 16;
						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Termin FA";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "FA-Nummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "FA Offen";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Anzahl/Stk";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bedarf/FA";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Termin MA";
						worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Verfügar";
						worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bedarf summiert";
						worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "S Extern";
						worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "S Intern";
						worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "K";
						worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "T.K.";
						worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Gesch.";

						rowNumber = headerRowNumber + 1;
						if(data.Success == true)
						{
							//Loop through
							foreach(var w in data.Body.BedarfNegative)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Termin_Bestatigen;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Fertigung;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.ArtikelNummer;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bezeichnung;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.FA_Offen;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Anzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Bedarf_FA;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Termin_MA;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Verfugbar;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Bedarf_Summiert;
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.S_Extetrn;
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.S_Intern;
								worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Kommisioniert_komplett;
								worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Kommisioniert_teilweise;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Kabel_geschnitten;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}
					#endregion FA nicht Gestartet
					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					//using (var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					//{
					//    range.Style.Font.Bold = true;
					//    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					//    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
					//    range.Style.Font.Color.SetColor(Color.Black);
					//    range.Style.ShrinkToFit = false;
					//}
					// Darker Blue in Top cell
					//worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					//if (data.Success == true)
					//{
					//    // Doc content
					//    if (data.Body != null && data.Body.SubItems.Count > 0)
					//    {
					//        using (var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
					//        {
					//            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					//            range.Style.Fill.BackgroundColor.SetColor(Color.White);
					//            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//        }
					//    }
					//}

					//// Thick countour
					//using (var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					//{
					//    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					//}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"Bedarf";
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
