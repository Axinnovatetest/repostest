namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetLandAssignementModel
	{
		public int? B_year { get; set; }
		public double? budget { get; set; }
		public int ID { get; set; }
		public string Land_name { get; set; }
		public int? LandId { get; set; }
		public decimal? TotalSpent { get; set; }

		public GetLandAssignementModel() { }
		public GetLandAssignementModel(Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity budget_LandsEntity)
		{
			ID = budget_LandsEntity.ID;
			Land_name = budget_LandsEntity.Land_name;
			budget = budget_LandsEntity.budget;
			B_year = budget_LandsEntity.B_year;
			LandId = budget_LandsEntity.LandId;
			TotalSpent = budget_LandsEntity.TotalSpent;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity ToBudgetLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity
			{
				ID = ID,
				Land_name = Land_name,
				budget = budget,
				B_year = B_year,
				LandId = LandId,
				TotalSpent = TotalSpent,
			};
		}
	}
}
