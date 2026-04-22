using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class UpgradeFABomRequestModel
	{
		public int ArticleId { get; set; }
		public int BomVerion { get; set; }
		public List<int> FaIds { get; set; }
	}
}
