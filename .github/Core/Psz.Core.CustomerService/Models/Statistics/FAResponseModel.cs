using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Statistics
{
	public class FAResponseModel
	{
		public int? TotFA { get; set; }
		public List<Item0> AllFAByLager { get; set; }
		public List<Item1> AllFAByLagerZbuchen { get; set; }
		public List<Item2> ByKennzeichen { get; set; }
		public List<Item3> FAGestartet { get; set; }
		public List<Item4> FAKomC { get; set; }
		public List<Item5> FAKomT { get; set; }
		public List<Item6> FATechn { get; set; }
		public class Item0
		{
			public string LagerName { get; set; }
			public int? Count { get; set; }
			public int? LagerNum { get; set; }

			public Item0()
			{

			}
			public Item0(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatLagerEntity entity)
			{
				LagerName = entity.LagerName;
				Count = entity.Count;
				LagerNum = entity.LagerNum;
			}
		}
		public class Item1
		{
			public string LagerName { get; set; }

			public int? Count { get; set; }
			public int? LagerNum { get; set; }

			public Item1()
			{

			}
			public Item1(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatLagerZuEntity entity)
			{
				LagerName = entity.LagerName;
				Count = entity.Count;
				LagerNum = entity.LagerNum;
			}
		}
		public class Item2
		{
			public string TypeKennzeichen { get; set; }

			public int? Count { get; set; }

			public Item2()
			{

			}
			public Item2(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKenzEntity entity)
			{
				TypeKennzeichen = entity.TypeKennzeichen;
				Count = entity.Count;
			}
		}
		public class Item3
		{
			public string Getstartet { get; set; }

			public int? Count { get; set; }
			public Item3()
			{

			}
			public Item3(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatGestEntity entity)
			{
				Getstartet = entity.Getstartet;
				Count = entity.Count;
			}
		}
		public class Item4
		{
			public string KommComp { get; set; }
			public int? Count { get; set; }
			public Item4()
			{

			}
			public Item4(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKomCoEntity entity)
			{
				KommComp = entity.KommComp;
				Count = entity.Count;
			}
		}
		public class Item5
		{
			public string KommTreil { get; set; }
			public int? Count { get; set; }
			public Item5()
			{

			}
			public Item5(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatKomTrEntity entity)
			{
				KommTreil = entity.KommTreil;
				Count = entity.Count;
			}

		}
		public class Item6
		{
			public string Technik { get; set; }
			public int? Count { get; set; }
			public Item6()
			{

			}
			public Item6(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatTechEntity entity)
			{
				Technik = entity.Technik;
				Count = entity.Count;
			}
		}
	}
}
