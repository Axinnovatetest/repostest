namespace Psz.Core.Apps.EDI.Models.Customers
{
	public class CustomerModel
	{
		public int Id { get; set; }
		public int CustomerNumber { get; set; }
		public int? AdressCustomerNumber { get; set; }
		public string Duns { get; set; }

		public string Type { get; set; }
		public string Name { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Contact { get; set; }
		public string Department { get; set; }
		public string StreetPOBox { get; set; }
		public string CountryPostcode { get; set; }
		public string Street { get; set; }
		public string POBox { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public CustomerModel()
		{

		}
		public CustomerModel(CustomerService.Models.OrderProcessing.CustomerModel customerModel)
		{
			if(customerModel == null)
				return;

			Id = customerModel.Id;
			Type = customerModel.Type;
			CustomerNumber = customerModel.CustomerNumber;
			AdressCustomerNumber = customerModel.AdressCustomerNumber;

			Name = customerModel.Name;
			Name2 = customerModel.Name2;
			Name3 = customerModel.Name3;

			Contact = customerModel.Contact;
			Department = customerModel.Department;
			CountryPostcode = customerModel.CountryPostcode;
			StreetPOBox = customerModel.StreetPOBox;
			Duns = customerModel.Duns;

			Country = customerModel.Country;
			Email = customerModel.Email;
			Fax = customerModel.Fax;
			Phone = customerModel.Phone;
			POBox = customerModel.POBox;
			Street = customerModel.Street;
		}
	}
}
