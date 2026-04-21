using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using System.Linq;

namespace Psz.Core.MaterialManagement.Helpers
{
	public class IdentifyFaultyArticlesHelper
	{
		/// <summary>
		/// calculate the cumulated orders starting from InitStock and Orders List .
		/// </summary>
		/// <param name="speceficOrders"></param>
		/// <param name="Stock"></param>
		/// <returns></returns>
		public static decimal CalculteCumilitiveStock(List<OrdersForArtikelByWeekModel> speceficOrders, decimal Stock)
		{
			var cumuitiveStock = Stock;
			var orderedquantityToCurrentWeek = speceficOrders.Select(x => x.orderedQuantitiy).ToList();
			foreach(var unitOrderdIWeek in orderedquantityToCurrentWeek)
			{
				cumuitiveStock = cumuitiveStock + unitOrderdIWeek.Value;
			}
			return cumuitiveStock;
		}
		/// <summary>
		/// remove the used orders from the init orders List
		/// </summary>
		/// <param name="orders"></param>
		/// <param name="speceficorders"></param>
		/// <returns></returns>
		public static bool DataCleaner(List<OrdersForArtikelByWeekModel> orders, List<OrdersForArtikelByWeekModel> speceficorders)
		{
			var Initindex = orders.Count;
			foreach(var item in speceficorders)
			{
				if(orders.IndexOf(item) >= 0)
				{
					orders.RemoveAt(orders.IndexOf(item));
				}

			}
			return orders.Count == Initindex - speceficorders.Count;
		}
		/// <summary>
		/// get specefic list of orders for one artikel in soecefic time span : the FA in question  
		/// </summary>
		/// <param name="orderstofilter"></param>
		/// <param name="week"></param>
		/// <param name="ArtikleNr"></param>
		/// <returns></returns>
		public static List<OrdersForArtikelByWeekModel> GetSpeceficOrdersForArticleAndWeek(List<OrdersForArtikelByWeekModel> orderstofilter, string week, int ArtikleNr)
		{
			return orderstofilter
				.Where(x =>
				x.artikelNr == ArtikleNr
				&& (
					SpecialHelper.CompareWeekPatternDiff(week, x.WeekO)
					|| SpecialHelper.CompareWeekPattern(week, x.WeekO)
					)
				).ToList();
		}
		/// <summary>
		/// return the faulty articles in a list... it makes some heavy work avoid putting it in a loop   
		/// </summary>
		/// <param name="orders"></param>
		/// <param name="FasForOneArtikelNr"></param>
		/// <param name="InitStock"></param>
		/// <returns></returns>
		public static List<ArticleAndFaultyWeek> Faulty(List<OrdersForArtikelByWeekModel> orders, List<FaultyArticlesInOpenFasPerWeekResponseModel> FasForOneArtikelNr, decimal InitStock)
		{
			var faultyArticles = new List<ArticleAndFaultyWeek>();
			FasForOneArtikelNr.Sort();
			orders.Sort();

			foreach(var item in FasForOneArtikelNr)
			{
				var OrdersForWeek = GetSpeceficOrdersForArticleAndWeek(orders, item.Week, item.Artikel_Nr);
				var cumulutiveStock = CalculteCumilitiveStock(OrdersForWeek, InitStock);
				DataCleaner(orders, OrdersForWeek);
				if(cumulutiveStock < item.NeedQuantity)
				{
					faultyArticles.Add(new ArticleAndFaultyWeek(item.Artikel_Nr, item.Week, item.Artikelnummer));
					break;
				}
				InitStock = cumulutiveStock - item.NeedQuantity.Value;
			}
			return faultyArticles;
		}

	}
}
