namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class GetArtikelNrReponseModel
	{
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public string Bestellnummer { get; set; }

		public GetArtikelNrReponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.ArtikelNummerBestellenumerFilterEntity artikel)
		{
			ArtikelNr = artikel.Artikel_Nr;
			ArtikelNummer = artikel.Artikelnummer;
			Bestellnummer = artikel.Bestellnummer;
		}
	}
	public class GetArtikelNrRequestModel
	{
		public string Filter { get; set; }
		public bool? IncludeDone { get; set; } = false;
	}
}
