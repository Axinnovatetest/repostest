namespace Psz.Core.BaseData.Models.Article.Configuration
{
	public class ArticleSampleModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ArticleSampleModel(Infrastructure.Data.Entities.Tables.BSD.ArticleSampleEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArticleSampleEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArticleSampleEntity
			{
				Id = Id,
				Name = Name
			};
		}
	}
}
