namespace Psz.Core.Logistics.Models.Statistics
{
	public class ROHOhneBedarfModel
	{
		public ROHOhneBedarfModel(Infrastructure.Data.Entities.Joins.Logistics.ROHOhneBedarfEntity ROHOhneBedarfEntity)
		{

			if(ROHOhneBedarfEntity == null)
				return;

			Bestand = ROHOhneBedarfEntity.Bestand;
			Artikelnummer = ROHOhneBedarfEntity.Artikelnummer;
			Bezeichnung1 = ROHOhneBedarfEntity.Bezeichnung1;
			Einkaufspreis = ROHOhneBedarfEntity.Einkaufspreis;



		}
		public decimal Bestand { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal Einkaufspreis { get; set; }
	}
}
