namespace Psz.Core.CRP.Models.Administration.AccessProfiles
{
	public class AddToUserModel
	{
		public int UserId { get; set; }
		public List<KeyValuePair<int, string>> ProfileIds { get; set; }
	}
}