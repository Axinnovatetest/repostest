using System;
using System.Collections.Generic;

namespace Psz.Core.CRP.Models.FA
{
	public class OpenFAForUpdateModel
	{
		public int? Fertigungsnummer { get; set; }
		public string? Artikelnummer { get; set; }
		public string? Kennzeichen { get; set; }
		public bool? UpdateS { get; set; }
		public int? Artikel_Nr { get; set; }
		public int? ID_Fer { get; set; }
		public int? Type_Update { get; set; }
		public bool? gedruckt { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public int? Lagerort_id { get; set; }
		public int? BomVersion { get; set; }
		public int? CPVersion { get; set; }
		public string? Index { get; set; }
		public string? Lager { get; set; }
		public DateTime? Datum { get; set; }
		public Decimal? Zeit { get; set; }

		public OpenFAForUpdateModel()
		{

		}
		public OpenFAForUpdateModel(Infrastructure.Data.Entities.Joins.FAUpdate.OpenFABETNEntity entity)
		{
			var lagerEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForFAStcklist(new List<int> { (int)entity.Lagerort_id });
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer((int)entity.Fertigungsnummer);
			Fertigungsnummer = entity.Fertigungsnummer;
			Artikelnummer = entity.Artikelnummer;
			Kennzeichen = entity.Kennzeichen;
			UpdateS = entity.UpdateS;
			Artikel_Nr = entity.Artikel_Nr;
			ID_Fer = entity.ID_Fer;
			Type_Update = entity.Type_Update;
			gedruckt = entity.gedruckt;
			FA_Druckdatum = entity.FA_Druckdatum;
			Lagerort_id = entity.Lagerort_id;
			BomVersion = entity.BomVersion;
			CPVersion = entity.CPVersion;
			Lager = lagerEntity[0]?.Lagerort;
			Datum = faEntity.Datum;
			Index = faEntity.KundenIndex;
			Zeit = faEntity.Zeit;
		}
		public OpenFAForUpdateModel(Infrastructure.Data.Entities.Joins.FAUpdate.OpenFANotVersionningEntity entity)
		{
			var lagerEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForFAStcklist(new List<int> { (int)entity.Lagerort_id });
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer((int)entity.Fertigungsnummer);
			Fertigungsnummer = entity.Fertigungsnummer;
			Artikelnummer = entity.Artikelnummer;
			Kennzeichen = entity.Kennzeichen;
			UpdateS = entity.UpdateS;
			Artikel_Nr = entity.Artikel_Nr;
			ID_Fer = entity.ID_Fer;
			Type_Update = entity.Type_Update;
			gedruckt = entity.gedruckt;
			FA_Druckdatum = entity.FA_Druckdatum;
			Lagerort_id = entity.Lagerort_id;
			Lager = lagerEntity[0]?.Lagerort;
			Datum = faEntity.Datum;
			Index = faEntity.KundenIndex;
			Zeit = faEntity.Zeit;
		}
	}
}
