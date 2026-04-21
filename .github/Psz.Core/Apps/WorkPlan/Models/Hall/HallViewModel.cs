using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.Apps.WorkPlan.Models.Hall
{
	public class HallViewModel
	{
		public int Id { get; set; }
		[Required]
		public String Name { get; set; }
		[Required]
		public String Adress { get; set; }
		[Required]
		[Range(1, int.MaxValue)]
		public int CountryId { get; set; }
		public String CountryName { get; set; }

		public string CreationUsername { get; set; }
		public DateTime CreationTime { get; set; }
		public string LastEditUsername { get; set; }
		public DateTime? LastEditTime { get; set; }
		public bool CanSafeDelete { get; set; }
		public HallViewModel() { }
		public HallViewModel(Infrastructure.Data.Entities.Tables.WPL.HallEntity hallDb)
		{
			this.Id = hallDb.Id;
			this.Name = hallDb.Name;
			this.Adress = hallDb.Adress;
			this.CountryId = hallDb.CountryId;
			this.CountryName = Helpers.User.getCountryNameById(hallDb.CountryId);
			this.LastEditTime = hallDb.LastEditTime ?? (DateTime?)null;
			this.LastEditUsername = hallDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(hallDb.LastEditUserId.Value) : "";
			this.CreationUsername = Helpers.User.GetUserNameById(hallDb.CreationUserId);
			this.CreationTime = hallDb.CreationTime;
			this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteHall(hallDb.Id);
		}


	}
}
