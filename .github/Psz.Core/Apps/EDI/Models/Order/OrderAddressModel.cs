namespace Psz.Core.Apps.EDI.Models.Order
{
	public class OrderAddressModel
	{
		public int Id { get; set; }
		public string Type { get; set; } // Anrede
		public string Name { get; set; } // Vorname/NameFirma
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Contact { get; set; }
		public string Department { get; set; } // Abteilung
		public string StreetPOBox { get; set; } // Straße/Postfach
		public string CountryPostcode { get; set; } //Land/PLZ/Ort
		public string OrderTitle { get; set; }
		public int DeliveryAddressId { get; set; }
		public string UnloadingPoint { get; set; }
		public string StorageLocation { get; set; }
	}
}
