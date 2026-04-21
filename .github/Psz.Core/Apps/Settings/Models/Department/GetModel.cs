using System;

namespace Psz.Core.Apps.Settings.Models.Department
{
	public class GetModel
	{
		public DateTime? ArchiveTime { get; set; }
		public long? ArchiveUserId { get; set; }
		public long CompanyId { get; set; }
		public string CompanyName { get; set; }
		public DateTime CreationTime { get; set; }
		public long CreationUserId { get; set; }
		public DateTime? DeleteTime { get; set; }
		public long? DeleteUserId { get; set; }
		public string Description { get; set; }
		public string HeadUserEmail { get; set; }
		public long HeadUserId { get; set; }
		public string HeadUserName { get; set; }
		public long Id { get; set; }
		public bool? IsArchived { get; set; }
		public bool? IsDeleted { get; set; }
		public DateTime? LastEditTime { get; set; }
		public long? LastEditUserId { get; set; }
		public string Name { get; set; }


		public GetModel()
		{ }
		public GetModel(Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity)
		{
			ArchiveTime = departmentEntity?.ArchiveTime;
			ArchiveUserId = departmentEntity?.ArchiveUserId;
			CompanyId = departmentEntity?.CompanyId ?? -1;
			CompanyName = departmentEntity?.CompanyName;
			CreationTime = departmentEntity?.CreationTime ?? DateTime.MinValue;
			CreationUserId = departmentEntity?.CreationUserId ?? -1;
			DeleteTime = departmentEntity?.DeleteTime;
			DeleteUserId = departmentEntity?.DeleteUserId;
			Description = departmentEntity?.Description;
			HeadUserEmail = departmentEntity?.HeadUserEmail;
			HeadUserId = departmentEntity?.HeadUserId ?? -1;
			HeadUserName = departmentEntity?.HeadUserName;
			Id = departmentEntity?.Id ?? -1;
			IsArchived = departmentEntity?.IsArchived;
			IsDeleted = departmentEntity?.IsDeleted;
			LastEditTime = departmentEntity?.LastEditTime;
			LastEditUserId = departmentEntity?.LastEditUserId;
			Name = departmentEntity?.Name;
		}

		public Infrastructure.Data.Entities.Tables.STG.DepartmentEntity ToDepartmentEntity()
		{
			return new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity
			{
				ArchiveTime = ArchiveTime,
				ArchiveUserId = ArchiveUserId,
				CompanyId = CompanyId,
				CompanyName = CompanyName,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				DeleteTime = DeleteTime,
				DeleteUserId = DeleteUserId,
				Description = Description,
				HeadUserEmail = HeadUserEmail,
				HeadUserId = HeadUserId,
				HeadUserName = HeadUserName,
				Id = Id,
				IsArchived = IsArchived,
				IsDeleted = IsDeleted,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				Name = Name,
			};
		}
	}
}
