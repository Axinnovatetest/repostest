using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.ROH
{
	public class LevelTwoDescriptionOrderModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int OrderInDescription { get; set; }
	}
	public class LevelTwoDescriptionOrderUpdateModel
	{
		public int IdLevelOne { get; set; }
		public List<LevelTwoDescriptionOrderModel> PropsWithOrders { get; set; }
	}
}