using Psz.Core.Common.Models;
using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class OutRequestModel: IPaginatedRequestModel
	{
		public int ArticleNr { get; set; }
		public string CustomerNr { get; set; }
		public string Type { get; set; }
		public string Number { get; set; }
		public string Ordered { get; set; }
		public string OpenCurrent { get; set; }
		public string Date { get; set; }
		public string Reference { get; set; }
		public bool? Booked { get; set; } = null;
		public string DistributionWarehouse { get; set; }
		public bool? Completed { get; set; } = null;
		public string SearchTerm { get; set; }
	}
	public class OutResponseModel: IPaginatedResponseModel<OutResponseItem>
	{
	}
	public class OutResponseItem
	{
		public string PSZ_Number { get; set; }

		public int? ArtikelNr { get; set; }
		public string CustomerNr { get; set; }
		public string Type { get; set; }
		public string Number { get; set; }
		public string Ordered { get; set; }
		public string OpenCurrent { get; set; }
		public DateTime? Date { get; set; }
		public string Reference { get; set; }
		public string Booked { get; set; }
		public string DistributionWarehouse { get; set; }
		public string Completed { get; set; }
		public int OrderId { get; set; }
		public OutResponseItem(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsOut logisticsOut)
		{
			if(logisticsOut == null)
				return;
			ArtikelNr = logisticsOut.ArtikelNr;
			PSZ_Number = logisticsOut.PSZ_Number;
			CustomerNr = logisticsOut.CustomerNr;
			Type = logisticsOut.Type;
			Number = logisticsOut.Number;
			Ordered = logisticsOut.Ordered;
			OpenCurrent = logisticsOut.OpenCurrent;
			Date = logisticsOut.Date;
			Reference = logisticsOut.Reference;
			Booked = logisticsOut.Booked;
			DistributionWarehouse = logisticsOut.DistributionWarehouse;
			Completed = logisticsOut.Completed;
			OrderId = logisticsOut.OrderId ?? 0;
		}
	}
}
