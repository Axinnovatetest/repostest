using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Statistics
{
	public class OrderProcessingResponseModel
	{
		public List<Item> AllABLS { get; set; }
		public List<Item1> AllPay { get; set; }
		public List<Item2> AllEDI { get; set; }
		public int? TotalProjects { get; set; }
		public int? LastCreatedProject { get; set; }
		public class Item1
		{
			public string Pay { get; set; }

			public int? Count { get; set; }
			public Item1()
			{

			}
			public Item1(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatPayEntity entity)
			{
				Pay = entity.Pay;
				Count = entity.Count;
			}
		}
		public class Item
		{
			public string Type { get; set; }
			public int? Count { get; set; }
			public Item()
			{

			}
			public Item(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTypEntity entity)
			{
				Type = entity.Type;
				Count = entity.Count;
			}
		}
		public class Item2
		{
			public string Type { get; set; }

			public int? Count { get; set; }
			public int? Edi { get; set; }

			public Item2()
			{

			}
			public Item2(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatEdiEntity entity)
			{
				Type = entity.Type;
				Count = entity.Count;
				Edi = entity.Edi;
			}
		}
	}
}
