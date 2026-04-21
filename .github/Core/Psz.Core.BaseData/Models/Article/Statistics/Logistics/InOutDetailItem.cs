using Psz.Core.Common.Models;
using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class InOutDetailsRequestModel: IPaginatedRequestModel
	{
		public string ArticleNumber { get; set; }
		public string Type { get; set; }
		public string OrderNr { get; set; }
		public DateTime? Date { get; set; }
		public string Name { get; set; }
		public string StorageLocationBefore { get; set; }
		public string StorageLocationAfter { get; set; }
		public string Rollennummer { get; set; }
	}
	public class InOutDetailsResponseModel: IPaginatedResponseModel<InOutDetailItem>
	{
	}
	public class InOutDetailItem
	{
		public string ArticleNumber { get; set; }
		public int? ArtikelNr { get; set; }
		public string Type { get; set; }
		public string OrderNumber { get; set; }
		public string Number { get; set; }
		public DateTime? Date { get; set; }
		public string Name { get; set; }
		public string StorageLocationBefore { get; set; }
		public string StorageLocationAfter { get; set; }
		public long? Rollennummer { get; set; }
		public string Gebucht_von { get; set; }
		public int OrderId { get; set; }
		public InOutDetailItem(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsInOutDetails logisticsOut)
		{
			if(logisticsOut == null)
				return;
			ArtikelNr = logisticsOut.ArtikelNr;
			ArticleNumber = logisticsOut.ArticleNumber;
			Type = logisticsOut.Type;
			OrderNumber = logisticsOut.OrderNumber;
			Number = logisticsOut.Number;
			Date = logisticsOut.Date;
			Name = logisticsOut.Name;
			StorageLocationBefore = logisticsOut.StorageLocationBefore;
			StorageLocationAfter = logisticsOut.StorageLocationAfter;
			Rollennummer = logisticsOut.Rollennummer;
			Gebucht_von = logisticsOut.Gebucht_von;
			OrderId = logisticsOut.OrderId ?? 0;
		}
	}
}
