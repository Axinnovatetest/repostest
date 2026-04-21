
namespace Psz.Core.CRP.Models.FA
{
	public class FAStucklistModel
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public Decimal? Anzahl { get; set; }
		public int? Vorgang_Nr { get; set; }
		public bool? ME_gebucht { get; set; }
		public int? Lagerort_ID { get; set; }
		// - 2022-09-29
		public bool IsUBG { get; set; } = false;
		public int UBGFertigungsnummer { get; set; }
		public int UBGFertigungsid { get; set; }
		public int ArticleId { get; set; }
		// - 2022-10-24
		public int UBGFaId { get; set; }
		public int UBGFaNummer { get; set; }
		public FAStucklistModel()
		{

		}

		public FAStucklistModel(Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity FAStucklitEntiy)
		{
			Anzahl = (decimal?)FAStucklitEntiy.Anzahl;
			Vorgang_Nr = FAStucklitEntiy.Vorgang_Nr;
			ME_gebucht = FAStucklitEntiy.ME_gebucht;
			Lagerort_ID = FAStucklitEntiy.Lagerort_ID;

			var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(FAStucklitEntiy.Artikel_Nr ?? -1);
			Artikelnummer = artikelEntity?.ArtikelNummer;
			Bezeichnung_1 = artikelEntity?.Bezeichnung1;
			// -
			IsUBG = artikelEntity?.UBG ?? false;
			ArticleId = FAStucklitEntiy.Artikel_Nr ?? -1;
			// -
			UBGFaId = FAStucklitEntiy.UBGFertigungsId ?? -1;
			UBGFaNummer = FAStucklitEntiy.UBGFertigungsnummer ?? -1;
		}
	}
}
