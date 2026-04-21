using System;

namespace Psz.Core.BaseData.Models.Article.Configuration.Quality
{
	public class MHDTagModel
	{
		public int Id { get; set; }
		public string Tag { get; set; }
		public string Description { get; set; }

		public int? CreateUserId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public DateTime? UpdateTime { get; set; }

		public MHDTagModel() { }
		public MHDTagModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelMHD_TagEntity checkStatusEntity)
		{
			if(checkStatusEntity == null)
				return;

			Id = checkStatusEntity.Id;
			Tag = checkStatusEntity.Tag;
			Description = checkStatusEntity.Description;
			CreateUserId = checkStatusEntity.CreateUserId;
			CreateTime = checkStatusEntity.CreateTime;
			UpdateUserId = checkStatusEntity.UpdateUserId;
			UpdateTime = checkStatusEntity.UpdateTime;
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArtikelMHD_TagEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelMHD_TagEntity
			{
				Id = Id,
				Tag = Tag,
				Description = Description,

				CreateUserId = CreateUserId,
				CreateTime = CreateTime,
				UpdateUserId = UpdateUserId,
				UpdateTime = UpdateTime
			};
		}
	}
}
