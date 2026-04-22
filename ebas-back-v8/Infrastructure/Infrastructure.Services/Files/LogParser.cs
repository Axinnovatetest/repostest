using System;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure.Services.Files.Parsing
{
	using Hangfire;
	using Infrastructure.Data.Access.Tables.FNC;
	using Infrastructure.Data.Access.Tables.SPR;
	using Infrastructure.Data.Access.Tables.Support.Feedback;
	using Infrastructure.Data.Entities.Tables.COR;
	using Infrastructure.Data.Entities.Tables.SPR;
	using Infrastructure.Data.Entities.Tables.Support.Feedback;
	using System.ComponentModel;
	using System.Data.SqlClient;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using static Infrastructure.Services.Files.Parsing.PerfLogParsingUtilities;

	public enum LogLevels
	{
		[Description("INFO")]
		Info,
		[Description("WARN")]
		Warning,
		[Description("TRACE")]
		Trace,
		[Description("DEBUG")]
		Debug,
		[Description("ERROR")]
		Error,
		[Description("FATAL")]
		Fatal
	}
	public class LogItem
	{
		public string LogDate { get; set; }
		public string Logger { get; set; }
		public string LogLevel { get; set; }
		public int RequestUserId { get; set; }
		public string RequestUserCompany { get; set; }
		public string RequestUserDepartment { get; set; }
		public string RequestUserName { get; set; }
		public string RequestPath { get; set; }
		public string RequestPathModule { get; set; }
		public string RequestPathController { get; set; }
		public string RequestPathMethod { get; set; }
		public string RequestMethod { get; set; }
		public string RequestDetails { get; set; }

		public LogItem(string jsonLine, List<UserEntity> users, List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> companies, List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> departments)
		{
			if(!string.IsNullOrWhiteSpace(jsonLine))
			{
				var x = jsonLine.IndexOf("||");
				try
				{
					LogDate = jsonLine.Substring(0, x)?.Trim().Substring(0, 10);


					// -
					var _x = jsonLine.Substring(x + 2)?.Trim();
					var y = _x?.Split('|');
					//try
					{
						Logger = y?[0]?.Trim();

						//try
						{
							LogLevel = y?[1]?.Trim();

							//try
							{
								RequestUserId = int.TryParse(y?[2]?.Trim(), out var id) ? id : -1;
								var u = users?.Find(z => z.Id == RequestUserId);
								if(u != null)
								{
									var uc = companies?.Find(c => c.Id == (u.CompanyId ?? -1));
									var ud = departments?.Find(d => d.Id == (u.DepartmentId ?? -1));

									RequestUserCompany = $"{uc?.Name}";
									RequestUserDepartment = $"{ud?.Name}";
									RequestUserName = $"{u.Name} [{u.Username}]";
								}
								//try
								{
									RequestPath = y?[3]?.Trim()?.Replace("api", "")?.Replace('/', '|')?.Trim('|');
									//try
									{
										var r = RequestPath?.Split('|');
										RequestPathModule = r?[0];
										RequestPathController = r?[1];
										RequestPathMethod = r?[2];
									}
									//catch { }
									//try
									{
										RequestMethod = y?[4]?.Trim();
										// try 
										{ RequestDetails = y?[5]?.Trim(); }
										//catch (Exception) { }
									}
									//catch (Exception) { }
								}
								//catch (Exception) { }
							}
							//catch (Exception) { }
						}
						//catch (Exception) { }
					}
					//catch (Exception) { }


				} catch(Exception) { }
			}
		}

	}
	public interface ILogParser
	{
		Task<List<KeyValuePair<string, int>>> SaveFeedbacksLogsToDBAsync(List<int> idsToExcludes);
		List<string> GetJsonLines();
		List<LogItem> GetLines(bool excludeIds);
		List<LogItem> GetLines(LogLevels filterLogLevel, bool excludeIds);
		byte[] GetExcelData(bool excludeIds);
		void GetExcelDataAsync(bool excludeIds, string forwardEmail = "");
	}
	public class LogParser: ILogParser
	{
		public string LogDateTimeRange { get; set; }
		public string FolderPath { get; set; }
		public string FilePattern { get; set; } = "nlog-all*.log";
		public List<int> ExcludedIds { get; set; }
		public Email.EmailParamtersModel EmailParams { get; set; }
		public LogParser(string folderPath, string logDateTimeRange,List<int> excludedIds = null, Email.EmailParamtersModel emailParams = null)
		{
			FolderPath = folderPath;
			ExcludedIds = excludedIds;
			EmailParams = emailParams;
			LogDateTimeRange = logDateTimeRange;
		}	
		List<string> getLines()
		{
			Infrastructure.Services.Logging.Logger.LogDebug($"start getLines");
			if(string.IsNullOrWhiteSpace(FolderPath))
				return null;

			var fileNames = Directory.GetFiles(FolderPath, FilePattern, SearchOption.AllDirectories);
			if(fileNames == null || fileNames.Length <= 0)
				return null;
			Infrastructure.Services.Logging.Logger.LogDebug($"getLines: filenames - {fileNames.Count()}");
			List<string> results = new List<string>();
			var limitDate = $"nlog-all-{DateTime.Today.AddDays(-9).ToString("yyyy-MM-dd")}.log";
			var _fileNames = fileNames.OrderByDescending(x => x.Substring(x.Length - 23)).ToList();
			int i = 0;
			Infrastructure.Services.Logging.Logger.LogDebug($"getLines: filenames (filter) - {_fileNames.Count()}");
			for(; i < _fileNames.Count; i++)
			{
				var ending = _fileNames[i].Substring(_fileNames[i].Length - 23);
				//if (ending.CompareTo(limitDate) <= 0)
				if(String.Compare(ending, limitDate, comparisonType: StringComparison.OrdinalIgnoreCase) <= 0)
				{
					break;
				}
			}
			fileNames = _fileNames.Take(i).ToArray();
			Infrastructure.Services.Logging.Logger.LogDebug($"getLines: filenames (final) - {_fileNames.Count()}");
			foreach(var filePath in fileNames)
			{
				results.AddRange(File.ReadLines(filePath));
			}
			Infrastructure.Services.Logging.Logger.LogDebug($"getLines: lines - {results.Count()}");

			return results;
		}

		public List<string> GetJsonLines()
		{
			return getLines();
		}
		public List<LogItem> GetLines(LogLevels filterLogLevel, bool excludeIds)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.LogDebug($"start GetLines");
				var _filterLogLevel = filterLogLevel.GetDescription();
				var lines = getLines()?.Where(x => x.IndexOf(_filterLogLevel) >= 0);
				Infrastructure.Services.Logging.Logger.LogDebug($"GetLines: lines - {lines.Count()}");
				if(excludeIds && ExcludedIds != null && ExcludedIds.Count > 0)
				{
					var _excludedIds = ExcludedIds.Select(x => x.ToString()).ToArray();
					lines = lines?.Where(x => !x.ContainsAny($"|{_excludedIds}|"));
				}
				//var xxx = lines.Count();
				Infrastructure.Services.Logging.Logger.LogDebug($"GetLines: lines (exc) - {lines.Count()}");
				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get();
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get();
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
				Infrastructure.Services.Logging.Logger.LogDebug($"end GetLines");
				return lines.Take(750000)
						?.Select(x => new LogItem(x, userEntities, companyEntities, departmentEntities))
						?.Where(x => !string.IsNullOrWhiteSpace(x.LogLevel) && x.LogLevel == _filterLogLevel)
						?.ToList();

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.LogTrace(e.StackTrace);
				throw;
			}
		}
		public List<LogItem> GetLines(bool excludeIds)
		{
			var lines = GetLines(LogLevels.Fatal, excludeIds);
			return lines;
		}


		#region >>>> BULK <<<<<<
		public byte[] GetExcelData(bool excludeIds)
		{
			List<LogItem> dataEntities = GetLines(LogLevels.Fatal, excludeIds);
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"data-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new System.IO.FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new OfficeOpenXml.ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"data");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 8;

					// Add some formatting to the worksheet
					worksheet.TabColor = System.Drawing.Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"Data";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "User";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Department";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Company";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Module";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Controller";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Method";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Full Path";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.LogDate;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.RequestUserName;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.RequestUserDepartment;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.RequestUserCompany;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.RequestPathModule;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.RequestPathController;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.RequestPathMethod;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.RequestPath;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Doc content
					if(dataEntities != null && dataEntities.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
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

					// Set some document properties
					package.Workbook.Properties.Title = $"Date";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

					return System.IO.File.ReadAllBytes(filePath);
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public async void GetExcelDataAsync(bool excludeIds, string forwardEmail = "")
		{
			List<LogItem> dataEntities = new List<LogItem>();
			await Task.Factory.StartNew(() =>
			{
				dataEntities = GetLines(LogLevels.Fatal, excludeIds);
			});
			Infrastructure.Services.Logging.Logger.LogDebug($"dataEntities: {dataEntities.Count} items");
			if(dataEntities != null && dataEntities.Count > 900000)
			{
				dataEntities = dataEntities.Take(900000).ToList();
			}
			try
			{
				Infrastructure.Services.Logging.Logger.LogDebug($"dataEntities 2: {dataEntities.Count} items");

				using(var stream = new MemoryStream())
				{
					// FIXME: Replace EPPlus by NPOI, or some other alt
					OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
					// Create the package and make sure you wrap it in a using statement
					using(var package = new OfficeOpenXml.ExcelPackage())
					{
						// add a new worksheet to the empty workbook
						OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"data");

						// Keep track of the row that we're on, but start with four to skip the header
						var headerRowNumber = 2;
						var startColumnNumber = 1;
						var numberOfColumns = 8;

						// Add some formatting to the worksheet
						worksheet.TabColor = System.Drawing.Color.Yellow;
						worksheet.DefaultRowHeight = 11;
						worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
						worksheet.Row(2).Height = 20;
						worksheet.Row(1).Height = 30;
						worksheet.Row(headerRowNumber).Height = 20;

						// Pre Header
						worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
						worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						worksheet.Cells[1, 1].Value = $"Data";
						worksheet.Cells[1, 1].Style.Font.Size = 16;

						// Start adding the header
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Date";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "User";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Department";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Company";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Module";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Controller";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Method";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Full Path";

						Infrastructure.Services.Logging.Logger.LogDebug($"dataEntities 3: {dataEntities.Count} items");
						var rowNumber = headerRowNumber + 1;
						if(dataEntities != null && dataEntities.Count > 0)
						{
							Infrastructure.Services.Logging.Logger.LogDebug($"dataEntities 4: {dataEntities.Count} items");
							// Loop through 
							foreach(var w in dataEntities)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.LogDate;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.RequestUserName;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.RequestUserDepartment;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.RequestUserCompany;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.RequestPathModule;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.RequestPathController;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.RequestPathMethod;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.RequestPath;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}

						//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
						using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
						{
							range.Style.Font.Bold = true;
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
							range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
							range.Style.ShrinkToFit = false;
						}
						// Darker Blue in Top cell
						worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

						// Doc content
						if(dataEntities != null && dataEntities.Count > 0)
						{
							using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
							{
								range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
								range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							}
							// Thick countour
							using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
							{
								range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
							}
						}


						// Fit the columns according to its content
						for(int i = 1; i <= numberOfColumns; i++)
						{
							worksheet.Column(i).AutoFit();
						}

						// Set some document properties
						package.Workbook.Properties.Title = $"Date";
						package.Workbook.Properties.Author = "PSZ ERP";
						package.Workbook.Properties.Company = "PSZ ERP";
						// -
						var emailSender = new Email.MailKit();
						emailSender.InitiateEmailSender(EmailParams);
						await emailSender.SendEmailAsync("[EBAS] APIs log", "Hello, this is the EBAS APIs log data you requested.", new List<string> { forwardEmail }, new List<KeyValuePair<string, Stream>> { new KeyValuePair<string, Stream>("API-logs-data.xlsx", new MemoryStream(package.GetAsByteArray())) });
					}
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}

		private static string[] GetLogsFilesPath(string folderPath, List<string> fileNames)
		{
			Logging.Logger.LogDebug($"start getLines");
			if(string.IsNullOrWhiteSpace(folderPath) || fileNames == null || fileNames.Count == 0)
				return null;
			try
			{
				var allFiles = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
				var matchedFiles = allFiles.Where(file => fileNames.Contains(Path.GetFileName(file))).ToArray();

				return matchedFiles.Length > 0 ? matchedFiles : null;
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.LogError($"Error while searching files: {ex.Message}");
				return null;
			}
		}

		public List<KeyValuePair<string, int>> SaveFeedbacksLogsToDB(List<int> idsToExcludes)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			List<KeyValuePair<string, int>> insertedValuesList = new List<KeyValuePair<string, int>>();
			try
			{
				botransaction.beginTransaction();

				var feedbackLogs = new List<FeedbackLogModel>();

				var lastLogsCaptureDate = ERP_LogsAccess.GetLastLogDate(botransaction.connection, botransaction.transaction).FirstOrDefault();

				List<DateTime> datesTimeRange = new List<DateTime>();


				if(lastLogsCaptureDate != null)
				{

					datesTimeRange = Reporting.DateHelpers.getDatesRange((DateTime)lastLogsCaptureDate.LogCaptureDate, DateTime.Today).ToList();
				}
				else
				{
					datesTimeRange.Add(DateTime.Today);
				}


				List<string> filenames = new List<string>();
				foreach(DateTime date in datesTimeRange)
				{
					filenames.Add($"nlog-all-{date.ToString("yyyy-MM-dd")}.log");
				}

				string[] allLogsFilesNames = Array.Empty<string>();

				allLogsFilesNames = GetLogsFilesPath(FolderPath, filenames);

				var allUsers = UserAccess.GetWithTransaction(botransaction.connection, botransaction.transaction).ToList();

				if(allLogsFilesNames == null || allLogsFilesNames.Length == 0)
				{
					throw new InvalidOperationException("No log file to process");
				}
				List<DateTime> datesList = new List<DateTime>();
				foreach(var item in allLogsFilesNames)
				{
					DateTime logDate = ExtractDateFromFilename(item);
					datesList.Add(logDate);
				}

				//Get existing logs by log capture date
				var allLogsByCaptureDate = ERP_LogsAccess.DeleteAllByLogCaptureDate(datesList, botransaction.connection, botransaction.transaction);
				// -
				insertedValuesList = ParseAndInsertFeedbacksLogsToDB(allLogsFilesNames, idsToExcludes, allUsers, botransaction.connection, botransaction.transaction);

				//TODO: handle transaction state (success or failure)

				Logging.Logger.Log("start commiting changes in transaction");
				if(botransaction.commit())
				{
					return insertedValuesList;
				}
				else
				{
					return new List<KeyValuePair<string, int>>();
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				//Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public async Task<List<KeyValuePair<string, int>>> SaveFeedbacksLogsToDBAsync(List<int> idsToExclude)
		{
			return await Task.Run(() =>
			{
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				var insertedValuesList = new List<KeyValuePair<string, int>>();

				try
				{
					botransaction.beginTransaction();

					int range = Convert.ToInt32(LogDateTimeRange);
					var today = DateTime.Today;
					var startDate = today.AddDays(-range);

					var datesTimeRange = Reporting.DateHelpers.getDatesRange(startDate, today);
					var filenames = datesTimeRange.Select(date => $"nlog-all-{date:yyyy-MM-dd}.log").ToList();

					var allLogsFilesNames = GetLogsFilesPathRecursive(FolderPath, filenames);

					if(allLogsFilesNames.Length == 0)
					{
						throw new InvalidOperationException("No log file to process");
					}

					var datesList = allLogsFilesNames.Select(ExtractDateFromFilename).ToHashSet();

					ERP_LogsAccess.DeleteAllByLogCaptureDate(
						datesList.ToList(),
						botransaction.connection,
						botransaction.transaction
					);

					var allUsers = UserAccess.GetWithTransaction(
						botransaction.connection,
						botransaction.transaction
					).ToList();

					insertedValuesList = ParseAndInsertFeedbacksLogsToDB(
						allLogsFilesNames,
						idsToExclude,
						allUsers,
						botransaction.connection,
						botransaction.transaction
					);

					Logging.Logger.Log("Start committing changes in transaction");

					return botransaction.commit()
						? insertedValuesList
						: new List<KeyValuePair<string, int>>();
				} catch(Exception e)
				{
					botransaction.rollback();
					Logging.Logger.Log(e);
					throw;
				}
			});
		}


		public static string[] GetLogsFilesPathRecursive(string rootFolder, List<string> filenames)
		{
			var matchingFiles = new List<string>();

			var subfolders = Directory.GetDirectories(rootFolder);

			foreach(var subfolder in subfolders)
			{
				var logsFolder = Path.Combine(subfolder, "Logs");
				if(Directory.Exists(logsFolder))
				{
					var logFiles = Directory.GetFiles(logsFolder, "*.log", SearchOption.AllDirectories);

					var filtered = logFiles
						.Where(f => filenames.Any(fn =>
							string.Equals(Path.GetFileName(f), fn, StringComparison.OrdinalIgnoreCase)))
						.ToList();

					matchingFiles.AddRange(filtered);
				}
			}

			return matchingFiles.ToArray();
		}

		#endregion
	}

	// perf Log parsing

	public class LogItemPerf
	{
		public int DayPart { get; set; }
		public DateTime LogDate { get; set; }
		public int RequestUserId { get; set; }
		public string RequestPathModule { get; set; }
		public string RequestPathController { get; set; }
		public string RequestPathMethod { get; set; }
		public string RequestMethod { get; set; }
		public LogItemPerf() { }
		public LogItemPerf(List<string> extractedData)
		{
			if(extractedData[3] == "GetUserData")
			{
				LogDate = ConvertToDateTime(extractedData[0]);
				RequestUserId = int.Parse(extractedData[1]);
				RequestPathController = extractedData[2];
				RequestPathMethod = extractedData[3];
				RequestPathModule = extractedData[4];
				RequestMethod = extractedData[5];
			}
			else
			{

				LogDate = ConvertToDateTime(extractedData[0]);
				RequestUserId = int.Parse(extractedData[1]);
				RequestPathModule = extractedData[2];
				RequestPathController = extractedData[3];
				RequestPathMethod = extractedData[4];
				RequestMethod = extractedData[5];
			}

			CalculateDayPart(LogDate);
		}
		private DateTime ConvertToDateTime(string dateTimeString) => DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture);
		private void CalculateDayPart(DateTime dateTime) => DayPart = (dateTime.Hour / 2) + 1;
	}
	public class PerfLogs
	{
		public int RequestUserId { get; set; }
		public string RequestPathModule { get; set; }
		public string RequestPathController { get; set; }
		public string RequestPathMethod { get; set; }
		public string RequestMethod { get; set; }
		public int TotalCallCount { get; set; }
		public int TotalDistinctCallUserCount { get; set; }
		public DateTime FirstCallTime { get; set; }
		public DateTime LastCallTime { get; set; }
		public int DayPart1Count { get; set; }
		public int DayPart2Count { get; set; }
		public int DayPart3Count { get; set; }
		public int DayPart4Count { get; set; }
		public int DayPart5Count { get; set; }
		public int DayPart6Count { get; set; }
		public int DayPart7Count { get; set; }
		public int DayPart8Count { get; set; }
		public int DayPart9Count { get; set; }
		public int DayPart10Count { get; set; }
		public int DayPart11Count { get; set; }
		public int DayPart12Count { get; set; }
	}
	public class PerfLogParsingUtilities
	{
		public static List<LogItemPerf> LogsItemsToSavePerThread = new List<LogItemPerf>();
		public static string FolderPath { get; set; }
		public static string LogExtractionpattern = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}\.\d{4}\|\|Infrastructure\.Services\.Logging\.Logger\|FATAL\|[-]?\d+\|/api/[\w/]+?\|\w+\|";
		public static string fatalLogPattern = @"^(?<LogCaptureDate>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4})\|\|.*\|FATAL\|(?<UserId>-?\d+)?\|/api/(?<Module>[^\/]+)/[^\/]+/(?<EndpointName>[^\/\|]+)\|(?<HttpMethod>[A-Z]+)?\|.*?(?<Message>.+)";
		public static string errorPattern1 = @"(?<=FATAL.*\n)(?<LogCaptureDate>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4})\|\|.*\|ERROR\|(?<ErrorLog>.*(?:\n(?!.*\|(FATAL|TRACE)\|).*)*)(?=\n.*\|TRACE\|)";
		public static string errorPattern2 = @"^(?<LogCaptureDate>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4})\|\|.*\|ERROR\|Exception: (?<Message>.+)$";
		public static string tracePattern = @"(?<LogCaptureDate>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4})\|\|Infrastructure\.Services\.Logging\.Logger\|TRACE\|(?<Message>[\s\S]*?)(?=\n\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4}|\Z)";


		[DisableConcurrentExecution(180)]
		public static void SavePerfLogsToDb(string logFolderPath, List<int> idsToExcludes)
		{
			string FileName = $"nlog-all-{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}.log";
			var Logs = new List<PerfLogs>();

			if(!Directory.Exists(logFolderPath))
				return; //throw new InvalidOperationException("No log file to process");

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

			try
			{
				var allLogsFilesNames = GetFilesToScrape(logFolderPath, FileName);

				if(allLogsFilesNames == null || allLogsFilesNames.Length == 0)
					return; 
				//throw new InvalidOperationException("No log file to process");

				foreach(var item in allLogsFilesNames)
				{
					Logs.AddRange(ParseLogsToModel(item, idsToExcludes));
				}

				var DataToSave = MapsModelToLogEntity(Logs);

				botransaction.beginTransaction();

				if(DataToSave is null || DataToSave.Count == 0)
				{
					throw new InvalidOperationException("The Log File to Process is empty or the application is unable to process the Logs  ");
				}

				var result = ApiCallsAccess.InsertWithTransaction(DataToSave, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{

				}
				else
				{

				}

			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
			}
		}

		#region >>>> Perf Log Parsing Methods implementation <<<<<<
		/*private static  List<PerfLogs> ProcessLogItems(List<LogItemPerf> logItems)
		{
			if(logItems == null || !logItems.Any())
				return new List<PerfLogs>();

			var groupedLogs = logItems
				.GroupBy(li => new
				{
					li.RequestPathModule,
					li.RequestPathController,
					li.RequestPathMethod,
					li.RequestUserId,
					li.RequestMethod
				});

			var perfLogsList = new List<PerfLogs>();

			foreach(var group in groupedLogs)
			{
				var perfLogs = new PerfLogs
				{
					RequestPathModule = group.Key.RequestPathModule,
					RequestPathController = group.Key.RequestPathController,
					RequestPathMethod = group.Key.RequestPathMethod,
					RequestMethod = group.Key.RequestMethod,
					TotalCallCount = group.Count(),
					TotalDistinctCallUserCount = group.Select(li => li.RequestUserId).Distinct().Count(),
					FirstCallTime = group.Min(li => li.LogDate),
					LastCallTime = group.Max(li => li.LogDate)
				};

				var dayPartGroups = group.GroupBy(li => li.DayPart)
										.ToDictionary(g => g.Key, g => g.Count());

				perfLogs.DayPart1Count = dayPartGroups.ContainsKey(1) ? dayPartGroups[1] : 0;
				perfLogs.DayPart2Count = dayPartGroups.ContainsKey(2) ? dayPartGroups[2] : 0;
				perfLogs.DayPart3Count = dayPartGroups.ContainsKey(3) ? dayPartGroups[3] : 0;
				perfLogs.DayPart4Count = dayPartGroups.ContainsKey(4) ? dayPartGroups[4] : 0;
				perfLogs.DayPart5Count = dayPartGroups.ContainsKey(5) ? dayPartGroups[5] : 0;
				perfLogs.DayPart6Count = dayPartGroups.ContainsKey(6) ? dayPartGroups[6] : 0;
				perfLogs.DayPart7Count = dayPartGroups.ContainsKey(7) ? dayPartGroups[7] : 0;
				perfLogs.DayPart8Count = dayPartGroups.ContainsKey(8) ? dayPartGroups[8] : 0;
				perfLogs.DayPart9Count = dayPartGroups.ContainsKey(9) ? dayPartGroups[9] : 0;
				perfLogs.DayPart10Count = dayPartGroups.ContainsKey(10) ? dayPartGroups[10] : 0;
				perfLogs.DayPart11Count = dayPartGroups.ContainsKey(11) ? dayPartGroups[11] : 0;
				perfLogs.DayPart12Count = dayPartGroups.ContainsKey(12) ? dayPartGroups[12] : 0;
				
				perfLogsList.Add(perfLogs);
			}

			return perfLogsList;
		}*/

		private static List<PerfLogs> ProcessLogItems(List<LogItemPerf> logItems)
		{
			if(logItems == null || !logItems.Any())
				return new List<PerfLogs>();

			// Group by RequestPathModule, RequestPathController, RequestPathMethod, RequestMethod, and RequestUserId
			var groupedLogs = logItems
				.GroupBy(li => new
				{
					li.RequestPathModule,
					li.RequestPathController,
					li.RequestPathMethod,
					li.RequestMethod,
					li.RequestUserId  // Adding RequestUserId to group by the user
				});

			var perfLogsList = new List<PerfLogs>();

			foreach(var group in groupedLogs)
			{
				var perfLogs = new PerfLogs
				{
					RequestPathModule = group.Key.RequestPathModule,
					RequestPathController = group.Key.RequestPathController,
					RequestPathMethod = group.Key.RequestPathMethod,
					RequestMethod = group.Key.RequestMethod,
					TotalCallCount = group.Count(),
					RequestUserId = group.Key.RequestUserId,

					// Since you group by RequestUserId, every group corresponds to a single user
					TotalDistinctCallUserCount = group.Select(li => li.RequestUserId).Distinct().Count(),

					FirstCallTime = group.Min(li => li.LogDate),
					LastCallTime = group.Max(li => li.LogDate)
				};

				// Group by DayPart and count each part
				var dayPartGroups = group.GroupBy(li => li.DayPart)
										.ToDictionary(g => g.Key, g => g.Count());

				perfLogs.DayPart1Count = dayPartGroups.ContainsKey(1) ? dayPartGroups[1] : 0;
				perfLogs.DayPart2Count = dayPartGroups.ContainsKey(2) ? dayPartGroups[2] : 0;
				perfLogs.DayPart3Count = dayPartGroups.ContainsKey(3) ? dayPartGroups[3] : 0;
				perfLogs.DayPart4Count = dayPartGroups.ContainsKey(4) ? dayPartGroups[4] : 0;
				perfLogs.DayPart5Count = dayPartGroups.ContainsKey(5) ? dayPartGroups[5] : 0;
				perfLogs.DayPart6Count = dayPartGroups.ContainsKey(6) ? dayPartGroups[6] : 0;
				perfLogs.DayPart7Count = dayPartGroups.ContainsKey(7) ? dayPartGroups[7] : 0;
				perfLogs.DayPart8Count = dayPartGroups.ContainsKey(8) ? dayPartGroups[8] : 0;
				perfLogs.DayPart9Count = dayPartGroups.ContainsKey(9) ? dayPartGroups[9] : 0;
				perfLogs.DayPart10Count = dayPartGroups.ContainsKey(10) ? dayPartGroups[10] : 0;
				perfLogs.DayPart11Count = dayPartGroups.ContainsKey(11) ? dayPartGroups[11] : 0;
				perfLogs.DayPart12Count = dayPartGroups.ContainsKey(12) ? dayPartGroups[12] : 0;

				perfLogsList.Add(perfLogs);
			}

			return perfLogsList;
		}

		public static DateTime ConvertToDateTime(string dateTimeString) => DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture);
		public static List<string> ExtractDataFromString(string logLine)
		{
			List<string> extractedData = new List<string>();

			var parts = logLine.Split('|');

			try
			{

				extractedData.Add(parts[0].Trim());

				if(parts.Length > 4)
				{
					extractedData.Add(parts[4].Trim());
				}
				else
				{
					extractedData.Add("0");
				}


				if(parts.Length > 5)
				{
					var apiPathParts = parts[5].Split('/');
					extractedData.Add(apiPathParts.Length > 2 ? apiPathParts[2] : "NoModule");
					extractedData.Add(apiPathParts.Length > 3 ? apiPathParts[3] : "NoModule");
					extractedData.Add(apiPathParts.Length > 4 ? apiPathParts[4] : "NoModule");
				}
				else
				{
					extractedData.Add("NoModule");
					extractedData.Add("NoController");
					extractedData.Add("NoMethod");
				}


				if(parts.Length > 6)
				{
					extractedData.Add(parts[6].Trim());
				}
				else
				{
					extractedData.Add("UnknownHTTPMethod");
				}
			} catch(Exception ex)
			{

				Console.WriteLine("Error parsing log line: " + ex.Message);
			}

			return extractedData;
		}
		/// <summary>
		/// return files names as string array from a given directory based on a file name pattern
		/// </summary>
		/// <param name="FolderPath"></param>
		/// <param name="FilePattern"></param>
		/// <returns></returns>
		private static string[] GetFilesToScrape(string FolderPath, string FilePattern)
		{
			Infrastructure.Services.Logging.Logger.LogDebug($"start getLines");
			if(string.IsNullOrWhiteSpace(FolderPath))
				return null;

			var fileNames = Directory.GetFiles(FolderPath, FilePattern, SearchOption.AllDirectories);
			if(fileNames == null || fileNames.Length <= 0)
				return null;
			return fileNames;
		}
		public static List<FeedbackLogModel> GetMatchedData(string filePath, List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> allUsers)
		{
			List<FeedbackLogModel> parsedEntries = new List<FeedbackLogModel>();
			using(FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using(StreamReader reader = new StreamReader(fileStream))
			{
				string line;
				FeedbackLogModel currentTraceLog = null;
				StringBuilder traceMessageBuilder = new StringBuilder();

				while((line = reader.ReadLine()) != null)
				{
					Match fatalMatch = Regex.Match(line, fatalLogPattern);
					Match errorMatch1 = Regex.Match(line, errorPattern1, RegexOptions.Multiline);
					Match errorMatch2 = Regex.Match(line, errorPattern2);
					Match traceMatch = Regex.Match(line, tracePattern);

					if(fatalMatch.Success)
					{
						var userId = fatalMatch.Groups["UserId"].Success ? int.Parse(fatalMatch.Groups["UserId"].Value) : (int?)null;
						var userName = userId.HasValue ? allUsers.Find(el => el.Id == userId.Value)?.Username : null;

						var logEntry = new FeedbackLogModel
						{
							LogCaptureDate = DateTime.Parse(fatalMatch.Groups["LogCaptureDate"].Value),
							Level = "FATAL",
							UserId = userId,
							UserName = userName,
							EndpointName = fatalMatch.Groups["EndpointName"].Value,
							EndpointMethod = fatalMatch.Groups["HttpMethod"].Success ? fatalMatch.Groups["HttpMethod"].Value : null,
							Message = fatalMatch.Groups["Message"].Value.Trim(),
							Module = fatalMatch.Groups["Module"].Value.Trim()
						};
						parsedEntries.Add(logEntry);
					}
					if(errorMatch1.Success)
					{
						var logEntry = new FeedbackLogModel
						{
							LogCaptureDate = DateTime.Parse(errorMatch1.Groups["LogCaptureDate"].Value),
							Level = "ERROR",
							Message = errorMatch1.Groups["ErrorLog"].Value.Trim()
						};
						parsedEntries.Add(logEntry);
					}
					if(errorMatch2.Success)
					{
						var logEntry = new FeedbackLogModel
						{
							LogCaptureDate = DateTime.Parse(errorMatch2.Groups["LogCaptureDate"].Value),
							Level = "ERROR",
							Message = errorMatch2.Groups["Message"].Value.Trim()
						};
						parsedEntries.Add(logEntry);
					}

					if(traceMatch.Success)
					{
						if(currentTraceLog != null)
						{
							currentTraceLog.Message = traceMessageBuilder.ToString().Trim();
							parsedEntries.Add(currentTraceLog);
						}

						currentTraceLog = new FeedbackLogModel
						{
							LogCaptureDate = DateTime.Parse(traceMatch.Groups["LogCaptureDate"].Value),
							Level = "TRACE"
						};

						traceMessageBuilder.Clear();
						traceMessageBuilder.AppendLine(traceMatch.Groups["Message"].Value);
					}
					else if(currentTraceLog != null && !Regex.IsMatch(line, @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4}"))
					{
						traceMessageBuilder.AppendLine(line);
					}
				}

				if(currentTraceLog != null)
				{
					currentTraceLog.Message = traceMessageBuilder.ToString().Trim();
					parsedEntries.Add(currentTraceLog);
				}
			}
			return parsedEntries;
		}
		#endregion
		public class FeedbackLogModel
		{
			public int Id { get; set; }
			public DateTime LogCaptureDate { get; set; }
			public string Level { get; set; }
			public string? EndpointName { get; set; }
			public string Message { get; set; }
			public string? EndpointMethod { get; set; }
			public int? UserId { get; set; }
			public string? Module { get; set; }
			public string? UserName { get; set; }
			public bool? Treated { get; set; }

			public FeedbackLogModel()
			{

			}
			public FeedbackLogModel(ERP_LogsEntity logsEntity)
			{
				if(logsEntity == null)
					return;
				Id = logsEntity.Id;
				LogCaptureDate = (DateTime)logsEntity.LogCaptureDate;
				Level = logsEntity?.LogLevel;
				EndpointName = logsEntity?.EndpointName;
				Message = logsEntity?.LogMessage;
				EndpointMethod = logsEntity?.EndpointMethod;
				UserId = logsEntity.UserId;
				Module = logsEntity?.Module;
				UserName = logsEntity?.UserName;
				Treated = logsEntity?.Treated;
			}
			public FeedbackLogModel(Infrastructure.Data.Entities.Tables.NLogs.ERP_Nlog_ExceptionsEntity logsEntity)
			{
				if(logsEntity == null)
					return;
				Id = logsEntity.Id;
				LogCaptureDate = (DateTime)logsEntity.Date;
				Level = logsEntity?.Level;
				EndpointName = logsEntity?.MemberName;
				Message = logsEntity?.Message;
				EndpointMethod = logsEntity?.SourceFilePath;
				UserId = logsEntity.EventId;
				//Module = logsEntity?.Module;
				//UserName = logsEntity?.UserName;
				//Treated = logsEntity?.Treated;
			}
		}
		public static DateTime ExtractDateFromFilename(string filePath)
		{
			try
			{
				string fileName = Path.GetFileNameWithoutExtension(filePath);
				string datePart = fileName.Replace("nlog-all-", "");
				return DateTime.ParseExact(datePart, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			} catch
			{
				return DateTime.MinValue;
			}
		}
		public static List<ApiCallsEntity> MapsModelToLogEntity(List<PerfLogs> data)
		{
			return data.Select(x => new Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity()
			{
				ApiArea = x.RequestPathModule,
				ApiController = x.RequestPathController,
				ApiMethod = x.RequestPathMethod,
				FirstCallTime = x.FirstCallTime,
				LastCallTime = x.LastCallTime,
				ProcessingTime = DateTime.Now,
				TotalCall02HCount = x.DayPart1Count,
				TotalCall04HCount = x.DayPart2Count,
				TotalCall06HCount = x.DayPart3Count,
				TotalCall08HCount = x.DayPart4Count,
				TotalCall10HCount = x.DayPart5Count,
				TotalCall12HCount = x.DayPart6Count,
				TotalCall14HCount = x.DayPart7Count,
				TotalCall16HCount = x.DayPart8Count,
				TotalCall18HCount = x.DayPart9Count,
				TotalCall20HCount = x.DayPart10Count,
				TotalCall22HCount = x.DayPart11Count,
				TotalCall24HCount = x.DayPart12Count,
				TotalCallCount = x.TotalCallCount,
				UserId = x.RequestUserId,
				TotalCallDistinctUserCount = x.TotalDistinctCallUserCount
			}).ToList();
		}

		public static List<ERP_LogsEntity> MapFeedbackModelToEntity(List<FeedbackLogModel> data)
		{
			return data.Select(x => new ERP_LogsEntity()
			{
				LogCaptureDate = x.LogCaptureDate,
				LogLevel = x.Level,
				LogMessage = x.Message,
				UserId = x.UserId,
				EndpointMethod = x.EndpointMethod,
				EndpointName = x.EndpointName,
				Module = x.Module,
				UserName = x.UserName
			}).ToList();
		}

		public static List<LogItemPerf> ParseLogItemsSimplified(List<string> logLines, List<int> IdExcludes)
		{
			List<LogItemPerf> logItems = new List<LogItemPerf>();


			foreach(var line in logLines)
			{
				List<string> extractedData = ExtractDataFromString(line);

				if(extractedData.Count == 6)
				{
					if(IdExcludes.Contains(int.Parse(extractedData[1])))
						continue;

					LogItemPerf logItem = new LogItemPerf(extractedData);
					logItems.Add(logItem);
				}
			}

			return logItems;

		}
		private static List<string> GetSpecificData(string filePath)
		{
			List<string> result = new List<string>();
			using(FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using(StreamReader reader = new StreamReader(fileStream))
			{
				string line;
				while((line = reader.ReadLine()) != null)
				{
					Match match = Regex.Match(line, LogExtractionpattern);
					if(match.Success)
					{
						result.Add(match.Value);
					}
				}
			}
			return result;
		}
		public static List<PerfLogs> ParseLogsToModel(string FilePath, List<int> IdsToExcludes)
		{
			var parsedData = ParseLogItemsSimplified(GetSpecificData(FilePath), IdsToExcludes);
			return ProcessLogItems(parsedData);
		}

		public static List<KeyValuePair<string, int>> ParseAndInsertFeedbacksLogsToDB(string[] allLogsFilesNames,
			List<int> idsToExcludes,
			List<Data.Entities.Tables.FNC.UserEntity> allUsers,
			SqlConnection boConnection,
			SqlTransaction boTransaction
	)
		{
			List<KeyValuePair<string, int>> insertedValuesList = new List<KeyValuePair<string, int>>();
			try
			{
				var Logs = new List<PerfLogs>();
				var feedbackLogs = new List<FeedbackLogModel>();
				foreach(var item in allLogsFilesNames)
				{
					feedbackLogs.AddRange(GetMatchedData(item, allUsers));
				}
				var feedbackDataToSave = MapFeedbackModelToEntity(feedbackLogs);

				if(feedbackDataToSave is null || feedbackDataToSave.Count == 0)
				{
					throw new InvalidOperationException("The Feedback Log File to Process is empty or the application is unable to process the Logs  ");
				}
				//InsertWithBulkTransaction
				int bulkInsertionResult = ERP_LogsAccess.InsertBulk(feedbackDataToSave, boConnection, boTransaction);

				insertedValuesList.Add(new KeyValuePair<string, int>("Logs insered elements", bulkInsertionResult));
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "||" + ex.Message + "||" + ex.StackTrace);
			}
			return insertedValuesList;
		}
	}
}
