namespace Psz.Core.FinanceControl.Models.Budget.Project
{
	public class UpdateBudgetModel
	{
		public int ProjectId { get; set; }
		public decimal BudgetAmount { get; set; }
		public string Notes { get; set; }
	}
}
