namespace Psz.Core.MaterialManagement.Orders.Models.Wareneingang
{
	public class SaveRequestModel
	{
		public int Nr { get; set; }
		public int BestellungNr { get; set; }
		public string LsNummer { get; set; }
		public List<Item> UpdatedArticle { get; set; }

		public SaveRequestModel() { }
	}
	public class SaveResponseModel
	{
		public int WareneingangId { get; set; }

		public SaveResponseModel()
		{

		}
	}

	public class Item
	{
		public bool disabledUpdate { get; set; }
		public int BestellungNr { get; set; }
		public int ArtikelNr { get; set; }
		public int bestelleteArtikelNr { get; set; }
		public int Lagerort { get; set; }
		public bool Erledigt_pos { get; set; }
		public decimal AktuelleAnzahl { get; set; }
		public bool EMPBBestatigung { get; set; }
		public bool COCbestatigung { get; set; }
		public decimal Grosse { get; set; }
		public DateTime? MHDDatum { get; set; }
		public string? MHDDatumString { get; set; }
		public bool MHD { get; set; }
		public bool ESD_Schu { get; set; }
		public decimal Offen { get; set; }
		public List<string> Messages { get; set; }
		public string ArtikelNummer { get; set; }
	}
}
