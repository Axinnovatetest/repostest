namespace Psz.Core.FinanceControl.Models.AddressType
{
	public class GetModel
	{
		public int Id { get; set; }
		public string Description { get; set; }

		public GetModel() { }
		public GetModel(Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity adressen_TypenEntity)
		{
			Id = adressen_TypenEntity.ID_typ;
			Description = adressen_TypenEntity.Bezeichnung;
		}

		public Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity ToTypenEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity
			{
				ID_typ = Id,
				Bezeichnung = Description
			};
		}
	}
}
