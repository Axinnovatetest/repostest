namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetCustomer_Project_DiverseModel
	{


		public string Customer_Contact_Description_Project_Diverse { get; set; }
		public string Customer_Contact_Project_Diverse { get; set; }
		public string Custommer_Name_Project_Diverse { get; set; }
		public int? Id_Customer_Project_Diverse { get; set; }
		public int Id_Diverse_Customer_Project { get; set; }
		public int? Id_Project_Diverse { get; set; }
		public int? kumdennummer_Project_Diverse { get; set; }
		public string Nr_Customer_Project_Diverse { get; set; }
		public string Ort_Project_Diverse { get; set; }


		public GetCustomer_Project_DiverseModel() { }

		public GetCustomer_Project_DiverseModel(Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity budget_Customer_Project_DiverseEntity)


		{


			Customer_Contact_Description_Project_Diverse = budget_Customer_Project_DiverseEntity.Customer_Contact_Description_Project_Diverse;
			Customer_Contact_Project_Diverse = budget_Customer_Project_DiverseEntity.Customer_Contact_Project_Diverse;
			Custommer_Name_Project_Diverse = budget_Customer_Project_DiverseEntity.Custommer_Name_Project_Diverse;
			Id_Customer_Project_Diverse = budget_Customer_Project_DiverseEntity.Id_Customer_Project_Diverse;
			Id_Diverse_Customer_Project = budget_Customer_Project_DiverseEntity.Id_Diverse_Customer_Project;
			Id_Project_Diverse = budget_Customer_Project_DiverseEntity.Id_Project_Diverse;
			kumdennummer_Project_Diverse = budget_Customer_Project_DiverseEntity.kumdennummer_Project_Diverse;
			Nr_Customer_Project_Diverse = budget_Customer_Project_DiverseEntity.Nr_Customer_Project_Diverse;
			Ort_Project_Diverse = budget_Customer_Project_DiverseEntity.Ort_Project_Diverse;


		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity ToBudgetLogProjects()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity
			{

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
