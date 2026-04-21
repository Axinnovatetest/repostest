using System;

namespace Psz.Core.CRP.Models.FA
{
	public class FAUpdatedFromExcelModel
	{
		public int? Fertigungsnummer { get; set; }
		public string? Artikelnummer { get; set; }
		public DateTime? Werk_termin { get; set; }

		public FAUpdatedFromExcelModel()
		{

		}

		public FAUpdatedFromExcelModel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity FAEntity)
		{
			var ArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)FAEntity.Artikel_Nr);
			Fertigungsnummer = FAEntity.Fertigungsnummer;
			Artikelnummer = ArtikelEntity?.ArtikelNummer;
			Werk_termin = FAEntity.Termin_Bestatigt2 ?? null;
		}
	}
}
