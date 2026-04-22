namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetArticlesModel
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


		public GetArticlesModel() { }

		public GetArticlesModel(Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity budget_ArticlesEntity)


		{
			Article_code = budget_ArticlesEntity.Article_code;
			Article_designation1 = budget_ArticlesEntity.Article_designation1;
			Article_designation2 = budget_ArticlesEntity.Article_designation2;
			Article_number = budget_ArticlesEntity.Article_number;
			Article_supplier = budget_ArticlesEntity.Article_supplier;
			Creator_Bind = budget_ArticlesEntity.Creator_Bind;
			Description = budget_ArticlesEntity.Description;
			Editor_Bind = budget_ArticlesEntity.Editor_Bind;
			Id_Currency = budget_ArticlesEntity.Id_Currency;
			Unit_Price = budget_ArticlesEntity.Unit_Price;

		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity ToBudgetArticles()
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
