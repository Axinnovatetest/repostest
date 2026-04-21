

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class AllocationsVsOrdersAmountModel
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public int? OrdersCount { get; set; }
		public decimal? ProjectBudget { get; set; }
		public decimal? OrdersAmount { get; set; }
		public AllocationsVsOrdersAmountModel()
		{

		}
		public AllocationsVsOrdersAmountModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.AllocationsVsOrdersAmountEntity entity)
		{
			Id = entity.Id;
			ProjectName = entity.ProjectName;
			OrdersCount = entity.OrdersCount;
			ProjectBudget = entity.ProjectBudget;
			OrdersAmount = entity.OrdersAmount;
		}
	}
}
