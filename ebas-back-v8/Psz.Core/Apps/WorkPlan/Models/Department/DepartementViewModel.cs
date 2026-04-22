using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.Apps.WorkPlan.Models.Department
{
	public class DepartementViewModelXXX
	{
		public int Id { get; set; }
		[Required]
		public String Name { get; set; }
		public string LastEditUsername { get; set; }
		public string CreationUsername { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime? LastEditTime { get; set; }
		public bool CanSafeDelete { get; set; }
		public string NameDE { get; set; }
		public string NameTN { get; set; }
		public string NameCZ { get; set; }
		public string NameAL { get; set; }

	}
}
