using System;

namespace Psz.Core.BaseData.Models.Article.Configuration.Quality
{
	public class ExternalStatusModel
	{
		public int Id { get; set; }
		public string Status { get; set; }
		public string Description { get; set; }

		public int? CreateUserId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public DateTime? UpdateTime { get; set; }

		public ExternalStatusModel() { }
		public ExternalStatusModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelExternalStatusEntity externalStatusEntity)
		{
			if(externalStatusEntity == null)
				return;

			Id = externalStatusEntity.Id;
			Status = externalStatusEntity.Status;
			Description = externalStatusEntity.Description;
			CreateUserId = externalStatusEntity.CreateUserId;
			CreateTime = externalStatusEntity.CreateTime;
			UpdateUserId = externalStatusEntity.UpdateUserId;
			UpdateTime = externalStatusEntity.UpdateTime;
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArtikelExternalStatusEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelExternalStatusEntity
			{
				Id = Id,
				Status = Status,
				Description = Description,

				CreateUserId = CreateUserId,
				CreateTime = CreateTime,
				UpdateUserId = UpdateUserId,
				UpdateTime = UpdateTime
			};
		}
	}
}
