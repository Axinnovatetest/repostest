using System;

namespace Psz.Core.Apps.WorkPlan.Models.WorkArea
{
	public class WorkAreaViewModel
	{
		public int Id { get; set; }

		public int Hall_Id { get; set; }

		public string Name { get; set; }
		public string HallName { get; set; }
		public string CreationUsername { get; set; }
		public DateTime CreationTime { get; set; }
		public string LastEditUsername { get; set; }
		public DateTime? LastEditTime { get; set; }

		public int CountryId { get; set; }
		public String CountryName { get; set; }
		public bool CanSafeDelete { get; set; }
		public int? Department_Id { get; set; }
		public string DepartmentName { get; set; }

		public WorkAreaViewModel() { }
		public WorkAreaViewModel(Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity workAreaDb)
		{
			this.Name = workAreaDb.Name;
			this.Id = workAreaDb.Id;
			this.Hall_Id = workAreaDb.HallId;
			this.HallName = workAreaDb.HallName; //Helpers.User.getHallNameById(workAreaDb.HallId);
			this.CreationTime = workAreaDb.CreationTime;
		//	this.CreationUsername = Helpers.User.GetUserNameById(workAreaDb.CreationUserId);
			this.LastEditTime = workAreaDb.LastEditTime.HasValue ? workAreaDb.LastEditTime.Value : (DateTime?)null;
		//	this.LastEditUsername = workAreaDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(workAreaDb.LastEditUserId.Value) : "";
			this.CountryId = workAreaDb.CountryId;
			this.CountryName = workAreaDb.CountryName;//Helpers.User.getCountryNameById(workAreaDb.CountryId);
		//	this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteWorkArea(workAreaDb.Id);
			this.Department_Id = workAreaDb.DepartmentId;
			this.DepartmentName = workAreaDb.DepartmentName;//Helpers.User.getDepartmentNameById(workAreaDb.DepartmentId);
		}
	}
}
