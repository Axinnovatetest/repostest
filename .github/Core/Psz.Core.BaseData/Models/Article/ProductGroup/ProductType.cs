namespace Psz.Core.BaseData.Models.Article.ProductGroup
{
	public class ProductType
	{
		public int Warentyp_ID { get; set; }
		public string Warentyp { get; set; }
		public ProductType(Infrastructure.Data.Entities.Tables.BSD.WarentypEntity warentypEntity)
		{
			if(warentypEntity == null)
				return;

			Warentyp_ID = warentypEntity.Warentyp_ID;
			Warentyp = warentypEntity.Warentyp;
		}
	}
}
