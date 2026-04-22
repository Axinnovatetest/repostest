namespace Psz.Core.CustomerService.Models.Statistics
{
	public class OrderProcessingLSCustomerResponseModel
	{
		public string CustomerName { get; set; }
		public int? Count { get; set; }
		public OrderProcessingLSCustomerResponseModel(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity entity)
		{
			CustomerName = entity.CustomerName;
			Count = entity.Count;
		}
	}
}
