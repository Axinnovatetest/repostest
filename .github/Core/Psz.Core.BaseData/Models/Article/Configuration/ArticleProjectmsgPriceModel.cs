namespace Psz.Core.BaseData.Models.Article.Configuration
{
	public class ArticleProjectmsgPriceModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Value { get; set; }
		public ArticleProjectmsgPriceModel(Infrastructure.Data.Entities.Tables.BSD.ArticleProjectmsgPriceEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
			Value = entity.Value ?? 0;
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArticleProjectmsgPriceEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArticleProjectmsgPriceEntity
			{
				Id = Id,
				Name = Name,
				Value = Value
			};
		}
	}
}
