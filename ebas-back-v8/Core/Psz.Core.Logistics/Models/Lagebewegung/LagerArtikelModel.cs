namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class LagerArtikelModel
	{
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public int lagerID { get; set; }
		public decimal bestand { get; set; }
		//------------------Pour Transfer---------------
		public decimal bestandOriginal { get; set; }
		public LagerArtikelModel()
		{
		}

		public LagerArtikelModel(Infrastructure.Data.Entities.Tables.Logistics.LagerArtikelEntity lagerArtikelEntity)
		{
			if(lagerArtikelEntity == null)
				return;
			artikelNr = lagerArtikelEntity.artikelNr;
			artikelnummer = lagerArtikelEntity.artikelnummer;
			lagerID = lagerArtikelEntity.lagerID;
			bestand = lagerArtikelEntity.bestand;
			bestandOriginal = lagerArtikelEntity.bestand;
		}
	}
}
