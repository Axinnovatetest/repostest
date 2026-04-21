namespace Psz.Core.Apps.WorkPlan.Handlers.Article
{
	public partial class Article
	{
		internal class Helpers
		{
			public static Infrastructure.Data.Entities.Tables.WPL.ArticleEntity ToDataEntity(Models.Article.ArticleModel data)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.ArticleEntity
				{
					Name = data.Name,
					Id = data.Id,
				};
			}
		}
	}
}
