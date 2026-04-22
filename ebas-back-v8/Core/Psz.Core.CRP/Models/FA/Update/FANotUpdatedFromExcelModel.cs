using System;

namespace Psz.Core.CRP.Models.FA
{
	public class FANotUpdatedFromExcelModel
	{
		public int? Fertigungsnummer { get; set; }
		public string? Artikelnummer { get; set; }
		public DateTime? Produktionstermin { get; set; }
		public string? Kennzeich { get; set; }

		public FANotUpdatedFromExcelModel()
		{

		}

		public FANotUpdatedFromExcelModel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity FAEntity)
		{
			var ArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)FAEntity.Artikel_Nr);
			Fertigungsnummer = FAEntity.Fertigungsnummer;
			Artikelnummer = ArtikelEntity?.ArtikelNummer;
			Produktionstermin = FAEntity.Termin_Bestatigt2 ?? null;
			Kennzeich = FAEntity.Kennzeichen;
		}
	}
}
