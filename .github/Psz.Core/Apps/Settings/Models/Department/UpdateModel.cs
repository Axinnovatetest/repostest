namespace Psz.Core.Apps.Settings.Models.Department
{
	public class UpdateModel
	{
		public long Id { get; set; }
		public long CompanyId { get; set; }
		public string CompanyName { get; set; }
		public string Description { get; set; }
		public string HeadUserEmail { get; set; }
		public long HeadUserId { get; set; }
		public string HeadUserName { get; set; }
		public string Name { get; set; }

		internal Infrastructure.Data.Entities.Tables.STG.DepartmentEntity ToEntity(Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity)
		{
			if(departmentEntity == null)
			{
				departmentEntity = new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity
				{
					Id = Id,
					CompanyId = CompanyId,
					CompanyName = CompanyName,
					Description = Description,
					HeadUserEmail = HeadUserEmail,
					HeadUserId = HeadUserId,
					HeadUserName = HeadUserName,
					Name = Name,
					IsArchived = false,
					IsDeleted = false
				};
			}
			else
			{
				departmentEntity.CompanyId = CompanyId;
				departmentEntity.CompanyName = CompanyName;
				departmentEntity.Description = Description;
				departmentEntity.HeadUserEmail = HeadUserEmail;
				departmentEntity.HeadUserId = HeadUserId;
				departmentEntity.HeadUserName = HeadUserName;
				departmentEntity.Name = Name;
			}

			return departmentEntity;
		}
	}
}
