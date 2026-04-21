namespace Psz.Core.BaseData.Models.Article.BillOfMaterial.VersionControl
{
	public class GetArticleListRequestModel
	{
		public bool IncludeFullyValidated { get; set; } = false;
		public bool? Engineering { get; set; }
		public bool? Quality { get; set; }
	}
}
