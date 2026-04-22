namespace Psz.Core.FinanceControl.Models.Budget
{
	public class checkUserBdgModel
	{
		public double? DeptBudget { get; set; }
		public double? SOMME_USERS { get; set; }
		public string USR { get; set; }
		public checkUserBdgModel()
		{

		}
		public checkUserBdgModel(Infrastructure.Data.Entities.Tables.FNC.CheckUserBdgEntity budget_check)
		{

			DeptBudget = budget_check.DeptBudget;
			SOMME_USERS = budget_check.SOMME_USERS;
			USR = budget_check.USR;
		}

		public Infrastructure.Data.Entities.Tables.FNC.CheckUserBdgEntity ToBudgetCheckTest()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.CheckUserBdgEntity
			{
				DeptBudget = DeptBudget,
				SOMME_USERS = SOMME_USERS,
				USR = USR
			};
		}
	}
}
