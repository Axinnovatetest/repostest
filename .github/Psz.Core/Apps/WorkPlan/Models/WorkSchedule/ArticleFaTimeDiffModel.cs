namespace Psz.Core.Apps.WorkPlan.Models.WorkSchedule
{
	public class ArticleFaTimeDiffModel
	{
		public int ArticleNr { get; set; }
		public string ArticleNumber { get; set; }
		public int FaNr { get; set; }
		public string FaNumber { get; set; }
		public decimal? FaQuantity { get; set; }
		public decimal? FaTime { get; set; }
		public decimal? ApTime { get; set; }
		public ArticleFaTimeDiffModel(Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleFaTimeEntity articleFaTimeEntity)
		{
			if(articleFaTimeEntity == null)
				return;

			ArticleNr = articleFaTimeEntity.ArticleNr;
			ArticleNumber = articleFaTimeEntity.ArticleNumber;
			FaNr = articleFaTimeEntity.FaNr;
			FaNumber = articleFaTimeEntity.FaNumber;
			FaQuantity = articleFaTimeEntity.FaQuantity;
			FaTime = articleFaTimeEntity.FaTime;
			ApTime = articleFaTimeEntity.ApTime;
		}
	}
}
