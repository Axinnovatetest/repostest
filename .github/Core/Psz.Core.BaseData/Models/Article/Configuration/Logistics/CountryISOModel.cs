namespace Psz.Core.BaseData.Models.Article.Configuration.Logistics
{
	public class CountryISOModel
	{
		public int Id { get; set; }
		public string alpha2Code { get; set; }
		public string alpha3Code { get; set; }
		public string Capital { get; set; }
		public string Description { get; set; }
		public string Flag { get; set; }
		public string Name { get; set; }
		public string NativeName { get; set; }
		public string NumericCode { get; set; }
		public string Region { get; set; }
		public string Subregion { get; set; }

		public CountryISOModel() { }
		public CountryISOModel(Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity countryISOEntity)
		{
			if(countryISOEntity == null)
				return;

			Id = countryISOEntity.Id;
			alpha2Code = countryISOEntity.alpha2Code;
			alpha3Code = countryISOEntity.alpha3Code;
			Capital = countryISOEntity.Capital;
			Description = countryISOEntity.Description;
			Flag = countryISOEntity.Flag;
			Name = countryISOEntity.Name;
			NativeName = countryISOEntity.NativeName;
			NumericCode = countryISOEntity.NumericCode;
			Region = countryISOEntity.Region;
			Subregion = countryISOEntity.Subregion;

		}
		public Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity
			{
				Id = Id,
				alpha2Code = alpha2Code,
				alpha3Code = alpha3Code,
				Capital = Capital,
				Description = Description,
				Flag = Flag,
				Name = Name,
				NativeName = NativeName,
				NumericCode = NumericCode,
				Region = Region,
				Subregion = Subregion
			};
		}
	}
}
