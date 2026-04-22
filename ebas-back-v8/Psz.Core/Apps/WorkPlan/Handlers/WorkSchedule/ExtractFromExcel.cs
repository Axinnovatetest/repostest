using System;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using Psz.Core.Apps.WorkPlan.Models.WorkPlan;



namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class WorkSchedule
	{
		public static Core.Models.ResponseModel<int> ExtractFromExcel(string filePath, int Id, Core.Identity.Models.UserModel user)
		{
			try
			{
				var workSchedule = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(Id);
				if(workSchedule == null)
				{
					throw new Core.Exceptions.NotFoundException("Work Schedule not found");
				}

				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(workSchedule.ArticleId);
				if(article == null)
				{
					throw new Core.Exceptions.NotFoundException("Article not found");
				}

				List<string> errors;
				var workScheduleDetails = ReadFromExcel(filePath, user.Id, article.ArtikelNummer, out errors, user.SelectedLanguage == null || string.IsNullOrEmpty(user.SelectedLanguage) || string.IsNullOrEmpty(user.SelectedLanguage) ? "EN" : user.SelectedLanguage);
				if(errors != null && errors.Count > 0)
				{
					return new Core.Models.ResponseModel<int>()
					{
						Body = -1,
						Success = true,
						Errors = errors
					};
				}
				if(workScheduleDetails != null)
				{
					foreach(var workScheduleDetail in workScheduleDetails.Positions)
					{
						var _workScheduleDetail = Core.Apps.WorkPlan.Helpers.WorkSchedule.setTotalTimeOperation(workScheduleDetail);
						_workScheduleDetail.WorkScheduleId = Id;
						_workScheduleDetail.OperationTimeValueAdding = Core.Apps.WorkPlan.Helpers.WorkSchedule.GetOperationTimeValueAddng(_workScheduleDetail);
						Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Insert(_workScheduleDetail);
					}
				}

				return new Core.Models.ResponseModel<int>()
				{
					Body = -1,
					Success = true,
					Errors = { $"{workScheduleDetails.Positions.Count} operation(s) imported successfully" }
				};
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);

				return new Core.Models.ResponseModel<int>() { Body = -1, Success = false, Errors = { exception.Message } };
			}
		}
		//16/09
		public static Core.Models.ResponseModel<List<WorkScheduleDetailsModel>> ExtractExcel(string filePath, int Id, Core.Identity.Models.UserModel user)
		{
			try
			{
				var workSchedule = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(Id);
				if(workSchedule == null)
				{
					throw new Core.Exceptions.NotFoundException("Work Schedule not found");
				}

				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(workSchedule.ArticleId);
				if(article == null)
				{
					throw new Core.Exceptions.NotFoundException("Article not found");
				}

				List<string> errors;
				var workScheduleDetails = ReadFromExcel(filePath, user.Id, article.ArtikelNummer, out errors, user.SelectedLanguage == null || string.IsNullOrEmpty(user.SelectedLanguage) || string.IsNullOrEmpty(user.SelectedLanguage) ? "EN" : user.SelectedLanguage);
				if(errors != null && errors.Count > 0)
				{
					return new Core.Models.ResponseModel<List<WorkScheduleDetailsModel>>()
					{
						Body = workScheduleDetails.Positions.Select(x => new WorkScheduleDetailsModel(x)).ToList(),
						Success = true,
						Errors = errors
					};
				}
				//if(workScheduleDetails != null)
				//{
				//	foreach(var workScheduleDetail in workScheduleDetails)
				//	{
				//		var _workScheduleDetail = Core.Apps.WorkPlan.Helpers.WorkSchedule.setTotalTimeOperation(workScheduleDetail);
				//		_workScheduleDetail.WorkScheduleId = Id;
				//		_workScheduleDetail.OperationTimeValueAdding = Core.Apps.WorkPlan.Helpers.WorkSchedule.GetOperationTimeValueAddng(_workScheduleDetail);
				//	//	Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Insert(_workScheduleDetail);
				//	}
				//}

				return new Core.Models.ResponseModel<List<WorkScheduleDetailsModel>>()
				{
					Body = null,
					Success = true,
					Errors = { $"{workScheduleDetails.Positions.Count} operation(s) imported successfully" }
				};
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);

				return new Core.Models.ResponseModel<List<WorkScheduleDetailsModel>>() { Body = null, Success = false, Errors = { exception.Message } };
			}
		}

		public static ReadFromXlsReturnModel ReadFromExcel(string filePath, int creationUserId, string articleNumber, out List<string> errors, string codeLanguage = "EN")
		{
			codeLanguage = GetSelectedLanguage(codeLanguage.Trim());
			errors = new List<string> { };
			var response = new ReadFromXlsReturnModel();
			try
			{
				FileInfo fileInfo = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				var package = new ExcelPackage(fileInfo);
				var worksheet = package.Workbook.Worksheets[0];
				var rowStart = worksheet.Dimension.Start.Row;
				var rowEnd = worksheet.Dimension.End.Row;

				// footer rows
				rowEnd -= 4;

				// get number of rows and columns in the sheet
				var rows = worksheet.Dimension.Rows;
				var columns = worksheet.Dimension.Columns;
				var startRowNumber = 5;
				var startColNumber = 1;

				if(rows > 1 && columns > 1)
				{

					// Get Article 3rd row, 1st col
					var xlsArticleNumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[3, 2]).ToLower().Trim();
					var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(xlsArticleNumber);
					if(article is null)
					{
						errors.Add($"Import article in excel is not found.");
						return null;
					}
					response.ArtikleNr = article?.ArtikelNr ?? 0;
					response.Artikelnummer = xlsArticleNumber;

					//if(articleNumber.ToLower() != xlsArticleNumber)
					//{
					//	errors.Add($"Import Article '{xlsArticleNumber}' does not match Work plan Article '{articleNumber}'");
					//	return null;
					//}
					//if(!string.IsNullOrEmpty(articleNumber))
					//{
					//	var xlsArticleNumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[3, 2]).ToLower().Trim();
					//	if(articleNumber.ToLower() != xlsArticleNumber)
					//	{
					//		errors.Add($"Import Article '{xlsArticleNumber}' does not match Work plan Article '{articleNumber}'");
					//		return null;
					//	}
					//}

					var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();
					var halls = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get();
					var workAreas = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get();
					var workMachines = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Get();

					// Standarad operation + i18n
					var departments = getDepartments(codeLanguage);
					var standardOperations = getStandardOperations(codeLanguage);
					var standardOperationDescriptions = getStandardOperationDescriptions(codeLanguage);

					//var standardOperationI18Ns = Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.GetByLanguage(codeLanguage);
					//var standardOperationDescriptionI18Ns = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByCodeLanguage(codeLanguage);

					countries = countries != null && countries.Count <= 0 ? null : countries;
					halls = halls != null && halls.Count <= 0 ? null : halls;
					departments = departments != null && departments.Count <= 0 ? null : departments;
					workAreas = workAreas != null && workAreas.Count <= 0 ? null : workAreas;
					workMachines = workMachines != null && workMachines.Count <= 0 ? null : workMachines;
					standardOperations = standardOperations != null && standardOperations.Count <= 0 ? null : standardOperations;
					standardOperationDescriptions = standardOperationDescriptions != null && standardOperationDescriptions.Count <= 0 ? null : standardOperationDescriptions;

					var workScheduleDetails = new List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> { };


					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							// stop when Operation number is null or empty string
							//var opNumberString = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.OperationNumber]);
							//if (string.IsNullOrEmpty(opNumberString) || string.IsNullOrWhiteSpace(opNumberString))
							//{
							//    break;
							//}

							// Only if operation is valid
							//if (int.TryParse(opNumberString, out int operationNumber))
							{
								var _country = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.CountryId]).ToLower().Trim();
								var _hall = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.HallId]).ToLower().Trim();
								var _department = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.DepartementId]).ToLower().Trim();
								var _workarea = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.WorkAreaId]).ToLower().Trim();
								var _workmachine = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.WorkStationMachineId]).ToLower().Trim();
								var _standardOperation = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.StandardOperationId]).ToLower().Trim();
								var _subStandardOperation = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.OperationDescriptionId]).ToLower().Trim();


								var country = countries?.Find(x => x.Name.ToLower().Trim() == _country || x.Designation.ToLower().Trim() == _country);
								var department = departments?.Find(x => x.Name.ToLower().Trim() == _department);
								var standardOperation = standardOperations?.Find(x => x.Name.ToLower().Trim() == _standardOperation);

								// except for the op description, every dropdown should a have a value | Ahmed changed his mind :-)
								// stop adding ws whenever we reach empty SDT Op!
								if(string.IsNullOrEmpty(_standardOperation) || string.IsNullOrWhiteSpace(_standardOperation))
								{
									if(i == startRowNumber) // - First row can be w/o std op
										continue;
									else
									{
										// 2021-09-13 - if we have anything defined and Std Op is null, reject import
										if(!string.IsNullOrWhiteSpace(_country) || !string.IsNullOrWhiteSpace(_hall)
											|| !string.IsNullOrWhiteSpace(_department) || !string.IsNullOrWhiteSpace(_workarea))
										{
											errors.Add($"Row {i}: invalid Operation '{_standardOperation}'.");
										}
										break;
									}
								}

								if(!string.IsNullOrEmpty(_country) && !string.IsNullOrWhiteSpace(_country))
								{
									if(!string.IsNullOrEmpty(_hall) && !string.IsNullOrWhiteSpace(_hall))
									{
										if(!string.IsNullOrEmpty(_department) && !string.IsNullOrWhiteSpace(_department))
										{
											if(!string.IsNullOrEmpty(_workarea) && !string.IsNullOrWhiteSpace(_workarea))
											{
												//if (!string.IsNullOrEmpty(_workmachine) && !string.IsNullOrWhiteSpace(_workmachine))
												{
													//if (!string.IsNullOrEmpty(sd) && !string.IsNullOrWhiteSpace(sd))
													{
														if(country != null)
														{
															if(department != null)
															{
																if(checkHall(country, halls, _hall, out var hall))
																{
																	if(checkWorkArea(department, hall, workAreas, _workarea, out var workArea))
																	{
																		if(checkMachine(workArea, workMachines, _workmachine, out var workMachine))
																		{
																			if(standardOperation != null)
																			{
																				if(checkOperationDescription(standardOperation, standardOperationDescriptions, _subStandardOperation, out var standardOperationDescription))
																				{
																					// 2021-08-04 Pfeiffer
																					var stdOccupancy = Common.Helpers.Formatters.XLS.GetFloat(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.StandardOccupancy]);
																					if(stdOccupancy > 0)
																					{
																						// 2021-08-04 Pfeiffer - 2026-02-18 Ceraku LotSize > 0
																						var lotSize = Common.Helpers.Formatters.XLS.GetInt(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.LotSizeSTD]);
																						if(lotSize > 0)
																						{
																							// - 2021-12-01 - Khelil - email from laabaied
																							var _amount = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.Amount]);
																							if(_amount > 0)
																							{
																								// - 2021-12-01 - Khelil - email from laabaied
																								var _operationTime = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.OperationTimeSeconds]);
																								if(_operationTime > 0)
																								{
																									var workSchedule = new Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity
																									{
																										OrderDisplayId = 1, // >>> Will be set later
																										OperationNumber = Common.Helpers.Formatters.XLS.GetInt(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.OperationNumber]), // >>> operationNumber,
																										SubOperationNumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.SubOperationNumber]),
																										PredecessorOperation = Common.Helpers.Formatters.XLS.GetInt(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.PredecessorOperation]),
																										PredecessorSubOperation = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.PredecessorSubOperation]),
																										CountryId = country != null ? country.Id : -1,
																										CountryName = country != null ? country.Name : "",
																										HallId = hall != null ? hall.Id : -1,
																										HallName = hall != null ? hall.Name : "",
																										DepartementId = department != null ? department.Id : -1,
																										DepartmentName = department != null ? department.Name : "",
																										WorkAreaId = workArea != null ? workArea.Id : -1,
																										WorkAreaName = workArea != null ? workArea.Name : "",
																										WorkStationMachineId = workMachine != null ? workMachine.Id : -1,
																										WorkStationMachineName = workMachine != null ? workMachine.Name : "",
																										FromToolInsert = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.FromToolInsert]),
																										FromToolInsert2 = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.FromToolInsert2]),
																										StandardOperationId = standardOperation != null ? standardOperation.Id : -1,
																										StandardOperationName = standardOperation != null ? standardOperation.Name : "",
																										OperationDescriptionId = standardOperationDescription != null ? standardOperationDescription.Id : -1,
																										OperationDescriptionName = standardOperationDescription != null ? standardOperationDescription.Name : "",
																										Comment = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.Comment]),
																										OperationValueAdding = standardOperation != null ? (bool?)standardOperation.OperationValueAdding : null, //getCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.OperationValueAdding]).ToLower().Trim() == "yes",
																										Amount = _amount, // Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.Amount]),
																										OperationTimeSeconds = _operationTime, // Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.OperationTimeSeconds]), // >>>> Ahmed
																										RelationOperationTime = string.Equals( Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.RelationOperationTime]) , "lot", StringComparison.OrdinalIgnoreCase )?0:1,//standardOperation != null ? standardOperation.RelationOperationTime : -1, //getCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.RelationOperationTime]).ToLower().Trim() == "lot" ? 0 : (getCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.RelationOperationTime]).ToLower().Trim() == "piece" ? 1 : -1),
																										StandardOccupancy = stdOccupancy,
																										SetupTimeMinutes = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.SetupTimeMinutes]),
																										LotSizeSTD = lotSize, // >>>> default 1
																										OperationTimeValueAdding = Math.Round((double)Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.OperationTimeValueAdding]), Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS), // <<<<<<
																										TotalTimeOperation = Math.Round(Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.TotalTimeOperation]), Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS), // <<<<<<
																										CreationTime = DateTime.Now,
																										CreationUserId = creationUserId,
																									};

																									// Updates - 2026-02-18 - Ceraku - read number from Excel
																									//workSchedule = setOperationNumber(workSchedule, (i == startRowNumber || workScheduleDetails.Count <= 0) ? null : workScheduleDetails[workScheduleDetails.Count - 1]);
																									//workSchedule = setTotalTimeOperation(workSchedule);
																									workSchedule = setDepartment(workSchedule, departments, workAreas);

																									workScheduleDetails.Add(workSchedule);
																								}
																								else
																								{
																									errors.Add($"Row {i}: invalid Operation Time '{_operationTime}' value.");
																								}
																							}
																							else
																							{
																								errors.Add($"Row {i}: invalid Amount '{_amount}' value.");
																							}
																						}
																						else
																						{
																							errors.Add($"Row {i}: invalid Lot Size '{lotSize}' value.");
																						}
																					}
																					else
																					{
																						errors.Add($"Row {i}: invalid Standard Occupancy '{stdOccupancy}' value.");
																					}
																				}
																				else
																				{
																					errors.Add($"Row {i}: invalid Sub operation '{_subStandardOperation}' in operation '{_standardOperation}'.");
																				}
																			}
																			else
																			{
																				errors.Add($"Row {i}: invalid Standard operation '{_standardOperation}'.");
																			}
																		}
																		else
																		{
																			errors.Add($"Row {i}: invalid Machine '{_workmachine}' in Work area '{_workarea}'.");
																		}
																	}
																	else
																	{
																		errors.Add($"Row {i}: invalid Work area '{_workarea}' in Hall '{_hall}' and Department '{_department}'.");
																	}
																}
																else
																{
																	errors.Add($"Row {i}: invalid Hall '{_hall}' in Country '{_country}'.");
																}
															}
															else
															{
																errors.Add($"Row {i}: invalid department '{_department}'.");
															}
														}
														else
														{
															errors.Add($"Row {i}: invalid country '{_country}'.");
														}
													}
													//else
													//{
													//    errors.Add($"Row {i}: invalid standard operation '{sd}'.");
													//}
												}
												//else
												//{
												//    errors.Add($"Row {i}: invalid work machine '{_workmachine}'.");
												//}
											}
											else
											{
												errors.Add($"Row {i}: invalid work area '{_workarea}'.");
											}
										}
										else
										{
											errors.Add($"Row {i}: invalid department '{_department}'.");
										}
									}
									else
									{
										errors.Add($"Row {i}: invalid hall '{_hall}'.");
									}
								}
								else
								{
									errors.Add($"Row {i}: invalid country '{_country}'.");
								}
							}
							//else
							//{
							//    errors.Add($"Row {i}: invalid operation number {getCellValue(worksheet.Cells[i, startColNumber + (int)ExcelColumnNumber.OperationNumber])}.");
							//    break;
							//}
						} catch(System.Exception exceptionInternal)
						{
							Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
							errors.Add($"Row {i}: unknown error.");
						}
					}
					// ---
					response.Positions = workScheduleDetails;
					return response;

				}
				else
				{
					errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
					return null;
				}
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}

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
				var articleName = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(workSchedule.ArticleId)?.ArtikelNummer;
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
				//var article = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workSchedule.ArticleId);
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(workSchedule.ArticleId);
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
					worksheet.Cells[3, 2].Value = $"{article?.ArtikelNummer}";
					//worksheet.Cells[3, 2, 3, numberOfColumns].Merge = true;
					worksheet.Cells[3, 3, 3, numberOfColumns].Merge = true;
					worksheet.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					var workingGroup = article?.Artikelkurztext;


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
						// -  2026-02-18 - Ceraku - take values from Excel
						//_workScheduleDetail.LotSizeSTD = faQuantity;
						//var workScheduleDetail = Core.Apps.WorkPlan.Helpers.WorkSchedule.setTotalTimeOperation(_workScheduleDetail);
						//workScheduleDetail.OperationTimeValueAdding = Core.Apps.WorkPlan.Helpers.WorkSchedule.GetOperationTimeValueAddng(_workScheduleDetail);
						var workScheduleDetail = _workScheduleDetail;

						var wa = workAreas?.Find(x => x.Id == workScheduleDetail.WorkAreaId)?.Name;

						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.OperationNumber].Value = workScheduleDetail.OperationNumber;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.SubOperationNumber].Value = workScheduleDetail.SubOperationNumber;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.PredecessorOperation].Value = workScheduleDetail.PredecessorOperation;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.PredecessorSubOperation].Value = workScheduleDetail.PredecessorSubOperation;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.CountryId].Value = countries?.Find(x => x.Id == workScheduleDetail.CountryId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.HallId].Value = halls?.Find(x => x.Id == workScheduleDetail.HallId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.DepartementId].Value = departments?.Find(x => x.Id == workScheduleDetail.DepartementId)?.Name;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.WorkAreaId].Value = string.Equals(wa, "t-w", StringComparison.OrdinalIgnoreCase) ? workingGroup : wa;
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
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.LotSizeSTD].Value = workScheduleDetail.LotSizeSTD;// - 2026-02-19 - Ceraku use LotSize from WPL // faQuantity.ToString(); // "1"; // 2021-08-04 Pfeiffer - workScheduleDetail.LotSizeSTD == -1 ? "" : workScheduleDetail.LotSizeSTD.ToString();
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
					worksheet.Cells[rowNumber + 3, startColumnNumber + (int)ExcelColumnNumber.TotalTimeOperation - 1].Value = totalTotalTimeOperation != 0
						? Math.Round(((decimal)totalOperationTimeValueAdding - totalTotalTimeOperation) / totalTotalTimeOperation, Psz.Core.Apps.WorkPlan.Helpers.WorkSchedule.EXCEL_ROUND_DECIMALS) * 100 + "%"
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
		public class dptMinimal
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public int IdDepartment { get; set; }
		}

		public class stdMinimal
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public int IdStandardOperation { get; set; }
			public double RelationOperationTime { get; set; }
			public bool OperationValueAdding { get; set; }
		}
		public class ReadFromXlsReturnModel
		{
			public int ArtikleNr { get; set; }
			public string Artikelnummer { get; set; }
			public List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> Positions { get; set; }
		}
		private static List<dptMinimal> getDepartments(string codeLanguage, bool replace = false)
		{
			var _ = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get();

			List<dptMinimal> standardOperations = null; //Enumerable.Empty<object>().Select(r => new { Id = 0, Name = "" }).ToList();
			if(_ != null)
			{
				standardOperations = _.Select(x => new dptMinimal
				{
					Id = x.Id,
					Name = x.Name,
					IdDepartment = x.Id
				}).ToList();
			}

			if(!string.IsNullOrEmpty(codeLanguage) && !string.IsNullOrWhiteSpace(codeLanguage) && codeLanguage.ToLower() != "en")
			{
				var __ = Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.GetByLanguage(codeLanguage);
				if(__ != null)
				{
					if(standardOperations == null)
						standardOperations = new List<dptMinimal>();

					foreach(var stdi18n in __)
					{
						var std = _.Find(x => x.Id == stdi18n.IdDepartment);
						if(std != null)
						{
							if(replace)
							{
								var idx = standardOperations.FindIndex(x => x.Id == std.Id);
								if(idx >= 0)
								{
									standardOperations[idx].Name = stdi18n.Name;
								}
							}
							else
							{
								standardOperations.Add(new dptMinimal
								{
									Id = std.Id,
									Name = stdi18n.Name,
									IdDepartment = std.Id
								});
							}
						}
					}
				}
			}

			return standardOperations;
		}
		private static List<stdMinimal> getStandardOperations(string codeLanguage, bool replace = false)
		{
			var _ = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get();

			List<stdMinimal> standardOperations = null; //Enumerable.Empty<object>().Select(r => new { Id = 0, Name = "" }).ToList();
			if(_ != null)
			{
				standardOperations = _.Select(x => new stdMinimal
				{
					Id = x.Id,
					Name = x.Name,
					OperationValueAdding = x.OperationValueAdding,
					RelationOperationTime = x.RelationOperationTime
				}).ToList();
			}

			if(!string.IsNullOrEmpty(codeLanguage) && !string.IsNullOrWhiteSpace(codeLanguage) && codeLanguage.ToLower() != "en")
			{
				var __ = Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.GetByLanguage(codeLanguage);
				if(__ != null)
				{
					if(standardOperations == null)
						standardOperations = new List<stdMinimal>();

					foreach(var stdi18n in __)
					{
						var std = _.Find(x => x.Id == stdi18n.IdStandardOperation);
						if(std != null)
						{
							if(replace)
							{
								var idx = standardOperations.FindIndex(x => x.Id == std.Id);
								if(idx >= 0)
								{
									standardOperations[idx].Name = stdi18n.Name;
								}
							}
							else
							{
								standardOperations.Add(new stdMinimal
								{
									Id = std.Id,
									Name = stdi18n.Name,
									OperationValueAdding = std.OperationValueAdding,
									RelationOperationTime = std.RelationOperationTime
								});
							}
						}
					}
				}
			}

			return standardOperations;
		}
		private static List<stdMinimal> getStandardOperationDescriptions(string codeLanguage, bool replace = false)
		{
			var _ = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Get();

			List<stdMinimal> standardOperations = null; //Enumerable.Empty<object>().Select(r => new { Id = 0, Name = "" }).ToList();
			if(_ != null)
			{
				standardOperations = _.Select(x => new stdMinimal { Id = x.Id, IdStandardOperation = x.StdOperationId, Name = x.Description }).ToList();
			}

			if(!string.IsNullOrEmpty(codeLanguage) && !string.IsNullOrWhiteSpace(codeLanguage) && codeLanguage.ToLower() != "en")
			{
				var __ = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByCodeLanguage(codeLanguage);
				if(__ != null)
				{
					if(standardOperations == null)
					{
						standardOperations = new List<stdMinimal>();
					}

					foreach(var stdI18n in __)
					{
						var std = _.Find(x => x.Id == stdI18n.IdStandardOperationDescription);
						if(std != null)
						{
							if(replace)
							{
								var idx = standardOperations.FindIndex(x => x.Id == std.Id);
								if(idx >= 0)
								{
									standardOperations[idx].Name = stdI18n.Name;
								}
							}
							else
							{
								standardOperations.Add(new stdMinimal
								{
									Id = std.Id,
									IdStandardOperation = std.StdOperationId,
									Name = stdI18n.Name
								});
							}

						}
					}
				}
			}

			return standardOperations;
		}

		//internal static string getCellValue(ExcelRange cell)
		//{
		//    var val = cell.Value;
		//    if (val == null)
		//    {
		//        return "";
		//    }

		//    return val.ToString();
		//}
		//internal static string EscapeDecimalSeparator(string input)
		//{
		//    if(!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
		//    {
		//        //input = input.Replace(",", ".");
		//    }

		//    return input;
		//}
		internal static Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity setOperationNumber(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity workSchedule,
			Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity previousWorkSchedule)
		{
			if(previousWorkSchedule == null)
			{
				workSchedule.OperationNumber = 10;
				workSchedule.PredecessorOperation = 0;
				return workSchedule;
			}

			if(previousWorkSchedule.WorkAreaId == workSchedule.WorkAreaId)
			{
				workSchedule.OperationNumber = previousWorkSchedule.OperationNumber;
			}
			else
			{
				workSchedule.OperationNumber = previousWorkSchedule.OperationNumber + 10;
			}

			//workSchedule.PredecessorOperation = workSchedule.OperationNumber;

			return workSchedule;
		}
		internal static Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity setDepartment(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity workSchedule,
			List<dptMinimal> departmentEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> workAreaEntities)
		{
			workSchedule.DepartementId = -1;
			if(workSchedule != null && workSchedule.WorkAreaId > 0 && departmentEntities != null && departmentEntities.Count > 0)
			{
				var wa = workAreaEntities?.Find(x => x.Id == workSchedule.WorkAreaId);
				if(wa != null && wa.DepartmentId.HasValue)
				{
					workSchedule.DepartementId = departmentEntities.Find(x => x.Id == wa.DepartmentId.Value)?.Id ?? -1;
				}
			}

			return workSchedule;
		}

		internal static bool checkHall(Infrastructure.Data.Entities.Tables.WPL.CountryEntity country,
			List<Infrastructure.Data.Entities.Tables.WPL.HallEntity> halls, string h, out Infrastructure.Data.Entities.Tables.WPL.HallEntity hall)
		{
			hall = null;
			if(string.IsNullOrEmpty(h) || string.IsNullOrWhiteSpace(h))
				return true;

			if(country == null || halls == null || halls.Count <= 0)
				return false;

			for(int i = 0; i < halls.Count; i++)
			{
				if((halls[i].Name.ToLower().Trim() == h || halls[i].Name.ToLower().Trim() == h.ToLower().Trim())
					&& halls[i].CountryId == country.Id)
				{
					hall = halls[i];
					return true;
				}
			}

			return false;
		}
		internal static bool checkWorkArea(
			dptMinimal department,
			Infrastructure.Data.Entities.Tables.WPL.HallEntity hall,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> workAreaEntities, string w,
			out Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity workAreaEntity)
		{
			workAreaEntity = null;
			if(string.IsNullOrEmpty(w) || string.IsNullOrWhiteSpace(w))
				return true;

			if(hall == null || workAreaEntities == null || workAreaEntities.Count <= 0)
				return false;

			for(int i = 0; i < workAreaEntities.Count; i++)
			{
				if((workAreaEntities[i].Name.ToLower().Trim() == w || workAreaEntities[i].Name.ToLower() == w.Trim().ToLower().Trim())
					&& workAreaEntities[i].HallId == hall.Id && workAreaEntities[i].DepartmentId == department.Id)
				{
					workAreaEntity = workAreaEntities[i];
					return true;
				}
			}

			return false;
		}
		internal static bool checkMachine(Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity workAreaEntity,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> workStationMachineEntities, string ww,
			out Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity workStationMachineEntity)
		{
			workStationMachineEntity = null;
			if(string.IsNullOrEmpty(ww) || string.IsNullOrWhiteSpace(ww))
				return true;

			if(workAreaEntity == null || workStationMachineEntities == null || workStationMachineEntities.Count <= 0)
				return false;

			for(int i = 0; i < workStationMachineEntities.Count; i++)
			{
				if((workStationMachineEntities[i].Name.ToLower().Trim() == ww || workStationMachineEntities[i].Name.ToLower().Trim() == ww.ToLower().Trim()) // .Replace(" ", "")
					&& workStationMachineEntities[i].WorkAreaId == workAreaEntity.Id)
				{
					workStationMachineEntity = workStationMachineEntities[i];
					return true;
				}
			}

			return false;
		}
		internal static bool checkOperationDescription(stdMinimal standardOperationEntity,
			List<stdMinimal> standardOperationDescriptionEntities, string s,
			out stdMinimal standardOperationDescriptionEntity)
		{
			standardOperationDescriptionEntity = null;
			if(string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
				return true;

			if(standardOperationEntity == null || standardOperationDescriptionEntities == null || standardOperationDescriptionEntities.Count <= 0)
				return false;

			for(int i = 0; i < standardOperationDescriptionEntities.Count; i++)
			{
				if((standardOperationDescriptionEntities[i].Name.ToLower().Trim() == s //standardOperationEntity.Name.ToLower().Trim() 
					|| standardOperationDescriptionEntities[i].Name.ToLower().Trim() == s.ToLower().Trim()) // standardOperationEntity.Name.ToLower().Trim().Replace(" ", ""))
					&& standardOperationDescriptionEntities[i].IdStandardOperation == standardOperationEntity.Id)
				{
					standardOperationDescriptionEntity = standardOperationDescriptionEntities[i];
					return true;
				}
			}

			return false;
		}


		public enum ExcelBoolean
		{
			yes = 0,
			no = 1
		}
		public enum ExcelColumnNumber
		{
			OperationNumber = 1,
			SubOperationNumber = 2,
			PredecessorOperation = 3,
			PredecessorSubOperation = 4,
			CountryId = 5,
			HallId = 6,
			DepartementId = 7,
			WorkAreaId = 8,
			WorkStationMachineId = 9,
			FromToolInsert = 10,
			FromToolInsert2 = 11,
			StandardOperationId = 12,
			OperationDescriptionId = 13,
			Comment = 14,
			OperationValueAdding = 15,
			Amount = 16,
			OperationTimeSeconds = 17,
			RelationOperationTime = 18,
			StandardOccupancy = 19,
			SetupTimeMinutes = 20,
			LotSizeSTD = 21,
			OperationTimeValueAdding = 22,
			TotalTimeOperation = 23,
			SetupRatio = 24
		}
		public static string GetHeaderName(ExcelColumnNumber excelColumnNumber, string language)
		{
			language = language?.ToUpper();
			switch(excelColumnNumber)
			{
				case ExcelColumnNumber.OperationNumber:
					if(language == "DE")
						return "OperationsNr";
					if(language == "TN")
						return "Numéro Opération";
					if(language == "CZ")
						return "Číslo operace";
					if(language == "AL")
						return "Nr I operacionit";
					// English
					return "Operation Number";
				case ExcelColumnNumber.SubOperationNumber:
					if(language == "DE")
						return "UnteroperationsNr";
					if(language == "TN")
						return "Numéro sous opération";
					if(language == "CZ")
						return "Číslo vedlejší operace";
					if(language == "AL")
						return "Nr I nen Operacionit";
					// English
					return "Sub Operation Number";
				case ExcelColumnNumber.PredecessorOperation:
					if(language == "DE")
						return "VorgängerunterOp";
					if(language == "TN")
						return "Opération Précédente";
					if(language == "CZ")
						return "Předchozí operace";
					if(language == "AL")
						return "Operacioni paraardhes";
					// English
					return "Predecessor Operation";
				case ExcelColumnNumber.PredecessorSubOperation:
					if(language == "DE")
						return "VorgängerunterOp";
					if(language == "TN")
						return "Sous Opération précédente";
					if(language == "CZ")
						return "Předchozí podoperace";
					if(language == "AL")
						return "Nen Operacioni paraardhes";
					// English
					return "Predecessor Sub Operation";
				case ExcelColumnNumber.CountryId:
					if(language == "DE")
						return "Land";
					if(language == "TN")
						return "Pays";
					if(language == "CZ")
						return "Země";
					if(language == "AL")
						return "Vend";
					// English
					return "Plant";
				case ExcelColumnNumber.HallId:
					if(language == "DE")
						return "Halle";
					if(language == "TN")
						return "Unité";
					if(language == "CZ")
						return "Hala";
					if(language == "AL")
						return "Godina";
					// English
					return "Hall";
				case ExcelColumnNumber.DepartementId:
					if(language == "DE")
						return "Abteilung";
					if(language == "TN")
						return "Département";
					if(language == "CZ")
						return "Oddělení";
					if(language == "AL")
						return "Departamenti";
					// English
					return "Department";
				case ExcelColumnNumber.WorkAreaId:
					if(language == "DE")
						return "Arbeitsbereich";
					if(language == "TN")
						return "Zone";
					if(language == "CZ")
						return "Pracovní oblast";
					if(language == "AL")
						return "Vendi I punes";
					// English
					return "Work Area";
				case ExcelColumnNumber.WorkStationMachineId:
					if(language == "DE")
						return "Maschine / Arbeitsstationen";
					if(language == "TN")
						return "Machine / Poste de travail";
					if(language == "CZ")
						return "Stroj/ oblast práce";
					if(language == "AL")
						return "Makinerite/ Stacioni i punes";
					// English
					return "Work Machine";
				case ExcelColumnNumber.FromToolInsert:
					if(language == "DE")
						return "Form 1";
					if(language == "TN")
						return "Outil 1";
					if(language == "CZ")
						return "Formu 1";
					if(language == "AL")
						return "Forma/Vegla/Inserti";
					// English
					return "From Tool Insert";
				case ExcelColumnNumber.FromToolInsert2:
					if(language == "DE")
						return "Form 2";
					if(language == "TN")
						return "Outil 1";
					if(language == "CZ")
						return "Formu 2";
					if(language == "AL")
						return "Forma/Vegla/Inserti 2";
					// English
					return "From Tool Insert2";
				case ExcelColumnNumber.StandardOperationId:
					if(language == "DE")
						return "Standardoperation";
					if(language == "TN")
						return "Opération Standard";
					if(language == "CZ")
						return "Standardní operace";
					if(language == "AL")
						return "Operacioni standart";
					// English
					return "Standard Operation";
				case ExcelColumnNumber.OperationDescriptionId:
					if(language == "DE")
						return "Operationsbeschreibung";
					if(language == "TN")
						return "Description Opération";
					if(language == "CZ")
						return "Popis operace";
					if(language == "AL")
						return "Pershkrimi I operacionit";
					// English
					return "Standard Operation Description";
				case ExcelColumnNumber.Comment:
					if(language == "DE")
						return "Kommentar";
					if(language == "TN")
						return "Commentaire";
					if(language == "CZ")
						return "Komentář";
					if(language == "AL")
						return "Koment";
					// English
					return "Comment";
				case ExcelColumnNumber.OperationValueAdding:
					if(language == "DE")
						return "Operation mit Mehrwert";
					if(language == "TN")
						return "Opération à valeur ajoutée";
					if(language == "CZ")
						return "Přidání hodnoty operace";
					if(language == "AL")
						return "Vlera e shtuar e operacionit";
					// English
					return "Operation Value Adding";
				case ExcelColumnNumber.Amount:
					if(language == "DE")
						return "Menge";
					if(language == "TN")
						return "Quantité";
					if(language == "CZ")
						return "Množství";
					if(language == "AL")
						return "Sasia";
					// English
					return "Amount";
				case ExcelColumnNumber.OperationTimeSeconds:
					if(language == "DE")
						return "Operationszeit (Sekunden)";
					if(language == "TN")
						return "Temps Opération (Seconde)";
					if(language == "CZ")
						return "Čas operace (sekundy)";
					if(language == "AL")
						return "Koha e operacionit (ne sekonda)";
					// English
					return "Operation time Seconds";
				case ExcelColumnNumber.RelationOperationTime:
					if(language == "DE")
						return "Beziehung Operationszeit";
					if(language == "TN")
						return "Relation temps Opération";
					if(language == "CZ")
						return "Provozní doba";
					if(language == "AL")
						return "Koha operacionale e nderveprimit";
					// English
					return "Relation Operation Time";
				case ExcelColumnNumber.StandardOccupancy:
					if(language == "DE")
						return "Standartbelegung";
					if(language == "TN")
						return "Occupation Standard";
					if(language == "CZ")
						return "Standardní obsazenost";
					if(language == "AL")
						return "Okupimi standard";
					// English
					return "Standard Occupancy";
				case ExcelColumnNumber.SetupTimeMinutes:
					if(language == "DE")
						return "Setupzeit";
					if(language == "TN")
						return "Temps Réglage";
					if(language == "CZ")
						return "Čas na přípravu";
					if(language == "AL")
						return "Koha e setupit";
					// English
					return "Setup Time Minutes";
				case ExcelColumnNumber.LotSizeSTD:
					if(language == "DE")
						return "Losgrößenstandard";
					if(language == "TN")
						return "Taille lot standard";
					if(language == "CZ")
						return "Standard velikosti šarže";
					if(language == "AL")
						return "Sasia e artikullit";
					// English
					return "Lot size standard";
				case ExcelColumnNumber.OperationTimeValueAdding:
					if(language == "DE")
						return "Operation mit Mehrwert";
					if(language == "TN")
						return "Opération à valeur ajoutée";
					if(language == "CZ")
						return "Přidání hodnoty operace";
					if(language == "AL")
						return "Vlera e shtuar e operacionit";
					// English
					return "Operation time value adding";
				case ExcelColumnNumber.TotalTimeOperation:
					if(language == "DE")
						return "Gesamtbetriebszeit";
					if(language == "TN")
						return "Temps total operation";
					if(language == "CZ")
						return "Celková doba provozu";
					if(language == "AL")
						return "Koha totale e operacionit";
					// English
					return "Total operation time";
				case ExcelColumnNumber.SetupRatio:
					if(language == "DE")
						return "Setup/Zeitverhältnis ohne Wertschöpfung";
					if(language == "TN")
						return "Réglage/Ratio non valeur ajouté";
					if(language == "CZ")
						return "Poměr času/času bez přidání hodnoty";
					if(language == "AL")
						return "Setup/Perqindja e kohes pa vlere te shtuar";
					// English
					return "Setup/No Value adding Time ratio";
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
		public static string GetSelectedLanguage(string language)
		{
			language = language?.ToUpper();
			if(language == "DE")
				return "DE";
			if(language == "FR")
				return "TN";
			if(language == "CS")
				return "CZ";
			if(language == "SQ")
				return "AL";
			// English
			return "EN";
		}
	}
}
