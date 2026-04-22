namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class UpdateOrderSupplierRequestModel
	{
		public int Id { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string ContactPerson { get; set; }
		public string department { get; set; }
		public string StreetPO_Box { get; set; }
		public string CountryZIPLocation { get; set; }
		public string letterSalutation { get; set; }
		public string Our_sign { get; set; }
		public string Your_sign { get; set; }

	}
	public class UpdateOrderSupplierResponseModel
	{

	}
}
