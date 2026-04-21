namespace Psz.Core.BaseData.Models.Article.BillOfMaterial.VersionControl
{
	public class ToggleStatusRequestModel
	{
		public long Id { get; set; }
		public int ArticleNr { get; set; }
		public Enums.ArticleEnums.BOMControlStatus Status { get; set; }
	}
}
