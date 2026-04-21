using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Statistics
{
	public class OrderProcessingCustomerResponseModel
	{
		public List<Item> ByProject { get; set; }
		public List<Item> ByAB { get; set; }
		public List<Item> ByLS { get; set; }
		public class Item
		{
			public string CustomerName { get; set; }

			public int? Count { get; set; }
			public Item()
			{

			}
			public Item(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerEntity entity)
			{
				if(entity == null)
					return;

				CustomerName = entity.CustomerName;
				Count = entity.Count;
			}

		}
	}
}
