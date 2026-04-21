namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetTypeProjectModel
	{
		public int IdT { get; set; }
		public string Type_Project { get; set; }


		public GetTypeProjectModel() { }

		public GetTypeProjectModel(Infrastructure.Data.Entities.Tables.FNC.Type_Project_BudgetEntity budget_Type_ProjectEntity)


		{
			IdT = budget_Type_ProjectEntity.IdT;
			Type_Project = budget_Type_ProjectEntity.Type_Project;


		}
		public Infrastructure.Data.Entities.Tables.FNC.Type_Project_BudgetEntity ToBudgetTypeProject()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Type_Project_BudgetEntity
			{
				IdT = IdT,
				Type_Project = Type_Project,

			};
		}
	}
}
