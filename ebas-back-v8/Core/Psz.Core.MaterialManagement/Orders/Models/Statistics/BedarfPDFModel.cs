namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class BedarfRequestModel
	{
		public string ArticleNumber { get; set; }
		public int ArticleId { get; set; }
		public int Land { get; set; }
	}

	public class BedarfPDFResponseModel
	{
		public Header DataHeader { get; set; }
		public List<NeedsModel> BedarfPositive { get; set; }
		public List<NeedsModel> BedarfNegative { get; set; }
		public List<SupplierModel> Suppliers { get; set; }
		public List<BestellungModel> SubItems { get; set; }

		public class Header
		{
			public string Artikelnummer { get; set; }
			public string Bezeichung { get; set; }
			public decimal Bestand { get; set; }
			public decimal Reserviert { get; set; }
			public string Title { get; set; }
			public string Date { get; set; }
		}

		public class NeedsModel
		{
			public decimal Anzahl { get; set; }
			public string ArtikelNummer { get; set; }
			public decimal Bedarf_FA { get; set; }
			public decimal Bedarf_Summiert { get; set; }
			public string Bezeichnung { get; set; }
			public string Bezeichnung_des_Bauteils { get; set; }
			public decimal FA_Offen { get; set; }
			public string Fertigung { get; set; }
			public bool Gestart { get; set; }
			public string Kabel_geschnitten { get; set; }
			public string Kommisioniert_komplett { get; set; }
			public string Kommisioniert_teilweise { get; set; }
			public decimal Reserviert_Menge { get; set; }
			public string S_Intern { get; set; }
			public string S_Extetrn { get; set; }
			public string Stucklisten_Artikelnummer { get; set; }
			public string Termin_Bestatigen { get; set; }
			public string Termin_MA { get; set; }
			public decimal Verfug_Ini { get; set; }
			public decimal Verfugbar { get; set; }
			public NeedsModel()
			{

			}
		}
		public class SupplierModel
		{
			public int Artikel_Nr { get; set; }
			public string Bestell_Nr { get; set; }
			public string Fax { get; set; }
			public int Lief_Nr { get; set; }
			public string Lieferant { get; set; }
			public int LT { get; set; }
			public decimal MQO { get; set; }
			public decimal Peis { get; set; }
			public string Standar_Liferent { get; set; }
			public string Telefon { get; set; }
			public SupplierModel()
			{

			}
		}
		public class BestellungModel
		{
			public string AB { get; set; }
			public string ABtermin { get; set; }
			public decimal Anzhal { get; set; }
			public string Bemerkung { get; set; }
			public int Lief_Nr { get; set; }
			public string Liefertermin { get; set; }
			public int PO { get; set; }
			public string VornameFirma { get; set; }
			public BestellungModel()
			{

			}
		}
	}
}
