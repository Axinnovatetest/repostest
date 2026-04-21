namespace Psz.Core.BaseData.Models.Settings.SalutationContactPerson
{
	public class SalutationContactPersonModel
	{
		public int Id { get; set; }
		public string Salutation { get; set; }

		public SalutationContactPersonModel()
		{

		}

		public SalutationContactPersonModel(Infrastructure.Data.Entities.Tables.BSD.Adressen_briefanredenEntity adressContactPersonEntity)
		{
			Id = adressContactPersonEntity.ID;
			Salutation = adressContactPersonEntity.Anrede;
		}

		public Infrastructure.Data.Entities.Tables.BSD.Adressen_briefanredenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Adressen_briefanredenEntity
			{
				ID = Id,
				Anrede = Salutation,
			};
		}
	}
}
