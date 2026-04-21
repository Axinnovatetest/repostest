using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetProjects_LogModel
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
		public double Proj_Budget { get; set; }
		public DateTime? Action_date { get; set; }
		public int Id_LS { get; set; }
		public int Id_state { get; set; }
		public int Id_user { get; set; }


		public GetProjects_LogModel() { }

		public GetProjects_LogModel(Infrastructure.Data.Entities.Tables.FNC.Project_Log_BudgetEntity budget_ProjectsLogEntity)


		{
			Id_proj = budget_ProjectsLogEntity.Id_proj;
			Name_proj = budget_ProjectsLogEntity.Name_proj;
			Id_Customer = budget_ProjectsLogEntity.Id_Customer;
			Nr_Customer = budget_ProjectsLogEntity.Nr_Customer;
			Proj_Budget = budget_ProjectsLogEntity.Proj_Budget;
			Id_Responsable = budget_ProjectsLogEntity.Id_Responsable;
			Id_State = budget_ProjectsLogEntity.Id_State;
			Id_Currency = budget_ProjectsLogEntity.Id_Currency;
			Id_Land = budget_ProjectsLogEntity.Id_Land;
			Id_Dept = budget_ProjectsLogEntity.Id_Dept;
			Id_Type = budget_ProjectsLogEntity.Id_Type;
			Description = budget_ProjectsLogEntity.Description;
			Action_date = budget_ProjectsLogEntity.Action_date;
			Id_LS = budget_ProjectsLogEntity.Id_LS;
			Id_state = budget_ProjectsLogEntity.Id_state;
			Id_user = budget_ProjectsLogEntity.Id_user;


		}
		public Infrastructure.Data.Entities.Tables.FNC.Project_Log_BudgetEntity ToBudgetLogProjects()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Project_Log_BudgetEntity
			{
				Id_proj = Id_proj,
				Name_proj = Name_proj,
				Id_Customer = Id_Customer,
				Nr_Customer = Nr_Customer,
				Proj_Budget = Proj_Budget,
				Id_Responsable = Id_Responsable,
				Id_State = Id_State,
				Id_Currency = Id_Currency,
				Id_Land = Id_Land,
				Id_Dept = Id_Dept,
				Id_Type = Id_Type,
				Description = Description,
				Action_date = Action_date,
				Id_LS = Id_LS,
				Id_state = Id_state,
				Id_user = Id_user,
			};
		}
	}
}
