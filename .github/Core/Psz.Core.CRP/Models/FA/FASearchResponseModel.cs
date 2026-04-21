
namespace Psz.Core.CRP.Models.FA
{
	public class FASearchResponseModel
	{
		public List<FAListModule> Orders { get; set; } = new List<FAListModule>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
