namespace Psz.Core.BaseData.Models.Article.Configuration
{
	public class ArticleStandortSerieModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ArticleStandortSerieModel(Infrastructure.Data.Entities.Tables.BSD.ArticleStandortSerieEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArticleStandortSerieEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArticleStandortSerieEntity
			{
				Id = Id,
				Name = Name,
			};
		}
	}
}
