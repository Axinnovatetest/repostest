namespace Psz.Core.BaseData.Models.Supplier
{
	public class SupplierDropdownModel
	{
		public int Nr { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public int Lieferantennummer { get; set; }
		public string Ort { get; set; }
		public string AdressType { get; set; }

		public SupplierDropdownModel()
		{

		}
		public SupplierDropdownModel(Infrastructure.Data.Entities.Joins.SupplierDropdownEntity supplierDropdownEntity)
		{
			Nr = supplierDropdownEntity.Nr;
			Name1 = supplierDropdownEntity.Name1;
			Name2 = supplierDropdownEntity.Name2;
			Lieferantennummer = supplierDropdownEntity.Lieferantennummer;
			Ort = supplierDropdownEntity.Ort;
			AdressType = supplierDropdownEntity.AdressType;
		}
	}
}
