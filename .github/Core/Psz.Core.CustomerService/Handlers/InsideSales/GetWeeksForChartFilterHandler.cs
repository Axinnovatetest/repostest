using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<List<KeyValuePair<string, string>>> GetWeeksForChartFilter(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();

			try
			{

				List<string> weekYearList = new List<string>();
				DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
				Calendar cal = dfi.Calendar;

				for(int i = -26; i <= 25; i++)
				{
					DateTime date = DateTime.Now.AddDays(i * 7);
					int week = ISOWeek.GetWeekOfYear(date);
					int year = date.Year;

					if(week == 53 && date.Month == 1)
					{
						year--;
					}
					else if(week == 53 && date.Month == 12)
					{
						year++;
					}

					weekYearList.Add($"{week}/{year}");
				}

				weekYearList = weekYearList.Distinct().OrderBy(s => s).ToList();
				weekYearList = weekYearList.OrderBy(s =>
				{
					string[] parts = s.Split('/');
					return int.Parse(parts[1]); // Year
				}).ThenBy(s =>
				{
					string[] parts = s.Split('/');
					return int.Parse(parts[0]); // Week
				}).ToList();
				var response = weekYearList.Select(x => new KeyValuePair<string, string>(x, x)).ToList();

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}