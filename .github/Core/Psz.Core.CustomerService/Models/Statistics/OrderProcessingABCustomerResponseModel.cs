namespace Psz.Core.CustomerService.Models.Statistics
{
	public class OrderProcessingABCustomerResponseModel
	{
		public string CustomerName { get; set; }
		public int? Count { get; set; }
		public OrderProcessingABCustomerResponseModel(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity entity)
		{
			CustomerName = entity.CustomerName;
			Count = entity.Count;
		}
	}
}
