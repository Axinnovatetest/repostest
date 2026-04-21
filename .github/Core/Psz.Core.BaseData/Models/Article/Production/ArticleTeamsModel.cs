
namespace Psz.Core.BaseData.Models.Article.Production
{
	public class ArticleTeamsResponseModel
	{
		public string Description { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public int? SiteId { get; set; }
		public string SitePrefix { get; set; }
		public char? TeamCategory { get; set; }
		public int? TeamIndex { get; set; }
		public ArticleTeamsResponseModel(Infrastructure.Data.Entities.Tables.BSD.TeamsEntity teamsEntity)
		{
			if(teamsEntity == null)
				return;
			// -
			Description = teamsEntity.Description;
			Id = teamsEntity.Id;
			Name = teamsEntity.Name; // - $"{x.SitePrefix}-TW{x.TeamIndex}-{x.TeamCategory}".Trim(new char[] { '-', ' ' }).ToUpper()
			SiteId = teamsEntity.SiteId;
			SitePrefix = teamsEntity.SitePrefix;
			TeamCategory = teamsEntity.TeamCategory;
			TeamIndex = teamsEntity.TeamIndex;
		}
	}
}
