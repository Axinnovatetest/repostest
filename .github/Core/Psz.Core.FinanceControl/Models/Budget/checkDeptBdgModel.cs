namespace Psz.Core.FinanceControl.Models.Budget
{
	public class checkDeptBdgModel
	{
		public double? LandBudget { get; set; }
		public double? SOMME_DEPT { get; set; }
		public string DPT { get; set; }
		public int? Id_DPT { get; set; }

		public double? LandBudgetSupplement { get; set; }
		public checkDeptBdgModel()
		{

		}
		public checkDeptBdgModel(Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity budget_check)
		{

			LandBudget = budget_check.LandBudget;
			SOMME_DEPT = budget_check.SOMME_DEPT;
			DPT = budget_check.DPT;
			LandBudgetSupplement = budget_check.LandBudgetSupplement;
			Id_DPT = budget_check.Id_DPT;

		}

		public Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity ToBudgetCheckTest()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity
			{
				LandBudget = LandBudget,
				SOMME_DEPT = SOMME_DEPT,
				DPT = DPT,
				Id_DPT = Id_DPT,
				LandBudgetSupplement = LandBudgetSupplement
			};
		}
	}
}
