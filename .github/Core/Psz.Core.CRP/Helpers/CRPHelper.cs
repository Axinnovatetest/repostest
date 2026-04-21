using Infrastructure.Services.Reporting.Models.MTM;
using OfficeOpenXml;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.CRP.Models.Forecasts;
using System.Diagnostics;
using System.Globalization;

namespace Psz.Core.CRP.Helpers
{
	public class CRPHelper
	{
		public static IEnumerable<ForecastPositionModel> ReadFromExcelHoch(string filePath, out List<string> errors)
		{
			errors = new List<string> { };

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

			if(rows > 1 && columns > 1)
			{
				var col1 = Convert.ToString(getCellValue(worksheet.Cells[1, 1]));
				var col2 = Convert.ToString(getCellValue(worksheet.Cells[1, 2]));
				var col3 = Convert.ToString(getCellValue(worksheet.Cells[1, 3]));
				var col4 = Convert.ToString(getCellValue(worksheet.Cells[1, 4]));
				if(col1 != "PSZ Artikelnummer" || col2 != "Material" || col3 != "Datum" || col4 != "Menge")
				{
					errors.Add($"Excel Columns Incompatible please download the right DRAFT");
					return null;
				}
				List<ForecastPositionModel> positions = new List<ForecastPositionModel>();
				var artikelnummern = new List<string>();
				for(int i = startRowNumber; i <= rowEnd; i++)
				{
					var artikelnummer = getCellValue(worksheet.Cells[i, startColNumber]).ToString();
					// - ignore empty rows
					if(string.IsNullOrEmpty(artikelnummer))
						continue;
					artikelnummern.Add(artikelnummer.Trim());
				}
				var _articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(artikelnummern);
				if(_articles == null || _articles.Count <= 0)
					return null;
				var _prices = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(_articles.Select(x => x.ArtikelNr).ToList());

				for(int i = startRowNumber; i <= rowEnd; i++)
				{
					var artikelnummer = getCellValue(worksheet.Cells[i, startColNumber]).ToString();
					// - ignore empty rows
					if(string.IsNullOrEmpty(artikelnummer))
						continue;

					var material = getCellValue(worksheet.Cells[i, startColNumber + 1]).ToString();
					var datum = getCellValue(worksheet.Cells[i, startColNumber + 2]).ToString();
					var menge = getCellValue(worksheet.Cells[i, startColNumber + 3]).ToString();
					var artikel = _articles.FirstOrDefault(x => x.ArtikelNummer.Trim().ToLower() == artikelnummer.Trim().ToLower());
					if(artikelnummer == null)
						errors.Add($"Row [{i}]: Artikelnummer must not be empty.");

					if(artikel == null)
					{
						errors.Add($"Row [{i}]: Article [{artikelnummer}] not found.");
						continue;
					}

					var kundenArtikel = artikel.CustomerItemNumber.StringIsNullOrEmptyOrWhiteSpaces() ? artikel.Bezeichnung1 : artikel.CustomerItemNumber;
					if(kundenArtikel != material)
						errors.Add($"Row [{i}]: invalid Customer article number [{material}].");
					if(material == null)
						errors.Add($"Row [{i}]: Material should not be empty.");
					CultureInfo culture = CultureInfo.InvariantCulture;
					DateTimeStyles styles = DateTimeStyles.None;
					if(!DateTime.TryParseExact(datum, "dd.MM.yyyy", culture, styles, out var v))
						errors.Add($"Row [{i}]: Invalid Date [{datum}]");
					if(datum == null)
						errors.Add($"Row [{i}]: Date sould not be empty.");
					if(!int.TryParse(menge, out var m))
						errors.Add($"Row [{i}]: Invalid menge [{menge}]");
					if(menge == null)
						errors.Add($"Row [{i}]: Menge should not be empty.");

					var _menge = int.TryParse(menge, out var mn) ? mn : 0;
					var _datum = DateTime.TryParseExact(datum, "dd.MM.yyyy", culture, styles, out var d) ? d : new DateTime(1999, 1, 1);
					var _price = _prices.FirstOrDefault(x => x.Artikel_Nr == artikel?.ArtikelNr);
					if(_price == null)
					{
						errors.Add($"Row [{i}]: Article [{artikelnummer}] missing VK price.");
						continue;
					}
					positions.Add(new ForecastPositionModel
					{
						ArtikelNr = artikel?.ArtikelNr ?? -1,
						Artikelnummer = artikelnummer,
						Datum = _datum,
						Material = material,
						Menge = _menge,
						VKE = _price.Verkaufspreis,
						Gesampreis = _price.Verkaufspreis * _menge,
						KW = ISOWeek.GetWeekOfYear(_datum),
						Jahr = _datum.Year
					});
					;
				}
				if(errors != null && errors.Count > 0)
					return null;
				else
					return positions;
			}
			else
			{
				errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
				return null;
			}
		}
		public static IEnumerable<ForecastPositionModel> ReadFromExcelQuer(string filePath, out List<string> errors)
		{
			errors = new List<string> { };

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

			var horizontalValuesColumns = columns - 2;

			if(rows > 1 && columns > 1)
			{

				var col1 = Convert.ToString(getCellValue(worksheet.Cells[1, 1]));
				var col2 = Convert.ToString(getCellValue(worksheet.Cells[1, 2]));
				if(col1 != "PSZ Nummer" || col2 != "Material")
				{
					errors.Add($"Excel Columns Incompatible please download the right DRAFT");
					return null;
				}

				var _articleNumbers = new List<string>();
				for(int i = startRowNumber; i <= rowEnd; i++)
				{
					var artikelnummer = getCellValue(worksheet.Cells[i, startColNumber]).ToString();
					// - ignore empty rows
					if(string.IsNullOrEmpty(artikelnummer))
						continue;
					_articleNumbers.Add(artikelnummer.Trim());
				}

				var _articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(_articleNumbers);
				if(_articles == null || _articles?.Count <= 0)
					return null;
				var _prices = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(_articles.Select(x => x.ArtikelNr).ToList());
				List<ForecastPositionModel> positions = new List<ForecastPositionModel>();
				int charIndex = 64;
				for(int i = startRowNumber; i <= rowEnd; i++)
				{
					var artikelnummer = getCellValue(worksheet.Cells[i, startColNumber]).ToString();
					// - ignore empty rows
					if(string.IsNullOrEmpty(artikelnummer))
						continue;

					var material = getCellValue(worksheet.Cells[i, startColNumber + 1]).ToString();
					var artikel = _articles.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == artikelnummer.Trim().ToLower());
					if(artikelnummer == null)
						errors.Add($"Row [{i}]: Artikelnummer must not be empty.");
					if(artikel == null)
					{
						errors.Add($"Row [{i}]: invalid Artikelnummer [{artikelnummer}].");
						continue;
					}
					var kundenArtikel = artikel.CustomerItemNumber.StringIsNullOrEmptyOrWhiteSpaces() ? artikel.Bezeichnung1 : artikel.CustomerItemNumber;
					if(kundenArtikel != material)
						errors.Add($"Row [{i}]: invalid Customer article number [{material}].");
					if(material == null)
						errors.Add($"Row [{i}]: Material should not be empty.");
					var horizontalValues = new List<KeyValuePair<string, string>>();
					for(int j = 3; j < horizontalValuesColumns + 3; j++)
					{
						var kwYear = getCellValue(worksheet.Cells[1, j]).ToString();
						var menge = getCellValue(cell: worksheet.Cells[i, j]).ToString();

						if(kwYear == null || !ValidateKWYear(kwYear))
							errors.Add($"Cell [{(char)(j + charIndex)}{i}]: Invalid KW/Year value [{kwYear}].");
						if(menge == null || !int.TryParse(menge, out var value))
							errors.Add($"Cell [{(char)(j + charIndex)}{i}]: Invalid Menge value [{menge}].");

						horizontalValues.Add(new KeyValuePair<string, string>(kwYear, menge));
					}
					var x = horizontalValues;
					var _price = _prices.FirstOrDefault(x => x.Artikel_Nr == artikel.ArtikelNr);
					if(_price == null)
					{
						errors.Add($"Row [{i}]: Article [{artikelnummer}] missing VK price.");
						continue;
					}
					positions.AddRange(horizontalValues.Select(h => new ForecastPositionModel
					{
						ArtikelNr = artikel?.ArtikelNr ?? -1,
						Artikelnummer = artikelnummer,
						Datum = Helpers.DatesHelper.FirstDateOfWeek(SplitValues(h.Key).Value, SplitValues(h.Key).Key),
						Material = material,
						Jahr = SplitValues(h.Key).Value,
						KW = SplitValues(h.Key).Key,
						Menge = int.TryParse(h.Value, out var m) ? m : 0,
						VKE = _price.Verkaufspreis,
						Gesampreis = _price.Verkaufspreis * (int.TryParse(h.Value, out var me) ? me : 0),
					}));

				}
				if(errors != null && errors.Count > 0)
					return null;

				return positions;
			}
			else
			{
				errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
				return null;
			}
		}
		internal static string getCellValue(ExcelRange cell)
		{
			var val = cell.Value;
			if(val == null)
			{
				return "";
			}

			return val.ToString().Trim();
		}
		internal static bool ValidateKWYear(string input)
		{
			var splitChar = input.Contains(".") ? "." : ",";
			string[] parts = input.Split(splitChar);

			if(parts.Length != 2)
				return false;

			if(!int.TryParse(parts[0], out int x))
				return false;

			if(x < 1 || x > 52)
				return false;

			if(!int.TryParse(parts[1], out int y))
				return false;

			int currentYear = DateTime.Now.Year;

			if(y < currentYear - 1 || y > currentYear + 1)
				return false;

			return true;
		}
		internal static KeyValuePair<int, int> SplitValues(string input)
		{
			var splitChar = input.Contains(".") ? "." : ",";
			string[] parts = input.Split(splitChar);
			return new KeyValuePair<int, int>(
				int.TryParse(parts[0], out var kw) ? kw : 0,
				int.TryParse(parts[1], out var year) ? year : 0
				);
		}
		public static int GetForecastVersion(int kundennummer, int typeId)
		{
			var lasVersion = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.GetLastVersion(kundennummer, typeId);
			return lasVersion < 0 ? 1 : lasVersion + 1;
		}
		public static List<WeekQuantityModel> GetBestand(FASystemModel data, List<(int Week, int Year)> range, decimal bestand)
		{
			var currWeek = Helpers.HorizonsHelper.GetIsoWeekNumber(DateTime.Now);
			var result = new List<WeekQuantityModel>();

			var bs = 0m;
			//for(int i = start; i <= end; i++)
			foreach(var i in range)
			{
				var original = bs;
				if(i.Week == currWeek && i.Year == DateTime.Now.Year)
				{
					bs += bestand;
				}
				bs = (bs + (data.FAMovement.FirstOrDefault(x => x.Week == i.Week && x.Year == i.Year)?.Quantity ?? 0))
						- (data.ABMovement.FirstOrDefault(x => x.Week == i.Week && x.Year == i.Year)?.Quantity ?? 0)
						- (data.LPMovement.FirstOrDefault(x => x.Week == i.Week && x.Year == i.Year)?.Quantity ?? 0)
						- (data.FCMovement.FirstOrDefault(x => x.Week == i.Week && x.Year == i.Year)?.Quantity ?? 0);
				if(data.InternBedarfMovement != null && data.InternBedarfMovement.Count > 0)
				{
					var intBedarf = data.InternBedarfMovement?.FirstOrDefault(x => x.Week == i.Week && x.Year == i.Year).Quantity ?? 0;
					bs = bs - intBedarf;
				}
				if(data.ExternBedarfMovement != null && data.ExternBedarfMovement.Count > 0)
				{
					var extBedarf = data.ExternBedarfMovement?.FirstOrDefault(x => x.Week == i.Week && x.Year == i.Year).Quantity ?? 0;
					bs = bs - extBedarf;
				}
				result.Add(new WeekQuantityModel
				{
					Week = i.Week,
					Year = i.Year,
					Quantity = bs,
				});

			}
			return result;
		}
		public static KeyValuePair<List<WeekQuantityModel>, List<WeekQuantityModel>> GetFACorrections(FASystemModel faSystem, List<(int Week, int Year)> range, decimal _bestand, decimal productionLotSize)
		{
			var horizons = Module.CTS.FAHorizons;
			var H1StartWeek = ISOWeek.GetWeekOfYear(DateTime.Now);
			var H1EndWeek = ISOWeek.GetWeekOfYear(DateTime.Today.AddDays(horizons.H1LengthInDays));

			var proposedFA = new List<WeekQuantityModel> { };
			var bestand = GetBestand(faSystem, range, _bestand);

			var firstPortion = GetPortionByRange(bestand,
				range.FirstOrDefault(x => x.Week == H1StartWeek),
				range.FirstOrDefault(x => x.Week == H1EndWeek + 2)
				);
			//bestand.Where(x => x.Week >= H1StartWeek && x.Week <= H1EndWeek + 2).ToList();

			//+1
			var faSumFirstPortion__ = bestand[1].Quantity + faSystem.FAMovement.Where(x => x.Week >= H1StartWeek && x.Week <= H1EndWeek + 1).ToList()
				.Sum(x => x.Quantity);
			var abSumFirstPortion__ = faSystem.ABMovement.Where(x => x.Week >= H1StartWeek && x.Week <= H1EndWeek + 1).ToList()
				.Sum(x => x.Quantity)
				+ faSystem.LPMovement.Where(x => x.Week >= H1StartWeek && x.Week <= H1EndWeek + 1).ToList()
				.Sum(x => x.Quantity)
				+ faSystem.FCMovement.Where(x => x.Week >= H1StartWeek && x.Week <= H1EndWeek + 1).ToList()
				.Sum(x => x.Quantity);
			if(abSumFirstPortion__ > faSumFirstPortion__)
			{
				var diff = abSumFirstPortion__ - faSumFirstPortion__;
				if(productionLotSize > 0)
				{
					diff = Math.Ceiling(diff / productionLotSize) * productionLotSize;
				}
				CorrectFA(faSystem, new WeekQuantityModel { Week = H1EndWeek, Quantity = diff }, true);
				proposedFA.Add(new WeekQuantityModel { Week = H1EndWeek + 1, Quantity = diff });
				bestand = GetBestand(faSystem, range, _bestand);
			}

			//+2
			var faSumFirstPortion = bestand[1].Quantity + faSystem.FAMovement.Where(x => x.Week <= H1EndWeek + 2).ToList()
				.Sum(x => x.Quantity);
			var abSumFirstPortion = faSystem.ABMovement.Where(x => x.Week <= H1EndWeek + 2).ToList()
				.Sum(x => x.Quantity)
				+ faSystem.LPMovement.Where(x => x.Week <= H1EndWeek + 2).ToList()
				.Sum(x => x.Quantity)
			+ faSystem.FCMovement.Where(x => x.Week <= H1EndWeek + 2).ToList()
				.Sum(x => x.Quantity);
			if(abSumFirstPortion > faSumFirstPortion)
			{
				var diff = abSumFirstPortion - faSumFirstPortion;
				if(productionLotSize > 0)
				{
					diff = Math.Ceiling(diff / productionLotSize) * productionLotSize;
				}
				CorrectFA(faSystem, new WeekQuantityModel { Week = H1EndWeek, Quantity = diff }, true);
				proposedFA.Add(new WeekQuantityModel { Week = H1EndWeek + 1, Quantity = diff });
				bestand = GetBestand(faSystem, range, _bestand);
			}

			var secondPortion = GetBestandPortion(range, bestand, H1EndWeek + 2);
			//bestand.Where(x => x.Week > H1EndWeek + 2 && x.Week <= end).ToList();
			while(secondPortion.Any(x => x.Quantity < 0))
			{
				var value = secondPortion.OrderBy(v => v.Week)
					.FirstOrDefault(x => x.Quantity < 0);
				var diff = value.Quantity * -1;
				if(productionLotSize > 0)
				{
					diff = Math.Ceiling(diff / productionLotSize) * productionLotSize;
				}
				CorrectFA(faSystem, new WeekQuantityModel { Week = value.Week, Quantity = diff });
				proposedFA.Add(new WeekQuantityModel { Week = value.Week - 2, Quantity = diff });
				bestand = GetBestand(faSystem, range, _bestand);
				secondPortion = GetBestandPortion(range, bestand, H1EndWeek + 2);
				//bestand.Where(x => x.Week >= H1EndWeek + 2 && x.Week <= end).ToList();
			}
			// - group fas of same kw
			// - group fas of same kw
			var _proposedFA = new List<WeekQuantityModel>();
			var distinctWeeks = proposedFA.DistinctBy(x => x.Week).Select(x => x.Week);
			if(distinctWeeks.Count() < proposedFA.Count)
			{
				foreach(var week in distinctWeeks)
				{
					_proposedFA.Add(new WeekQuantityModel { Week = week, Quantity = proposedFA.Where(x => x.Week == week)?.Sum(x => x.Quantity) ?? 0 });
				}
			}
			else
			{
				_proposedFA = proposedFA;
			}
			return new KeyValuePair<List<WeekQuantityModel>, List<WeekQuantityModel>>(bestand, _proposedFA);
		}
		private static void CorrectFA(FASystemModel faSystem, WeekQuantityModel item, bool inFrozenZone = false)
		{
			try
			{

				var quantity = item.Quantity;
				var correctionWeek = inFrozenZone
					? item.Week + 1
					: item.Week - 2;
				var index = faSystem.FAMovement.FindIndex(x => x.Week == correctionWeek);
				faSystem.FAMovement[index].Quantity += quantity;
			} catch(Exception es)
			{

				throw;
			}
		}
		public static List<WeekQuantityModel> GetBestandPortion(List<(int Week, int Year)> range, List<WeekQuantityModel> data, int maxPortionWeek)
		{
			if(range.Count == 0 || data.Count == 0)
				return new List<WeekQuantityModel>();

			var maxRange = range.OrderBy(x=>x.Year).ThenBy(x=> x.Week).Last();
			var maxPortionYear = maxPortionWeek > maxRange.Week ? maxRange.Year : maxRange.Year + 1;
			
			var maxPortionDate = Helpers.DatesHelper.FirstDateOfWeek(maxPortionYear, maxPortionWeek);
			var maxRangeDate = Helpers.DatesHelper.FirstDateOfWeek(maxRange.Year, maxRange.Week);

			return data.Where(d => maxRangeDate < Helpers.DatesHelper.FirstDateOfWeek(d.Year, d.Week)
												&& Helpers.DatesHelper.FirstDateOfWeek(d.Year, d.Week) < maxPortionDate).ToList();
		}
		public static List<WeekQuantityModel> GetPortionByRange(List<WeekQuantityModel> data, (int Week, int Year) from, (int Week, int Year) to)
		{
			return data.Where(d =>
					(d.Year > from.Year || (d.Year == from.Year && d.Week >= from.Week)) &&
					(d.Year < to.Year || (d.Year == to.Year && d.Week <= to.Week))
					).ToList();
		}
	}
}