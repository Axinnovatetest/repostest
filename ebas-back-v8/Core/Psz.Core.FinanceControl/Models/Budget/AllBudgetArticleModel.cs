namespace Psz.Core.FinanceControl.Models.Budget
{
	public class AllBudgetArticleModel
	{
		public string Article_code { get; set; }
		public string Article_designation1 { get; set; }
		public string Article_designation2 { get; set; }
		public int Article_number { get; set; }
		public int Article_supplier { get; set; }
		public string Article_supplier_name { get; set; }
		public int? Lieferantennummer { get; set; }
		public int? Nr { get; set; }
		public string Ort { get; set; }
		public int? Creator_Bind { get; set; }
		public string Article_creator_name { get; set; }
		public string Description { get; set; }
		public int? Editor_Bind { get; set; }
		public string Article_editor_name { get; set; }
		public int Id_Currency { get; set; }
		public string Symol { get; set; }
		public double Unit_Price { get; set; }




		public AllBudgetArticleModel() { }

		public AllBudgetArticleModel(Infrastructure.Data.Entities.Tables.FNC.AllBudget_ArticleEntity allDataArticleEntity)


		{

			Article_code = allDataArticleEntity.Article_code;
			Article_designation1 = allDataArticleEntity.Article_designation1;
			Article_designation2 = allDataArticleEntity.Article_designation2;
			Article_number = allDataArticleEntity.Article_number;
			Article_supplier = allDataArticleEntity.Article_supplier;
			Article_supplier_name = allDataArticleEntity.Article_supplier_name;
			Lieferantennummer = allDataArticleEntity.Lieferantennummer;
			Nr = allDataArticleEntity.Nr;
			Ort = allDataArticleEntity.Ort;
			Creator_Bind = allDataArticleEntity.Creator_Bind;
			Article_creator_name = allDataArticleEntity.Article_creator_name;
			Description = allDataArticleEntity.Description;
			Editor_Bind = allDataArticleEntity.Editor_Bind;
			Article_editor_name = allDataArticleEntity.Article_editor_name;
			Id_Currency = allDataArticleEntity.Id_Currency;
			Symol = allDataArticleEntity.Symol;
			Unit_Price = allDataArticleEntity.Unit_Price;

		}
		public Infrastructure.Data.Entities.Tables.FNC.AllBudget_ArticleEntity ToBudgetallDataArticles()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AllBudget_ArticleEntity
			{
				Article_code = Article_code,
				Article_designation1 = Article_designation1,
				Article_designation2 = Article_designation2,
				Article_number = Article_number,
				Article_supplier = Article_supplier,
				Article_supplier_name = Article_supplier_name,
				Lieferantennummer = Lieferantennummer,
				Nr = Nr,
				Ort = Ort,
				Creator_Bind = Creator_Bind,
				Article_creator_name = Article_creator_name,
				Description = Description,
				Editor_Bind = Editor_Bind,
				Article_editor_name = Article_editor_name,
				Id_Currency = Id_Currency,
				Symol = Symol,
				Unit_Price = Unit_Price,
			};
		}
	}
}
