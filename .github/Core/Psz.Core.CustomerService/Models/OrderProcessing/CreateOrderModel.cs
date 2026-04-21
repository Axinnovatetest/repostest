namespace Psz.Core.CustomerService.Models.OrderProcessing
{
	public class CreateOrderModel
	{
		public int CustomerId { get; set; }
		public int SupplierId { get; set; }
		public string DocumentCustomer { get; set; }
		public int BlanketTypeId { get; set; }
		public int InvoiceTypeId { get; set; }
		public bool? Converted { get; set; }
	}
}
