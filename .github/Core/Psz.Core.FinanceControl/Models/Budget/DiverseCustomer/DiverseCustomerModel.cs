namespace Psz.Core.FinanceControl.Models.Budget.DiverseCustomer
{
	public class DiverseCustomerModel
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

		public DiverseCustomerModel() { }
		public DiverseCustomerModel(Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity DiverseCustomerEntity)
		{
			Customer_Contact_Description_Project_Diverse = DiverseCustomerEntity.Customer_Contact_Description_Project_Diverse;
			Customer_Contact_Project_Diverse = DiverseCustomerEntity.Customer_Contact_Project_Diverse;
			Custommer_Name_Project_Diverse = DiverseCustomerEntity.Custommer_Name_Project_Diverse;
			Id_Customer_Project_Diverse = DiverseCustomerEntity.Id_Customer_Project_Diverse;
			Id_Diverse_Customer_Project = DiverseCustomerEntity.Id_Diverse_Customer_Project;
			Id_Project_Diverse = DiverseCustomerEntity.Id_Project_Diverse == null ? -1 : DiverseCustomerEntity.Id_Project_Diverse;
			kumdennummer_Project_Diverse = DiverseCustomerEntity.kumdennummer_Project_Diverse == null ? -1 : DiverseCustomerEntity.kumdennummer_Project_Diverse;
			Nr_Customer_Project_Diverse = DiverseCustomerEntity.Nr_Customer_Project_Diverse;
			Ort_Project_Diverse = DiverseCustomerEntity.Ort_Project_Diverse;

		}

		public Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity ToDiverseCustomerEntity()
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

