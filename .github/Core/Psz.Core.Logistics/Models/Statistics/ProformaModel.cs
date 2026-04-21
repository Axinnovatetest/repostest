namespace Psz.Core.Logistics.Models.Statistics
{
	public class ProformaModel
	{
		public ProformaModel(Infrastructure.Data.Entities.Joins.Logistics.Proforma Proforma)
		{

			if(Proforma == null)
				return;

			Artikelnummer = Proforma.Artikelnummer;
			Bezeichnung1 = Proforma.Bezeichnung1;
			Bestand = Proforma.Bestand;
			Einheit = Proforma.Einheit;
			EK = Proforma.EK;
			EK_Summe = Proforma.EK_Summe;
			Gewicht = Proforma.Gewicht;
			Gesamtgewicht = Proforma.Gesamtgewicht;
			Zolltarif_nr = Proforma.Zolltarif_nr;
			Ursprungsland = Proforma.Ursprungsland;
			Name1 = Proforma.Name1;
			Praeferenz_Aktuelles_jahr = Proforma.Praeferenz_Aktuelles_jahr;
			Standardlieferant = Proforma.Standardlieferant;
			totalRows = Proforma.totalRows;



		}
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal Bestand { get; set; }
		public string Einheit { get; set; }
		public decimal EK { get; set; }
		public decimal EK_Summe { get; set; }
		public string Gewicht { get; set; }
		public decimal Gesamtgewicht { get; set; }
		public string Zolltarif_nr { get; set; }
		public string Ursprungsland { get; set; }
		public string Name1 { get; set; }
		public string Praeferenz_Aktuelles_jahr { get; set; }
		public string Standardlieferant { get; set; }
		public int totalRows { get; set; }

	}
}
