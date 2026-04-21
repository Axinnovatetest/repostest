using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class AllDataProjectModel
	{
		public int Id_proj { get; set; }
		public string Name_proj { get; set; }
		public double Proj_Budget { get; set; }
		public int? Id_Customer { get; set; }
		public string Customer_Name { get; set; }
		public int? Kundennummer { get; set; }
		public int? Nr { get; set; }
		public string Ort { get; set; }
		public string Nr_Customer { get; set; }
		public int Id_Responsable { get; set; }
		public string Responsable_Name { get; set; }
		public int Id_State { get; set; }
		public string State { get; set; }
		public int? Id_Currency { get; set; }
		public string Symol { get; set; }
		public int? Id_Land { get; set; }
		public string Land_name { get; set; }
		public int? Id_Dept { get; set; }
		public string Departement_Name { get; set; }
		public int Id_Type { get; set; }
		public string Type_Project { get; set; }
		public string Description { get; set; }
		public DateTime? Action_date { get; set; }


		public AllDataProjectModel() { }

		public AllDataProjectModel(Infrastructure.Data.Entities.Tables.FNC.AllDataProjectEntity allDataProjectEntity)


		{

			Id_proj = allDataProjectEntity.Id_proj;
			Name_proj = allDataProjectEntity.Name_proj;
			Proj_Budget = allDataProjectEntity.Proj_Budget;
			Id_Customer = allDataProjectEntity.Id_Customer;
			Customer_Name = allDataProjectEntity.Customer_Name;
			Kundennummer = allDataProjectEntity.Kundennummer;
			Nr = allDataProjectEntity.Nr;
			Ort = allDataProjectEntity.Ort;
			Nr_Customer = allDataProjectEntity.Nr_Customer;
			Id_Responsable = allDataProjectEntity.Id_Responsable;
			Responsable_Name = allDataProjectEntity.Responsable_Name;
			Id_State = allDataProjectEntity.Id_State;
			State = allDataProjectEntity.State;
			Id_Currency = allDataProjectEntity.Id_Currency;
			Symol = allDataProjectEntity.Symol;
			Id_Land = allDataProjectEntity.Id_Land;
			Land_name = allDataProjectEntity.Land_name;
			Id_Dept = allDataProjectEntity.Id_Dept;
			Departement_Name = allDataProjectEntity.Departement_Name;
			Id_Type = allDataProjectEntity.Id_Type;
			Type_Project = allDataProjectEntity.Type_Project;
			Description = allDataProjectEntity.Description;
			//Action_date = allDataProjectEntity.Action_date;

		}
		public Infrastructure.Data.Entities.Tables.FNC.AllDataProjectEntity ToBudgetallDtaProjects()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AllDataProjectEntity
			{
				Id_proj = Id_proj,
				Name_proj = Name_proj,
				Proj_Budget = Proj_Budget,
				Id_Customer = Id_Customer,
				Customer_Name = Customer_Name,
				Kundennummer = Kundennummer,
				Nr = Nr,
				Ort = Ort,
				Nr_Customer = Nr_Customer,
				Id_Responsable = Id_Responsable,
				Responsable_Name = Responsable_Name,
				Id_State = Id_State,
				State = State,
				Id_Currency = Id_Currency,
				Symol = Symol,
				Id_Land = Id_Land,
				Land_name = Land_name,
				Id_Dept = Id_Dept,
				Departement_Name = Departement_Name,
				Id_Type = Id_Type,
				Type_Project = Type_Project,
				Description = Description,
				//Action_date = Action_date,

			};
		}
	}
}
