namespace Psz.Core.BaseData.Models.Supplier
{
	public class AdressenListModel
	{
		public string Name1 { get; set; }
		public int? Lieferantennummer { get; set; }
		public string Ort { get; set; }
		public int Nr { get; set; }
		public string Symbol { get; set; }
		public AdressenListModel()
		{

		}

		public AdressenListModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressEntity)
		{
			Name1 = adressEntity.Name1;
			Lieferantennummer = adressEntity.Lieferantennummer;
			Ort = adressEntity.Ort;
			Nr = adressEntity.Nr;
		}
	}

}
