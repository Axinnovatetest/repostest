using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetDeliveryNotesCompilationHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<DeliveryNotesCompilationResponseModel>>
	{
		private DeliveryNotesCompilationRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetDeliveryNotesCompilationHandler(Identity.Models.UserModel user, DeliveryNotesCompilationRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<DeliveryNotesCompilationResponseModel>> HandleAsync()
		{
			var validationResponse = await this.ValidateAsync();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				return ResponseModel<DeliveryNotesCompilationResponseModel>.SuccessResponse(
					 new DeliveryNotesCompilationResponseModel(
						 Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetDeliveryNotesCompilation(this._data.CustomerNumber, this._data.DateFrom ?? DateTime.Today, this._data.DateTo ?? DateTime.Today)));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public async Task<ResponseModel<DeliveryNotesCompilationResponseModel>> ValidateAsync()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return await ResponseModel<DeliveryNotesCompilationResponseModel>.AccessDeniedResponseAsync();
			}

			return await ResponseModel<DeliveryNotesCompilationResponseModel>.SuccessResponseAsync();
		}
		public async Task<ResponseModel<byte[]>> GetPDF()
		{
			var response = await this.HandleAsync();
			if(!response.Success)
			{
				return null;
			}
			var logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
			var reportData = new Psz.Core.CustomerService.Reporting.Models.DeliveryNotesCompilationModel
			{
				Header = new Psz.Core.CustomerService.Reporting.Models.DeliveryNoteModel
				{
					Angebote_Unser_Zeichen = response.Body.Angebote_Unser_Zeichen,
					Anrede = response.Body.Anrede,
					Country = getCity(response.Body.Angebote_Unser_Zeichen, response.Body.Country),
					CustomerNumber = response.Body.CustomerNumber,
					DocumentTitle = response.Body.DocumentTitle,
					LAnrede = response.Body.LAnrede,
					LCountry = response.Body.LCountry,
					LName1 = response.Body.LName1,
					LName2 = response.Body.LName2,
					LStreet = response.Body.LStreet,
					MessageHeader = response.Body.MessageHeader,
					Name1 = response.Body.Name1,
					Name2 = getDepartment(response.Body.Angebote_Unser_Zeichen),
					PosText = response.Body.PosText,
					ShippingMethod = response.Body.ShippingMethod,
					Street = getStreet(response.Body.Angebote_Unser_Zeichen, response.Body.Street),
					VAT_ID = response.Body.VAT_ID,
					TopHeader = "PSZ electronic GmbH • Im Gstaudach 6 • 92648 Vohenstrauß",
				},
				Deliveries = response.Body.Items?.Select(x => new Psz.Core.CustomerService.Reporting.Models.DeliveryNoteModel.DeliveryNoteItemModel
				{
					Angebote_Angebot_Nr = x.Angebote_Angebot_Nr,
					Angebote_Bezug = x.Angebote_Bezug,
					ANgeboteArtikel_Anzahl = x.ANgeboteArtikel_Anzahl,
					ANgeboteArtikel_Bezeichnung1 = x.ANgeboteArtikel_Bezeichnung1,
					ANgeboteArtikel_Bezeichnung2 = x.ANgeboteArtikel_Bezeichnung2,
					ANgeboteArtikel_Bezeichnung2_Kunde = x.ANgeboteArtikel_Bezeichnung2_Kunde,
					ANgeboteArtikel_Bezeichnung3 = x.ANgeboteArtikel_Bezeichnung3,
					ANgeboteArtikel_Einheit = x.ANgeboteArtikel_Einheit,
					ANgeboteArtikel_EinzelCu_Gewicht = x.ANgeboteArtikel_EinzelCu_Gewicht,
					ANgeboteArtikel_GesamtCu_Gewicht = x.ANgeboteArtikel_GesamtCu_Gewicht,
					ANgeboteArtikel_Liefertermin = x.ANgeboteArtikel_Liefertermin,
					Artikelnummer = x.Artikelnummer,
					Ursprungsland = x.Ursprungsland,
					Zolltarif_nr = x.Zolltarif_nr,

				})?.ToList(),
				Logo = logo
			};
			var footerData = new Reporting.Models.DeliveryNotesCompilationFooterModel
			{
				FooterAddress1 = "Im Gstaudach 6",
				FooterAddress2 = "92648 Vohenstrauß",
				FooterAddress3 = "Tel.: +49 9651 924 117-0",

				FooterBankLabel = "Bankverbindung:",
				FooterBankValue1 = "Commerzbank AG Filiale Weiden",
				FooterBankValue2 = "Raiffeisenbank im Naabtal eG",
				FooterBankValue3 = "HypoVereinsbank Weiden",
				FooterBankValue4 = "",

				FooterLabelUst = "Ust.-Id-Nr.:",
				FooterValueUst = "DE 813 706 578",
				FooterLabelSite = "Sitz:",
				FooterValueSite = "Vohenstrauß",
				FooterLabelFax = "Fax:",
				FooterValueFax = "+49 9651 924 117-212",

				FooterLabelManager = "Geschäftsführer:",
				FooterValueManager = "Werner Steinbacher",
				FooterLabelManager2 = "",
				FooterValueManager2 = "",
				FooterLabelTaxId = "Steuernummer:",
				FooterValueTaxId = "255/135/40526",

				FooterLabelHRB = "HRB:",
				FooterValueHRB = "2907 AG Weiden",
				FooterLabelEmail = "E-mail:",
				FooterValueEmail = "info@psz-electronic.com",
				FooterLabelCustomsId = "Zollnummer:",
				FooterValueCustomsId = "488 26 28",

				FooterAccountLabel = "Konto:",
				FooterAccountValue1 = "775 321 300",
				FooterAccountValue2 = "3 22 66 03",
				FooterAccountValue3 = "234 354 89",
				FooterAccountValue4 = "",

				FooterBLZLabel = "BLZ:",
				FooterBLZValue1 = "753 400 90",
				FooterBLZValue2 = "750691 71",
				FooterBLZValue3 = "753 200 75",
				FooterBLZValue4 = "",

				FooterIBANLabel = "IBAN:",
				FooterIBANValue1 = "DE41 7534 0090 0775 3213 00",
				FooterIBANValue2 = "DE04 7506 9171 0003 2266 03",
				FooterIBANValue3 = "DE56 7532 0075 0023 4354 89",
				FooterIBANValue4 = "",

				FooterSWIFTLabel = "SWIFT-BIC:",
				FooterSWIFTValue1 = "COBADEFF753",
				FooterSWIFTValue2 = "GENODEF1SWD",
				FooterSWIFTValue3 = "HYVEDEMM454",
				FooterSWIFTValue4 = ""
			};
			var byteResponse = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
			{
				BodyData = reportData,
				BodyTemplate = "CTS_STK_DNzuSTLG_Body",
				DocumentTitle = "",
				FooterCenterText = "",
				FooterData = footerData,
				FooterLeftText = "",
				FooterTemplate = "CTS_STK_DNzuSTLG_Footer",
				FooterWithCounter = false,
				HasFooter = true,
				HasHeader = true,
				HeaderFirstPageOnly = false,
				HeaderLogoWithCounter = true,
				HeaderLogoWithText = false,
				HeaderText = "",
				Logo = logo,
				Rotate = false

			});
			return ResponseModel<byte[]>.SuccessResponse(byteResponse);
			//return ResponseModel<byte[]>.SuccessResponse(Psz.Core.CustomerService.Reporting.IText.GetPDF(new Psz.Core.CustomerService.Reporting.Models.DeliveryNoteModel
			//{
			//	Angebote_Unser_Zeichen = response.Body.Angebote_Unser_Zeichen,
			//	Anrede = response.Body.Anrede,
			//	Country = getCity(response.Body.Angebote_Unser_Zeichen, response.Body.Country), // response.Body.Country,
			//	CustomerNumber = response.Body.CustomerNumber,
			//	DocumentTitle = response.Body.DocumentTitle,
			//	LAnrede = response.Body.LAnrede,
			//	LCountry = response.Body.LCountry,
			//	LName1 = response.Body.LName1,
			//	LName2 = response.Body.LName2,
			//	LStreet = response.Body.LStreet,
			//	MessageHeader = response.Body.MessageHeader,
			//	Name1 = response.Body.Name1,
			//	Name2 = getDepartment(response.Body.Angebote_Unser_Zeichen), // response.Body.Name2,
			//	PosText = response.Body.PosText,
			//	ShippingMethod = response.Body.ShippingMethod,
			//	Street = getStreet(response.Body.Angebote_Unser_Zeichen, response.Body.Street), // response.Body.Street,
			//	VAT_ID = response.Body.VAT_ID
			//},
			//response.Body.Items?.Select(x => new Psz.Core.CustomerService.Reporting.Models.DeliveryNoteModel.DeliveryNoteItemModel
			//{
			//	Angebote_Angebot_Nr = x.Angebote_Angebot_Nr,
			//	Angebote_Bezug = x.Angebote_Bezug,
			//	ANgeboteArtikel_Anzahl = x.ANgeboteArtikel_Anzahl,
			//	ANgeboteArtikel_Bezeichnung1 = x.ANgeboteArtikel_Bezeichnung1,
			//	ANgeboteArtikel_Bezeichnung2 = x.ANgeboteArtikel_Bezeichnung2,
			//	ANgeboteArtikel_Bezeichnung2_Kunde = x.ANgeboteArtikel_Bezeichnung2_Kunde,
			//	ANgeboteArtikel_Bezeichnung3 = x.ANgeboteArtikel_Bezeichnung3,
			//	ANgeboteArtikel_Einheit = x.ANgeboteArtikel_Einheit,
			//	ANgeboteArtikel_EinzelCu_Gewicht = x.ANgeboteArtikel_EinzelCu_Gewicht,
			//	ANgeboteArtikel_GesamtCu_Gewicht = x.ANgeboteArtikel_GesamtCu_Gewicht,
			//	ANgeboteArtikel_Liefertermin = x.ANgeboteArtikel_Liefertermin,
			//	Artikelnummer = x.Artikelnummer,
			//	Ursprungsland = x.Ursprungsland,
			//	Zolltarif_nr = x.Zolltarif_nr,

			//})?.ToList())
			//);
		}
		public async Task<byte[]> GetXLS()
		{
			var response = await this.HandleAsync();
			if(!response.Success)
			{
				return null;
			}

			// - 
			try
			{
				var data = response.Body.Items;
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"data-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"LS zus STG");

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
					worksheet.Cells[1, 1].Value = $"LS - {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// - Customer address
					worksheet.Cells[headerRowNumber + 1, startColumnNumber].Value = response.Body.LAnrede;
					worksheet.Cells[headerRowNumber + 2, startColumnNumber].Value = response.Body.LName1;
					worksheet.Cells[headerRowNumber + 3, startColumnNumber].Value = response.Body.LName2;
					worksheet.Cells[headerRowNumber + 4, startColumnNumber].Value = response.Body.LStreet;
					worksheet.Cells[headerRowNumber + 5, startColumnNumber].Value = response.Body.LCountry;
					headerRowNumber += 5;

					// - title
					worksheet.Cells[headerRowNumber + 2, startColumnNumber].Value = response.Body.DocumentTitle;
					worksheet.Cells[headerRowNumber + 2, startColumnNumber].Style.Font.Size = 16;
					worksheet.Cells[headerRowNumber + 2, startColumnNumber].Style.Font.Bold = true;
					worksheet.Cells[headerRowNumber + 2, startColumnNumber].Style.Font.Italic = true;
					worksheet.Cells[headerRowNumber + 2, startColumnNumber].Style.Font.UnderLine = true;
					headerRowNumber += 2;

					// - customer data
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 0].Value = "Ihr Zeichen:";
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 1].Value = response.Body.CustomerNumber;
					worksheet.Cells[headerRowNumber + 2, startColumnNumber + 0].Value = "Versandart:";
					worksheet.Cells[headerRowNumber + 2, startColumnNumber + 1].Value = response.Body.ShippingMethod;
					worksheet.Cells[headerRowNumber + 3, startColumnNumber + 0].Value = response.Body.VAT_ID;
					worksheet.Cells[headerRowNumber + 4, startColumnNumber + 0].Value = response.Body.PosText;
					headerRowNumber += 4;

					var dataLS = data.Select(x => x.Angebote_Angebot_Nr ?? 0).Distinct().OrderBy(x => x);
					headerRowNumber += 2;
					foreach(var LSItem in dataLS)
					{
						// -
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = $"LS-Nummer: {LSItem}";
						headerRowNumber++;

						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 2";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Einheit";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Anzahl";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Stk Gew.";


						var rowNumber = headerRowNumber + 1;
						var firstRowNumber = rowNumber;
						// Loop through 
						foreach(var w in response.Body?.Items.OrderBy(x => x.ANgeboteArtikel_Liefertermin)?.Where(x => x.Angebote_Angebot_Nr == LSItem)?.OrderBy(x => x.Artikelnummer))
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.ANgeboteArtikel_Bezeichnung1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.ANgeboteArtikel_Bezeichnung2;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.ANgeboteArtikel_Einheit;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.ANgeboteArtikel_Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = $"{w?.ANgeboteArtikel_EinzelCu_Gewicht}kg";
							//worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							// - second row - w/o headers
							rowNumber += 1;
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = $"Ihre Bestellung/PO#: {w?.Angebote_Bezug}";
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = $"";
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = $"Lieferdatum: {w?.ANgeboteArtikel_Liefertermin?.ToString("dd/MM/yyyy")}";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = $"Ursprungsland: {w?.Ursprungsland}";
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = $"Zolltarif-Nr.: {w?.Zolltarif_nr}";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = $"{w?.ANgeboteArtikel_GesamtCu_Gewicht}kg";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;


							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}

						// - borders
						using(var range = worksheet.Cells[firstRowNumber - 1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
						// - header color
						using(var range = worksheet.Cells[firstRowNumber - 1, 1, firstRowNumber - 1, numberOfColumns])
						{
							range.Style.Font.Bold = true;
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
							range.Style.Font.Color.SetColor(Color.Black);
							range.Style.ShrinkToFit = false;
						}
						// - 
						headerRowNumber = rowNumber + 0;
					}

					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Gew.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = $"{data.Select(x => x.ANgeboteArtikel_GesamtCu_Gewicht ?? 0).Sum()}kg";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Style.Font.Bold = true;
					// - borders
					using(var range = worksheet.Cells[headerRowNumber, 5, headerRowNumber, 6])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					// - header color
					using(var range = worksheet.Cells[headerRowNumber, 5, headerRowNumber, 5])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}

					// - RG address
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Rechnungsadresse";
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Style.Font.Bold = true;
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Style.Font.UnderLine = true;
					worksheet.Cells[headerRowNumber + 1, startColumnNumber + 0].Value = response.Body.Anrede;
					worksheet.Cells[headerRowNumber + 2, startColumnNumber + 0].Value = response.Body.Name1;
					worksheet.Cells[headerRowNumber + 3, startColumnNumber + 0].Value = getDepartment(response.Body.Angebote_Unser_Zeichen);
					worksheet.Cells[headerRowNumber + 4, startColumnNumber + 0].Value = getStreet(response.Body.Angebote_Unser_Zeichen, response.Body.Street);
					worksheet.Cells[headerRowNumber + 5, startColumnNumber + 0].Value = getCity(response.Body.Angebote_Unser_Zeichen, response.Body.Country);

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"LS zus STLG";
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
		internal string getDepartment(int unserZeichen)
		{
			switch(unserZeichen)
			{
				case 10014:
					return "Abteilung FR-F3 / BK 301";
				case 10096:
					return "Rechnungsprüfung";
				case 10064:
					return "Abteilung FR-F3 / BK 22";
				case 10367:
					return "Abteilung FR-F3 / BK316";
				case 10358:
					return "Abteilung FR-F3 / BK 22";
				case 10150:
					return "Abteilung FR-F3 / BK 84";
				default:
					return "Buchhaltung";
			}
		}
		internal string getStreet(int unserZeichen, string strasse)
		{
			switch(unserZeichen)
			{
				case 10014:
					return "Postfach 700941";
				case 10064:
					return "Postfach 700941";
				case 10367:
					return "Postfach 700941";
				case 10358:
					return "Postfach 700941";
				case 10150:
					return "Postfach 700941";
				default:
					return strasse;
			}
		}
		internal string getCity(int unserZeichen, string ort)
		{
			switch(unserZeichen)
			{
				case 10014:
				case 10064:
				case 10367:
				case 10358:
				case 10150:
					return "D-22009 Hamburg";
				default:
					return ort;
			}
		}
	}
}
