namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetCustomerProjectModel
	{
		public string Customer_Name { get; set; }
		public int? Kundennummer { get; set; }
		public string Ort { get; set; }
		public int Nr { get; set; }


		public GetCustomerProjectModel() { }

		public GetCustomerProjectModel(Infrastructure.Data.Entities.Tables.FNC.Kunden_Project_BudgetEntity budget_Kunden_ProjectEntity)


		{
			Customer_Name = budget_Kunden_ProjectEntity.Customer_Name;
			Kundennummer = budget_Kunden_ProjectEntity.Kundennummer;
			Ort = budget_Kunden_ProjectEntity.Ort;
			Nr = budget_Kunden_ProjectEntity.Nr;

		}
		public Infrastructure.Data.Entities.Tables.FNC.Kunden_Project_BudgetEntity ToBudgetKundenProject()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Kunden_Project_BudgetEntity
			{
				Customer_Name = Customer_Name,
				Kundennummer = Kundennummer,
				Ort = Ort,
				Nr = Nr

			};
		}
	}
}
