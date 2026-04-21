namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetDeptNamesModel
	{
		public int ID { get; set; }
		public string Departement_name { get; set; }
		public string Land { get; set; }

		public GetDeptNamesModel() { }
		public GetDeptNamesModel(Infrastructure.Data.Entities.Tables.FNC.Budget_deptsNamesEntity budget_DeptsEntity)
		{
			ID = budget_DeptsEntity.ID;
			Departement_name = budget_DeptsEntity.Departement_name;
			Land = budget_DeptsEntity.Land;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_deptsNamesEntity ToBudgetdepts()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Budget_deptsNamesEntity
			{
				ID = ID,
				Departement_name = Departement_name,
				Land = Land,
			};
		}
	}
}
