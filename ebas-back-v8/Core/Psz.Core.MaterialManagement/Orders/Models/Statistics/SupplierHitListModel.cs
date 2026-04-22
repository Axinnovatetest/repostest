namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class SupplierHitListRequestModel: IPaginatedRequestModel
	{
		public string ArticleNumber { get; set; }
		public DateTime DateFrom { get; set; } = DateTime.Today.AddMonths(-6);
		public DateTime DateTo { get; set; } = DateTime.Today.AddMonths(+6);
	}
	public class SupplierHitListResponseModel: IPaginatedResponseModel<SupplierHitListItem>
	{
	}
	public class SupplierHitListItem
	{
		public decimal? Einkaufsvolumen { get; set; }
		public string Name1 { get; set; }
		public SupplierHitListItem(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_SupplierHitCount supplierHitCount)
		{
			if(supplierHitCount == null)
				return;
			Name1 = supplierHitCount.Name1;
			Einkaufsvolumen = supplierHitCount.Einkaufsvolumen;
		}
	}
}
