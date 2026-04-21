namespace Psz.Core.Logistics.Models.Statistics
{
	public class InventurlisteEFModel
	{
		public InventurlisteEFModel(Infrastructure.Data.Entities.Joins.Logistics.InventurlisteEF InventurlisteEF)
		{

			if(InventurlisteEF == null)
				return;

			Artikelnummer = InventurlisteEF.Artikelnummer;
			Bezeichnung1 = InventurlisteEF.Bezeichnung1;
			Bestand = InventurlisteEF.Bestand;
			totalRows = InventurlisteEF.totalRows;



		}
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int? Bestand { get; set; }
		public int totalRows { get; set; }
	}
}
