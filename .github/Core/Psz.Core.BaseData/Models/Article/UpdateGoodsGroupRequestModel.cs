using System;

namespace Psz.Core.BaseData.Models.Article
{
	public class UpdateGoodsGroupRequestModel
	{
		public int OriginalArticleId { get; set; }
		public int GoodsGroupId { get; set; }
		public string GoodsGroupName { get; set; }
		public int? GoodsTypeId { get; set; }
		public string GoodsTypeName { get; set; }

		public bool OverwriteArticleNumber { get; set; } = false;
		public string ArticleNumber { get; set; }
		public int CustomerId { get; set; }
		public int CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public string CustomerItemNumber { get; set; }
		public string CustomerItemIndex { get; set; }
		public DateTime? CustomerIndexDate { get; set; }
		public int ProductionCountryId { get; set; }
		public string ProductionCountryName { get; set; }
		public int ProductionSiteId { get; set; }
		public string ProductionSiteName { get; set; }
		public string ProductionSiteCode { get; set; }
	}
}
