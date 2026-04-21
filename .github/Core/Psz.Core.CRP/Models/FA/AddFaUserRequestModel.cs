namespace Psz.Core.CRP.Models.FA
{

	public class AddFaUserRequestModel
	{
		public List<FaUserRequestModel> Items { get; set; }
	}

	public class FaUserRequestModel
	{
		public string Username { get; set; }
		public int UserId { get; set; }
	}
}
