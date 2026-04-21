using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class WorkSchedule
	{
		public static Core.Models.ResponseModel<byte[]> ExportToExcel(int Id, string selectedLanguage, int faQuantity = 1)
		{
			try
			{
				if(string.IsNullOrEmpty(selectedLanguage) || string.IsNullOrWhiteSpace(selectedLanguage))
					selectedLanguage = "EN";

				var workSchedule = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(Id);
				if(workSchedule == null)
					throw new Core.Exceptions.NotFoundException("Work schedule not found");

				var workScheduleDetails = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByWorkScheduleId(workSchedule.Id);
				workScheduleDetails = workScheduleDetails ?? new System.Collections.Generic.List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> { };
				var articleName = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workSchedule.ArticleId)?.Name;
				articleName = articleName ?? "[Article Not Found]";

				var filePath = Path.Combine(Path.GetTempPath(), $"{articleName}-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				var returnedFilePath = SaveToExcelFile(filePath, workSchedule, workScheduleDetails, selectedLanguage, faQuantity);

				return Core.Models.ResponseModel<byte[]>.SuccessResponse(File.ReadAllBytes(returnedFilePath));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		internal static string SaveToExcelFile(string filePath,
			Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity workSchedule,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> workScheduleDetails,
			string selectedLanguage = "EN", int faQuantity = 1)
		{
			selectedLanguage = selectedLanguage.ToUpper();
			try
			{
				var article = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workSchedule.ArticleId);
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();
				var halls = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get();
				//var departments = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get();
				var workAreas = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get();
				var workMachines = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Get();
				//var standardOperations = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get();
				//var standardOperationDescriptions = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Get();

				// Standarad operation + i18n
				var departments = getDepartments(selectedLanguage, true);
				var standardOperations = getStandardOperations(selectedLanguage, true);
				var standardOperationDescriptions = getStandardOperationDescriptions(selectedLanguage, true);

				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Work Schedule - {DateTime.Now.ToString("yyyy/MM/dd")}");

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
					worksheet.Cells[2, 2, 2, numberOfColumns].Merge = true;
					worksheet.Cells[2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

					var lastEditUSer = workSchedule.LastEditUserId.HasValue ? Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int)workSchedule.LastEditUserId)?.Name
						: Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int)workSchedule.CreationUserId)?.Name;
					var lastEditTime = workSchedule.LastEditTime.HasValue ? workSchedule.LastEditTime : workSchedule.CreationTime;

					lastEditUSer = lastEditUSer ?? "[User not Found]";
					lastEditTime = lastEditTime ?? new DateTime(1900, 1, 1);

					worksheet.Cells[2, 2].Value = $"Work schedule:  {workSchedule.Name}, last edited by {lastEditUSer.ToUpper()} on {((DateTime)lastEditTime).ToString("yyy-MM-dd HH:mm:ss")}";
					worksheet.Cells[3, 2].Value = $"{article?.Name}";
					//worksheet.Cells[3, 2, 3, numberOfColumns].Merge = true;
					worksheet.Cells[3, 3, 3, numberOfColumns].Merge = true;
					worksheet.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationNumber].Value = GetHeaderName(ExcelColumnNumber.OperationNumber, selectedLanguage); // "Operation Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.SubOperationNumber].Value = GetHeaderName(ExcelColumnNumber.SubOperationNumber, selectedLanguage); // "Sub Operation Number"; 
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.PredecessorOperation].Value = GetHeaderName(ExcelColumnNumber.PredecessorOperation, selectedLanguage); // "Predecessor Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.PredecessorSubOperation].Value = GetHeaderName(ExcelColumnNumber.PredecessorSubOperation, selectedLanguage); // "Predecessor Sub Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.CountryId].Value = GetHeaderName(ExcelColumnNumber.CountryId, selectedLanguage); // "Plant";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.HallId].Value = GetHeaderName(ExcelColumnNumber.HallId, selectedLanguage); // "Hall";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.DepartementId].Value = GetHeaderName(ExcelColumnNumber.DepartementId, selectedLanguage); // "Department";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.WorkAreaId].Value = GetHeaderName(ExcelColumnNumber.WorkAreaId, selectedLanguage); // "Work Area";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.WorkStationMachineId].Value = GetHeaderName(ExcelColumnNumber.WorkStationMachineId, selectedLanguage); // "Work Machine";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FromToolInsert].Value = GetHeaderName(ExcelColumnNumber.FromToolInsert, selectedLanguage); // "From Tool Insert";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.FromToolInsert2].Value = GetHeaderName(ExcelColumnNumber.FromToolInsert2, selectedLanguage); // "From Tool Insert2";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.StandardOperationId].Value = GetHeaderName(ExcelColumnNumber.StandardOperationId, selectedLanguage); // "Standard Operation";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationDescriptionId].Value = GetHeaderName(ExcelColumnNumber.OperationDescriptionId, selectedLanguage); // "Standard Operation Description";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Comment].Value = GetHeaderName(ExcelColumnNumber.Comment, selectedLanguage); // "Comment";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationValueAdding].Value = GetHeaderName(ExcelColumnNumber.OperationValueAdding, selectedLanguage); // "Operation Value Adding";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Amount].Value = GetHeaderName(ExcelColumnNumber.Amount, selectedLanguage); // "Amount";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationTimeSeconds].Value = GetHeaderName(ExcelColumnNumber.OperationTimeSeconds, selectedLanguage); // "Operation time Seconds";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.RelationOperationTime].Value = GetHeaderName(ExcelColumnNumber.RelationOperationTime, selectedLanguage); // "Relation Operation Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.StandardOccupancy].Value = GetHeaderName(ExcelColumnNumber.StandardOccupancy, selectedLanguage); // "Standard Occupancy";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.SetupTimeMinutes].Value = GetHeaderName(ExcelColumnNumber.SetupTimeMinutes, selectedLanguage); // "Setup Time Minutes";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.LotSizeSTD].Value = GetHeaderName(ExcelColumnNumber.LotSizeSTD, selectedLanguage); // "Lot size standard";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationTimeValueAdding].Value = GetHeaderName(ExcelColumnNumber.OperationTimeValueAdding, selectedLanguage); // "Operation time value adding";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation].Value = GetHeaderName(ExcelColumnNumber.TotalTimeOperation, selectedLanguage); // "Total operation time";


					var rowNumber = headerRowNumber + 1;
					var totalOperationTimeValueAdding = 0d;
					var totalTotalTimeOperation = 0m;
					// Loop through 
					foreach(var _workScheduleDetail in workScheduleDetails)
					{
						_workScheduleDetail.LotSizeSTD = faQuantity;
						var workScheduleDetail = Core.Apps.WorkPlan.Helpers.WorkSchedule.setTotalTimeOperation(_workScheduleDetail);
						workScheduleDetail.OperationTimeValueAdding = Core.Apps.WorkPlan.Helpers.WorkSchedule.GetOperationTimeValueAddng(_workScheduleDetail);

						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationNumber].Value = workScheduleDetail.OperationNumber;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.SubOperationNumber].Value = workScheduleDetail.SubOperationNumber;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.PredecessorOperation].Value = workScheduleDetail.PredecessorOperation;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.PredecessorSubOperation].Value = workScheduleDetail.PredecessorSubOperation;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.CountryId].Value = countries?.Find(x => x.Id == workScheduleDetail.CountryId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.HallId].Value = halls?.Find(x => x.Id == workScheduleDetail.HallId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.DepartementId].Value = departments?.Find(x => x.Id == workScheduleDetail.DepartementId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.WorkAreaId].Value = workAreas?.Find(x => x.Id == workScheduleDetail.WorkAreaId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.WorkStationMachineId].Value = workMachines?.Find(x => x.Id == workScheduleDetail.WorkStationMachineId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FromToolInsert].Value = workScheduleDetail.FromToolInsert;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.FromToolInsert2].Value = workScheduleDetail.FromToolInsert2;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.StandardOperationId].Value = standardOperations?.Find(x => x.Id == workScheduleDetail.StandardOperationId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationDescriptionId].Value = standardOperationDescriptions?.Find(x => x.Id == workScheduleDetail.OperationDescriptionId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Comment].Value = workScheduleDetail.Comment;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationValueAdding].Value = workScheduleDetail.OperationValueAdding.HasValue == false ? "" : (workScheduleDetail.OperationValueAdding == false ? "No" : "Yes");
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Amount].Value = workScheduleDetail.Amount;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationTimeSeconds].Value = workScheduleDetail.OperationTimeSeconds == -1 ? "" : Math.Round(workScheduleDetail.OperationTimeSeconds, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS).ToString();
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.RelationOperationTime].Value = workScheduleDetail.RelationOperationTime == 0 ? "Lot" : workScheduleDetail.RelationOperationTime == 1 ? "Piece" : "";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.StandardOccupancy].Value = Math.Round(workScheduleDetail.StandardOccupancy, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.SetupTimeMinutes].Value = Math.Round(workScheduleDetail.SetupTimeMinutes, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.LotSizeSTD].Value = faQuantity.ToString(); // "1"; // 2021-08-04 Pfeiffer - workScheduleDetail.LotSizeSTD == -1 ? "" : workScheduleDetail.LotSizeSTD.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationTimeValueAdding].Value = workScheduleDetail.OperationTimeValueAdding == -1 ? "" : workScheduleDetail.OperationTimeValueAdding.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation].Value = workScheduleDetail.TotalTimeOperation == -1 ? "" : Math.Round(workScheduleDetail.TotalTimeOperation, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS).ToString();

						totalOperationTimeValueAdding += Math.Round(workScheduleDetail.OperationTimeValueAdding, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
						totalTotalTimeOperation += Math.Round(workScheduleDetail.TotalTimeOperation, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					// footer (4 rows)
					worksheet.Cells[rowNumber + 1, startColumnNumber + (int)ExcelColumnNumber.OperationTimeValueAdding].Value = Math.Round(totalOperationTimeValueAdding, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);
					worksheet.Cells[rowNumber + 1, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation].Value = Math.Round(totalTotalTimeOperation, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS);

					worksheet.Cells[rowNumber + 2, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation].Value = "";
					worksheet.Cells[rowNumber + 3, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation - 2].Value = $"{GetHeaderName(ExcelColumnNumber.SetupRatio, selectedLanguage)}: ";
					worksheet.Cells[rowNumber + 3, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation - 1].Value = totalOperationTimeValueAdding > 0
						? Math.Round(((double)totalTotalTimeOperation - (double)totalOperationTimeValueAdding) / totalOperationTimeValueAdding, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS) * 100 + "%"
						: "NaN";
					worksheet.Cells[rowNumber + 4, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation].Value = "";

					// Doc content
					if(workScheduleDetails != null && workScheduleDetails.Count > 0)
					{
						using(var range = worksheet.Cells[2, 2, rowNumber - 1, numberOfColumns])
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
					using(var range = worksheet.Cells[2, 2, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}

					//// Totals
					worksheet.Cells[rowNumber + 1, startColumnNumber + (int)ExcelColumnNumber.OperationTimeValueAdding].Style.Font.Bold = true;
					worksheet.Cells[rowNumber + 1, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation].Style.Font.Bold = true;

					worksheet.Cells[rowNumber + 3, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation - 2].Style.Font.Bold = true;
					worksheet.Cells[rowNumber + 3, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation - 1].Style.Font.Bold = true;

					/// --------
					////set the number of cells with content in Sheet 2, range C1 - C25 into I27
					//worksheet.Cells["I27"].Formula = "=COUNT('" + worksheet.Name + "'!" + worksheet.Cells["A1:B25"] + ")";

					////calculate all the values of the formulas in the Excel file
					//Workbook.Calculate();


					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Work Schedule";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

					return filePath;
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
