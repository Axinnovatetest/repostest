using System;

namespace Psz.Core.Apps.Main.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public DateTime CreationTime { get; set; }
		public string Name { get; set; }
		public string SelectedLanguage { get; set; }
		public Core.Identity.Models.AccessProfileModel Access { get; set; }
	}
}
