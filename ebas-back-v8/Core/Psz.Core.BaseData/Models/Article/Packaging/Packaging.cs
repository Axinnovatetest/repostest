namespace Psz.Core.BaseData.Models.Article.Packaging
{
	public class Packaging
	{
		public int Id { get; set; }
		public string Artikelnummer { get; set; }
		public string Masse_LxBxH__in_mm_ { get; set; }
		public string Packmittel_Karton { get; set; }
		public Packaging(Infrastructure.Data.Entities.Tables.BSD.Verpackungseinheiten_DefinitionenEntity verpackungseinheiten_DefinitionenEntity)
		{
			if(verpackungseinheiten_DefinitionenEntity == null)
				return;

			Id = verpackungseinheiten_DefinitionenEntity.Id;
			Artikelnummer = verpackungseinheiten_DefinitionenEntity.Artikelnummer;
			Masse_LxBxH__in_mm_ = verpackungseinheiten_DefinitionenEntity.Masse_LxBxH__in_mm_;
			Packmittel_Karton = verpackungseinheiten_DefinitionenEntity.Packmittel_Karton;
		}
		public Infrastructure.Data.Entities.Tables.BSD.Verpackungseinheiten_DefinitionenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Verpackungseinheiten_DefinitionenEntity
			{
				Id = Id,
				Artikelnummer = Artikelnummer,
				Masse_LxBxH__in_mm_ = Masse_LxBxH__in_mm_,
				Packmittel_Karton = Packmittel_Karton
			};
		}
	}
}
