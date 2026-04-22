using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class LogisticsInOutDetailsModel
	{
		public string ArticleNumber { get; set; }
		public string Type { get; set; }
		public string OrderNumber { get; set; }
		public string Number { get; set; }
		public DateTime? Date { get; set; }
		public string Name { get; set; }
		public string StorageLocationBefore { get; set; }
		public string StorageLocationAfter { get; set; }
		public LogisticsInOutDetailsModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsInOutDetails logisticsOut)
		{
			if(logisticsOut == null)
				return;

			ArticleNumber = logisticsOut.ArticleNumber;
			Type = logisticsOut.Type;
			OrderNumber = logisticsOut.OrderNumber;
			Number = logisticsOut.Number;
			Date = logisticsOut.Date;
			Name = logisticsOut.Name;
			StorageLocationBefore = logisticsOut.StorageLocationBefore;
			StorageLocationAfter = logisticsOut.StorageLocationAfter;
		}
	}
}
