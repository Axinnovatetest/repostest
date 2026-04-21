namespace Psz.Core.BaseData.Models.AddressType
{
	public class AdreesTypeModel
	{
		public int Id { get; set; }
		public string Description { get; set; }

		public AdreesTypeModel() { }
		public AdreesTypeModel(Infrastructure.Data.Entities.Tables.BSD.Adressen_typenEntity adressen_TypenEntity)
		{
			Id = adressen_TypenEntity.ID_typ;
			Description = adressen_TypenEntity.Bezeichnung;
		}

		public Infrastructure.Data.Entities.Tables.BSD.Adressen_typenEntity ToTypenEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Adressen_typenEntity
			{
				ID_typ = Id,
				Bezeichnung = Description
			};
		}
	}
}
