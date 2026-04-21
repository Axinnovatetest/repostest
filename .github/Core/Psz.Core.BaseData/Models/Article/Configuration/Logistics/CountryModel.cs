namespace Psz.Core.BaseData.Models.Article.Configuration.Logistics
{
	public class CountryModel
	{
		public int ID { get; set; }
		public string Land { get; set; }
		public string Hinweis { get; set; }

		public CountryModel() { }
		public CountryModel(Infrastructure.Data.Entities.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenEntity countryEntity)
		{
			if(countryEntity == null)
				return;

			ID = countryEntity.ID;
			Land = countryEntity.Land;
			Hinweis = countryEntity.Hinweis;

		}
		public Infrastructure.Data.Entities.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenEntity
			{
				ID = ID,
				Land = Land,
				Hinweis = Hinweis
			};
		}
	}
}
