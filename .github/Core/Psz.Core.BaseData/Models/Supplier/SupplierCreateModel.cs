namespace Psz.Core.BaseData.Models.Supplier
{
	public class SupplierCreateModel
	{
		public int? AddressNr { get; set; }
		//adressen items
		public string StraBe { get; set; }
		public string Land { get; set; }
		public string PLZ_StraBe { get; set; }
		public string Ort { get; set; }
		public int? Adresstyp { get; set; }
		public int? Lieferantennummer { get; set; }
		public string Telefon { get; set; }
		public string Mail { get; set; }
		public string Anrede { get; set; }
		public string Name1 { get; set; }
		//lieferanten items
		public int? Konditionszuordnung { get; set; }
		public string Branche { get; set; }
		public string Lieferantengruppe { get; set; }
		public bool Umsatzsteuer { get; set; }
		public int? Sprache { get; set; }
		public int? Wahrung { get; set; }
		public string Zahlungsweise { get; set; }
		public string Versandart { get; set; }
		public string Stufe { get; set; }

		public SupplierCreateModel()
		{

		}

	}
}
