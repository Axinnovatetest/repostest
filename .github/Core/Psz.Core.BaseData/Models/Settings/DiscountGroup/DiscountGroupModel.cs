namespace Psz.Core.BaseData.Models.DiscountGroup
{
	public class DiscountGroupModel
	{
		public int Id { get; set; }
		public string Description { get; set; } // Beschreibung
		public int GroupName { get; set; } // Rabatthauptgruppe

		public DiscountGroupModel()
		{

		}
		public DiscountGroupModel(Infrastructure.Data.Entities.Tables.BSD.RabatthauptGruppenEntity rabatthauptGruppenEntity)
		{
			Id = rabatthauptGruppenEntity.ID;
			Description = rabatthauptGruppenEntity.Beschreibung;
			GroupName = rabatthauptGruppenEntity.Rabatthauptgruppe;
		}

		public Infrastructure.Data.Entities.Tables.BSD.RabatthauptGruppenEntity ToEntity(int id)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.RabatthauptGruppenEntity
			{
				ID = Id,
				Beschreibung = Description,
				Rabatthauptgruppe = id
			};
		}
	}
}
