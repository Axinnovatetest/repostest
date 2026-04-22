
using Infrastructure.Data.Access.Tables.BSD;
using Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static Infrastructure.Services.Reporting.IText;
namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class GetReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Psz.Core.Logistics.Models.InverntoryStockModels.ReportRequestModel _data;
		private Core.Identity.Models.UserModel _user;
		public GetReportHandler(Psz.Core.Logistics.Models.InverntoryStockModels.ReportRequestModel request, Core.Identity.Models.UserModel user)
		{
			_data = request;
			_user = user;
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


				byte[] reportData = _data.ReportId switch
				{
					6 => GetReportWip_PDF(),
					7 => GetReportWipTotal_PDF(),
					8 => GetReportDiffFull_PDF(),
					9 => GetReportDiffTopRoh_PDF(),
					10 => GetReportDiffTopFg_PDF(),
					11 => GetReportDiffFull_PDF(true),
					12 => GetReportPrices_PDF(),
					_ => GetReport()
				};

				return ResponseModel<byte[]>.SuccessResponse(reportData);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public byte[] GetReport()
		{
			try
			{
				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Report 1");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 6;

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
					worksheet.Cells[1, 1].Value = $"Report {this._data.ReportId} - Generated {DateTime.Now:dd.MM.yyyy}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Fertigungs nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikel nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Offene Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Fa Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Fa Geschnitten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Fa Kommisioniert";

					int rowNumber = headerRowNumber + 1;

					// -

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN_LIGHT));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN));


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
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

		public byte[] GetReportWip_PDF()
		{
			var sections = new List<object>
			{

			};
			var data = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.GetXlsWipProdData(this._data.LagerId ?? 0);
			foreach(var fa in data.Select(x => x.FA).Distinct())
			{
				var faRow = data.Where(x => x.FA == fa).FirstOrDefault();
				if(faRow == null)
					continue;

				sections.Add(
				new Infrastructure.Services.Reporting.IText.PdfTableSection<WipModel>
				{
					TableTitle = "",
					TotalLabel = "WIP-Gesamtwert (€):",
					TotalPropertyName = "WIP",
					Items = new List<WipModel>
					{
							new WipModel
							{
								FA = fa,
								OpenQty =faRow.OpenQty,
								Artikelnummer = faRow.Item,
								Bereich = "Kommissionierung",
								Fertigungsgrad = faRow.UserPicked.ToString("0.##"),
								WIP = faRow.FaPicked.ToString("0.00")
							},
							new WipModel
							{
								FA = fa,
								OpenQty =faRow.OpenQty,
								Artikelnummer = faRow.Item,
								Bereich = "Schneiderei",
								Fertigungsgrad = faRow.UserCut.ToString("0.##"),
								WIP = faRow.FaCut.ToString("0.00")
							},
							new WipModel
							{
								FA = fa,
								OpenQty =faRow.OpenQty,
								Artikelnummer = faRow.Item,
								Bereich = "Vorbereitung",
								Fertigungsgrad = faRow.UserPreped.ToString("0.##"),
								WIP = faRow.FaPreped.ToString("0.00")
							},
							new WipModel
							{
								FA = fa,
								OpenQty =faRow.OpenQty,
								Artikelnummer = faRow.Item,
								Bereich = "Montage",
								Fertigungsgrad = faRow.UserAssembled.ToString("0.##"),
								WIP = faRow.FaAssembled.ToString("0.00")
							},
							new WipModel
							{
								FA = fa,
								OpenQty =faRow.OpenQty,
								Artikelnummer = faRow.Item,
								Bereich = "Krimp",
								Fertigungsgrad = faRow.UserCrimped.ToString("0.##"),
								WIP = faRow.FaCrimped.ToString("0.00")
							},
							new WipModel
							{
								FA = fa,
								OpenQty =faRow.OpenQty,
								Artikelnummer = faRow.Item,
								Bereich = "Visuelle Kontrolle",
								Fertigungsgrad = faRow.UserOpticalInspected.ToString("0.##"),
								WIP = faRow.FaOpticalInspected.ToString("0.00")
							},
							new WipModel
							{
								FA = fa,
								OpenQty =faRow.OpenQty,
								Artikelnummer = faRow.Item,
								Bereich = "Elektrische Kontrolle",
								Fertigungsgrad = faRow.UserElectricalInspected.ToString("0.##"),
								WIP = faRow.FaElectricalInspected.ToString("0.00")
							},
					}
				});
			}

			var companyInfo = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
			var pdfBytes = Infrastructure.Services.Reporting.IText.GenerateMultiTableWithTotalPdf(sections,
			documentTitle: "Report 6: WIP-Erfassung",
			documentSubTitle: $"Standort: {((Enums.LagerEnum)(this._data.LagerId))} | Jahr: {DateTime.Now.Year}",
			documentDescription: $"Beschreibung: Der Bericht zeigt den Work-in-Progress (WIP)-Wert sowohl pro Fertigungsauftrag (FA) als auch den Gesamtwert aller laufenden Aufträge.\r\nEr dient dazu, den Produktionsfortschritt und die gebundenen Material- bzw. Arbeitskosten auf Auftragsebene sowie im Gesamtüberblick transparent darzustellen.",
			pageBreakBetweenTables: false,
			companyName: companyInfo.Name,
			companyAddress: $"{companyInfo.Address}, {companyInfo.PostalCode} {companyInfo.City}, {companyInfo.Country}",
			logoData: companyInfo.Logo);

			return pdfBytes;
		}
		public byte[] GetReportWipTotal_PDF()
		{
			var data = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.GetWipTotals(this._data.LagerId ?? -1)
					?.DistinctBy(x => x.FA)?.OrderBy(x => x.Due);
			var sections = new List<object>
			{
			};

			var customers = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get()
				?.OrderBy(x => x.Kunde)?.ToDictionary(x => x.Nummerschlüssel, x => x.Kunde);

			var customerWip = new List<WipCustomerModel>();
			foreach(var item in customers)
			{
				if(!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
				{
					var fas = data.Where(x => x.Item.StartsWith(item.Key)).OrderBy(x => x.Due).Distinct();
					if(fas?.Count() > 0)
					{
						sections.Add(
							new Infrastructure.Services.Reporting.IText.PdfTableSection<WipTotalModel>
							{
								TableTitle = $"{item.Key} | {item.Value}",
								TotalLabel = "WIP-Gesamtwert (€):",
								TotalPropertyName = "Total",
								Items = fas?.Select(x => new WipTotalModel(x))
							});
						// -
						customerWip.Add(new WipCustomerModel
						{
							Customer = $"{item.Key} | {item.Value}",
							Year = DateTime.Now.Year,
							FaCount = fas.Count(),
							WipTotal = fas.Sum(x => x.Total).ToString("0.00")
						});
					}
				}
			}
			// - 
			sections.Add(new Infrastructure.Services.Reporting.IText.PdfTableSection<WipCustomerModel>
			{
				TableTitle = $"WIP Zusammengfassung",
				TotalLabel = "WIP-Gesamtwert (€):",
				TotalPropertyName = "WipTotal",
				Items = customerWip
			});

			var companyInfo = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
			var pdfBytes = Infrastructure.Services.Reporting.IText.GenerateMultiTableWithTotalPdf(sections,
			documentTitle: "Report 7: WIP-Gesamt",
			documentSubTitle: $"Standort: {((Enums.LagerEnum)(this._data.LagerId))} | Jahr: {DateTime.Now.Year}",
			documentDescription: $"Beschreibung: Der Bericht zeigt den Gesamtbestand an Work in Progress (WIP) innerhalb der PSZ-Gruppe, aufgeschlüsselt nach Standorten.\r\nEr bietet einen Überblick über den aktuellen Produktionsfortschritt und die noch in Bearbeitung befindlichen Aufträge an allen Unternehmensstandorten, um Transparenz über den Fertigungsstatus und die Materialbindung zu gewährleisten.",
			pageBreakBetweenTables: true,
			companyName: companyInfo.Name,
			companyAddress: $"{companyInfo.Address}, {companyInfo.PostalCode} {companyInfo.City}, {companyInfo.Country}",
			logoData: companyInfo.Logo);

			return pdfBytes;
		}
		public byte[] GetReportDiffFull_PDF(bool forAudit = false)
		{
			var roh = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceFull(this._data.LagerId ?? -1);
			var ef = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceFull(this._data.LagerId ?? -1, DateTime.Now.Year, "EF");
			var sections = new List<object>();
			//  - summary table
			sections.Add(
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffSummaryModel>
				{
					TotalLabel = "Gesamt",
					TotalPropertyName = "Diff",
					TableTitle = "Zusammenfassung",
					Items = new List<DiffSummaryModel>
					{
						new DiffSummaryModel(Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceSumFull(this._data.LagerId ?? -1)),
						new DiffSummaryModel(Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceSumFull(this._data.LagerId ?? -1, DateTime.Now.Year, "EF"))
					}
				});
			// -
			if(forAudit)
			{
				sections.Add(
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffAuditModel>
				{
					TableTitle = "Artikel ROH",
					Items = roh?.Select(x => new DiffAuditModel(x))
				});
				sections.Add(new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffAuditModel>
				{
					TableTitle = "Artikel FG",
					Items = ef?.Select(x => new DiffAuditModel(x))
				});
			}
			else
			{
				sections.Add(
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffModel>
				{
					TableTitle = "Artikel ROH",
					Items = roh?.Select(x => new DiffModel(x))
				});
				sections.Add(new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffModel>
				{
					TableTitle = "Artikel FG",
					Items = ef?.Select(x => new DiffModel(x))
				});
			}

			var companyInfo = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
			var pdfBytes = Infrastructure.Services.Reporting.IText.GenerateMultiTableWithTotalPdf(sections,
			documentTitle: "Report 8: Differenz Inventur",
			documentSubTitle: $"Standort: {((Enums.LagerEnum)(this._data.LagerId))} | Jahr: {DateTime.Now.Year}",
			documentDescription: $"Beschreibung: Der Bericht zeigt alle Inventurdifferenzen, berechnet als IST-Werte (aus dem Sanner-System) minus SOLL-Werte (aus EBAS).\r\nEr dient zur Identifizierung von Abweichungen zwischen den erfassten physischen Beständen und den im System hinterlegten Sollwerten, um mögliche Buchungs- oder Prozessfehler schnell zu erkennen und zu korrigieren.",
			pageBreakBetweenTables: false,
			companyName: companyInfo.Name,
			companyAddress: $"{companyInfo.Address}, {companyInfo.PostalCode} {companyInfo.City}, {companyInfo.Country}",
			logoData: companyInfo.Logo);

			return pdfBytes;
		}
		public byte[] GetReportDiffTopRoh_PDF()
		{
			var sections = new List<object>
			{
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffModel>
				{
					TableTitle = "Top 20 Bestand-DIFF. Positiv | Preis",
					Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceTopValue(this._data.LagerId ?? -1)?.Select(x=> new DiffModel(x))
				},
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffModel>
				{
					TableTitle = "Top 20 Bestand-DIFF. Negativ | Preis",
					Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceTopValue(this._data.LagerId ?? -1, negativeDifference: true)?.Select(x=> new DiffModel(x))
				},
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffQuantityModel>
				{
					TableTitle = "Top 20 Bestand-DIFF. Positiv | Menge",
					Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceTopQuantity(this._data.LagerId ?? -1)?.Select(x=> new DiffQuantityModel(x))
				},
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffQuantityModel>
				{
					TableTitle = "Top 20 Bestand-DIFF. Negativ | Menge",
					Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceTopQuantity(this._data.LagerId ?? -1, negativeDifference: true)?.Select(x=> new DiffQuantityModel(x))
				}
			};

			var companyInfo = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
			return Infrastructure.Services.Reporting.IText.GenerateMultiTablePdf(sections,
			documentTitle: "Report 9: TOP Differenz ROH",
			documentSubTitle: $"Standort: {((Enums.LagerEnum)(this._data.LagerId))} | Jahr: {DateTime.Now.Year}",
			documentDescription: $"Beschreibung: Der Bericht zeigt die Top 20 Abweichungen bei Rohmaterialen (ROH) – sowohl positive als auch negative Differenzen – in Menge und Wert (€).\r\nEr dient zur schnellen Erkennung der größten Bestandsabweichungen, um Über- oder Unterbestände zu identifizieren und die Ursachen für Schwankungen auf Mengen- und Werteebene zu analysieren.\r\n\r\nDer ROH-Preis wird nach folgender Priorität bestimmt: zuerst der niedrigste Preis aus aktuellen Bestellungen, anschließend – falls keine vorhanden sind – der Preis der ältesten Bestellung der letzten fünf Jahre. Wenn auch diese fehlt, wird der Standardpreis aus EBAS verwendet.",
			pageBreakBetweenTables: true,
			companyName: companyInfo.Name,
			companyAddress: $"{companyInfo.Address}, {companyInfo.PostalCode} {companyInfo.City}, {companyInfo.Country}",
			logoData: companyInfo.Logo);
		}
		public byte[] GetReportDiffTopFg_PDF()
		{
			var sections = new List<object>
			{
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffModel>
				{
					TableTitle = "Top 20 Bestand-DIFF. Positiv | Preis",
					Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceTopValue(this._data.LagerId ?? -1, DateTime.Now.Year, 20, "EF")?.Select(x=> new DiffModel(x))
				},
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffModel>
				{
					TableTitle = "Top 20 Bestand-DIFF. Negativ | Preis",
					Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceTopValue(this._data.LagerId ?? -1, DateTime.Now.Year, 20, "EF", negativeDifference: true)?.Select(x => new DiffModel(x))
				},
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffQuantityModel>
				{
					TableTitle = "Top 20 Bestand-DIFF. Positiv | Menge",
					Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceTopQuantity(this._data.LagerId ?? -1, DateTime.Now.Year, 20, "EF")?.Select(x=> new DiffQuantityModel(x))
				},
				new Infrastructure.Services.Reporting.IText.PdfTableSection<DiffQuantityModel>
				{
					TableTitle = "Top 20 Bestand-DIFF. Negativ | Menge",
					Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetDifferenceTopQuantity(this._data.LagerId ?? -1, DateTime.Now.Year, 20, "EF", negativeDifference: true)?.Select(x => new DiffQuantityModel(x))
				}
			};

			var companyInfo = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
			return Infrastructure.Services.Reporting.IText.GenerateMultiTablePdf(sections,
			documentTitle: "Report 10: TOP Differenz FG",
			documentSubTitle: $"Standort: {((Enums.LagerEnum)(this._data.LagerId))} | Jahr: {DateTime.Now.Year}",
			documentDescription: $"Beschreibung: Der Bericht zeigt die Top 20 Abweichungen bei Fertigwaren (FG) – sowohl positive als auch negative Differenzen – in Menge und Wert (€).\r\nEr dient zur schnellen Erkennung der größten Bestandsabweichungen, um Über- oder Unterbestände zu identifizieren und die Ursachen für Schwankungen auf Mengen- und Werteebene zu analysieren.",
			pageBreakBetweenTables: true,
			companyName: companyInfo.Name,
			companyAddress: $"{companyInfo.Address}, {companyInfo.PostalCode} {companyInfo.City}, {companyInfo.Country}",
			logoData: companyInfo.Logo);
		}
		public byte[] GetReportPrices_PDF(bool forAudit = false)
		{
			var sections = new List<object>();
			sections.Add(new Infrastructure.Services.Reporting.IText.PdfTableSection<ArticleModel>
			{
				TableTitle = "Artikelliste",
				Items = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetArticlePrices(LagerHelperHandler.GetWarehouseIds(this._data.LagerId ?? -1), LagerHelperHandler.GetProductionWarehouseIds(this._data.LagerId ?? -1))?.Select(x => new ArticleModel(x))
			});

			var companyInfo = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
			var pdfBytes = Infrastructure.Services.Reporting.IText.GenerateMultiTablePdf(sections,
			documentTitle: "",
			documentSubTitle: $"Standort: {((Enums.LagerEnum)(this._data.LagerId))} | Jahr: {DateTime.Now.Year}",
			documentDescription: $"Beschreibung: Der Bericht zeigt alle Artieklpreise.",
			pageBreakBetweenTables: false,
			companyName: companyInfo.Name,
			companyAddress: $"{companyInfo.Address}, {companyInfo.PostalCode} {companyInfo.City}, {companyInfo.Country}",
			logoData: companyInfo.Logo);

			return pdfBytes;
		}
		public class WipModel
		{
			[Display(Name = "FA")]
			public int FA { get; set; }
			[Display(Name = "Menge")]
			public int OpenQty { get; set; }
			public string Artikelnummer { get; set; }
			public string Bereich { get; set; }
			[Display(Name = "Fertigungsgrad")]
			public string Fertigungsgrad { get; set; }
			[Display(Name = "WIP (€)")]
			public string WIP { get; set; }
		}
		public class WipTotalModel
		{
			[PdfIgnore]
			public string ArtikelNr { get; set; }
			public int FA { get; set; }
			[PdfIgnore]
			public int Id { get; set; }
			[PdfIgnore]
			public int IdFa { get; set; }
			[Display(Name = "Jahr")]
			public int InventoryYear { get; set; }
			[PdfIgnore]
			public int LagerId { get; set; }
			[Display(Name = "FA-Datum")]
			public string Due { get; set; }
			[Display(Name = "Artikelnummer")]
			public string Item { get; set; }
			[Display(Name = "FA-Menge")]
			public int OpenQty { get; set; }
			[Display(Name = "WIP-Wert")]
			public string Total { get; set; }
			public WipTotalModel(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipTotalEntity entity)
			{
				if(entity is null)
				{
					return;
				}
				ArtikelNr = entity.ArtikelNr;
				Due = entity.Due.ToString("dd.MM.yyyy");
				FA = entity.FA;
				Id = entity.Id;
				IdFa = entity.IdFa;
				InventoryYear = entity.InventoryYear?.Year ?? 0;
				Item = entity.Item;
				LagerId = entity.LagerId;
				OpenQty = entity.OpenQty;
				Total = entity.Total.ToString("0.00");
			}
		}
		public class WipCustomerModel
		{
			[Display(Name = "Kunde")]
			public string Customer { get; set; }
			[Display(Name = "Jahr")]
			public int Year { get; set; }
			[Display(Name = "FA Anzahl")]
			public int FaCount { get; set; }
			[Display(Name = "WIP-Wert")]
			public string WipTotal { get; set; }
		}
		public class DiffModel
		{
			public string Artikelnummer { get; set; }
			[Display(Name = "Bestand Lager (SOLL)")]
			public string BestandL { get; set; }
			[Display(Name = "gescannte Menge (IST)")]
			public decimal Menge { get; set; }
			[PdfIgnore]
			public string Einkaufspreis { get; set; }
			[Display(Name = "Diff. (€)")]
			public string Diff { get; set; }
			[PdfIgnore]
			public int? Jahr { get; set; }

			public DiffModel(Infrastructure.Data.Entities.Joins.Logistics.InventoryDifferenceEntity entity)
			{
				if(entity is null)
				{
					return;
				}

				Artikelnummer = entity.Artikelnummer;
				BestandL = entity.BestandL.ToString("0.##");
				Einkaufspreis = entity.Einkaufspreis.ToString("0.00");
				Jahr = entity.Jahr;
				Menge = entity.Menge;
				Diff = $"{((entity.Menge - entity.BestandL) * entity.Einkaufspreis).ToString("0.00")}";
			}
		}
		public class DiffQuantityModel
		{
			public string Artikelnummer { get; set; }
			[Display(Name = "Bestand Lager (SOLL)")]
			public string BestandL { get; set; }
			[Display(Name = "gescannte Menge (IST)")]
			public decimal Menge { get; set; }
			[Display(Name = "Diff. (STK)")]
			public string Diff { get; set; }

			public DiffQuantityModel(Infrastructure.Data.Entities.Joins.Logistics.InventoryDifferenceEntity entity)
			{
				if(entity is null)
				{
					return;
				}

				Artikelnummer = entity.Artikelnummer;
				BestandL = entity.BestandL.ToString("0.##");
				Menge = entity.Menge;
				Diff = $"{((entity.Menge - entity.BestandL)).ToString("0.00")}";
			}
		}
		public class DiffAuditModel
		{
			public string Artikelnummer { get; set; }
			[PdfIgnore]
			[Display(Name = "Bestand Lager (SOLL)")]
			public string BestandL { get; set; }
			[Display(Name = "Inventurmenge")]
			public decimal Menge { get; set; }
			public string Einkaufspreis { get; set; }
			[PdfIgnore]
			[Display(Name = "Diff. (€)")]
			public string Diff { get; set; }
			[PdfIgnore]
			public int? Jahr { get; set; }

			public DiffAuditModel(Infrastructure.Data.Entities.Joins.Logistics.InventoryDifferenceEntity entity)
			{
				if(entity is null)
				{
					return;
				}

				Artikelnummer = entity.Artikelnummer;
				BestandL = entity.BestandL.ToString("0.##");
				Einkaufspreis = entity.Einkaufspreis.ToString("0.00");
				Jahr = entity.Jahr;
				Menge = entity.Menge;
				Diff = $"{((entity.Menge - entity.BestandL) * entity.Einkaufspreis).ToString("0.00")}";
			}
		}
		public class DiffSummaryModel
		{
			public string Type { get; set; }
			[Display(Name = "Lager (SOLL €)")]
			public string StockValue { get; set; }
			[Display(Name = "gescannte (IST €)")]
			public string ScannedValue { get; set; }
			[Display(Name = "Diff. (€)")]
			public string Diff { get; set; }

			public DiffSummaryModel(Infrastructure.Data.Entities.Joins.Logistics.InventoryDifferenceSumEntity entity)
			{
				if(entity is null)
				{
					return;
				}

				Type = entity.Warengruppe;
				ScannedValue = (entity.ScannedValue).ToString("0.##");
				StockValue = (entity.StockValue).ToString("0.##");
				Diff = $"{((entity.ScannedValue - entity.StockValue)).ToString("0.00")}";
			}
		}
		public class ArticleModel
		{
			public string Artikelnummer { get; set; }
			[Display(Name = "Preis (€)")]
			public string Price { get; set; }

			public ArticleModel(Infrastructure.Data.Entities.Joins.Logistics.RohArticlePricesEntity entity)
			{
				if(entity is null)
				{
					return;
				}

				Artikelnummer = entity.ArticleNumber;
				Price = entity.Price.ToString("0.####");
			}
		}
	}
}
