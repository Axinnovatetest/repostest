namespace Psz.Core.BaseData.Models.Customer
{
	public class CreateCustomerModel
	{
		//adressen items
		public string StraBe { get; set; }
		public string Land { get; set; }
		public string PLZ_StraBe { get; set; }
		public string Ort { get; set; }
		public int? Adresstyp { get; set; }
		public int? Kundennummer { get; set; }
		public string Telefon { get; set; }
		public string Mail { get; set; }
		public string Anrede { get; set; }
		public string Name1 { get; set; }
		//kunden items
		public int? Konditionszuordnung { get; set; }
		public string Branche { get; set; }
		public string Kundengruppe { get; set; }
		public bool Umsatzsteuer { get; set; }
		public int? Sprache { get; set; }
		public int? Wahrung { get; set; }
		public string Zahlungsweise { get; set; }
		public string Versandart { get; set; }
		//
		public int? Belegkreis { get; set; }
		public int? Rabattgruppe { get; set; }
		// - 2022-12-19 - Frishholz

		public string Debitorennummer { get; set; }
		public int? FibuRahmen { get; set; }
		// - 2023-04-04
		public int? AddressNr { get; set; }
	}
}
