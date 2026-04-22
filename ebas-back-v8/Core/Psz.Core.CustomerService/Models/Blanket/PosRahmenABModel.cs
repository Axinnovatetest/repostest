using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class PosRahmenABModel
	{
		public int? Nr { get; set; }
		public int? Position { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Bezeichnung2 { get; set; }
		public string Bezeichnung3 { get; set; }
		public string Einheit { get; set; }
		public decimal? Menge { get; set; }
		public decimal? RefWert { get; set; }
		public decimal? Geliefert { get; set; }
		public int? ArtieklNr { get; set; }
		public string ArtikelNummer { get; set; }
		public string WarungName { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? DateOfExpiry { get; set; }

		public PosRahmenABModel()
		{ }
		public PosRahmenABModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity Entity, Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEtnity, Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity extensionEnnity)
		{
			Nr = Entity?.Nr;
			Position = Entity?.Position;
			Bezeichnung1 = Entity.Bezeichnung1;
			Bezeichnung2 = Entity.Bezeichnung2;
			Bezeichnung3 = Entity.Bezeichnung3;
			Einheit = Entity.Einheit;
			Menge = Entity.OriginalAnzahl;
			RefWert = Entity.Gesamtpreis;
			Geliefert = Entity.Geliefert;
			ArtieklNr = Entity.ArtikelNr;
			ArtikelNummer = artikelEtnity.ArtikelNummer;
			WarungName = extensionEnnity.WahrungName;
			ValidFrom = extensionEnnity?.GultigAb;
			DateOfExpiry = extensionEnnity?.GultigBis;
		}

	}
}
