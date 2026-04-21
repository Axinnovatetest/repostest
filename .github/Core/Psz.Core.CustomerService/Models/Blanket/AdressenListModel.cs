namespace Psz.Core.CustomerService.Models.Blanket
{
	public class AdressenListModel
	{
		public string Name1 { get; set; }
		public int? Lieferantennummer { get; set; }
		public string Ort { get; set; }
		public int Nradressen { get; set; }
		public int Nrsupplier { get; set; }

		public string Symbol { get; set; }
		public AdressenListModel()
		{

		}

		public AdressenListModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressEntity)
		{
			Name1 = adressEntity.Name1;
			Lieferantennummer = adressEntity.Lieferantennummer;
			Ort = adressEntity.Ort;
			Nradressen = adressEntity.Nr;
			//Nrsupplier = (Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(adressEntity.Nr) != null) ? Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(adressEntity.Nr).Nr : -1;
		}
	}
}
