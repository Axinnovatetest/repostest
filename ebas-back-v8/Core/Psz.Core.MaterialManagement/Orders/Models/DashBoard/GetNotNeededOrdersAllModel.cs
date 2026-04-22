namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class NotNeededOrdersAllResponseModel
	{
		public string Week { get; set; }
		public decimal Total { get; set; }
		public decimal Anzahl { get; set; }

		public NotNeededOrdersAllResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.NotNeededOrdersAllEntity notNeededOrders)
		{
			Week = notNeededOrders.Week;
			Total = notNeededOrders.Total;
			Anzahl = notNeededOrders.Anzahl;
		}
	}
	public class NotNeededOrdersAllRequestModel
	{
		public int Area { get; set; }
	}
}
