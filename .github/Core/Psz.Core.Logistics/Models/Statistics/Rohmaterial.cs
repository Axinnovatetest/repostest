namespace Psz.Core.Logistics.Models.Statistics
{
	public class Rohmaterial
	{
		public Rohmaterial(Infrastructure.Data.Entities.Joins.Logistics.InventurlisteRohmaterial InventurlisteRohmaterial)
		{

			if(InventurlisteRohmaterial == null)
				return;

			ArtikelNr = InventurlisteRohmaterial.ArtikelNr;
			Artikelnummer = InventurlisteRohmaterial.Artikelnummer;
			Bezeichnung1 = InventurlisteRohmaterial.Bezeichnung1;
			Bestand = InventurlisteRohmaterial.Bestand;
			Lagerort = InventurlisteRohmaterial.Lagerort;
			EK = InventurlisteRohmaterial.EK;
			EK_Summe = InventurlisteRohmaterial.EK_Summe;
			Gewicht = InventurlisteRohmaterial.Gewicht;
			Gesamtgewicht = InventurlisteRohmaterial.Gesamtgewicht;
			Zolltarif_nr = InventurlisteRohmaterial.Zolltarif_nr;
			Ursprungsland = InventurlisteRohmaterial.Ursprungsland;
			LieferantenNr = InventurlisteRohmaterial.LieferantenNr;
			Name1 = InventurlisteRohmaterial.Name1;
			BestellNr = InventurlisteRohmaterial.BestellNr;
			BezeichnungAL = InventurlisteRohmaterial.BezeichnungAL;
			Praferenz = InventurlisteRohmaterial.Praferenz;
			totalRows = InventurlisteRohmaterial.totalRows;


		}
		public Rohmaterial()
		{
		}

		public decimal? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal Bestand { get; set; }
		public string Lagerort { get; set; }
		public decimal EK { get; set; }
		public decimal EK_Summe { get; set; }
		public string Gewicht { get; set; }
		public decimal Gesamtgewicht { get; set; }
		public string? Zolltarif_nr { get; set; }
		public string Ursprungsland { get; set; }
		public decimal LieferantenNr { get; set; }
		public string Name1 { get; set; }
		public string BestellNr { get; set; }
		public string BezeichnungAL { get; set; }
		public string Praferenz { get; set; }
		public int totalRows { get; set; }
	}
}
