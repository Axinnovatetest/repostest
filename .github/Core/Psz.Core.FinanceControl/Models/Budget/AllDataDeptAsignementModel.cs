namespace Psz.Core.FinanceControl.Models.Budget
{
	public class AllDataDeptAsignementModel
	{
		public int? B_year { get; set; }

		public int ID { get; set; }
		public string Land_name { get; set; }
		public string Departement_name { get; set; }
		public double? SommebudgetDept { get; set; }
		public double? SommebudgetUser { get; set; }
		public double? NotAssignedBudgetUser { get; set; }
		public double? budget { get; set; }
		public int? DepartmentId { get; set; }

		public int? LandId { get; set; }
		public decimal? TotalSpent { get; set; }

		public AllDataDeptAsignementModel() { }
		public AllDataDeptAsignementModel(Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_departementEntity budget_LandsEntity)
		{
			ID = budget_LandsEntity.ID;
			Land_name = budget_LandsEntity.Land_name;
			budget = budget_LandsEntity.budget;
			B_year = budget_LandsEntity.B_year;
			Departement_name = budget_LandsEntity.Departement_name;
			SommebudgetDept = budget_LandsEntity.SommebudgetDept;
			SommebudgetUser = budget_LandsEntity.SommebudgetUser;
			NotAssignedBudgetUser = budget_LandsEntity.NotAssignedBudgetUser;
			LandId = budget_LandsEntity.LandId;
			DepartmentId = budget_LandsEntity.DepartmentId;
			TotalSpent = budget_LandsEntity.TotalSpent;
		}
		public Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_departementEntity ToBudgetLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_departementEntity
			{
				ID = ID,
				Land_name = Land_name,
				budget = budget,
				B_year = B_year,
				Departement_name = Departement_name,
				SommebudgetDept = SommebudgetDept,
				SommebudgetUser = SommebudgetUser,
				NotAssignedBudgetUser = NotAssignedBudgetUser,
				LandId = LandId,
				DepartmentId = DepartmentId,
				TotalSpent = TotalSpent,
			};
		}
	}
}
