using Infrastructure.Data.Entities.Joins.Logistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class ArticleReceivedResponseModel
	{
		public ArticleReceivedResponseModel(ArticleReceivedEntity x)
		{
			if(x is null)
				return;

			// -
			Date = x.Datum ?? DateTime.MinValue;
			Quantity = x.Menge ?? 0;
			PackagingNr = x.Verpackungsnr;
		}

		public DateTime Date { get; set; }
		public decimal Quantity { get; set; }
		public int PackagingNr { get; set; }
	}
	public class ArticleReceivedRequestModel
	{
		public int LagerId { get; set; }
		public string Article { get; set; }
	}
}
