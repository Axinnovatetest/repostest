namespace Psz.Core.CustomerService.Models.Statistics
{
	public class TimeLagerResponseModel
	{
		public decimal? Time { get; set; }
		public string LagerName { get; set; }
		public TimeLagerResponseModel(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeByLagerEntity entity)
		{
			LagerName = entity.LagerName;
			Time = entity.Time;

		}
	}
}
