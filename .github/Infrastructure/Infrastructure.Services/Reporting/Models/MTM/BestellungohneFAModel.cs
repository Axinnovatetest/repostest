using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;

namespace Infrastructure.Services.Reporting.Models.MTM
{
	public class BestellungohneFAModel
	{
		public string Artikelnummer { get; set; }
		public string Lieferant { get; set; }
		public string Bestellung_Nr { get; set; }
		public string Anzahl { get; set; }
		public string Wunschtermin { get; set; }//
		public string Bestatigter_Termin { get; set; }//
		public BestellungohneFAModel(BestellungohneFAEntity data)
		{
			Artikelnummer = data.Artikelnummer ?? string.Empty;
			Lieferant = data.Lieferant ?? string.Empty;
			Bestellung_Nr = data.Bestellung_Nr.ToString();
			Anzahl = data.Anzahl.ToString();
			Wunschtermin = data.Wunschtermin.Value.ToString("dd/MM/yyyy");
			Bestatigter_Termin = data.Bestatigter_Termin.Value.ToString("dd/MM/yyyy");
		}
	}
	public class Articles
	{

		public string Artikelnummer { get; set; }
		public Articles(string artiklenummer)
		{
			Artikelnummer = artiklenummer ?? "";
		}
	}
	public class PlantRegionModel
	{
		public string plantlocation { get; set; }

		public PlantRegionModel(string plantloc)
		{
			plantlocation = plantloc ?? "";
		}
	}

	public class UserModelBestelle
	{
		public string UserName { get; set; }

		public UserModelBestelle(string userName)
		{
			UserName = userName ?? "";
		}
	}
}
