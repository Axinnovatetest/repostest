using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderModel2
	{
		public int Id_Order { get; set; }
		public List<Article.ArticleModel> Articles { get; set; }

		public OrderModel2()
		{ }

		public OrderModel2(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
		 List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> articles,
		 List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> bestellteArticles,
		 List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> articleCustomerEntity
		)
		{
			this.Id_Order = orderEntity.OrderId;
			if(articles != null && articles.Count > 0)
			{
				Articles = new List<Article.ArticleModel>();
				foreach(var item in articles)
				{
					var bestellteArticleItem = bestellteArticles?.Find(x => x.Nr == item.BestellteArtikelNr);
					Articles.Add(new Article.ArticleModel(item, bestellteArticleItem, articleCustomerEntity));
				}
			}
		}
	}
}
