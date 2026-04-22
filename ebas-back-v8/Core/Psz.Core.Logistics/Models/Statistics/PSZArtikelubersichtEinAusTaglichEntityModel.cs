using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class PSZArtikelubersichtEinAusTaglichEntityModel
	{
		public PSZArtikelubersichtEinAusTaglichEntityModel(Infrastructure.Data.Entities.Joins.Logistics.PSZArtikelubersichtEinAusTaglichEntity PSZArtikelubersichtEinAusTaglichEntity)
		{
			if(PSZArtikelubersichtEinAusTaglichEntity == null)
			{
				return;
			}
			Artikelnummer = PSZArtikelubersichtEinAusTaglichEntity.Artikelnummer;
			Typ = PSZArtikelubersichtEinAusTaglichEntity.Typ;
			BestellungNr = PSZArtikelubersichtEinAusTaglichEntity.BestellungNr;
			Anzahl = PSZArtikelubersichtEinAusTaglichEntity.Anzahl;
			Datum = PSZArtikelubersichtEinAusTaglichEntity.Datum;
			Name1 = PSZArtikelubersichtEinAusTaglichEntity.Name1;
			Lagerplatz_von = PSZArtikelubersichtEinAusTaglichEntity.Lagerplatz_von;
			Lagerplatz_nach = PSZArtikelubersichtEinAusTaglichEntity.Lagerplatz_nach;
			Mindestbestellmenge = PSZArtikelubersichtEinAusTaglichEntity.Mindestbestellmenge;
			Verpackungseinheit = PSZArtikelubersichtEinAusTaglichEntity.Verpackungseinheit;
			totalRows = PSZArtikelubersichtEinAusTaglichEntity.totalRows;
		}
		public string Artikelnummer { get; set; }
		public string Typ { get; set; }
		public int? BestellungNr { get; set; }
		public decimal? Anzahl { get; set; }
		public DateTime? Datum { get; set; }
		public string Name1 { get; set; }
		public int Lagerplatz_von { get; set; }
		public int Lagerplatz_nach { get; set; }
		public decimal? Mindestbestellmenge { get; set; }
		public decimal? Verpackungseinheit { get; set; }
		public int totalRows { get; set; }
	}
}
