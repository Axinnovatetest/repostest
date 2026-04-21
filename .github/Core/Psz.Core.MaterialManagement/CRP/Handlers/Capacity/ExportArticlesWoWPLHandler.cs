using OfficeOpenXml;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class ExportArticlesWoWPLHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel user { get; set; }
		private ArticlesWoWPLRequestModel _data { get; set; }

		public ExportArticlesWoWPLHandler(Identity.Models.UserModel user, ArticlesWoWPLRequestModel data)
		{
			this.user = user;
			this._data = data;
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

					return Perform(this.user, this._data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;// e;
				}
			}
		}

		public static ResponseModel<byte[]> Perform(Identity.Models.UserModel user, ArticlesWoWPLRequestModel _data)
		{
			var articleEntities = (Infrastructure.Data.Access.Joins.MTM.CRP.ArticleAccess.GetArticlesWoWPL(_data.warengruppeEF, _data.wStuckliste, _data.wFa, _data.wOpenFa, _data.lager, _data.faDateFrom, _data.faDateTill)
				   ?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleWpl>())
				   .Select(x => new ArticlesWoWPLResponseModel(x))?.ToList();

			return ResponseModel<byte[]>.SuccessResponse(getExcel(user.SelectedLanguage, articleEntities));
		}
		public ResponseModel<byte[]> Validate()
		{
			throw new NotImplementedException();
		}
		static byte[] getExcel(string selectedLanguage, List<ArticlesWoWPLResponseModel> fertigungFaultyTimeEntities)
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
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Articles without active Work Plan - {DateTime.Now.ToString("yyyy/MM/dd")}");

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

					worksheet.Cells[2, 2].Value = $"Articles without active Work Plan list [{DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")}]";
					worksheet.Cells[3, 2].Value = $"";
					worksheet.Cells[3, 2, 3, numberOfColumns + 1].Merge = true;
					worksheet.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ArticleNumber].Value = GetHeaderName(ExcelColumnNumber.ArticleNumber, selectedLanguage); // "Operation Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Designation1].Value = GetHeaderName(ExcelColumnNumber.Designation1, selectedLanguage); // "Sub Operation Number"; 
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ProductGroup].Value = GetHeaderName(ExcelColumnNumber.ProductGroup, selectedLanguage); // "Predecessor Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ProductionTime].Value = GetHeaderName(ExcelColumnNumber.ProductionTime, selectedLanguage); // "Predecessor Sub Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.StatusRelease].Value = GetHeaderName(ExcelColumnNumber.StatusRelease, selectedLanguage); // "Plant";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.StatusInternalTN].Value = GetHeaderName(ExcelColumnNumber.StatusInternalTN, selectedLanguage); // "Hall";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.StatusCheckTN].Value = GetHeaderName(ExcelColumnNumber.StatusCheckTN, selectedLanguage); // "Department";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.HourlyRate].Value = GetHeaderName(ExcelColumnNumber.HourlyRate, selectedLanguage); // "Department";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.LotSize].Value = GetHeaderName(ExcelColumnNumber.LotSize, selectedLanguage); // "Department";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var fa in fertigungFaultyTimeEntities)
					{
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ArticleNumber].Value = fa.ArticleNumber;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Designation1].Value = fa.Designation1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ProductGroup].Value = fa.ProductGroup;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ProductionTime].Value = fa.ProductionTime;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.StatusRelease].Value = fa.StatusRelease;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.StatusInternalTN].Value = fa.StatusInternalTN;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.StatusCheckTN].Value = fa.StatusCheckTN;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.HourlyRate].Value = fa.HourlyRate;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.LotSize].Value = fa.LotSize;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					// Doc content
					if(fertigungFaultyTimeEntities != null && fertigungFaultyTimeEntities.Count > 0)
					{
						using(var range = worksheet.Cells[2, 2, rowNumber - 1, numberOfColumns + 1])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
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
					worksheet.Cells[rowNumber + 1, startColumnNumber + (int)ExcelColumnNumber.StatusInternalTN].Style.Font.Bold = true;
					worksheet.Cells[rowNumber + 1, startColumnNumber + (int)ExcelColumnNumber.StatusCheckTN].Style.Font.Bold = true;


					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Articles without active Work Plan";
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
			ArticleNumber = 1,
			Designation1 = 2,
			ProductGroup = 3,
			ProductionTime = 4,
			HourlyRate = 5,
			LotSize = 6,
			StatusRelease = 7,
			StatusInternalTN = 8,
			StatusCheckTN = 9
		}
		public static string GetHeaderName(ExcelColumnNumber excelColumnNumber, string language)
		{
			language = language?.ToUpper();
			switch(excelColumnNumber)
			{
				case ExcelColumnNumber.ArticleNumber:
					if(language == "DE")
						return "Artikelnummer";
					if(language == "TN")
						return "Nr";
					if(language == "CZ")
						return "Nr";
					if(language == "AL")
						return "Nr";
					// English
					return "Number";
				case ExcelColumnNumber.Designation1:
					if(language == "DE")
						return "Bezeichnung 1";
					if(language == "TN")
						return "Designation 1";
					if(language == "CZ")
						return "Designation 1";
					if(language == "AL")
						return "Designation 1";
					// English
					return "Designation 1";
				case ExcelColumnNumber.ProductGroup:
					if(language == "DE")
						return "Warengruppe";
					if(language == "TN")
						return "Product Group";
					if(language == "CZ")
						return "Product Group";
					if(language == "AL")
						return "Product Group";
					// English
					return "Product Group";
				case ExcelColumnNumber.ProductionTime:
					if(language == "DE")
						return "Produktionzeit";
					if(language == "TN")
						return "Production Time";
					if(language == "CZ")
						return "Production Time";
					if(language == "AL")
						return "Production Time";
					// English
					return "Production Time";
				case ExcelColumnNumber.StatusRelease:
					if(language == "DE")
						return "Freigabestatus";
					if(language == "TN")
						return "Release Status";
					if(language == "CZ")
						return "Release Status";
					if(language == "AL")
						return "Release Status";
					// English
					return "Release Status";
				case ExcelColumnNumber.StatusInternalTN:
					if(language == "DE")
						return "Freigabestatus TN intern";
					if(language == "TN")
						return "Internal Status TN";
					if(language == "CZ")
						return "Internal Status TN";
					if(language == "AL")
						return "Internal Status TN";
					// English
					return "Internal Status TN";
				case ExcelColumnNumber.StatusCheckTN:
					if(language == "DE")
						return "Prüfstatus TN Ware";
					if(language == "TN")
						return "Check Status TN";
					if(language == "CZ")
						return "Check Status TN";
					if(language == "AL")
						return "Check Status TN";
					// English
					return "Check Status TN";
				case ExcelColumnNumber.HourlyRate:
					if(language == "DE")
						return "Stundensatz";
					if(language == "TN")
						return "Hourly Rate";
					if(language == "CZ")
						return "Hourly Rate";
					if(language == "AL")
						return "Hourly Rate";
					// English
					return "Hourly Rate";
				case ExcelColumnNumber.LotSize:
					if(language == "DE")
						return "Losgöße";
					if(language == "TN")
						return "Lot Size";
					if(language == "CZ")
						return "Lot Size";
					if(language == "AL")
						return "Lot Size";
					// English
					return "Lot Size";
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
