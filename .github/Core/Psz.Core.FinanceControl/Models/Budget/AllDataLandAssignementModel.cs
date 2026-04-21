namespace Psz.Core.FinanceControl.Models.Budget
{
	public class AllDataLandAssignementModel
	{
		public int? B_year { get; set; }
		public double? budget { get; set; }
		public int ID { get; set; }
		public string Land_name { get; set; }
		public double? SommeSupplement { get; set; }
		public double? SommebudgetSupplement { get; set; }
		public double? SommebudgetDept { get; set; }
		public double? NotAssignedBudgetDept { get; set; }
		public int? LandId { get; set; }
		public decimal? TotalSpent { get; set; }

		public AllDataLandAssignementModel() { }
		public AllDataLandAssignementModel(Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_landEntity budget_LandsEntity)
		{
			ID = budget_LandsEntity.ID;
			Land_name = budget_LandsEntity.Land_name;
			budget = budget_LandsEntity.budget;
			B_year = budget_LandsEntity.B_year;
			SommeSupplement = budget_LandsEntity.SommeSupplement;
			SommebudgetSupplement = budget_LandsEntity.SommebudgetSupplement;
			SommebudgetDept = budget_LandsEntity.SommebudgetDept;
			NotAssignedBudgetDept = budget_LandsEntity.NotAssignedBudgetDept;
			LandId = budget_LandsEntity.LandId;
			TotalSpent = budget_LandsEntity.TotalSpent;
		}
		public Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_landEntity ToBudgetLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_landEntity
			{
				ID = ID,
				Land_name = Land_name,
				budget = budget,
				B_year = B_year,
				SommeSupplement = SommeSupplement,
				SommebudgetSupplement = SommebudgetSupplement,
				SommebudgetDept = SommebudgetDept,
				NotAssignedBudgetDept = NotAssignedBudgetDept,
				LandId = LandId,
				TotalSpent = TotalSpent,
			};
		}
	}
}
