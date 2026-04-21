using System;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class KapazitatKurzModel
	{
		public string Kunde { get; set; }
		public int? FA { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public DateTime? TerminProd { get; set; }
		public int? Auftragsmenge { get; set; }
		public string Artikelnummer { get; set; }
		public string Kunden { get; set; }
		public Decimal? Auftragszeit_h { get; set; }
		public Decimal? UmsatzCZ { get; set; }
		public Decimal? AnzahlMA { get; set; }
		public int? Lagerort_id { get; set; }
		public Decimal? Stundensatz { get; set; }
		public KapazitatKurzModel()
		{

		}

		public KapazitatKurzModel(Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity entity)
		{
			Kunde = entity.Kunde;
			FA = entity.FA;
			Wunschtermin = entity.Wunschtermin;
			TerminProd = entity.TerminProd;
			Auftragsmenge = entity.Auftragsmenge;
			Artikelnummer = entity.Artikelnummer;
			Kunden = entity.Kunden;
			Auftragszeit_h = entity.Auftragszeit_h;
			UmsatzCZ = entity.UmsatzCZ;
			AnzahlMA = entity.AnzahlMA;
			Lagerort_id = entity.Lagerort_id;
			Stundensatz = entity.Stundensatz;
		}
	}
}
