namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class UpdateOrderDetailsRequestModel
	{
		public int id { get; set; }
		public string shipping_method { get; set; }
		public int editor { get; set; }
		public string Relation { get; set; }
		public string conditions { get; set; }
		public string incoming_delivery_notes { get; set; }


		public string payment_method { get; set; }
		public DateTime confirmation_requested_Date { get; set; }
		public string incoming_invoice_no { get; set; }
		public string free_text { get; set; }



	}
	public class UpdateOrderDetailsResponseModel
	{

	}
}
