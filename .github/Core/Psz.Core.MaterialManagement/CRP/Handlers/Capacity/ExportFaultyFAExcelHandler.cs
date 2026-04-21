using Infrastructure.Data.Access.Tables.WPL;
using OfficeOpenXml;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.IO;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class ExportFaultyFAExcelHandler: IHandle<GetAnalyseReportRequestModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel user { get; set; }
		public int year { get; set; }
		public int weekFrom { get; set; }
		public int? weekUntil { get; set; }
		public int countryId { get; set; }
		public int? hallId { get; set; }

		public ExportFaultyFAExcelHandler(int year, int weekFrom, int? weekUntil, int countryId, int? hallId, Identity.Models.UserModel user)
		{
			this.year = year;
			this.weekFrom = weekFrom;
			this.weekUntil = weekUntil;
			this.countryId = countryId;
			this.hallId = hallId;
			this.user = user;
		}

		public ResponseModel<byte[]> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new SharedKernel.Exceptions.UnauthorizedException();
					}

					return Perform(this.user, this.year, this.weekFrom, this.weekUntil, this.countryId, this.hallId);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<byte[]> Perform(Identity.Models.UserModel user, int year, int weekFrom, int? weekUntil, int countryId, int? hallId)
		{
			#region > Validation
			var countryEntity = CountryAccess.Get(countryId);
			if(countryEntity == null || countryEntity.IsArchived)
			{
				return ResponseModel<byte[]>.FailureResponse("Country is not found");
			}
			#endregion

			var _weekUntil = weekUntil ?? (weekFrom + (6 * 4));
			if(_weekUntil > 53)
			{
				_weekUntil = 53;
			}

			var countryEntities = CountryAccess.Get();
			var hallEntities = HallAccess.Get();
			var countryName = countryEntities?.Find(x => x.Id == countryId)?.Name;
			var lagerortId = GetAnalyseReportHandler.getLagerortId(countryName, hallEntities?.Find(x => x.Id == hallId)?.Name);

			var faultyFertigungEntities =
					Infrastructure.Data.Access.Joins.MTM.CRP.FertigungFaultyTimeAccess.Get(
					lagerortId,
					Helpers.DateTimeHelper.FirstDateOfWeekISO8601(year, weekFrom),
					Helpers.DateTimeHelper.FirstDateOfWeekISO8601(year, (int)_weekUntil))
				?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.FertigungFaultyTimeEntity>();


			return ResponseModel<byte[]>.SuccessResponse(getExcel(user.SelectedLanguage, year, countryName, faultyFertigungEntities));
		}
		public ResponseModel<byte[]> Validate()
		{
			throw new NotImplementedException();
		}
		static byte[] getExcel(string selectedLanguage, int year, string countryName, List<Infrastructure.Data.Entities.Joins.MTM.CRP.FertigungFaultyTimeEntity> fertigungFaultyTimeEntities)
		{
			selectedLanguage = selectedLanguage ?? "DE";
			string filePath = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Faulty FA - {DateTime.Now.ToString("yyyy/MM/dd")}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 4;
					var startColumnNumber = 1;
					var numberOfColumns = Enum.GetNames(typeof(ExcelColumnNumber)).Length;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Blue;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(3).Height = 20;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[2, 2, 2, numberOfColumns + 1].Merge = true;
					worksheet.Cells[2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

					worksheet.Cells[2, 2].Value = $"Faulty FA list [{DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")}]";
					worksheet.Cells[3, 2].Value = $"Country: {countryName} - Year:{year}";
					worksheet.Cells[3, 2, 3, numberOfColumns + 1].Merge = true;
					worksheet.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.WeekNumber].Value = GetHeaderName(ExcelColumnNumber.WeekNumber, selectedLanguage); // "Operation Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaNumber].Value = GetHeaderName(ExcelColumnNumber.FaNumber, selectedLanguage); // "Sub Operation Number"; 
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaDate].Value = GetHeaderName(ExcelColumnNumber.FaDate, selectedLanguage); // "Predecessor Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaArticle].Value = GetHeaderName(ExcelColumnNumber.FaArticle, selectedLanguage); // "Predecessor Sub Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaQuantity].Value = GetHeaderName(ExcelColumnNumber.FaQuantity, selectedLanguage); // "Plant";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaUnitTime].Value = GetHeaderName(ExcelColumnNumber.FaUnitTime, selectedLanguage); // "Hall";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaTotalTime].Value = GetHeaderName(ExcelColumnNumber.FaTotalTime, selectedLanguage); // "Department";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var fa in fertigungFaultyTimeEntities)
					{

						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.WeekNumber].Value = fa.FaDate.HasValue ? Helpers.DateTimeHelper.GetIso8601WeekOfYear(fa.FaDate.Value) : 0;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaNumber].Value = fa.FaNumber;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaDate].Value = fa.FaDate.HasValue ? fa.FaDate.Value.ToString("dd.MM.yyyy") : "";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaArticle].Value = fa.FaArticle;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaQuantity].Value = fa.FaQuantity;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaUnitTime].Value = fa.FaUnitTime;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaTotalTime].Value = fa.FaTotalTime;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					// Doc content
					if(fertigungFaultyTimeEntities != null && fertigungFaultyTimeEntities.Count > 0)
					{
						using(var range = worksheet.Cells[2, 2, rowNumber - 1, numberOfColumns + 1])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}

					// Pre + Header
					using(var range = worksheet.Cells[2, 2, headerRowNumber, numberOfColumns + 1])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}

					//// Totals
					worksheet.Cells[rowNumber + 1, startColumnNumber + (int)ExcelColumnNumber.FaUnitTime].Style.Font.Bold = true;
					worksheet.Cells[rowNumber + 1, startColumnNumber + (int)ExcelColumnNumber.FaTotalTime].Style.Font.Bold = true;


					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Faulty FAs";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();
				}
				// -
				return File.ReadAllBytes(filePath);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}

		public enum ExcelColumnNumber
		{
			WeekNumber = 1,
			FaNumber = 2,
			FaDate = 3,
			FaArticle = 4,
			FaQuantity = 5,
			FaUnitTime = 6,
			FaTotalTime = 7
		}
		public static string GetHeaderName(ExcelColumnNumber excelColumnNumber, string language)
		{
			language = language?.ToUpper();
			switch(excelColumnNumber)
			{
				case ExcelColumnNumber.WeekNumber:
					if(language == "DE")
						return "KW";
					if(language == "TN")
						return "KW";
					if(language == "CZ")
						return "KW";
					if(language == "AL")
						return "KW";
					// English
					return "KW";
				case ExcelColumnNumber.FaNumber:
					if(language == "DE")
						return "FA-Nummer";
					if(language == "TN")
						return "Numéro FA";
					if(language == "CZ")
						return "FA-Number";
					if(language == "AL")
						return "FA-Number";
					// English
					return "FA-Number";
				case ExcelColumnNumber.FaDate:
					if(language == "DE")
						return "Termin";
					if(language == "TN")
						return "Date";
					if(language == "CZ")
						return "Date";
					if(language == "AL")
						return "Date";
					// English
					return "Date";
				case ExcelColumnNumber.FaArticle:
					if(language == "DE")
						return "Artikelnummer";
					if(language == "TN")
						return "Numéro article";
					if(language == "CZ")
						return "Article number";
					if(language == "AL")
						return "Article number";
					// English
					return "Article number";
				case ExcelColumnNumber.FaQuantity:
					if(language == "DE")
						return "Menge";
					if(language == "TN")
						return "Quantité";
					if(language == "CZ")
						return "Quantity";
					if(language == "AL")
						return "Quantity";
					// English
					return "Quantity";
				case ExcelColumnNumber.FaUnitTime:
					if(language == "DE")
						return "Vorgabezeit (min)";
					if(language == "TN")
						return "Temps unité (min)";
					if(language == "CZ")
						return "Default time (min)";
					if(language == "AL")
						return "Default time (min)";
					// English
					return "Default time (min)";
				case ExcelColumnNumber.FaTotalTime:
					if(language == "DE")
						return "Gesamtzeit (Std)";
					if(language == "TN")
						return "Temps total (Hr)";
					if(language == "CZ")
						return "Total Time (Hrs)";
					if(language == "AL")
						return "Total Time (Hrs)";
					// English
					return "Total Time (Hrs)";
				default:
					if(language == "DE")
						return "";
					if(language == "TN")
						return "";
					if(language == "CZ")
						return "";
					if(language == "AL")
						return "";
					// English
					return "";
			}
		}
	}
}
