using System;

namespace Psz.Core.BaseData.Models.Article.Configuration.Quality
{
	public class CheckStatusModel
	{
		public int Id { get; set; }
		public string Status { get; set; }
		public string Description { get; set; }

		public int? CreateUserId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public DateTime? UpdateTime { get; set; }

		public CheckStatusModel() { }
		public CheckStatusModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity checkStatusEntity)
		{
			if(checkStatusEntity == null)
				return;

			Id = checkStatusEntity.Id;
			Status = checkStatusEntity.Status;
			Description = checkStatusEntity.Description;
			CreateUserId = checkStatusEntity.CreateUserId;
			CreateTime = checkStatusEntity.CreateTime;
			UpdateUserId = checkStatusEntity.UpdateUserId;
			UpdateTime = checkStatusEntity.UpdateTime;
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity
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
