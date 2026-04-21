namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class OrdersForArtikelByWeekModel: IComparable<OrdersForArtikelByWeekModel>
	{
		public decimal? orderedQuantitiy { get; set; }
		public int artikelNr { get; set; }
		public string WeekO { get; set; }
		public OrdersForArtikelByWeekModel(Infrastructure.Data.Entities.Joins.MTM.Order.OrdersForArticlesByWeekEntity data)
		{
			orderedQuantitiy = data.orderedQuantitiy;
			artikelNr = data.artikelNr;
			WeekO = data.WeekO;
		}
		public int CompareTo(OrdersForArtikelByWeekModel other)
		{
			return Psz.Core.MaterialManagement.Helpers.SpecialHelper.CompareWeekPatternDiff(WeekO, other.WeekO) ? 1 : -1;
		}
	}
}
