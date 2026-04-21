using System;

namespace Psz.Core.BaseData.Models.Article.Configuration.Quality
{
	public class InternalStatusModel
	{
		public int Id { get; set; }
		public string Status { get; set; }
		public string Description { get; set; }

		public int? CreateUserId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public DateTime? UpdateTime { get; set; }

		public InternalStatusModel() { }
		public InternalStatusModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelInternalStatusEntity internalStatusEntity)
		{
			if(internalStatusEntity == null)
				return;

			Id = internalStatusEntity.Id;
			Status = internalStatusEntity.Status;
			Description = internalStatusEntity.Description;
			CreateUserId = internalStatusEntity.CreateUserId;
			CreateTime = internalStatusEntity.CreateTime;
			UpdateUserId = internalStatusEntity.UpdateUserId;
			UpdateTime = internalStatusEntity.UpdateTime;
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArtikelInternalStatusEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelInternalStatusEntity
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
