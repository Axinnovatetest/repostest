namespace Psz.Core.CustomerService.Models.Statistics
{
	public class TimeArticleResponseModel
	{
		public decimal? Time { get; set; }
		public string ArtikelNum { get; set; }
		public TimeArticleResponseModel(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTimeAREntity entity)
		{
			ArtikelNum = entity.ArtikelNum;
			Time = entity.Time;
		}
	}
}
