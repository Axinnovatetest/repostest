namespace Psz.Core.Logistics.Models.Statistics
{
	public class CableWithoutOrderModel
	{
		public CableWithoutOrderModel(Infrastructure.Data.Entities.Joins.Logistics.CableWithoutOrderEntity CableWithoutOrderEntity)
		{
			if(CableWithoutOrderEntity == null)
			{
				return;
			}
			Bestand = CableWithoutOrderEntity.Bestand;
			Artikelnummer = CableWithoutOrderEntity.Artikelnummer;
			Bezeichnung1 = CableWithoutOrderEntity.Bezeichnung1;


		}
		public decimal Bestand { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
	}
}
