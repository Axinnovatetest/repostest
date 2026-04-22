namespace Psz.Core.Logistics.Models.Statistics
{
	public class ExcessRohmaterialModel
	{
		public ExcessRohmaterialModel(Infrastructure.Data.Entities.Joins.Logistics.ExcessRohmaterialEntity ExcessRohmaterialEntity)
		{
			ArtikelNr = ExcessRohmaterialEntity.ArtikelNr;
			Artikelnummer = ExcessRohmaterialEntity.Artikelnummer;
			Bezeichnung1 = ExcessRohmaterialEntity.Bezeichnung1;
			SummevonBestand = ExcessRohmaterialEntity.SummevonBestand;
			Einkaufspreis = ExcessRohmaterialEntity.Einkaufspreis;
			Kosten = ExcessRohmaterialEntity.Kosten;
			totalRows = ExcessRohmaterialEntity.totalRows;
		}
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal SummevonBestand { get; set; }
		public decimal Einkaufspreis { get; set; }
		public decimal Kosten { get; set; }
		public int totalRows { get; set; }
	}
}
