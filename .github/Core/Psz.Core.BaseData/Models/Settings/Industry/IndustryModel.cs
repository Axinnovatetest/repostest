using System;

namespace Psz.Core.BaseData.Models.Industry
{
	public class IndustryModel
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }

		public DateTime? LastUpdateTime { get; set; }

		public int? LastUpdateUserId { get; set; }
		public int Type { get; set; }

		public IndustryModel()
		{

		}
		public IndustryModel(Infrastructure.Data.Entities.Tables.BSD.IndustryEntity industryEntity)
		{
			Id = industryEntity.Id;
			Description = industryEntity.Description;
			Name = industryEntity.Name;
			CreationTime = industryEntity.CreationTime;
			CreationUserId = industryEntity.CreationUserId;
			LastUpdateTime = industryEntity.LastUpdateTime;
			LastUpdateUserId = industryEntity.LastUpdateUserId;
			Type = (int)industryEntity.Type;
		}

		public Infrastructure.Data.Entities.Tables.BSD.IndustryEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.IndustryEntity
			{
				Id = Id,
				Description = Description,
				Name = Name,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				LastUpdateTime = LastUpdateTime,
				LastUpdateUserId = LastUpdateUserId,
				Type = Type,
			};
		}
	}
}
