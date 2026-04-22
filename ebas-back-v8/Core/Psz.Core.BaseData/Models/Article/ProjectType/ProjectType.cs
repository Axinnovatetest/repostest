namespace Psz.Core.BaseData.Models.Article.ProjectType
{
	public class ProjectType
	{
		public string Description { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }

		public ProjectType(Infrastructure.Data.Entities.Tables.BSD.ProjectTypeEntity projectTypeEntity)
		{
			if(projectTypeEntity == null)
				return;

			Id = projectTypeEntity.Id;
			Name = projectTypeEntity.Name;
			Description = projectTypeEntity.Description;

		}
		public Infrastructure.Data.Entities.Tables.BSD.ProjectTypeEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ProjectTypeEntity
			{
				Id = Id,
				Name = Name,
				Description = Description
			};
		}
	}
}
