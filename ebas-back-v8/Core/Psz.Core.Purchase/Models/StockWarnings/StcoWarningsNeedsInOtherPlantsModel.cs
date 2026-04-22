using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class StcoWarningsNeedsInOtherPlantsModel
	{
		public string Lager { get; set; }
		public decimal? Needed { get; set; }
		public decimal? Bedarf { get; set; }
		public decimal? OpenOrders { get; set; }
		public StcoWarningsNeedsInOtherPlantsModel()
		{

		}
		public StcoWarningsNeedsInOtherPlantsModel(Infrastructure.Data.Entities.Joins.PRS.StockWarningsNeedsInOtherPlantsEntity entity)
		{
			Lager = entity.Lager;
			Needed = entity.Needed;
			Bedarf = entity.Bedarf;
			OpenOrders = entity.OpenOrders;
		}
	}
}
