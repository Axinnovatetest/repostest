namespace Psz.Core.BaseData.Models.Customer
{
	public class CustomerDropdownModel
	{
		public int Nr { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public int Kundennummer { get; set; }
		public string Ort { get; set; }
		public string AdressType { get; set; }

		public CustomerDropdownModel()
		{

		}
		public CustomerDropdownModel(Infrastructure.Data.Entities.Joins.CustomerDropdownEntity customerDropdownEntity)
		{
			Nr = customerDropdownEntity.Nr;
			Name1 = customerDropdownEntity.Name1;
			Name2 = customerDropdownEntity.Name2;
			Kundennummer = customerDropdownEntity.Kundennummer;
			Ort = customerDropdownEntity.Ort;
			AdressType = customerDropdownEntity.AdressType;
		}
	}
}
