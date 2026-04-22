namespace Psz.Core.CustomerService.Models.Statistics
{
	public class TimeFAResponseModel
	{
		public decimal? Time { get; set; }
		public int? Fertigungsnummer { get; set; }
		public TimeFAResponseModel(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeFAEntity entity)
		{
			Fertigungsnummer = entity.Fertigungsnummer;
			Time = entity.Time;
		}
	}
}
