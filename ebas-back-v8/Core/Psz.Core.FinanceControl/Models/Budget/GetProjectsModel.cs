namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetProjectsModel
	{
		public string Description { get; set; }
		public int? Id_Currency { get; set; }
		public int? Id_Customer { get; set; }
		public int? Id_Dept { get; set; }
		public int? Id_Land { get; set; }
		public int Id_proj { get; set; }
		public int Id_Responsable { get; set; }
		public int Id_State { get; set; }
		public int Id_Type { get; set; }
		public string Name_proj { get; set; }
		public string Nr_Customer { get; set; }
		public decimal Proj_Budget { get; set; }

		public string CompanyName { get; set; }
		public string CurrencyName { get; set; }
		public string CustomerName { get; set; }
		public string DepartmentName { get; set; }
		public string ResponsableEmail { get; set; }
		public string ResponsableName { get; set; }
		public string Type { get; set; }
		public GetProjectsModel() { }

		public GetProjectsModel(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity budget_ProjectsEntity)
		{
			Id_proj = budget_ProjectsEntity.Id;
			Name_proj = budget_ProjectsEntity.ProjectName;
			Id_Customer = budget_ProjectsEntity.CustomerId;
			Nr_Customer = budget_ProjectsEntity.CustomerNr;
			Proj_Budget = budget_ProjectsEntity.ProjectBudget;
			Id_Responsable = budget_ProjectsEntity.ResponsableId;
			Id_State = budget_ProjectsEntity.Id_State;
			Id_Currency = budget_ProjectsEntity.CurrencyId;
			Id_Land = budget_ProjectsEntity.CompanyId;
			Id_Dept = budget_ProjectsEntity.DepartmentId;
			Id_Type = budget_ProjectsEntity.Id_Type;
			Description = budget_ProjectsEntity.Description;

			CompanyName = budget_ProjectsEntity.CompanyName;
			CurrencyName = budget_ProjectsEntity.CurrencyName;
			CustomerName = budget_ProjectsEntity.CustomerName;
			DepartmentName = budget_ProjectsEntity.DepartmentName;
			ResponsableEmail = budget_ProjectsEntity.ResponsableEmail;
			ResponsableName = budget_ProjectsEntity.ResponsableName;
			Type = budget_ProjectsEntity.Type;

		}
		public Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity ToBudgetProjects()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity
			{
				Id = Id_proj,
				ProjectName = Name_proj,
				CustomerId = Id_Customer,
				CustomerNr = Nr_Customer,
				ProjectBudget = Proj_Budget,
				ResponsableId = Id_Responsable,
				Id_State = Id_State,
				CurrencyId = Id_Currency,
				CompanyId = Id_Land,
				DepartmentId = Id_Dept,
				Id_Type = Id_Type,
				Description = Description,
				CompanyName = CompanyName,
				CurrencyName = CurrencyName,
				CustomerName = CustomerName,
				DepartmentName = DepartmentName,
				ResponsableEmail = ResponsableEmail,
				ResponsableName = ResponsableName,
				Type = Type
			};
		}
	}
}
