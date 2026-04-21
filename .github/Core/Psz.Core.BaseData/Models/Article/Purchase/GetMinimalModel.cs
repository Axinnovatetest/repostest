namespace Psz.Core.BaseData.Models.Article.Purchase
{
	public class GetMinimalModel
	{
		public int? Lieferanten_Nr { get; set; }
		public int? Lieferantennummer { get; set; }
		public string Lieferantenname { get; set; }
		public bool? Standardlieferant { get; set; }
		public double? Einkaufspreis { get; set; }
		public double? Einkaufspreis1 { get; set; }
		public string Symbol { get; set; }
		public string Einkaufspreis1_gultig_bis { get; set; }
		public double? Umsatzsteuer { get; set; }
		public decimal? Rabatt { get; set; }
		public double? Preiseinheit { get; set; }
		public double? Einkaufspreis2 { get; set; }
		public string Einkaufspreis2_gultig_bis { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }
		public double? Pruftiefe_WE { get; set; }
		public int Nr { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		public string Angebot { get; set; }
		public string Angebot_Datum { get; set; }
		public string Bestell_Nr { get; set; }
		public double? Mindestbestellmenge { get; set; }
		public double? Verpackungseinheit { get; set; }
		public string Warengruppe { get; set; }
		public string Bemerkungen { get; set; }
		public int? ArtikelNr { get; set; }
		public int CustomPricesCount { get; set; } = 0;
		public int SupplierId { get; set; }
		public int OfferId { get; set; }
		public int? FileId { get; set; }

		public GetMinimalModel(Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity entity)
		{
			Lieferanten_Nr = entity.Lieferanten_Nr;
			Standardlieferant = entity.Standardlieferant;
			Einkaufspreis = entity.Einkaufspreis;
			Einkaufspreis1 = entity.Einkaufspreis1;
			Einkaufspreis1_gultig_bis = entity.Einkaufspreis1_gultig_bis?.ToString("dd/MM/yyyy");
			Umsatzsteuer = entity.Umsatzsteuer;
			Rabatt = entity.Rabatt;
			Preiseinheit = entity.Preiseinheit;
			Einkaufspreis2 = entity.Einkaufspreis2;
			Einkaufspreis2_gultig_bis = entity.Einkaufspreis2_gultig_bis?.ToString("dd/MM/yyyy");
			Wiederbeschaffungszeitraum = entity.Wiederbeschaffungszeitraum;
			Nr = entity.Nr;
			Artikelbezeichnung = entity.Artikelbezeichnung;
			Artikelbezeichnung2 = entity.Artikelbezeichnung2;
			Angebot = entity.Angebot;
			Angebot_Datum = entity.Angebot_Datum?.ToString("dd/MM/yyyy");
			Bestell_Nr = entity.Bestell_Nr;
			Mindestbestellmenge = entity.Mindestbestellmenge;
			Verpackungseinheit = entity.Verpackungseinheit;
			Warengruppe = entity.Warengruppe;
			Bemerkungen = entity.Bemerkungen;
			ArtikelNr = entity.ArtikelNr;
			switch(entity.Pruftiefe_WE)
			{
				case 1:
					Pruftiefe_WE = 1;
					break;
				case 2:
					Pruftiefe_WE = 0.5;
					break;
				case 3:
					Pruftiefe_WE = 0.05;
					break;
				default:
					Pruftiefe_WE = 0.01;
					break;
			}
		}
	}
}
