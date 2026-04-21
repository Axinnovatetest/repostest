namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FAUpdatedFromExcelModel
	{
		public string Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Werk_termin { get; set; }
		public int Id { get; set; }

		public FAUpdatedFromExcelModel()
		{

		}

		public FAUpdatedFromExcelModel(int werkOrwunch, Infrastructure.Data.Entities.Tables.PRS.FertigungEntity FAEntity)
		{
			if(FAEntity == null)
				return;
			var ArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)FAEntity.Artikel_Nr);
			Fertigungsnummer = FAEntity.Fertigungsnummer.HasValue ? FAEntity.Fertigungsnummer.Value.ToString() : "";
			Artikelnummer = ArtikelEntity?.ArtikelNummer;
			Id = FAEntity.ID;
			if(werkOrwunch == 1)
				Werk_termin = FAEntity.Termin_Bestatigt2.HasValue ? FAEntity.Termin_Bestatigt2.Value.ToString("dd/MM/yyyy") : "";
			else
				Werk_termin = FAEntity.Termin_Bestatigt1.HasValue ? FAEntity.Termin_Bestatigt1.Value.ToString("dd/MM/yyyy") : "";
		}

		public FAUpdatedFromExcelModel(Infrastructure.Data.Entities.Joins.FAExcelUpdate.FAWunshUpdateEntity updatedEntity)
		{
			if(updatedEntity == null)
				return;
			Fertigungsnummer = updatedEntity.Fertigungsnummer.HasValue ? updatedEntity.Fertigungsnummer.Value.ToString() : "";
			Artikelnummer = updatedEntity.Artikelnummer;
			Werk_termin = updatedEntity.Termin_Bestätigt1;
			Id = updatedEntity.FertigungId;
		}

		public FAUpdatedFromExcelModel(Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity entity)
		{
			if(entity == null)
				return;
			Fertigungsnummer = entity.Fertigungsnummer.HasValue ? entity.Fertigungsnummer.Value.ToString() : "";
			Artikelnummer = entity.Artikelnummer;
			Werk_termin = entity.Termin_Bestätigt1.HasValue ? entity.Termin_Bestätigt1.Value.ToString("dd/MM/yyyy") : "";
			Id = entity.FertigungId;
		}
	}
}
