
namespace Psz.Core.BaseData.Models.Settings.CoCType
{
	public class CoCTypeRequestModel
	{
		public string Confirmation { get; set; }
		public string Content { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Version { get; set; }

		public Infrastructure.Data.Entities.Tables.BSD.CocTypeEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.CocTypeEntity
			{
				Confirmation = Confirmation,
				Content = Content,
				Id = Id,
				Name = Name,
				Version = Version,
			};
		}
	}
	public class CoCTypeResponseModel
	{
		public string Confirmation { get; set; }
		public string Content { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Version { get; set; }
		public CoCTypeResponseModel(Infrastructure.Data.Entities.Tables.BSD.CocTypeEntity entity)
		{
			if(entity is null)
			{
				return;
			}

			Confirmation = entity.Confirmation;
			Content = entity.Content;
			Id = entity.Id;
			Name = entity.Name;
			Version = entity.Version;
		}
	}
}
