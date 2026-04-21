namespace Psz.Core.BaseData.Models.Article.Purchase
{
	public class CustomPriceModel
	{
		public int Id { get; set; }
		public decimal Price { get; set; }
		public decimal QuantityFrom { get; set; }
		public int OrderNumber { get; set; }
		public CustomPriceModel()
		{

		}
		public CustomPriceModel(Infrastructure.Data.Entities.Tables.BSD.Bestellnummern_StaffelpreiseEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// -
			Id = entity.ID;
			Price = entity.Einkaufspreis ?? 0;
			QuantityFrom = entity.ab_Anzahl ?? 0;
			OrderNumber = entity.nummer ?? 0;
		}
		public Infrastructure.Data.Entities.Tables.BSD.Bestellnummern_StaffelpreiseEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Bestellnummern_StaffelpreiseEntity
			{
				ID = Id,
				Einkaufspreis = Price,
				ab_Anzahl = QuantityFrom,
				nummer = OrderNumber,
			};
		}
	}
	public class CustomerPriceRequestModel
	{
	}
}
