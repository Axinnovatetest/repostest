namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetArticleOrderModel
	{
		public string bindName { get; set; }
		public string Artikelnummer { get; set; }
		public string Article_Name1 { get; set; }
		public string Article_Name2 { get; set; }
		public int Artikel_Nr { get; set; }


		public GetArticleOrderModel() { }

		public GetArticleOrderModel(Infrastructure.Data.Entities.Tables.FNC.Artikel_Order_BudgetEntity budget_Artikel_OrderEntity)


		{
			bindName = budget_Artikel_OrderEntity.bindName;
			Artikelnummer = budget_Artikel_OrderEntity.Artikelnummer;
			Article_Name1 = budget_Artikel_OrderEntity.Article_Name1;
			Article_Name2 = budget_Artikel_OrderEntity.Article_Name2;
			Artikel_Nr = budget_Artikel_OrderEntity.Artikel_Nr;


		}
		public Infrastructure.Data.Entities.Tables.FNC.Artikel_Order_BudgetEntity ToBudgetArtikelOrder()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Artikel_Order_BudgetEntity
			{
				bindName = bindName,
				Artikelnummer = Artikelnummer,
				Article_Name1 = Article_Name1,
				Article_Name2 = Article_Name2,
				Artikel_Nr = Artikel_Nr,

			};
		}
	}
}
