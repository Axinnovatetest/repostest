

namespace Psz.Core.CRP.Models.FA
{
	public class FAListModule
	{
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichung_1 { get; set; }
		public int? FA_Menge { get; set; }
		public string FA_Status { get; set; }
		public DateTime? Produktionstermin { get; set; }
		public string Lager { get; set; }
		public bool Gestartet { get; set; }
		// - 2022-06-20
		public int FaId { get; set; }
		public int? Artikel_Nr { get; set; }
		public string ArticleIndexKunde { get; set; }
		public bool? Prio { get; set; }
		public FAListModule()
		{

		}
		public FAListModule(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity FAEntity)
		{
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(FAEntity.Artikel_Nr.HasValue ? FAEntity.Artikel_Nr.Value : -1);
			var lagerEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForFAStcklist(new List<int> { FAEntity.Lagerort_id ?? -1 });
			if(FAEntity != null)
			{
				Fertigungsnummer = FAEntity.Fertigungsnummer;
				FA_Menge = FAEntity.Originalanzahl;
				FA_Status = FAEntity.Kennzeichen;
				Produktionstermin = FAEntity.Termin_Bestatigt1;
				//
				Artikelnummer = articleEntity?.ArtikelNummer;
				Bezeichung_1 = articleEntity?.Bezeichnung1;
				//
				Lager = lagerEntity != null && lagerEntity.Count > 0 ? lagerEntity[0]?.Lagerort : null;
				Gestartet = FAEntity.FA_Gestartet ?? false;
				// - 2022-06-20
				FaId = FAEntity.ID;
				ArticleIndexKunde = FAEntity.KundenIndex;
				//- 15-05-2025
				Prio = FAEntity.Prio ?? false;
				Artikel_Nr = FAEntity.Artikel_Nr;
			}
		}
	}
}