using System;

namespace Psz.Core.CRP.Models.FA
{
	public class FAProductionPlannugModel
	{
		public string Status { get; set; }
		public string Kunde { get; set; }
		public string Atribut { get; set; }
		public int? FA { get; set; }
		public DateTime? Kundentermin { get; set; }
		public DateTime? Plantermin { get; set; }
		public string Bemerkung1 { get; set; }
		public string Bemerkung2 { get; set; }
		public bool? Sonderfertigung { get; set; }
		public string Bemerkung_CS { get; set; }
		public Decimal? Originalmenge { get; set; }
		public Decimal? Menge_erledigt { get; set; }
		public Decimal? Menge_offen { get; set; }
		public string Sysmo { get; set; }
		public string PSZ_Nummer { get; set; }
		public string Freigabestatus { get; set; }
		public Decimal? FA_Zeit { get; set; }
		public Decimal? FA_Lohn { get; set; }
		public Decimal? man { get; set; }
		public string Index { get; set; }
		public DateTime? Indexdatum { get; set; }
		public bool? Technik { get; set; }

		public FAProductionPlannugModel()
		{

		}

		public FAProductionPlannugModel(Infrastructure.Data.Entities.Joins.FAPlannung.FAProduktionPlannungEntity produktionPlannungEntity)
		{
			Status = produktionPlannungEntity.Status;
			Kunde = produktionPlannungEntity.Kunde;
			Atribut = produktionPlannungEntity.Atribut;
			FA = produktionPlannungEntity.FA;
			Kundentermin = produktionPlannungEntity.Kundentermin;
			Plantermin = produktionPlannungEntity.Plantermin;
			Bemerkung1 = produktionPlannungEntity.Bemerkung1;
			Bemerkung2 = produktionPlannungEntity.Bemerkung2;
			Sonderfertigung = produktionPlannungEntity.Sonderfertigung;
			Bemerkung_CS = produktionPlannungEntity.Bemerkung_CS;
			Originalmenge = produktionPlannungEntity.Originalmenge;
			Menge_erledigt = produktionPlannungEntity.Menge_erledigt;
			Menge_offen = produktionPlannungEntity.Menge_offen;
			Sysmo = produktionPlannungEntity.Sysmo;
			PSZ_Nummer = produktionPlannungEntity.PSZ_Nummer;
			Freigabestatus = produktionPlannungEntity.Freigabestatus;
			FA_Zeit = produktionPlannungEntity.FA_Zeit;
			FA_Lohn = produktionPlannungEntity.FA_Lohn;
			man = produktionPlannungEntity.man;
			Index = produktionPlannungEntity.Index;
			Indexdatum = produktionPlannungEntity.Indexdatum;
			Technik = produktionPlannungEntity.Technik;
		}
	}
}
