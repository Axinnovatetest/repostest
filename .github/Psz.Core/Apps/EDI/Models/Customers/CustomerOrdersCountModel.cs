namespace Psz.Core.Apps.EDI.Models.Customers
{
	public class CustomerOrdersCountModel
	{
		public int Id { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Contact { get; set; }
		public string Department { get; set; }
		public string StreetPOBox { get; set; }
		public string CountryPostcode { get; set; }
		public int CustomerNumber { get; set; }
		public int? AdressCustomerNumber { get; set; }

		public int Count { get; set; }
	}
}
