using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class MainRequestModel
	{
		public string Sklad { get; set; }
		public DateTime? letzteBewegung { get; set; }
		public string CisloPSZ { get; set; }
		public decimal Mnozstvi { get; set; }
		public string CisloVyrobce { get; set; }
		public string KontrolaOk { get; set; }
		public string WE_VOH_ID { get; set; }
		public int totalNombre { get; set; }
		public MainRequestModel(Infrastructure.Data.Entities.Joins.Logistics.StatisticsEntity StatisticsEntity)
		{

			if(StatisticsEntity == null)
				return;

			Sklad = StatisticsEntity.Sklad;
			letzteBewegung = StatisticsEntity.letzteBewegung;
			CisloPSZ = StatisticsEntity.CisloPSZ;
			Mnozstvi = StatisticsEntity.Mnozstvi;
			CisloVyrobce = StatisticsEntity.CisloVyrobce;
			KontrolaOk = StatisticsEntity.KontrolaOk;
			WE_VOH_ID = StatisticsEntity.WE_VOH_ID;
			totalNombre = StatisticsEntity.totalNombre;


		}
	}
}
