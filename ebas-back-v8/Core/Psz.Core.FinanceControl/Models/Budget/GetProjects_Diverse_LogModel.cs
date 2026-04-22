using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetProjects_Diverse_LogModel
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

		public string Customer_Contact_Description_Project_Diverse { get; set; }
		public string Customer_Contact_Project_Diverse { get; set; }
		public string Custommer_Name_Project_Diverse { get; set; }
		public int? Id_Customer_Project_Diverse { get; set; }
		public int Id_Diverse_Customer_Project { get; set; }
		public int? Id_Project_Diverse { get; set; }
		public int? kumdennummer_Project_Diverse { get; set; }
		public string Nr_Customer_Project_Diverse { get; set; }
		public string Ort_Project_Diverse { get; set; }


		public GetProjects_Diverse_LogModel() { }

		public GetProjects_Diverse_LogModel(Infrastructure.Data.Entities.Tables.FNC.Project_Log_Diverse_BudgetEntity budget_ProjectsLogEntity)


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

			Customer_Contact_Description_Project_Diverse = budget_ProjectsLogEntity.Customer_Contact_Description_Project_Diverse;
			Customer_Contact_Project_Diverse = budget_ProjectsLogEntity.Customer_Contact_Project_Diverse;
			Custommer_Name_Project_Diverse = budget_ProjectsLogEntity.Custommer_Name_Project_Diverse;
			Id_Customer_Project_Diverse = budget_ProjectsLogEntity.Id_Customer_Project_Diverse;
			Id_Diverse_Customer_Project = budget_ProjectsLogEntity.Id_Diverse_Customer_Project;
			Id_Project_Diverse = budget_ProjectsLogEntity.Id_Project_Diverse;
			kumdennummer_Project_Diverse = budget_ProjectsLogEntity.kumdennummer_Project_Diverse;
			Nr_Customer_Project_Diverse = budget_ProjectsLogEntity.Nr_Customer_Project_Diverse;
			Ort_Project_Diverse = budget_ProjectsLogEntity.Ort_Project_Diverse;


		}
		public Infrastructure.Data.Entities.Tables.FNC.Project_Log_Diverse_BudgetEntity ToBudgetLogProjects()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Project_Log_Diverse_BudgetEntity
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
				Customer_Contact_Description_Project_Diverse = Customer_Contact_Description_Project_Diverse,
				Customer_Contact_Project_Diverse = Customer_Contact_Project_Diverse,
				Custommer_Name_Project_Diverse = Custommer_Name_Project_Diverse,
				Id_Customer_Project_Diverse = Id_Customer_Project_Diverse,
				Id_Diverse_Customer_Project = Id_Diverse_Customer_Project,
				Id_Project_Diverse = Id_Project_Diverse,
				kumdennummer_Project_Diverse = kumdennummer_Project_Diverse,
				Nr_Customer_Project_Diverse = Nr_Customer_Project_Diverse,
				Ort_Project_Diverse = Ort_Project_Diverse,
			};
		}
	}
}
