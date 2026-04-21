namespace Psz.Core.BaseData.Models.Article.Statistics.Technic
{
	public class TechnicianResponseModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public TechnicianResponseModel()
		{

		}

		public TechnicianResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.TechnicTechnikerEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.ID;
			Name = entity.Name;
		}
		public TechnicianResponseModel(Infrastructure.Data.Entities.Tables.COR.UserEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
		}
	}

	public class TechnicianModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public TechnicianModel()
		{

		}
		public Infrastructure.Data.Entities.Tables.BSD.PSZ_TechnikerEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.PSZ_TechnikerEntity
			{
				ID = Id,
				Name = Name
			};
		}
	}
}
