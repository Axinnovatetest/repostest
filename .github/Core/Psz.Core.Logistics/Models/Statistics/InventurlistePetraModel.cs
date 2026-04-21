namespace Psz.Core.Logistics.Models.Statistics
{
	public class InventurlistePetraModel
	{
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public int totalRows { get; set; }
		public InventurlistePetraModel(Infrastructure.Data.Entities.Joins.Logistics.InventurlistePetraEntity InventurlistePetraEntity)
		{
			if(InventurlistePetraEntity == null)
			{
				return;
			}
			ArtikelNr = InventurlistePetraEntity.ArtikelNr;
			Artikelnummer = InventurlistePetraEntity.Artikelnummer;
			Bezeichnung1 = InventurlistePetraEntity.Bezeichnung1;
			Bestand = InventurlistePetraEntity.Bestand;
			Verkaufspreis = InventurlistePetraEntity.Verkaufspreis;
			totalRows = InventurlistePetraEntity.TotalRows;
		}
	}
}
