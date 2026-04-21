namespace Psz.Core.BaseData.Models.Article.ProjectClass
{
	public class AddRequestModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public AddRequestModel(Infrastructure.Data.Entities.Tables.BSD.ProjectClassEntity projectClassEntity)
		{
			if(projectClassEntity == null)
				return;

			Id = projectClassEntity.Id;
			Name = projectClassEntity.Name;
		}

		public Infrastructure.Data.Entities.Tables.BSD.ProjectClassEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ProjectClassEntity
			{
				Description = "",
				Id = Id,
				Name = Name
			};
		}
	}
}
