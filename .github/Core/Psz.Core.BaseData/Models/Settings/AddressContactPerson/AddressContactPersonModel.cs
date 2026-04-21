namespace Psz.Core.BaseData.Models.Settings.AddressContactPerson
{
	public class AddressContactPersonModel
	{
		public int Id { get; set; }
		public string Description { get; set; }

		public AddressContactPersonModel()
		{

		}

		public AddressContactPersonModel(Infrastructure.Data.Entities.Tables.BSD.Adressen_anredenEntity adressContactPersonEntity)
		{
			Id = adressContactPersonEntity.ID;
			Description = adressContactPersonEntity.Anrede;
		}

		public Infrastructure.Data.Entities.Tables.BSD.Adressen_anredenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Adressen_anredenEntity
			{
				ID = Id,
				Anrede = Description,
			};
		}
	}
}
