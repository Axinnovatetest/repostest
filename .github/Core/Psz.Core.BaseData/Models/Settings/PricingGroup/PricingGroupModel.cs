namespace Psz.Core.BaseData.Models.PricingGroup
{
	public class PricingGroupModel
	{
		//public int? Surcharge { get; set; }//Aufschlag
		//public int? Surcharge_rate { get; set; }//Aufschlagsatz
		public double? Surcharge { get; set; }//Aufschlag
		public double? Surcharge_rate { get; set; }//Aufschlagsatz
		public string Comment { get; set; }//Bemerkung
		public int Id { get; set; }//ID
		public int? PriceGroup { get; set; }//Preisgruppe

		public PricingGroupModel()
		{

		}

		public PricingGroupModel(Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity priceGroupEntity)
		{
			Surcharge = priceGroupEntity.Aufschlag;
			Surcharge_rate = priceGroupEntity.Aufschlagsatz;
			Comment = priceGroupEntity.Bemerkung;
			Id = priceGroupEntity.ID;
			PriceGroup = priceGroupEntity.Preisgruppe;
		}

		public Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity ToEntity(int group)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity
			{
				Aufschlag = Surcharge,
				Aufschlagsatz = Surcharge_rate,
				Bemerkung = Comment,
				ID = Id,
				Preisgruppe = (int)group,
			};
		}
	}
}
