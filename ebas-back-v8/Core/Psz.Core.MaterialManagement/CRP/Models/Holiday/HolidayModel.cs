using System.Globalization;

namespace Psz.Core.MaterialManagement.CRP.Models.Holiday
{
	public class HolidayModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int CreationUserId { get; set; }
		public string CreationUsername { get; set; }

		public DateTime Date { get; set; }
		public string WeekDay { get; set; }
		public int Day { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public int WeekNumber { get; set; }
		public bool? IsOverwritten { get; set; }

		public HolidayModel() { }
		public HolidayModel(Infrastructure.Data.Entities.Tables.MTM.HolidayEntity holidayEntity,
			string creationUserName, string userLanguage = "en")
		{
			this.Id = holidayEntity.Id;
			this.Name = holidayEntity.Name;

			this.CountryId = holidayEntity.CountryId;
			this.CountryName = holidayEntity.CountryName;
			this.HallId = holidayEntity.HallId;
			this.HallName = holidayEntity.HallName;

			this.CreationUserId = holidayEntity.CreationUserId;
			this.CreationUsername = creationUserName;

			this.Date = holidayEntity.Day;
			this.WeekDay = getWeekDay(holidayEntity.Day, userLanguage);
			this.Day = holidayEntity.Day.Day;
			this.Month = holidayEntity.Day.Month;
			this.Year = holidayEntity.Day.Year;
			this.WeekNumber = holidayEntity.WeekNumber ?? 0;
			this.IsOverwritten = holidayEntity.IsOverwritten;
		}

		static string getWeekDay(DateTime d, string language)
		{
			switch(language.ToLower())
			{
				case "en":
					return d.ToString("dddd", CultureInfo.InvariantCulture);
				case "de":
					return d.ToString("dddd", new CultureInfo("DE-de"));
				case "fr":
					return d.ToString("dddd", new CultureInfo("FR-fr"));
				default:
					return d.ToString("dddd", CultureInfo.InvariantCulture);
			}
		}
	}
}
