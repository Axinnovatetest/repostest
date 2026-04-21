using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.Apps.WorkPlan.Models.Country
{

	public class CountryViewModel
	{
		public int Id { get; set; }
		[Required]
		public String Name { get; set; }
		public String LastEditUsername { get; set; }
		public string CreationUsername { get; set; }
		public String Designation { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime? LastEditTime { get; set; }
		public bool CanSafeDelete { get; set; }

		public CountryViewModel() { }
		public CountryViewModel(Infrastructure.Data.Entities.Tables.WPL.CountryEntity countryDb)
		{
			this.Id = countryDb.Id;
			this.CreationTime = countryDb.CreationTime;
			this.Designation = countryDb.Designation;
			this.LastEditUsername = countryDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(countryDb.LastEditUserId.Value) : "";
			this.CreationUsername = Helpers.User.GetUserNameById(countryDb.CreationUserId);
			this.LastEditTime = countryDb.LastEditTime.HasValue ? countryDb.LastEditTime.Value : (DateTime?)null;
			this.Name = countryDb.Name;
			this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteCountry(countryDb.Id);
		}
	}
}
