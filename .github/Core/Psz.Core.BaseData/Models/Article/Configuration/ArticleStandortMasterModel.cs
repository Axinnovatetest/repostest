namespace Psz.Core.BaseData.Models.Article.Configuration
{
	public class ArticleStandortMasterModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ArticleStandortMasterModel(Infrastructure.Data.Entities.Tables.BSD.ArticleStandortMasterEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArticleStandortMasterEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArticleStandortMasterEntity
			{
				Id = Id,
				Name = Name
			};
		}
	}
}
