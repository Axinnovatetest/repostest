namespace Psz.Core.Apps.EDI.Models.Customers
{
	public class CustomerOrderErrorsCountModel
	{
		public int ClientId { get; set; }
		public string ClientName { get; set; }
		public int CustomerNumber { get; set; }
		public int? AdressCustomerNumber { get; set; }
		public int Count { get; set; }
	}
}
