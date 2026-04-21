using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Helpers
{
	public class DelforHelper
	{
		public static List<Psz.Core.CRP.Models.Delfor.DeliveryForcastLineItemModel> ReadDelforFromExcel(string filePath, out List<string> errors, bool commaSeperator, bool checkFrequency)
		{
			errors = new List<string> { };
			var result = new List<Psz.Core.CRP.Models.Delfor.DeliveryForcastLineItemModel>();
			try
			{
				//setting up the excel library
				var fileInfo = new System.IO.FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				var package = new ExcelPackage(fileInfo);
				var worksheet = package.Workbook.Worksheets[0];
				var rowStart = worksheet.Dimension.Start.Row;
				var rowEnd = worksheet.Dimension.End.Row;

				// get number of rows and columns in the sheet
				var rows = worksheet.Dimension.Rows;
				var columns = worksheet.Dimension.Columns;
				var startRowNumber = 1;
				var startColNumber = 1;

				if(rows > 1 && columns > 1)
				{
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						var value = worksheet.Cells[i, startColNumber].getCellValue().Trim();
						if(value == "Position")
						{
							//getting item plans lines to itirate
							var itemPlansCount = 0;
							for(int x = i + 9; x < rowEnd; x++)
							{
								if(worksheet.Cells[x, startColNumber].getCellValue().Trim() != "Position")
									itemPlansCount++;
								else
									break;
							}
							//getting line item values
							var position = worksheet.Cells[i, startColNumber + 1].getCellValue().Trim();
							var material = worksheet.Cells[i + 1, startColNumber + 1].getCellValue().Trim();
							var psz_artikelnummer = worksheet.Cells[i + 2, startColNumber + 1].getCellValue().Trim();
							var Eingeteilte_Menge = worksheet.Cells[i + 3, startColNumber + 1].getCellValue().Trim();
							var Gelieferte_Menge = worksheet.Cells[i + 4, startColNumber + 1].getCellValue().Trim();
							var Letzter_Wareneing = worksheet.Cells[i + 5, startColNumber + 1].getCellValue().Trim();
							var Letzte_Lieferung = worksheet.Cells[i + 6, startColNumber + 1].getCellValue().Trim();
							var Am = worksheet.Cells[i + 7, startColNumber + 1].getCellValue().Trim();
							var Lieferscheinnummer = worksheet.Cells[i + 8, startColNumber + 1].getCellValue().Trim();
							var item = new Psz.Core.CRP.Models.Delfor.DeliveryForcastLineItemModel
							{
								Material = material,
								Position = position,
								PSZ_Artikelnummer = psz_artikelnummer,
								Am = DateTime.TryParse(Am, out var am) ? am : null,
								Eingeteilte_Menge = CommaSeperatorChecker(commaSeperator, Eingeteilte_Menge),
								Gelieferte_Menge = CommaSeperatorChecker(commaSeperator, Gelieferte_Menge),
								Letzter_Wareneing = CommaSeperatorChecker(commaSeperator, Letzter_Wareneing),
								Letzte_Lieferung = CommaSeperatorChecker(commaSeperator, Letzte_Lieferung),
								Lieferscheinnummer = int.TryParse(Lieferscheinnummer, out var lf) ? lf : 0,
								LineItemPlans = new List<Psz.Core.CRP.Models.Delfor.DeliveryForcastLineItemPlanModel> { },
							};
							//iterating through item plans
							var start = i + 10;
							var limit = (start + itemPlansCount) >= rowEnd ? (start + itemPlansCount) : (start + itemPlansCount) - 2;
							for(int j = start; j < limit; j++)
							{
								var frequency = FrequencyChecker(checkFrequency, worksheet.Cells[j, startColNumber + 1].getCellValue().Trim());
								var liefertermin = worksheet.Cells[j, startColNumber + 2].getCellValue().Trim();
								var einteilungs_FZ = CommaSeperatorChecker(commaSeperator, worksheet.Cells[j, startColNumber + 3].getCellValue().Trim());
								var menge = CommaSeperatorChecker(commaSeperator, worksheet.Cells[j, startColNumber + 4].getCellValue().Trim());
								var abw = CommaSeperatorChecker(commaSeperator, worksheet.Cells[j, startColNumber + 5].getCellValue().Trim());
								item.LineItemPlans.Add(new Psz.Core.CRP.Models.Delfor.DeliveryForcastLineItemPlanModel
								{
									Period = frequency,
									Liefertermin = liefertermin,
									Einteilungs_FZ = einteilungs_FZ,
									Menge = menge,
									Abw = abw,
								});
							}
							result.Add(item);
							i = limit;
						}
					}
				}
				else
					errors.Add("Excel empty.");
				return result;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static void ValidateDelforData(List<Psz.Core.CRP.Models.Delfor.DeliveryForcastLineItemModel> data, out List<string> warnings, Identity.Models.UserModel user = null)
		{
			warnings = new List<string>();
			var positions = new List<string>();
			var frequencyes = new List<string> { "m", "d", "w", "t" };
			foreach(var line in data)
			{
				line.Valid = true;
				// validating line items
				if(string.IsNullOrEmpty(line.PSZ_Artikelnummer) || string.IsNullOrWhiteSpace(line.PSZ_Artikelnummer))
				{
					warnings.Add($"Artikelnummer shuold not be empty in position {line.Position}");
					line.Valid = false;
				}
				if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(line.PSZ_Artikelnummer) == null)
				{
					warnings.Add($"Artikelnummer {line.PSZ_Artikelnummer} not found in position {line.Position}");
					line.Valid = false;
				}
				if(line.Eingeteilte_Menge < 0 || !line.Eingeteilte_Menge.HasValue)
				{
					warnings.Add($"Eingeteilte_Menge cannot be negative or empty in position {line.Position}");
					line.Valid = false;
				}
				if(line.Gelieferte_Menge < 0 || !line.Gelieferte_Menge.HasValue)
				{
					warnings.Add($"Gelieferte_Menge cannot be negative or empty in position {line.Position}");
					line.Valid = false;
				}
				if(line.Letzter_Wareneing < 0 || !line.Letzter_Wareneing.HasValue)
				{
					warnings.Add($"Letzter_Wareneing cannot be negative or empty in position {line.Position}");
					line.Valid = false;
				}
				if(line.Letzte_Lieferung < 0 || !line.Letzte_Lieferung.HasValue)
				{
					warnings.Add($"Letzte_Lieferung cannot be negative or empty in position {line.Position}");
					line.Valid = false;
				}
				if(!line.Am.HasValue)
				{
					warnings.Add($"invalid or empty date (Am) in position {line.Position}");
					line.Valid = false;
				}
				if(string.IsNullOrEmpty(line.Position) || string.IsNullOrWhiteSpace(line.Position))
				{
					warnings.Add($"position number should not be null");
					line.Valid = false;
				}
				if(positions.Contains(line.Position))
				{
					warnings.Add($"duplicate position number {line.Position}");
					line.Valid = false;
				}
				else
					positions.Add(line.Position);
				//validationg line item plans
				foreach(var plan in line.LineItemPlans)
				{
					plan.Valid = true;
					DateTime? lfDate = DateTime.TryParseExact(plan.Liefertermin, "dd.MM.yyyy",
				   CultureInfo.InvariantCulture,
				   DateTimeStyles.None, out var f) ? f : null;
					if(!frequencyes.Contains(plan.Period.ToLower()))
					{
						warnings.Add($"Frequency must be W,D,T,M  in position {line.Position}, (or check frequency)");
						plan.Valid = false;
					}
					if(plan.Period.ToLower() == "d" || plan.Period.ToLower() == "t")
					{
						if(!string.IsNullOrEmpty(plan.Liefertermin) && !string.IsNullOrWhiteSpace(plan.Liefertermin) && lfDate == null || (lfDate < new DateTime(1900, 1, 1) || lfDate > new DateTime(9999, 1, 1)))
						{
							warnings.Add($"wrong date format at position {line.Position}");
							plan.Valid = false;
						}
						else
						{
							// - 2023-11-02 - Heidenreich - accept position in the past - for all H
							if(lfDate >= DateTime.Today)
							{
								var horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasDLFPosHorizonRight(lfDate ?? new DateTime(1900, 1, 1), user, out List<string> messages);
								if(!horizonCheck)
								{
									foreach(var msg in messages)
									{
										warnings.Add($"{msg} (Position [{line.Position}])");
									}
									plan.Valid = false;
								}
							}
						}
					}
					if(string.IsNullOrEmpty(plan.Liefertermin) || string.IsNullOrWhiteSpace(plan.Liefertermin))
					{
						warnings.Add($"Liefertermin should not be empty at position {line.Position}");
						plan.Valid = false;
					}

					if(plan.Period.ToLower() == "w" || plan.Period.ToLower() == "m")
					{
						if(!string.IsNullOrEmpty(plan.Liefertermin) && !string.IsNullOrWhiteSpace(plan.Liefertermin))
						{
							var planDates = Helpers.DelforHelper.GetItemPlanDates(plan.Period, plan.Liefertermin);
							var dates = splitDates(plan.Liefertermin);
							if(dates.Key == 0 || dates.Value == 0)
							{
								warnings.Add($"wrong Liefertermin format ({plan.Liefertermin}) at position {line.Position}");
								plan.Valid = false;

							}
							if(plan.Period.ToLower() == "w" && dates.Key > 53)
							{
								warnings.Add($"wrong combination ({plan.Liefertermin}) ,week number must no be bigger then 53 in position {line.Position}");
								plan.Valid = false;
							}
							if(plan.Period.ToLower() == "m" && dates.Key > 12)
							{
								warnings.Add($"wrong combination ({plan.Liefertermin}) ,month number must not be bigger then 12 in position {line.Position}");
								plan.Valid = false;
							}
							if(dates.Value < 1900 || dates.Value > 9999)
							{
								warnings.Add($"wrong combination ({plan.Liefertermin}) , Year number must be between 1900 and 9999 in position {line.Position}");
								plan.Valid = false;
							}
							var horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasDLFPosHorizonRight(planDates.Value ?? new DateTime(1900, 1, 1), user, out List<string> messages);
							if(!horizonCheck)
							{
								foreach(var msg in messages)
								{
									warnings.Add($"{msg} (position {line.Position})");
								}
								plan.Valid = false;
							}
						}
					}
					if(plan.Menge < 0 || !plan.Menge.HasValue)
					{
						warnings.Add($"menge must not be negative or empty in position {line.Position}");
						plan.Valid = false;
					}
					if(plan.Einteilungs_FZ < 0 || !plan.Einteilungs_FZ.HasValue)
					{
						warnings.Add($"Einteilungs_FZ must not be negative or empty in position {line.Position}");
						plan.Valid = false;
					}
				}

			}
		}
		public static KeyValuePair<DateTime?, DateTime?> GetItemPlanDates(string period, string periodValue)
		{
			var result = new KeyValuePair<DateTime?, DateTime?>();
			if(period.ToLower() == "w")
			{
				var splitChar = periodValue.Contains(",") ? ',' : '.';
				var split = periodValue.Split(splitChar);
				var week = int.TryParse(split[0], out var w) ? w : 0;
				var year = int.TryParse(split[1], out var y) ? y : 0;

				var firstDateOfweek = FirstDateOfWeek(year, week, new CultureInfo("de-DE"));
				var lastDateOfweek = firstDateOfweek.AddDays(6);
				result = new KeyValuePair<DateTime?, DateTime?>(firstDateOfweek, lastDateOfweek);
			}
			if(period.ToLower() == "m")
			{
				var splitChar = periodValue.Contains(",") ? ',' : '.';
				var split = periodValue.Split(splitChar);
				var month = int.TryParse(split[0], out var w) ? w : 0;
				var year = int.TryParse(split[1], out var y) ? y : 0;

				var firstDateOfMonth = new DateTime(year, month, 1);
				var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

				result = new KeyValuePair<DateTime?, DateTime?>(firstDateOfMonth, lastDateOfMonth);
			}
			if(period.ToLower() == "d" || period.ToLower() == "t")
				result = new KeyValuePair<DateTime?, DateTime?>(DateTime.TryParse(periodValue, out var dt) ? dt : null, null);
			return result;
		}
		public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
		{
			DateTime jan1 = new DateTime(year, 1, 1);
			int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
			DateTime firstWeekDay = jan1.AddDays(daysOffset);
			int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
			if((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
			{
				weekOfYear -= 1;
			}
			return firstWeekDay.AddDays(weekOfYear * 7);
		}
		public static KeyValuePair<int, int> splitDates(string value)
		{
			if(!value.Contains(",") && !value.Contains("."))
				return new KeyValuePair<int, int>(0, 0);
			var splitChar = value.Contains(",") ? ',' : '.';
			var splitted = value.Split(splitChar);
			var val1 = int.TryParse(splitted[0], out var t) ? t : 0;
			var val2 = int.TryParse(splitted[1], out var t1) ? t1 : 0;
			return new KeyValuePair<int, int>(val1, val2);
		}
		public static decimal? CommaSeperatorChecker(bool check, string value)
		{
			if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
				return null;
			if(!check)
				return decimal.TryParse(value, out var v) ? v : 0;
			else
			{
				if(value.Contains(","))
					return (decimal.TryParse(value, out var v1) ? v1 : 0) * 1000;
				else
					return decimal.TryParse(value, out var v2) ? v2 : 0;
			}
		}
		public static string FrequencyChecker(bool check, string frequency)
		{
			if(!check)
				return frequency;
			else
			{
				if(string.IsNullOrEmpty(frequency) || string.IsNullOrWhiteSpace(frequency))
					return "D";
				else
					return frequency;
			}
		}
		public static void moveErrorToNewFile(string fileName)
		{
			var moveTo = Path.Combine(Module.EDISettings.Delfor.NewDirectoryName, Path.GetFileName(fileName));
			var moveFrom = fileName;
			lock(Locks.Locks.DocumentsLock)
			{
				try
				{
					moveTo = checkAndFixFileName(moveTo);
					createIfNotExists(moveTo);

					File.Move(moveFrom, moveTo);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
		public static string checkAndFixFileName(string filePath)
		{
			var extension = System.IO.Path.GetExtension(filePath);

			int i = 0;
			while(File.Exists(filePath))
			{
				i++;
				var toAppendText = GenerateRandomKey(1, allowLetters: true, customChars: "_-+.%");
				filePath = filePath.Replace(extension, string.Concat(Enumerable.Repeat(toAppendText, i)) + extension);
			}

			return filePath;
		}
		public static void createIfNotExists(string filePath)
		{
			var dir = Path.GetDirectoryName(filePath);
			if(!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}
		public static string GenerateRandomKey(int lenght,
		   bool allowNumbers = true,
		   bool allowLetters = true,
		   bool uppers = true,
		   bool lowers = true,
		   string customChars = null)
		{
			if(!allowNumbers && !allowLetters)
			{
				allowNumbers = true;
				allowLetters = true;
			}

			if(allowLetters && !allowNumbers
				&& !uppers && !lowers)
			{
				uppers = true;
			}

			var chars = "";
			if(allowNumbers)
			{
				chars += "0123456789";
			}
			if(allowLetters)
			{
				if(!string.IsNullOrEmpty(customChars))
				{
					chars += customChars;
				}
				else
				{
					if(uppers)
					{
						chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
					}
					if(lowers)
					{
						chars += "abcdefghijklmnopqrstuvwxyz";
					}
				}
			}

			var data = new byte[lenght];
			using(var crypto = new RNGCryptoServiceProvider())
			{
				crypto.GetBytes(data);
			}
			var result = new StringBuilder(lenght);
			foreach(byte b in data)
			{
				result.Append(chars[b % (chars.Length)]);
			}
			return result.ToString();
		}
	}
}
