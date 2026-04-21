namespace Psz.Core.BaseData.Models.Article
{
	public class IndexSiblingsRequestModel
	{
		public int CustomerNumber { get; set; }
		public string CustomerItemNumber { get; set; }
		public string CustomerItemIndex { get; set; }
		public bool? IsSpecialNumber { get; set; } = false;
	}
}
