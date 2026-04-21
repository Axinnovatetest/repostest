namespace Psz.Core.BaseData.Models.Article.ProductGroup
{
	public class ProductGroup
	{
		public int ID { get; set; }
		public string Bezeichnung { get; set; }
		public string Hinweis { get; set; }
		public string Warengruppe { get; set; }

		public ProductGroup(Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity warengruppenEntity)
		{
			if(warengruppenEntity == null)
				return;

			ID = warengruppenEntity.ID;
			Bezeichnung = warengruppenEntity.Bezeichnung;
			Hinweis = warengruppenEntity.Hinweis;
			Warengruppe = warengruppenEntity.Warengruppe;

		}

		public Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity
			{
				ID = ID,
				Bezeichnung = Bezeichnung,
				Hinweis = Hinweis,
				Warengruppe = Warengruppe
			};
		}
	}
}
