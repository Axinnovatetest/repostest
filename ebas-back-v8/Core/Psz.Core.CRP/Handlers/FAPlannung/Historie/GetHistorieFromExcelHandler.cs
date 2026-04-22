using Infrastructure.Data.Access.Tables.PRS;
using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using PdfSharp.Quality;
using Psz.Core.BaseData;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Diagnostics;
using System.Globalization;
using static Infrastructure.Data.Access.Joins.ArticleStatisticsAccess;
using static Psz.Core.BaseData.Enums.BomChangeEnums;
using static Psz.Core.Common.Helpers.Formatters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<List<Models.FAPlanning.Historie.HistorieFaPlannungDetailsModel>> GetHistorieFromExcel(UserModel user, Core.Common.Models.ImportFileModel data)
		{
			if(user == null)
				return ResponseModel<List<Models.FAPlanning.Historie.HistorieFaPlannungDetailsModel>>.AccessDeniedResponse();

			try
			{
				var errors = new List<string>();
				var response = ReadFromExcel(data.FilePath, out errors);
				if(errors != null && errors.Count > 0)
					return ResponseModel<List<Models.FAPlanning.Historie.HistorieFaPlannungDetailsModel>>.FailureResponse(errors);
				return ResponseModel<List<Models.FAPlanning.Historie.HistorieFaPlannungDetailsModel>>.SuccessResponse(
					response?.Select(r => new Models.FAPlanning.Historie.HistorieFaPlannungDetailsModel(r)).ToList()
					);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity> ReadFromExcel(string filePath, out List<string> errors)
		{
			errors = new List<string> { };
			try
			{
				var fileInfo = new System.IO.FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				var package = new ExcelPackage(fileInfo);
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
				var _list = new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity>();
				var supposedHeaders = new List<string>();
				var headers = new List<string>
				{
					"Werk",
					"Planungsstatus",
					"Customer",
					"CS Kontakt",
					"PB",
					"Atribut",
					"Short",
					"FA Number",
					"Comment 1",
					"Comment 2",
					"FA Qty",
					"Shipped Qty",
					"Open Qty",
					"PN PSZ",
					"Status TN",
					"Order Time",
					"Costs",
					"Shipped Qty Man",
					"Kommisioniert_teilweise",
					"Kommisioniert_komplett",
					"Kabel_geschnitten",
					"Kabel_geschnitten_Datum",
					"Termin Werk",
					"Ack Date",
					"KW",
					"FA_Druckdatum",
					"Freigabestatus",
					"Wish Date",
					"Bemerkung",
					"Gewerk_Teilweise_Bemerkung",
					"Verpackungsart",
					"Verpackungsmenge",
					"Losgroesse",
					"Techniker",
					"Kontakt",
					"Technik Kontakt TN",
					"Status Intern",
					"erstelldatum",
					"Bemerkung_Kommissionierung_AL",

				};
				for(int i = 1; i <= 39; i++)
				{
					supposedHeaders.Add(Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[1, i]));
				}
				if(!headers.SequenceEqual(supposedHeaders))
				{
					errors.Add($"Invalid file format: Incompatible Headers");
					return null;
				}

				// - 2025-07-17 - misc optimization
				//var faNumbers = new HashSet<int>();

				//for(int i = startRowNumber; i <= rowEnd; i++)
				//{
				//	var fanRaw = XLS.GetCellValue(worksheet.Cells[i, startColNumber + 7]);
				//	if(int.TryParse(fanRaw, out int faNum))
				//		faNumbers.Add(faNum);
				//}
				//var allFertigungen = FertigungAccess.GetByFertigungsnummerBatch(faNumbers);
				//var artikelIds = allFertigungen.Select(f => f.Artikel_Nr).Distinct();
				//var allArtikel = ArtikelAccess.GetByIds(artikelIds);

				if(rows > 1 && columns > 1)
				{
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						var werk = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber]);
						var Planungsstatus = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 1]);
						var Customer = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 2]);
						var CSKontakt = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 3]);
						var PB = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 4]);
						var Atribut = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 5]);
						var Short = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 6]);
						var FANumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 7]);
						var Comment1 = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 8]);
						var Comment2 = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 9]);
						var FAQty = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 10]);
						var ShippedQty = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 11]);
						var OpenQty = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 12]);
						var PNPSZ = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 13]);
						var StatusTN = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 14]);
						var OrderTime = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 15]);
						var Costs = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 16]);
						var ShippedQtyMan = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 17]);
						var Kommisioniert_teilweise = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 18]);
						var Kommisioniert_komplett = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 19]);
						var Kabel_geschnitten = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 20]);
						var Kabel_geschnitten_Datum = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 21]);
						var TerminWerk = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 22]);
						var AckDate = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 23]);
						var KW = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 24]);
						var FA_Druckdatum = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 25]);
						var Freigabestatus = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 26]);
						var WishDate = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 27]);
						var Bemerkung = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 28]);
						var Gewerk_Teilweise_Bemerkung = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 29]);
						var Verpackungsart = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 30]);
						var Verpackungsmenge = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 31]);
						var Losgroesse = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 32]);
						var Techniker = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 33]);
						var Kontakt = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 34]);
						var TechnikKontaktTN = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 35]);
						var StatusIntern = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 36]);
						var erstelldatum = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 37]);
						var Bemerkung_Kommissionierung_AL = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 38]);

						//vales check
						var FANumberCheck = int.TryParse(werk, out int n4);
						if(!FANumberCheck)
							errors.Add($"Row [{i}] Coloumn [{startColNumber + 7}] wrong Shipped FA Number Qty format [{FANumber}].");
						var fa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(Convert.ToInt32(FANumber));
						var Artikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(fa?.Artikel_Nr ?? -1);
						if(fa == null)
							errors.Add($"Row [{i}] Coloumn [{startColNumber + 7}] FA Number [{FANumber}] not found.");
						if(Artikel == null)
							errors.Add($"Row [{i}] Coloumn [{startColNumber + 7}] FA Number [{FANumber}], article not found.");
						if(Customer.IsNullOrEmptyOrWitheSpaces())
							errors.Add($"Row [{i}] Coloumn [{startColNumber + 2}] Missing Customer name");
						else
						{
							var kries = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByCustomerNumberUnique(Artikel?.CustomerNumber ?? -1);
							//if(kries != null && kries.Kunde.Trim().ToLower() != Customer.Trim().ToLower())
							//	errors.Add($"Row [{i}] Customer [{Customer}] does not match fa [{FANumber}]");

							var usCulture = "de-DE";
							_list.Add(new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_detailsEntity
							{
								Ack_Date = DateTime.TryParse(AckDate, new CultureInfo(usCulture, false), out DateTime d) ? d : null,
								Atribut = Atribut,
								Bemerkung = Bemerkung,
								Bemerkung_Kommissionierung_AL = Bemerkung_Kommissionierung_AL,
								Comment_1 = Comment1,
								Comment_2 = Comment2,
								Costs = Costs,
								CS_Kontakt = CSKontakt,
								Customer = Customer,
								CustomerNumber = kries?.Kundennummer,
								erstelldatum = DateTime.TryParse(erstelldatum, new CultureInfo(usCulture, false), out DateTime d0) ? d0 : null,
								FA_Druckdatum = DateTime.TryParse(FA_Druckdatum, new CultureInfo(usCulture, false), out DateTime d1) ? d1 : null,
								FA_Number = Convert.ToInt32(FANumber),
								FA_Qty = Convert.ToInt32(FAQty),
								Freigabestatus = Freigabestatus,
								Gewerk_Teilweise_Bemerkung = Gewerk_Teilweise_Bemerkung,
								Kabel_geschnitten = Kabel_geschnitten == "VRAI" ? true : false,
								Kabel_geschnitten_Datum = DateTime.TryParse(Kabel_geschnitten_Datum, new CultureInfo(usCulture, false), out DateTime d2) ? d2 : null,
								Kommisioniert_komplett = Kommisioniert_komplett == "VRAI" ? true : false,
								Kommisioniert_teilweise = Kommisioniert_teilweise == "VRAI" ? true : false,
								Kontakt = Kontakt,
								KW = Convert.ToInt32(KW),
								Losgroesse = Convert.ToDecimal(Losgroesse),
								Open_Qty = Convert.ToInt32(OpenQty),
								Order_Time = Convert.ToDecimal(OrderTime),
								PB = PB,
								Planungsstatus = Planungsstatus,
								PN_PSZ = PNPSZ,
								Shipped_Qty = Convert.ToInt32(ShippedQty),
								Shipped_Qty_Man = Convert.ToInt32(ShippedQtyMan),
								Short = Short,
								Status_Intern = StatusIntern,
								Status_TN = StatusTN,
								Techniker = Techniker,
								Technik_Kontakt_TN = TechnikKontaktTN,
								Termin_Werk = DateTime.TryParse(TerminWerk, new CultureInfo(usCulture, false), out DateTime d3) ? d3 : null,
								Verpackungsart = Verpackungsart,
								Verpackungsmenge = Convert.ToInt32(Verpackungsmenge),
								Werk = Convert.ToInt32(werk),
								Wish_Date = DateTime.TryParse(WishDate, new CultureInfo(usCulture, false), out DateTime d4) ? d4 : null,
							});
						}
					}
					return _list;
				}
				else
				{
					errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
					return null;
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}