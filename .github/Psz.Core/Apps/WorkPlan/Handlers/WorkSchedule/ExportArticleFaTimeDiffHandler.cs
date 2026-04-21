using OfficeOpenXml;
using Psz.Core.Apps.WorkPlan.Models.WorkSchedule;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public class ExportArticleFaTimeDiffHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel user { get; set; }
		private int? lager { get; set; }

		public ExportArticleFaTimeDiffHandler(Identity.Models.UserModel user, int? lager)
		{
			this.user = user;
			this.lager = lager;
		}

		public ResponseModel<byte[]> Handle()
		{
			lock(new object())
			{
				try
				{
					if(user == null)
					{
						throw new SharedKernel.Exceptions.UnauthorizedException();
					}

					return Perform(this.user, this.lager);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<byte[]> Perform(Identity.Models.UserModel user, int? lager)
		{
			var articleEntities = (Infrastructure.Data.Access.Joins.MTM.CRP.ArticleFaTimeAccess.GetArticlesFaTimeDiff(lager)
				   ?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleFaTimeEntity>())
				   .Select(x => new ArticleFaTimeDiffModel(x))?.ToList();

			return ResponseModel<byte[]>.SuccessResponse(getExcel(user.SelectedLanguage, articleEntities));
		}
		public ResponseModel<byte[]> Validate()
		{
			throw new NotImplementedException();
		}
		static byte[] getExcel(string selectedLanguage, List<ArticleFaTimeDiffModel> fertigungFaultyTimeEntities)
		{
			selectedLanguage = selectedLanguage ?? "DE";
			string filePath = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				//ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"FA - Work Plan - {DateTime.Now.ToString("yyyy/MM/dd")}");

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

					worksheet.Cells[2, 2].Value = $"FA - Work plan Time comparison [{DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")}]";
					worksheet.Cells[3, 2].Value = $"";
					worksheet.Cells[3, 2, 3, numberOfColumns + 1].Merge = true;
					worksheet.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ArticleNumber].Value = GetHeaderName(ExcelColumnNumber.ArticleNumber, selectedLanguage); // "Operation Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaNumber].Value = GetHeaderName(ExcelColumnNumber.FaNumber, selectedLanguage); // "Sub Operation Number"; 
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaQuantity].Value = GetHeaderName(ExcelColumnNumber.FaQuantity, selectedLanguage); // "Predecessor Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FaTime].Value = GetHeaderName(ExcelColumnNumber.FaTime, selectedLanguage); // "Predecessor Sub Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ApTime].Value = GetHeaderName(ExcelColumnNumber.ApTime, selectedLanguage); // "Plant";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var fa in fertigungFaultyTimeEntities)
					{
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ArticleNumber].Value = fa.ArticleNumber;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaNumber].Value = fa.FaNumber;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaQuantity].Value = fa.FaQuantity;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FaTime].Value = fa.FaTime;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ApTime].Value = fa.ApTime;

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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Comparison FA - Work Plan Time";
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
			FaNumber = 2,
			FaQuantity = 3,
			FaTime = 4,
			ApTime = 5,
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
				case ExcelColumnNumber.FaNumber:
					if(language == "DE")
						return "FA-Nummer";
					if(language == "TN")
						return "FA Number";
					if(language == "CZ")
						return "FA Number";
					if(language == "AL")
						return "FA Number";
					// English
					return "FA Number";
				case ExcelColumnNumber.FaQuantity:
					if(language == "DE")
						return "FA Menge";
					if(language == "TN")
						return "FA Quantity";
					if(language == "CZ")
						return "FA Quantity";
					if(language == "AL")
						return "FA Quantity";
					// English
					return "FA Quantity";
				case ExcelColumnNumber.FaTime:
					if(language == "DE")
						return "FA Zeit (Std)";
					if(language == "TN")
						return "FA Time (Hrs)";
					if(language == "CZ")
						return "FA Time (Hrs)";
					if(language == "AL")
						return "FA Time (Hrs)";
					// English
					return "FA Time (Hrs)";
				case ExcelColumnNumber.ApTime:
					if(language == "DE")
						return "Arbeitsplan Zeit (Std)";
					if(language == "TN")
						return "Work plan Time (Hrs)";
					if(language == "CZ")
						return "Work plan Time (Hrs)";
					if(language == "AL")
						return "Work plan Time (Hrs)";
					// English
					return "Work plan Time (Hrs)";
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
