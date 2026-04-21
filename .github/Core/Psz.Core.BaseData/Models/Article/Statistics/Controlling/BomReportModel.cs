namespace Psz.Core.BaseData.Models.Article.Statistics.Controlling
{
	public class BomReportModel
	{
		public string Position { get; set; }
		public string ArticleNumber { get; set; }

		public int? ArtikleNr { get; set; }
		public string Quantity { get; set; }
		public string DesignationBom { get; set; }
		public string Supplier { get; set; }
		public string OrderNumber { get; set; }

		public BomReportModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingBomReport controllingBomReport)
		{
			if(controllingBomReport == null)
				return;
			ArtikleNr = controllingBomReport.ArtikleNr;
			Position = controllingBomReport.Position;
			ArticleNumber = controllingBomReport.ArticleNumber;
			Quantity = controllingBomReport.Quantity;
			DesignationBom = controllingBomReport.DesignationBom;
			Supplier = controllingBomReport.Supplier;
			OrderNumber = controllingBomReport.OrderNumber;
		}
	}
}
