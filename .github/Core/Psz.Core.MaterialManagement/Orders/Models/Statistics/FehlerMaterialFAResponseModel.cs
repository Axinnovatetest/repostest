namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class FehlerMaterialFAList
	{
		public int? Artikel_nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichung1 { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? BedarfGesamt { get; set; }
		public decimal? Verfugbar { get; set; }
		public string Name1 { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public string Lagerort { get; set; }
		public FehlerMaterialFAList()
		{

		}
		public FehlerMaterialFAList(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FehlermaterialFAEntity entity)
		{
			Artikel_nr = entity.Artikel_nr;
			Artikelnummer = entity.Artikelnummer;
			Bezeichung1 = entity.Bezeichung1;
			Bestand = Math.Round(entity.Bestand ?? 0m, 2);
			BedarfGesamt = Math.Round(entity.BedarfGesamt ?? 0m, 2);
			Verfugbar = Math.Round(entity.Verfugbar ?? 0m, 2);
			Name1 = entity.Name1;
			Einkaufspreis = Math.Round(entity.Einkaufspreis ?? 0m, 3);
			Lagerort = entity.Lagerort;
		}
	}
	public class FehlerMaterialFAResponseModel
	{
		public string Artikelnummer { get; set; }
		public string Lagerort { get; set; }
		public List<FehlerMaterialFAList> FehlerMaterialList { get; set; }
	}
	public class FehlerMaterialFARequestModel
	{
		public int Artikel_nr { get; set; }
		public decimal Menge { get; set; }
		public DateTime Date { get; set; }
		public int Lager { get; set; }
	}
}
