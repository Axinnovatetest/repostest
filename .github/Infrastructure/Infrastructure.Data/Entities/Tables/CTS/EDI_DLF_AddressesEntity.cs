using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class EDI_DLF_AddressesEntity
	{
		public string Country { get; set; }
		public string CustomerNumber { get; set; }
		public string FactoryNumber { get; set; }
		public string HouseNumber { get; set; }
		public int Id { get; set; }
		public string Location { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string PostalCode { get; set; }
		public string StorageLocation { get; set; }
		public string StorageLocationDescription { get; set; }
		public string Street { get; set; }

		public EDI_DLF_AddressesEntity() { }

		public EDI_DLF_AddressesEntity(DataRow dataRow)
		{
			Country = (dataRow["Country"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Country"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerNumber"]);
			FactoryNumber = (dataRow["FactoryNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FactoryNumber"]);
			HouseNumber = (dataRow["HouseNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HouseNumber"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Location = (dataRow["Location"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Location"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			PostalCode = (dataRow["PostalCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PostalCode"]);
			StorageLocation = (dataRow["StorageLocation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StorageLocation"]);
			StorageLocationDescription = (dataRow["StorageLocationDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StorageLocationDescription"]);
			Street = (dataRow["Street"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Street"]);
		}

		public EDI_DLF_AddressesEntity ShallowClone()
		{
			return new EDI_DLF_AddressesEntity
			{
				Country = Country,
				CustomerNumber = CustomerNumber,
				FactoryNumber = FactoryNumber,
				HouseNumber = HouseNumber,
				Id = Id,
				Location = Location,
				Name1 = Name1,
				Name2 = Name2,
				PostalCode = PostalCode,
				StorageLocation = StorageLocation,
				StorageLocationDescription = StorageLocationDescription,
				Street = Street
			};
		}
	}
}

