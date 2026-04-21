namespace Psz.Core.CRP.Models.Administration.AccessProfiles
{
	public class AddUsersModel
	{
		public int ProfileId { get; set; }
		public List<int> UserIds { get; set; }
	}
}
