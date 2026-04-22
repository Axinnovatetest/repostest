
namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class StockWarningsLogsModel
	{
		public DateTime? Date { get; set; }
		public int Id { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }
		public StockWarningsLogsModel()
		{

		}
		public StockWarningsLogsModel(Infrastructure.Data.Entities.Joins.PRS.__PRS_StockWarnings_ComputeLogsEntity entity)
		{
			Date = entity.Date;
			Id = entity.Id;
			UserId = entity.UserId;
			Username = entity.UserId == -1 ? "System Agent" : entity.Username;
		}
	}
}