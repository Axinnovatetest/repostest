namespace Psz.Core.MaterialManagement.CRP.Helpers
{
	using Infrastructure.Data.Access.Tables.MTM;
	using Infrastructure.Data.Entities.Tables.MTM;
	using System.Linq;
	public class Capacity
	{
		public static List<CapacityEntity> AddHolidays(List<CapacityEntity> capacityEntities, int? countryId, int? hallId, int year, int weekFrom, int weekUntil)
		{
			var holidayEntities = HolidayAccess.GetBy_CountryId_HallId_DayRange(countryId,
				hallId,
				Helpers.DateTimeHelper.FirstDateOfWeekISO8601(year, weekFrom),
				Helpers.DateTimeHelper.FirstDateOfWeekISO8601(year, weekUntil),
				null, null) ?? new List<HolidayEntity>();

			var _capacityEntities = new List<CapacityEntity>();
			foreach(var capacityItem in capacityEntities)
			{
				var capacityWeekHolidays = holidayEntities.Where(x => x.WeekNumber == capacityItem.WeekNumber)?.ToList();
				// - No holiday in current capacity's week
				if(capacityWeekHolidays == null || capacityWeekHolidays.Count <= 0)
				{
					_capacityEntities.Add(capacityItem);
					continue;
				}

				// - Holidays in capacity's week
				var weekSaturday = capacityItem.WeekLastDay.AddDays(-1);
				var capacityWeekHolidays_weekDays = capacityWeekHolidays.Where(x => x.Day < weekSaturday)?.ToList();
				// var capacityWeekHolidays_weekEnds = capacityWeekHolidays.Where(x => x.Day >= weekSaturday)?.ToList(); // - Weekend Holiday is OVERWRITTEN if Special Shifts, or just IGNORED Otherwise

				var _capacityItem = Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan.CalculateItemHandler.Perform(
					new Models.CapacityPlan.CalculateItemModel(
						capacityItem,
						capacityWeekHolidays_weekDays?.Count ?? 0, 0))
					.ToCapacity(capacityItem);
				_capacityItem.Id = capacityItem.Id;
				// -
				_capacityEntities.Add(_capacityItem);
			}

			// -
			return _capacityEntities;
		}
	}
}
