namespace Psz.Core.Apps.EDI.Models.Delfor.Production
{
	public class OriginalArticleModel
	{
		public int Id { get; set; } // artikel-nr
		public string Nummer { get; set; } // nummer
		public string Description { get; set; } // Bezeichnung 1
		public string ReleaseStatus { get; set; } // freigabestatus

		public OriginalArticleModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			Id = artikelEntity.ArtikelNr;
			Nummer = artikelEntity.ArtikelNummer;
			Description = artikelEntity.Bezeichnung1;
			ReleaseStatus = artikelEntity.Freigabestatus;
		}
	}
}
