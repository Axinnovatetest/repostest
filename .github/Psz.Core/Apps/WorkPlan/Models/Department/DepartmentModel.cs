using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.Department
{
	public class CreateModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public string CreationUsername { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public string LastEditUsername { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public Boolean IsArchived { get; set; }
		public bool CanSafeDelete { get; set; }

		public string NameDE { get; set; }
		public string NameTN { get; set; }
		public string NameCZ { get; set; }
		public string NameAL { get; set; }

		public CreateModel() { }
		public CreateModel(Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity departmentDb,
			List<Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity> departmentI18NDb)
		{
			this.Id = departmentDb.Id;
			this.CreationTime = departmentDb.CreationTime;
			this.LastEditUsername = departmentDb.LastEditUserId.HasValue ? Infrastructure.Data.Access.Tables.COR.UserAccess.Get(departmentDb.LastEditUserId.Value)?.Username : "";
			this.CreationUsername = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(departmentDb.CreationUserId)?.Username;
			this.LastEditTime = departmentDb.LastEditTime.HasValue ? departmentDb.LastEditTime.Value : (DateTime?)null;
			this.Name = departmentDb.Name;
			this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteDepartement(departmentDb.Id);

			this.NameDE = departmentI18NDb?.Find(s => s.CodeLanguage.ToUpper() == "DE")?.Name;
			this.NameTN = departmentI18NDb?.Find(s => s.CodeLanguage.ToUpper() == "TN")?.Name;
			this.NameCZ = departmentI18NDb?.Find(s => s.CodeLanguage.ToUpper() == "CZ")?.Name;
			this.NameAL = departmentI18NDb?.Find(s => s.CodeLanguage.ToUpper() == "AL")?.Name;
		}
	}
}
