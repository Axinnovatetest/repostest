namespace Psz.Core.FinanceControl.Models.DiscountGroup
{
	public class DiscountGroupModel
	{
		public int Id { get; set; }
		public string Desription { get; set; } // Beschreibung
		public int GroupName { get; set; } // Rabatthauptgruppe

		public DiscountGroupModel()
		{

		}
		public DiscountGroupModel(Infrastructure.Data.Entities.Tables.FNC.RabatthauptGruppenEntity rabatthauptGruppenEntity)
		{
			Id = rabatthauptGruppenEntity.ID;
			Desription = rabatthauptGruppenEntity.Beschreibung;
			GroupName = rabatthauptGruppenEntity.Rabatthauptgruppe;
		}
	}
}
