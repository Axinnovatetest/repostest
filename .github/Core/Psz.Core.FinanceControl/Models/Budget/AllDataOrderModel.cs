using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class AllDataOrderModel
	{
		public int Id_Order { get; set; }
		public string Order_Number { get; set; }
		public string Type_Order { get; set; }
		public int? Id_Project { get; set; }
		//public int? Id_proj { get; set; }
		public int Id_Type { get; set; }
		public string Type_Project { get; set; }
		public int? Id_Currency_Order { get; set; }
		public string Symol { get; set; }
		public string Name_proj { get; set; }
		public int? Id_Dept { get; set; }
		public string Dept_name { get; set; }
		public int? Id_Land { get; set; }
		public string Land_name { get; set; }
		public string Description { get; set; }
		public double Proj_Budget { get; set; }
		//public double Proj_Rest_Budget { get; set; }
		public int? Id_Customer { get; set; }
		public string Customer_Name { get; set; }
		public int? Kundennummer { get; set; }
		public string Ort { get; set; }
		public string Nr { get; set; }
		public string Nr_Customer { get; set; }
		//public string Customer_Contact { get; set; }
		//public string Customer_Contact_Description { get; set; }
		public int Id_Responsable { get; set; }
		public string Responsable_Name { get; set; }
		public int Id_State { get; set; }
		public string State { get; set; }
		public int Nr_version_Order { get; set; }
		public int? Id_Level { get; set; }
		public string Level_Order { get; set; }
		public int? Id_Status_Order { get; set; }
		public string Status_Order { get; set; }
		public int? Id_Supplier { get; set; }
		public string Order_supplier_name { get; set; }
		public string Order_supplier_adress { get; set; }

		public int? Lieferantennummer { get; set; }
		public string Lieferantennummer_Order_Diverse { get; set; }
		public string Ort_Order_Supplier_Diverse { get; set; }
		public string Supplier_Contact_Description_Order_Diverse { get; set; }

		public string Supplier_Contact_Order_Diverse { get; set; }
		public string Supplier_Name_Order_Diverse { get; set; }
		public int Id_User { get; set; }
		public string Order_User_Name { get; set; }
		public double User_Budget { get; set; }
		//public double User_Rest_Budget { get; set; }
		//public int Order_year { get; set; }
		public int? Id_Dept_Responsable { get; set; }
		public string Dept_Responsable_Name { get; set; }
		public DateTime? Order_date { get; set; }
		public DateTime? Version_Order_date { get; set; }
		public double TotalCost_Order { get; set; }
		//public int Year { get; }





		public AllDataOrderModel() { }

		public AllDataOrderModel(Infrastructure.Data.Entities.Tables.FNC.AllDataOrderEntity allDataOrderEntity)


		{
			Id_Order = allDataOrderEntity.Id_Order;
			Order_Number = allDataOrderEntity.Order_Number;
			Type_Order = allDataOrderEntity.Type_Order;
			Id_Project = allDataOrderEntity.Id_Project;
			//Id_proj = allDataOrderEntity.Id_proj;
			Id_Type = allDataOrderEntity.Id_Type;
			Type_Project = allDataOrderEntity.Type_Project;
			Id_Currency_Order = allDataOrderEntity.Id_Currency_Order;
			Symol = allDataOrderEntity.Symol;
			Name_proj = allDataOrderEntity.Name_proj;
			Id_Dept = allDataOrderEntity.Id_Dept;
			Dept_name = allDataOrderEntity.Dept_name;
			Id_Land = allDataOrderEntity.Id_Land;
			Land_name = allDataOrderEntity.Land_name;
			Description = allDataOrderEntity.Description;
			Proj_Budget = allDataOrderEntity.Proj_Budget;
			//Proj_Rest_Budget = allDataOrderEntity.Proj_Rest_Budget;
			Id_Customer = allDataOrderEntity.Id_Customer;
			Customer_Name = allDataOrderEntity.Customer_Name;
			Kundennummer = allDataOrderEntity.Kundennummer;
			Ort = allDataOrderEntity.Ort;
			Nr = allDataOrderEntity.Nr;
			Nr_Customer = allDataOrderEntity.Nr_Customer;
			//Customer_Contact = allDataOrderEntity.Customer_Contact;
			//Customer_Contact_Description = allDataOrderEntity.Customer_Contact_Description;
			Id_Responsable = allDataOrderEntity.Id_Responsable;
			Responsable_Name = allDataOrderEntity.Responsable_Name;
			Id_State = allDataOrderEntity.Id_State;
			State = allDataOrderEntity.State;
			Nr_version_Order = allDataOrderEntity.Nr_version_Order;
			Id_Level = allDataOrderEntity.Id_Level;
			Level_Order = allDataOrderEntity.Level_Order;
			Id_Status_Order = allDataOrderEntity.Id_Status_Order;
			Status_Order = allDataOrderEntity.Status_Order;
			Id_Supplier = allDataOrderEntity.Id_Supplier;
			Order_supplier_name = allDataOrderEntity.Order_supplier_name;
			Order_supplier_adress = allDataOrderEntity.Order_supplier_adress;

			Lieferantennummer = allDataOrderEntity.Lieferantennummer;
			Lieferantennummer_Order_Diverse = allDataOrderEntity.Lieferantennummer_Order_Diverse;
			Ort_Order_Supplier_Diverse = allDataOrderEntity.Ort_Order_Supplier_Diverse;
			Supplier_Contact_Description_Order_Diverse = allDataOrderEntity.Supplier_Contact_Description_Order_Diverse;

			Supplier_Contact_Order_Diverse = allDataOrderEntity.Supplier_Contact_Order_Diverse;
			Supplier_Name_Order_Diverse = allDataOrderEntity.Supplier_Name_Order_Diverse;
			Id_User = allDataOrderEntity.Id_User;
			Order_User_Name = allDataOrderEntity.Order_User_Name;
			User_Budget = allDataOrderEntity.User_Budget;
			//User_Rest_Budget = allDataOrderEntity.User_Rest_Budget;
			//Order_year = allDataOrderEntity.Order_year;
			Id_Dept_Responsable = allDataOrderEntity.Id_Dept_Responsable;
			Dept_Responsable_Name = allDataOrderEntity.Dept_Responsable_Name;


			Order_date = allDataOrderEntity.Order_date;
			Version_Order_date = allDataOrderEntity.Version_Order_date;
			TotalCost_Order = allDataOrderEntity.TotalCost_Order;

		}
		public Infrastructure.Data.Entities.Tables.FNC.AllDataOrderEntity ToBudgetallDtaOrders()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AllDataOrderEntity
			{
				Id_Order = Id_Order,
				Order_Number = Order_Number,
				Type_Order = Type_Order,
				Id_Project = Id_Project,
				//Id_proj = Id_proj,
				Id_Type = Id_Type,
				Type_Project = Type_Project,
				Id_Currency_Order = Id_Currency_Order,
				Symol = Symol,
				Name_proj = Name_proj,
				Id_Dept = Id_Dept,
				Dept_name = Dept_name,
				Id_Land = Id_Land,
				Land_name = Land_name,
				Description = Description,
				Proj_Budget = Proj_Budget,
				//Proj_Rest_Budget = Proj_Rest_Budget,
				Id_Customer = Id_Customer,
				Customer_Name = Customer_Name,
				Kundennummer = Kundennummer,
				Ort = Ort,
				Nr = Nr,
				Nr_Customer = Nr_Customer,
				//Customer_Contact = Customer_Contact,
				//Customer_Contact_Description = Customer_Contact_Description,
				Id_Responsable = Id_Responsable,
				Responsable_Name = Responsable_Name,
				Id_State = Id_State,
				State = State,
				Nr_version_Order = Nr_version_Order,
				Id_Level = Id_Level,
				Level_Order = Level_Order,
				Id_Status_Order = Id_Status_Order,
				Status_Order = Status_Order,
				Id_Supplier = Id_Supplier,
				Order_supplier_name = Order_supplier_name,
				Order_supplier_adress = Order_supplier_adress,

				Lieferantennummer = Lieferantennummer,
				Lieferantennummer_Order_Diverse = Lieferantennummer_Order_Diverse,
				Ort_Order_Supplier_Diverse = Ort_Order_Supplier_Diverse,
				Supplier_Contact_Description_Order_Diverse = Supplier_Contact_Description_Order_Diverse,

				Supplier_Contact_Order_Diverse = Supplier_Contact_Order_Diverse,
				Supplier_Name_Order_Diverse = Supplier_Name_Order_Diverse,
				Id_User = Id_User,
				Order_User_Name = Order_User_Name,
				User_Budget = User_Budget,
				//User_Rest_Budget = User_Rest_Budget,
				//Order_year = Order_year,
				Id_Dept_Responsable = Id_Dept_Responsable,
				Dept_Responsable_Name = Dept_Responsable_Name,

				Order_date = Order_date,
				Version_Order_date = Version_Order_date,
				TotalCost_Order = TotalCost_Order,

			};
		}
	}
}
