namespace Psz.Core.BaseData.Models.ShippingMethod
{
	public class ShippingMethodModel
	{
		public int ID { get; set; }
		public string ShippingMethod { get; set; }

		public ShippingMethodModel()
		{

		}

		public ShippingMethodModel(Infrastructure.Data.Entities.Tables.BSD.Versandarten_AuswahlEntity shippingMethodEntity)
		{
			ID = shippingMethodEntity.ID;
			ShippingMethod = shippingMethodEntity.Versandarten;
		}

		public Infrastructure.Data.Entities.Tables.BSD.Versandarten_AuswahlEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Versandarten_AuswahlEntity
			{
				ID = ID,
				Versandarten = ShippingMethod,
			};
		}
	}
}
