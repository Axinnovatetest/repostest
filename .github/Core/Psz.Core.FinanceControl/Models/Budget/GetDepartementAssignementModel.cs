namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetDepartementAssignementModel
	{
		public int? B_year { get; set; }
		public string Departement_name { get; set; }
		public int ID { get; set; }
		public string Land_name { get; set; }
		public double? budget { get; set; }
		public int? DepartmentId { get; set; }

		public int? LandId { get; set; }
		public decimal? TotalSpent { get; set; }
		public GetDepartementAssignementModel() { }
		public GetDepartementAssignementModel(Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity budget_DeptsEntity)
		{
			ID = budget_DeptsEntity.ID;
			Land_name = budget_DeptsEntity.Land_name;
			Departement_name = budget_DeptsEntity.Departement_name;
			budget = budget_DeptsEntity.budget;
			B_year = budget_DeptsEntity.B_year;
			LandId = budget_DeptsEntity.LandId;
			DepartmentId = budget_DeptsEntity.DepartmentId;
			TotalSpent = budget_DeptsEntity.TotalSpent;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity ToBudgetdeptsAssign()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity
			{
				ID = ID,
				Land_name = Land_name,
				Departement_name = Departement_name,
				budget = budget,
				B_year = B_year,
				LandId = LandId,
				DepartmentId = DepartmentId,
				TotalSpent = TotalSpent,
			};
		}
	}
}
