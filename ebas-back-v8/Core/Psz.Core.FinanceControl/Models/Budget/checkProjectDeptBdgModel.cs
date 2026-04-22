namespace Psz.Core.FinanceControl.Models.Budget
{
	public class checkProjectDeptBdgModel
	{

		public double? Budget_DEPT { get; set; }
		public string DPT { get; set; }
		public checkProjectDeptBdgModel()
		{

		}
		public checkProjectDeptBdgModel(Infrastructure.Data.Entities.Tables.FNC.checkProjectDeptBdgEntity budget_check)
		{

			Budget_DEPT = budget_check.Budget_DEPT;
			DPT = budget_check.DPT;
		}

		public Infrastructure.Data.Entities.Tables.FNC.checkProjectDeptBdgEntity ToBudgetCheckTest()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.checkProjectDeptBdgEntity
			{

				Budget_DEPT = Budget_DEPT,
				DPT = DPT
			};
		}
	}
}
