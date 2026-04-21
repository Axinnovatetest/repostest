using Psz.Core.Common.Models;
using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class SupplierHitCountRequestModel: IPaginatedRequestModel
	{
		public string ArticleNumber { get; set; }
		public DateTime DateFrom { get; set; } = DateTime.Today.AddMonths(-6);
		public DateTime DateTo { get; set; } = DateTime.Today.AddMonths(+6);
	}
	public class SupplierHitCountResponseModel: IPaginatedResponseModel<SupplierHitCountItem>
	{
	}
	public class SupplierHitCountItem
	{
		public decimal? Einkaufsvolumen { get; set; }
		public string Name1 { get; set; }
		public SupplierHitCountItem(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_SupplierHitCount supplierHitCount)
		{
			if(supplierHitCount == null)
				return;

			// - 
			Name1 = supplierHitCount.Name1;
			Einkaufsvolumen = supplierHitCount.Einkaufsvolumen;
		}
	}
}
