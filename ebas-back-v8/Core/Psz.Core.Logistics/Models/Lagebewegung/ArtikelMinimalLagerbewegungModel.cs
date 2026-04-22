namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class ArtikelMinimalLagerbewegungModel
	{
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string einheit { get; set; }
		public bool MHD { get; set; }
		public ArtikelMinimalLagerbewegungModel()
		{

		}
		public ArtikelMinimalLagerbewegungModel(Infrastructure.Data.Entities.Tables.Logistics.ArtikelMinimalLagerbewegungEntity artikelEntity)
		{
			if(artikelEntity == null)
				return;
			artikelNr = artikelEntity.artikelNr;
			artikelnummer = artikelEntity.artikelnummer;
			bezeichnung1 = artikelEntity.bezeichnung1;
			einheit = artikelEntity.einheit;
		}
	}
}
