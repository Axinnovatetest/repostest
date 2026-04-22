namespace Psz.Core.FinanceControl.Models.Budget
{
	public class BudgetArticleModel
	{
		public string Article_code { get; set; }
		public string Article_designation1 { get; set; }
		public string Article_designation2 { get; set; }
		public int Article_number { get; set; }
		public int Article_supplier { get; set; }

		public int? Creator_Bind { get; set; }

		public string Description { get; set; }
		public int? Editor_Bind { get; set; }

		public int Id_Currency { get; set; }

		public double Unit_Price { get; set; }



		public BudgetArticleModel() { }

		public BudgetArticleModel(Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity ArticleEntity)


		{

			Article_code = ArticleEntity.Article_code;
			Article_designation1 = ArticleEntity.Article_designation1;
			Article_designation2 = ArticleEntity.Article_designation2;
			Article_number = ArticleEntity.Article_number;
			Article_supplier = ArticleEntity.Article_supplier;
			Creator_Bind = ArticleEntity.Creator_Bind;
			Description = ArticleEntity.Description;
			Editor_Bind = ArticleEntity.Editor_Bind;
			Id_Currency = ArticleEntity.Id_Currency;
			Unit_Price = ArticleEntity.Unit_Price;

		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity ToBudgetarticle()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity
			{
				Article_code = Article_code,
				Article_designation1 = Article_designation1,
				Article_designation2 = Article_designation2,
				Article_number = Article_number,
				Article_supplier = Article_supplier,
				Creator_Bind = Creator_Bind,
				Description = Description,
				Editor_Bind = Editor_Bind,
				Id_Currency = Id_Currency,
				Unit_Price = Unit_Price,
			};
		}
	}
}
