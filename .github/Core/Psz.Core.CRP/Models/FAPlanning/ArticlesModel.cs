using iText.StyledXmlParser.Jsoup.Nodes;
using Psz.Core.Common.Models;


namespace Psz.Core.CRP.Models.FAPlanning
{
	public class ArticlesModel
	{
		public int ArticleNr { get; set; }
		public string Artikelnummer { get; set; }
		public bool UBG { get; set; }
		public bool Deficit { get; set; }
		public int DeficitType { get; set; }
		public ArticlesModel()
		{

		}
	}
	public class ArticlesRequestModel: IPaginatedRequestModel
	{
		public string? Kundenkreis { get; set; }
		public int? Kundennumer { get; set; }
		public int Period { get; set; }
		public string? Artikelnummer { get; set; }
		public int? DeficitType { get; set; }
		public int? Unit { get; set; }
	}
	public class ArticlesResponseModel: IPaginatedResponseModel<ArticlesModel> { }
	public class ArticleKwDetailRequestModel
	{
		public int Kw { get; set; }
		public int Year { get; set; }
		public int ArticleId { get; set; }
	}

	public class ArticleKwDetailResponseModel
	{
		public int Kw { get; set; }
		public int ArticleId { get; set; }
		public List<Item> FAs { get; set; }
		public List<Item> ABs { get; set; }
		public List<Item> LPs { get; set; }
		public List<Item> FCs { get; set; }
		public class Item
		{
			public int Id { get; set; }
			public string Number { get; set; }
			public decimal Quantity { get; set; }
			public bool Manual { get; set; }
			public int CustomerNumber { get; set; }
			public Item(Infrastructure.Data.Entities.Joins.CRP.FAPlannungArticlesKwDataEntity entity)
			{
				if(entity == null)
					return;
				// -
				Id = entity.Id ?? 0;
				Number = entity.Number ?? "";
				Quantity = entity.Quantity ?? 0;
				CustomerNumber = entity.CustomerNumber ?? 0;
				Manual = entity.Manual ?? false;
			}
		}
	}
}
