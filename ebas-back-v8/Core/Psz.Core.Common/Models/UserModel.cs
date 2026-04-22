using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Common.Models
{
	public class UserCommonModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public DateTime CreationTime { get; set; }
		public string Name { get; set; }
		public string SelectedLanguage { get; set; } = "en";
		public bool SuperAdministrator { get; set; } = false;
		public string Email { get; set; }
		public bool IsGlobalDirector { get; set; } = false;
		public bool IsCorporateDirector { get; set; } = false;
		public bool IsAdministrator { get; set; } = false;

		public string Telephone { get; set; }
		public string Fax { get; set; }
		public int CompanyId { get; set; }
		public long DepartmentId { get; set; }
		public string CompanyName { get; set; }
		public string DepartmentName { get; set; }

		public int? Number { get; set; }
		public string LegacyUserName { get; set; }
	}
}
