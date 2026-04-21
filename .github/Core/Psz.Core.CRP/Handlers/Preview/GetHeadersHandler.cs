using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Preview;
using Psz.Core.CRP.Interfaces;
using System.Globalization;

namespace Psz.Core.CRP.Handlers.Preview
{
	public partial class PreviewService: IPreviewService
	{
		public ResponseModel<List<PreviewHeaderWeekResponseModel>> GetHeadersHandler(Identity.Models.UserModel user)
		{
			if(user == null)
				return ResponseModel<List<PreviewHeaderWeekResponseModel>>.AccessDeniedResponse();

			var results = new List<PreviewHeaderWeekResponseModel>();

			DateTime startDate = DateTime.Today.AddDays(-7); // * -7 to include backlog 
			DateTime endDate = startDate.AddYears(1).AddDays(7); // - restore the week for backlog    
			DateTime current = startDate;
			DateTime h1 = startDate.AddDays(Module.CTS?.FAHorizons?.H1LengthInDays ?? 42).AddDays(7);
			DateTime h2 = h1.AddDays((Module.CTS?.FAHorizons?.H2KWLength ?? 84) * 7).AddDays(7);

			// Adjust to Monday if startDate is not Monday
			while(current.DayOfWeek != DayOfWeek.Monday)
			{
				current = current.AddDays(-1);
			}

			while(current <= endDate)
			{
				var calendar = CultureInfo.InvariantCulture.Calendar;
				var weekNumber = ISOWeek.GetWeekOfYear(current);
				var year = current.Year;

				// Handle edge case for ISO weeks that belong to the previous year
				if(weekNumber == 1 && current.Month == 12)
				{
					year = current.AddYears(1).Year;
				}
				else
				if(weekNumber >= 52 && current.Month == 1)
				{
					year = current.AddYears(-1).Year;
				}

				results.Add(new PreviewHeaderWeekResponseModel
				{
					Day = current.Day,
					DayName = current.Day.ToString("00"),
					Month = current.Month,
					MonthLongName = current.ToString("MMM"),
					MonthName = current.Month.ToString("00"),
					Horizon = current <= h1 ? 1 : current <= h2 ? 2 : 3,
					Week = weekNumber,
					WeekName = weekNumber.ToString("00"),
					Year = year,
					YearName = current.Month == 1 && weekNumber > 10 ? (year + 1).ToString("0000") : (current.Month == 12 && weekNumber < 10 ? (year - 1).ToString("0000") : year.ToString("0000"))
				});
				current = current.AddDays(7); // Jump to next Monday
			}

			results = results.OrderBy(x => x.Year).ThenBy(x => x.Week).ToList();
			return ResponseModel<List<PreviewHeaderWeekResponseModel>>.SuccessResponse(results);
		}
	}
}
